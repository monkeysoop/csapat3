
using System.Diagnostics;

namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const double MARGIN = 20;
    private const double BORDERTHICKNESS = 4;
    private const double SIDELENGTH = 20;

    private double XLength;
    private double YLength;

    private double Step => SIDELENGTH * (_viewModel?.Zoom ?? 1);

    private SimulationWindow? _simWindow;
    private StartWindow? _startWindow;
    private ReplayWindow? _replayWindow;
    private ViewModel.ViewModel? _viewModel;
    private bool _ctrlDown;
    private Point _mousePos;

    public App()
    {
        Startup += OnStartup;
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

        if (fd.ShowDialog() is false)
            return false;

        _viewModel = new ReplayViewModel(fd.FileName);

        _replayWindow = new ReplayWindow
        {
            WindowState = WindowState.Maximized,
            DataContext = _viewModel
        };

        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw(_replayWindow.MapCanvas)); // UI elemts have to be updated with this call when it is called from another thread
        _viewModel.PropertyChanged += OnPropertyChanged;

        _replayWindow.SizeChanged += (_, _) => { Calculate(_replayWindow.MapCanvas); Redraw(_replayWindow.MapCanvas); };

        _replayWindow.Show();

        Calculate(_replayWindow.MapCanvas);
        Redraw(_replayWindow.MapCanvas);

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
            WindowState = WindowState.Maximized,
            DataContext = _viewModel
        };

        var canvas = _simWindow.MapCanvas;

        _viewModel.Loaded += (_, _) => Dispatcher.Invoke(() => { Calculate(canvas); Redraw(canvas); _simWindow.Cursor = Cursors.Arrow; }); // UI elemts have to be updated with this call when it is called from another thread
        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw(canvas));
        _viewModel.PropertyChanged += OnPropertyChanged;

        _simWindow.SizeChanged += (_, _) => { Calculate(canvas); Redraw(canvas); };
        _simWindow.KeyDown += (_, e) =>
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                _ctrlDown = true;
            }
        };

        _simWindow.KeyUp += (_, e) =>
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                _ctrlDown = false;
            }
        };

        _simWindow.ScrollViewer.PreviewMouseWheel += (_, e) =>
        {
            if (_ctrlDown)
            {
                if (e.Delta > 0)
                    _viewModel.Zoom *= 1.1;
                else
                    _viewModel.Zoom /= 1.1;
            }

            e.Handled = true;
        };

        _simWindow.ScrollViewer.MouseMove += (x, e) =>
        {
            var p = e.GetPosition(x as IInputElement);

            if (e.LeftButton is MouseButtonState.Pressed)
            {
                _simWindow.ScrollViewer.ScrollToHorizontalOffset(_simWindow.ScrollViewer.HorizontalOffset + (_mousePos.X - p.X));
                _simWindow.ScrollViewer.ScrollToVerticalOffset(_simWindow.ScrollViewer.VerticalOffset + (_mousePos.Y - p.Y));
            }

            _mousePos = p;
        };

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
        var w = _viewModel!.Width;
        var h = _viewModel.Height;

        XLength = (w + 1) * Step - (_viewModel.Zoom - 1) * MARGIN;
        YLength = (h + 1) * Step - (_viewModel.Zoom - 1) * MARGIN;

        c.Width = XLength + 2 * MARGIN;
        c.Height = YLength + 2 * MARGIN;
    }

    /// <summary>
    /// Clears the canvas and redraws every element
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Redraw(Canvas c)
    {
        c.Children.Clear();

        //DrawFrame(c);
        DrawGrid(c);
        DrawWalls(c);
        DrawRobots(c);
    }

    /// <summary>
    /// Draws the frame of the map
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawFrame(Canvas c)
    {
        List<Line> l = [];

        // TOP
        l.Add(new Line()
        {
            Stroke = Brushes.Black,
            StrokeThickness = BORDERTHICKNESS,
            X1 = MARGIN - 2,
            Y1 = MARGIN,
            X2 = XLength + 2,
            Y2 = MARGIN,
        });

        // BOTTOM
        l.Add(new Line()
        {
            Stroke = Brushes.Black,
            StrokeThickness = BORDERTHICKNESS,
            X1 = MARGIN - 2,
            Y1 = YLength,
            X2 = XLength + 2,
            Y2 = YLength,
        });

        // LEFT
        l.Add(new Line()
        {
            Stroke = Brushes.Black,
            StrokeThickness = BORDERTHICKNESS,
            X1 = MARGIN,
            Y1 = MARGIN,
            X2 = MARGIN,
            Y2 = YLength,
        });

        // RIGHT
        l.Add(new Line()
        {
            Stroke = Brushes.Black,
            StrokeThickness = BORDERTHICKNESS,
            X1 = XLength,
            Y1 = MARGIN,
            X2 = XLength,
            Y2 = YLength,
        });

        l.ForEach(x => c.Children.Add(x));
    }

    /// <summary>
    /// Draws the grid
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawGrid(Canvas c)
    {
        for (var i = 0; i <= _viewModel!.Width; i++)
        {
            c.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                X1 = MARGIN + i * Step,
                Y1 = MARGIN,
                X2 = MARGIN + i * Step,
                Y2 = YLength,
            });
        }

        for (var i = 0; i <= _viewModel.Height; i++)
        {
            c.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                X1 = MARGIN,
                Y1 = MARGIN + i * Step,
                X2 = XLength,
                Y2 = MARGIN + i * Step,
            });
        }
    }

    /// <summary>
    /// Draws the robots and their targets (if they have one) to the canvas
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawRobots(Canvas c)
    {
        var fontSize = 14 * Math.Sqrt(_viewModel!.Zoom);

        foreach (var r in _viewModel.Robots)
        {
            Thickness t;

            t.Left = MARGIN + 1 + r.Position.X * Step;
            t.Top = MARGIN + 1 + r.Position.Y * Step;

            var grid = new Grid
            {
                Width = Step - 2,
                Height = Step - 2,
                Margin = t
            };

            grid.Children.Add(new Ellipse()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = new SolidColorBrush(Color.FromRgb(9, 194, 248)), // this is the color in the example
                Width = Step - 2,
                Height = Step - 2
            });

            grid.Children.Add(new TextBlock()
            {
                Text = r.ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontSize
            });

            c.Children.Add(grid);

            if (r.Task is null) continue;

            t.Left = MARGIN + r.Task.Position.X * Step;
            t.Top = MARGIN + r.Task.Position.Y * Step;

            grid = new Grid
            {
                Width = Step,
                Height = Step,
                Margin = t
            };

            grid.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 0,
                Fill = Brushes.Orange,
                Width = Step,
                Height = Step
            });

            grid.Children.Add(new TextBlock()
            {
                Text = r.ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontSize
            });

            c.Children.Add(grid);
        }
    }

    /// <summary>
    /// Draws the walls to the canvas
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawWalls(Canvas c)
    {
        foreach (var w in _viewModel!.Walls)
        {
            Thickness t;

            t.Left = MARGIN + w.Position.X * Step;
            t.Top = MARGIN + w.Position.Y * Step;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Black,
                Width = Step,
                Height = Step,
                Margin = t
            });
        }
    }

    #endregion

    /// <summary>
    /// Redraws the canvas when the zoom property changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Zoom":
                    Calculate(_replayWindow?.MapCanvas ?? _simWindow?.MapCanvas ?? throw new System.Exception());
                    Redraw(_replayWindow?.MapCanvas ?? _simWindow!.MapCanvas);                
                break;
        }
    }
}
