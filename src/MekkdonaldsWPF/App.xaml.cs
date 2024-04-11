﻿namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const double MARGIN = 0;
    private const double SIDELENGTH = 20;

    private int XLength;
    private int YLength;

    private int Step => (int)Math.Round(SIDELENGTH * (_viewModel?.Zoom ?? 1));

    private SimulationWindow? _simWindow;
    private StartWindow? _startWindow;
    private ReplayWindow? _replayWindow;
    private ViewModel.ViewModel? _viewModel;

    private bool _ctrlDown;
    private Point _mousePos;

    private ImageBrush _rectangel;
    private readonly ImageBrush[] _ellipses = new ImageBrush[4];
    private readonly Dictionary<Robot, Grid> _robots = [];
    private readonly Dictionary<Robot, Grid> _targets = [];

    public App()
    {
        Startup += OnStartup;

        DrawElements();

        if (_rectangel is null || _ellipses.Any(x => x is null))
            throw new System.Exception("Failed to load images");
    }

    private void OnStartup(object sender, StartupEventArgs e)
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
    /// Closes the start window
    /// </summary>
    private void DisposeStartWindow()
    {
        _startWindow!.Close(); // can't be null
        _startWindow.SimButton.Click -= SimButton_Click;
        _startWindow.ReplayButton.Click -= ReplayButton_Click;
        _startWindow = null;
    }

    /// <summary>
    /// Opens a replay window
    /// </summary>
    /// <returns>Wether to user want's to proceed with opening the window</returns>
    private bool OpenReplay()
    {
        var fd = new OpenFileDialog()
        {
            Filter = "Json files (*.json)|*.json",
            Title = "Log File"
        };

        if (fd.ShowDialog() is false) return false;

        var logPath = fd.FileName;

        fd = new OpenFileDialog()
        {
            Filter = "Map file (*.map)|*.map",
            Title = "Map file"
        };

        if (fd.ShowDialog() is false) return false;

        _viewModel = new ReplayViewModel(logPath, fd.FileName);

        _replayWindow = new ReplayWindow
        {
            DataContext = _viewModel
        };

        _viewModel.Loaded += (_, _) => OnLoaded(_replayWindow, _replayWindow.MapCanvas);

        _viewModel.Tick += OnTick;
        _viewModel.PropertyChanged += OnPropertyChanged;

        _replayWindow.SizeChanged += (_, _) => OnSizeChanged(_replayWindow.MapCanvas);

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
    /// <returns>Wether to user want's to proceed with opening the window</returns>
    private bool OpenSim()
    {
        var fd = new OpenFileDialog()
        {
            Filter = "Json file (*.json)|*.json",
            Title = "Config file"
        };

        if (fd.ShowDialog() is false) return false;

        _viewModel = new SimulationViewModel(fd.FileName);

        _simWindow = new SimulationWindow
        {
            DataContext = _viewModel
        };

        _viewModel.Loaded += (_, _) => OnLoaded(_simWindow, _simWindow.MapCanvas);

        _viewModel.Tick += OnTick;
        _viewModel.PropertyChanged += OnPropertyChanged;

        _simWindow.SizeChanged += (_, _) => OnSizeChanged(_simWindow.MapCanvas);

        _simWindow.KeyDown += OnKeyDown;

        _simWindow.KeyUp += OnKeyUp;

        _simWindow.ScrollViewer.PreviewMouseWheel += OnMouseWheel;

        _simWindow.ScrollViewer.MouseMove += OnMouseMove;

        _simWindow.ScrollViewer.ManipulationDelta += OnManipulationDelta;

        _simWindow.ScrollViewer.Cursor = Cursors.Hand;

        _simWindow.Show();

        DisplayLoading(_simWindow);

        return true;
    }

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
        var w = _viewModel!.Width;
        var h = _viewModel.Height;

        c.Width = XLength = w * Step;
        c.Height = YLength = h * Step;

        var fontSize = 12 * Math.Sqrt(_viewModel.Zoom);

        foreach (var g in _robots.Values)
        {
            g.Width = g.Height = Step - 2;
            (g.Children[0] as TextBlock ?? throw new System.Exception()).FontSize = fontSize;
        }

        foreach (var g in _targets.Values)
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
        foreach (var r in _viewModel!.Robots)
        {
            _robots[r].Margin = new Thickness(MARGIN + 1 + r.Position.X * Step, MARGIN + 1 + r.Position.Y * Step, 0, 0);
            _robots[r].Background = _ellipses[(int)r.Direction];

            if (r.Task is not null)
            {
                _targets[r].Margin = new Thickness(MARGIN + r.Task.Position.X * Step, MARGIN + r.Task.Position.Y * Step, 0, 0);
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
        using var bm = new Bitmap(Step * _viewModel!.Width * 2, Step * _viewModel.Height * 2);
        using var g = Graphics.FromImage(bm);

        for (var i = 0; i <= _viewModel.Width; i++)
        {
            g.DrawLine(Pens.Black, i * Step * 2, 0, i * Step * 2, YLength * 2);
        }

        for (var i = 0; i <= _viewModel.Height; i++)
        {
            g.DrawLine(Pens.Black, 0, i * Step * 2, XLength * 2, i * Step * 2);
        }

        foreach (var w in _viewModel!.Walls)
        {
            g.FillRectangle(Brushes.Black, w.Position.X * Step * 2, w.Position.Y * Step * 2, Step * 2, Step * 2);
        }

        using var memory = new MemoryStream();
        bm.Save(memory, ImageFormat.Png);
        memory.Position = 0;
        var r = new BitmapImage();
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
        var fontSize = 12 * Math.Sqrt(_viewModel!.Zoom);

        foreach (var r in _viewModel!.Robots)
        {
            var grid = new Grid
            {
                Width = Step - 2,
                Height = Step - 2,
                Margin = new Thickness(MARGIN + 1 + r.Position.X * Step, MARGIN + 1 + r.Position.Y * Step, 0, 0),
                Background = _ellipses[(int)r.Direction]
            };

            grid.Children.Add(new TextBlock()
            {
                Text = r.ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontSize
            });

            _robots[r] = grid;

            grid = new Grid
            {
                Width = Step,
                Height = Step,
                Background = _rectangel
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
                grid.Margin = new Thickness(MARGIN + r.Task.Position.X * Step, MARGIN + r.Task.Position.Y * Step, 0, 0);
                grid.Visibility = Visibility.Visible;
            }
            else
            {
                grid.Visibility = Visibility.Hidden;
            }

            c.Children.Add(grid);

            _targets[r] = grid;
        }

        foreach (var g in _robots.Values)
        {
            c.Children.Add(g);
        }
    }

    /// <summary>
    /// Draws the images used for the robots and their targets
    /// </summary>
    private void DrawElements()
    {
        {
            using var bm = new Bitmap(500, 500);
            using var g = Graphics.FromImage(bm);

            g.FillRectangle(Brushes.Orange, 0, 0, 500, 500);

            using var memory = new MemoryStream();
            bm.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            var r = new BitmapImage();
            r.BeginInit();
            r.StreamSource = memory;
            r.CacheOption = BitmapCacheOption.OnLoad;
            r.EndInit();

            _rectangel = new ImageBrush(r);
        }

        {
            for (int i = 0; i < 4; i++)
            {
                using var bm = new Bitmap(500, 500);
                using var g = Graphics.FromImage(bm);

                g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(9, 194, 248)), 0, 0, 500, 500);

                switch (i)
                {
                    case 0:
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Red, 50), 250, 0, 250, 250);
                        break;
                    case 1:
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Red, 50), 250, 250, 500, 250);
                        break;
                    case 2:
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Red, 50), 250, 250, 250, 500);
                        break;
                    case 3:
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Red, 50), 0, 250, 250, 250);
                        break;
                }

                using var memory = new MemoryStream();
                bm.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var r = new BitmapImage();
                r.BeginInit();
                r.StreamSource = memory;
                r.CacheOption = BitmapCacheOption.OnLoad;
                r.EndInit();

                _ellipses[i] = new ImageBrush(r);
            }
        }


    }

    #endregion

    #region Event Handlers    
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
        if (e.Key is Key.LeftCtrl || e.Key is Key.RightCtrl)
        {
            _ctrlDown = true;
        }
        else if (e.Key is Key.Escape) Current.Shutdown();
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

        var p = e.GetPosition(ie);

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
        Calculate(canvas);
        Redraw();
    }

    private void OnLoaded(Window window, Canvas canvas)
    {
        Dispatcher.Invoke(() =>
        {
            Calculate(canvas);
            DrawGrid(canvas);
            InitRobots(canvas);
            Redraw();
            window.Cursor = Cursors.Arrow;
        });
    }
    #endregion
}
