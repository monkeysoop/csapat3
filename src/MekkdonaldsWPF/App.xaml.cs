﻿using Mekkdonalds.Simulation.PathFinding;

using System.Globalization;

namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const double SIDELENGTH = 20;

    private int XLength;
    private int YLength;

    private int Step => (int)Math.Round(SIDELENGTH * (_viewModel?.Zoom ?? 1));

    private SimulationWindow? _simWindow;
    private StartWindow? _startWindow;
    private ReplayWindow? _replayWindow;
    private ViewModel.ViewModel? _viewModel;
    private MouseButtonEventHandler? _mouseDoubleClickHandler;
    private SizeChangedEventHandler? _onSizeChangedHandler;
    private EventHandler? _onLoadedHandler;

    private bool _loaded;

    private bool _ctrlDown;
    private Point _mousePos;

    private Robot? _selectedRobot;

    private ImageBrush _rectangle;
    private readonly ImageBrush[] _ellipses = new ImageBrush[4];
    private readonly Dictionary<Robot, Grid> _robots = [];
    private readonly Dictionary<Robot, Grid> _targets = [];

    public App()
    {
        Startup += (_,_) => StartWindow();

        DrawElements();

        if (_rectangle is null || _ellipses.Any(x => x is null))
        {
            var ex = new System.Exception("Failed to load images");
            Exception(this, ex);
            throw ex;
        }
    }

    private void StartWindow()
    {
        _startWindow = new StartWindow();

        _startWindow.Show();

        _startWindow.SimButton.Click += SimButton_Click;
        _startWindow.ReplayButton.Click += ReplayButton_Click;
    }

    private void ReplayButton_Click(object sender, RoutedEventArgs e)
    {
        if (!OpenReplay()) return;
        Current.MainWindow = _replayWindow;
        DisposeStartWindow();
    }

    private void SimButton_Click(object sender, RoutedEventArgs e)
    {
        if (!OpenSim()) return;
        Current.MainWindow = _simWindow;
        DisposeStartWindow();
    }

    /// <summary>
    /// Closes the start window. Only call this from the UI thread!
    /// </summary>
    private void DisposeStartWindow()
    {
        DisposeWindow(_startWindow);
        _startWindow = null;
    }

    /// <summary>
    /// Disposes a window and removes all event handlers
    /// </summary>
    /// <param name="window">The window to dispose</param>
    private void DisposeWindow(object? window)
    {
        _loaded = false;

        if (window is StartWindow startWindow)
        {
            startWindow.Close();

            startWindow.SimButton.Click -= SimButton_Click;
            startWindow.ReplayButton.Click -= ReplayButton_Click;
            return;
        }

        if (window is SimulationWindow simulationWindow)
        {
            try
            {
                simulationWindow.Close();
            }
            catch (InvalidOperationException ex) { if (!ex.Message.Contains("Cannot set Visibility to Visible")) throw; }

            simulationWindow.SizeChanged -= _onSizeChangedHandler;
            simulationWindow.KeyDown -= OnKeyDown;
            simulationWindow.KeyUp -= OnKeyUp;
            simulationWindow.ScrollViewer.PreviewMouseWheel -= OnMouseWheel;
            simulationWindow.ScrollViewer.MouseMove -= OnMouseMove;
            simulationWindow.ScrollViewer.ManipulationDelta -= OnManipulationDelta;
            simulationWindow.ScrollViewer.MouseDoubleClick -= _mouseDoubleClickHandler!;
            
            if (_viewModel is SimulationViewModel simulationViewModel)
            {
                simulationViewModel.Exception -= Exception;
                simulationViewModel.Loaded -= _onLoadedHandler;
                simulationViewModel.Tick -= OnTick;
                simulationViewModel.PropertyChanged -= OnPropertyChanged;
                simulationViewModel.Ended -= OnEnded;
            }
        }
        else if (window is ReplayWindow replayWindow)
        {
            replayWindow.Close();

            replayWindow.SizeChanged -= _onSizeChangedHandler;
            replayWindow.KeyDown -= OnKeyDown;
            replayWindow.KeyUp -= OnKeyUp;
            replayWindow.ScrollViewer.PreviewMouseWheel -= OnMouseWheel;
            replayWindow.ScrollViewer.MouseMove -= OnMouseMove;
            replayWindow.ScrollViewer.ManipulationDelta -= OnManipulationDelta;
        }

        _robots.Clear();
        _targets.Clear();

        //_viewModel = null;
        _onSizeChangedHandler = null;
        _onLoadedHandler = null;
        _mouseDoubleClickHandler = null;
    }

    /// <summary>
    /// Opens a replay window
    /// </summary>
    /// <returns>Whether to user want's to proceed with opening the window</returns>
    private bool OpenReplay()
    {
        OpenFileDialog fd = new()
        {
            Filter = "Json files (*.json)|*.json",
            Title = "Log File",
            InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "logs")
        };

        if (fd.ShowDialog() is false) return false;

        string logPath = fd.FileName;

        fd = new OpenFileDialog()
        {
            Filter = "Map file (*.map)|*.map",
            Title = "Map file",
            InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "maps")
        };

        if (fd.ShowDialog() is false) return false;

        try
        {
            _viewModel = new ReplayViewModel(logPath, fd.FileName);
        }
        catch (System.Exception ex)
        {
            Exception(this, ex);
            return false;
        }

        _replayWindow = new ReplayWindow
        {
            DataContext = _viewModel
        };

        _onLoadedHandler = (_, _) => OnLoaded(_replayWindow, _replayWindow.MapCanvas);

        _viewModel.Loaded += _onLoadedHandler;

        _viewModel.Tick += OnTick;
        _viewModel.PropertyChanged += OnPropertyChanged;

        _onSizeChangedHandler = (_, _) => OnSizeChanged(_replayWindow.MapCanvas);

        _replayWindow.SizeChanged += _onSizeChangedHandler;

        _replayWindow.KeyDown += OnKeyDown;

        _replayWindow.KeyUp += OnKeyUp;

        _replayWindow.ScrollViewer.PreviewMouseWheel += OnMouseWheel;

        _replayWindow.ScrollViewer.MouseMove += OnMouseMove;

        _replayWindow.ScrollViewer.ManipulationDelta += OnManipulationDelta;

        _replayWindow.ScrollViewer.Cursor = Cursors.Hand;

        _replayWindow.Show();

        DisplayLoading(_replayWindow);

        return true;
    }

    /// <summary>
    /// Opens a simulation window
    /// </summary>
    /// <returns>Whether to user want's to proceed with opening the window</returns>
    private bool OpenSim()
    {
        OpenFileDialog fd = new()
        {
            Filter = "Json file (*.json)|*.json",
            Title = "Config file",
            InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "configs")
        };

        if (fd.ShowDialog() is false) return false;

        Type algorithm = typeof(AStar);

        string configFile = fd.FileName;

        if (_startWindow!.BFS.IsChecked!.Value) algorithm = typeof(BFS);
        else if (_startWindow.DFS.IsChecked!.Value) algorithm = typeof(DFS);

        int simulationLength = int.Parse(_startWindow.Length.Text);
        double speed = double.Parse(_startWindow.Speed.Text, NumberStyles.Any, CultureInfo.InvariantCulture);

        SimulationViewModel simulationViewModel;

        try
        {
            simulationViewModel = new(configFile, algorithm, speed, simulationLength);
        }
        catch (System.Exception ex)
        {
            Exception(this, ex);
            return false;
        }

        _viewModel = simulationViewModel;
        simulationViewModel.Ended += OnEnded;

        _simWindow = new SimulationWindow
        {
            DataContext = _viewModel,
        };

        _simWindow.Title += $" - Simulation - {System.IO.Path.GetFileName(fd.FileName)}";

        _onLoadedHandler = (_, _) => OnLoaded(_simWindow, _simWindow.MapCanvas);

        _viewModel.Loaded += _onLoadedHandler;

        _viewModel.Tick += OnTick;
        _viewModel.PropertyChanged += OnPropertyChanged;
        _viewModel.Exception += Exception;

        _onSizeChangedHandler = (_, _) => OnSizeChanged(_simWindow.MapCanvas);

        _simWindow.SizeChanged += _onSizeChangedHandler;

        _simWindow.KeyDown += OnKeyDown;

        _simWindow.KeyUp += OnKeyUp;

        _simWindow.ScrollViewer.PreviewMouseWheel += OnMouseWheel;

        _simWindow.Closing += (sender, e) => { simulationViewModel.Dispose(); DisposeWindow(sender); };

        _mouseDoubleClickHandler = (_, e) =>
        {
            Point point = e.GetPosition(_simWindow.MapCanvas);

            if (_selectedRobot is not null)
            {
                Robot r = _selectedRobot;
                Task.Run(() => simulationViewModel.AssignTask(r, (int)Math.Floor(point.X / Step), (int)Math.Floor(point.Y / Step)));
                _selectedRobot = null;
            }
        };

        _simWindow.ScrollViewer.MouseDoubleClick += _mouseDoubleClickHandler;

        _simWindow.ScrollViewer.MouseMove += OnMouseMove;

        _simWindow.ScrollViewer.ManipulationDelta += OnManipulationDelta;

        _simWindow.ScrollViewer.Cursor = Cursors.Hand;

        _simWindow.Show();

        DisplayLoading(_simWindow);

        return true;
    }

    private static void DisplayLoading(Window w)
    {
        w.Cursor = Cursors.Wait;
    }

    #region Drawing

    /// <summary>
    /// Calculates the dimensions required to draw the grid
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Calculate(Canvas c)
    {
        int w = _viewModel!.Width;
        int h = _viewModel.Height;

        c.Width = XLength = w * Step;
        c.Height = YLength = h * Step;

        double fontSize = 12 * Math.Sqrt(_viewModel.Zoom);

        foreach (Grid g in _robots.Values)
        {
            g.Width = g.Height = Step - 2;
            (g.Children[0] as TextBlock ?? throw new System.Exception()).FontSize = fontSize;
        }

        foreach (Grid g in _targets.Values)
        {
            g.Width = g.Height = Step;
            (g.Children[0] as TextBlock ?? throw new System.Exception()).FontSize = fontSize;
        }
    }

    /// <summary>
    /// Clears the canvas and redraws every element
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Redraw()
    {
        if (_viewModel is null) return;

        foreach (Robot r in _viewModel.Robots)
        {
            _robots[r].Margin = new Thickness(1 + r.Position.X * Step, 1 + r.Position.Y * Step, 0, 0);
            _robots[r].Background = _ellipses[(int)r.Direction];

            if (r.Task is not null)
            {
                _targets[r].Margin = new Thickness(r.Task.Position.X * Step, r.Task.Position.Y * Step, 0, 0);
                _targets[r].Visibility = Visibility.Visible;
            }
            else
            {
                _targets[r].Visibility = Visibility.Hidden;
            }
        }
    }

    /// <summary>
    /// Draws the grid
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawGrid(Canvas c)
    {
        using Bitmap bm = new(Step * _viewModel!.Width * 2, Step * _viewModel.Height * 2);
        using Graphics g = Graphics.FromImage(bm);

        for (int i = 0; i <= _viewModel.Width; i++)
        {
            g.DrawLine(Pens.Black, i * Step * 2, 0, i * Step * 2, YLength * 2);
        }

        for (int i = 0; i <= _viewModel.Height; i++)
        {
            g.DrawLine(Pens.Black, 0, i * Step * 2, XLength * 2, i * Step * 2);
        }

        foreach (Wall w in _viewModel!.Walls)
        {
            g.FillRectangle(Brushes.Black, w.Position.X * Step * 2, w.Position.Y * Step * 2, Step * 2, Step * 2);
        }

        using MemoryStream memory = new();
        bm.Save(memory, ImageFormat.Png);
        memory.Position = 0;
        BitmapImage r = new();
        r.BeginInit();
        r.StreamSource = memory;
        r.CacheOption = BitmapCacheOption.OnLoad;
        r.EndInit();

        c.Background = new ImageBrush(r);
    }

    /// <summary>
    /// Creates the grids for the robots and their targets
    /// </summary>
    /// <param name="c"></param>
    private void InitRobots(Canvas c)
    {
        _robots.Clear();
        _targets.Clear();

        double fontSize = 12 * Math.Sqrt(_viewModel!.Zoom);

        foreach (Robot r in _viewModel!.Robots)
        {
            Grid grid = new()
            {
                Width = Step - 2,
                Height = Step - 2,
                Margin = new Thickness(1 + r.Position.X * Step, 1 + r.Position.Y * Step, 0, 0),
                Background = _ellipses[(int)r.Direction]
            };

            grid.Children.Add(new TextBlock()
            {
                Text = r.ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontSize
            });

            grid.MouseDown += (_, _) => _selectedRobot = r;

            _robots[r] = grid;

            grid = new Grid
            {
                Width = Step,
                Height = Step,
                Background = _rectangle
            };

            grid.Children.Add(new TextBlock()
            {
                Text = r.ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontSize
            });

            if (r.Task is not null)
            {
                grid.Margin = new Thickness(r.Task.Position.X * Step, r.Task.Position.Y * Step, 0, 0);
                grid.Visibility = Visibility.Visible;
            }
            else
            {
                grid.Visibility = Visibility.Hidden;
            }

            c.Children.Add(grid);

            _targets[r] = grid;
        }

        foreach (Grid g in _robots.Values)
        {
            c.Children.Add(g);
        }
    }

    private static void RotateBitmap(ref Bitmap bmp, float angle)
    {
        Bitmap tempBmp = new(bmp.Width, bmp.Height);
        using Graphics g = Graphics.FromImage(tempBmp);

        g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
        g.RotateTransform(angle);
        g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
        g.DrawImage(bmp, new PointF(0, 0));

        bmp.Dispose(); //dispose old bitmap
        bmp = tempBmp; //assign new bitmap to reference
    }

    /// <summary>
    /// Draws the images used for the robots and their targets
    /// </summary>
    private void DrawElements()
    {
        {
            using Bitmap bm = new(500, 500);
            using Graphics g = Graphics.FromImage(bm);

            g.FillRectangle(Brushes.Orange, 0, 0, 500, 500);

            using MemoryStream memory = new();
            bm.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            BitmapImage r = new();
            r.BeginInit();
            r.StreamSource = memory;
            r.CacheOption = BitmapCacheOption.OnLoad;
            r.EndInit();

            _rectangle = new ImageBrush(r);
        }

        {
            Bitmap bm = new(500, 500);
            using Graphics g = Graphics.FromImage(bm);

            g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(9, 194, 248)), 0, 250, 500, 250);
            g.FillPie(new SolidBrush(System.Drawing.Color.FromArgb(9, 194, 248)), 0, 0, 500, 500, 180, 180);
            // eyes
            //g.FillEllipse(Brushes.Black, 150, 80, 75, 75);
            //g.FillEllipse(Brushes.Black, 275, 80, 75, 75);

            for (int i = 0; i < 4; i++)
            {
                using MemoryStream memory = new();
                bm.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage r = new();
                r.BeginInit();
                r.StreamSource = memory;
                r.CacheOption = BitmapCacheOption.OnLoad;
                r.EndInit();

                _ellipses[i] = new ImageBrush(r);

                RotateBitmap(ref bm, 90);
            }

            bm.Dispose();
        }
    }

    #endregion

    #region Event Handlers

    private void OnTick(object? sender, EventArgs e)
    {
#if DEBUG
        try
        { Dispatcher.Invoke(Redraw); }
        catch (TaskCanceledException)
        { }
#else
        Dispatcher.Invoke(Redraw);
#endif
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Zoom":
                Calculate(_replayWindow?.MapCanvas ?? _simWindow?.MapCanvas ?? throw new System.Exception());
                Redraw();
                break;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.LeftCtrl:
            case Key.RightCtrl:
                _ctrlDown = true;
                break;
            case Key.Escape:
                Current.Shutdown();
                break;
            case Key.Space:
                _viewModel?.Toggle();
                break;

        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.LeftCtrl || e.Key is Key.RightCtrl)
        {
            _ctrlDown = false;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (_ctrlDown)
        {
            if (e.Delta > 0)
                _viewModel!.Zoom *= 1.1;
            else
                _viewModel!.Zoom /= 1.1;
        }

        e.Handled = true;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not IInputElement ie) return;

        Point p = e.GetPosition(ie);

        if (e.LeftButton is MouseButtonState.Pressed)
        {
            if (_simWindow is not null)
            {
                _simWindow.ScrollViewer.ScrollToHorizontalOffset(_simWindow.ScrollViewer.HorizontalOffset + (_mousePos.X - p.X));
                _simWindow.ScrollViewer.ScrollToVerticalOffset(_simWindow.ScrollViewer.VerticalOffset + (_mousePos.Y - p.Y));
            }
            else if (_replayWindow is not null)
            {
                _replayWindow.ScrollViewer.ScrollToHorizontalOffset(_replayWindow.ScrollViewer.HorizontalOffset + (_mousePos.X - p.X));
                _replayWindow.ScrollViewer.ScrollToVerticalOffset(_replayWindow.ScrollViewer.VerticalOffset + (_mousePos.Y - p.Y));
            }
        }

        _mousePos = p;
    }

    private void OnManipulationDelta(object? sender, ManipulationDeltaEventArgs e)
    {
        // Pinching in
        if (e.DeltaManipulation.Scale.X < 1.0 || e.DeltaManipulation.Scale.Y < 1.0)
            _viewModel!.Zoom /= 1.1;

        // Pinching out
        else if (e.DeltaManipulation.Scale.X > 1.0 || e.DeltaManipulation.Scale.Y > 1.0)
            _viewModel!.Zoom *= 1.1;

        e.Handled = true;
    }

    private void OnSizeChanged(Canvas canvas)
    {
        if (!_loaded) return;
        Calculate(canvas);
        Redraw();
    }

    private void OnLoaded(Window window, Canvas canvas)
    {
        _loaded = true;

        Dispatcher.Invoke(() =>
        {
            Calculate(canvas);
            DrawGrid(canvas);
            InitRobots(canvas);
            Redraw();
            window.Cursor = Cursors.Arrow;
        });
    }

    private void OnEnded(object? sender, EventArgs e)
    {
        MessageBox.Show("Simulation ended", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

        Dispatcher.Invoke(_simWindow!.Close);        
    }
    #endregion

    private static void Exception(object? _, System.Exception exception)
    {
        MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
