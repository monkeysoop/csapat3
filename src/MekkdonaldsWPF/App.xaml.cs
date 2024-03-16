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

        _replayWindow.SizeChanged += (_, _) => { Calculate(_replayWindow.MapCanvas); Redraw(_replayWindow.MapCanvas); };

        foreach (var r in _viewModel.Robots)
        {
            r.Assign(r.Position.X + 3, r.Position.Y + 4);
        }

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

        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw(_simWindow.MapCanvas)); // UI elemts have to be updated with this call when it is called from another thread

        _simWindow.SizeChanged += (_, _) => { Calculate(_simWindow.MapCanvas); Redraw(_simWindow.MapCanvas); };

        foreach (var r in _viewModel.Robots)
        {
            r.Assign(r.Position.X + 3, r.Position.Y + 4);
        }

        _simWindow.Show();

        Calculate(_simWindow.MapCanvas);
        Redraw(_simWindow.MapCanvas);

        return true;
    }

    /// <summary>
    /// Calculates the dimensions required to draw the grid
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Calculate(Canvas c)
    {
        var (w, h) = _viewModel!.Size;

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
        DrawRobots(c);
        DrawWalls(c);
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
        for (var i = 0; i <= _viewModel!.Size.W; i++)
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

        for (var i = 0; i <= _viewModel!.Size.H; i++)
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
        foreach (var r in _viewModel!.Robots)
        {
            Thickness t;

            t.Left = MARGIN + 2 + r.Position.X * Step;
            t.Top = MARGIN + 2 + r.Position.Y * Step;

            c.Children.Add(new Ellipse()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Blue,
                Width = Step - 4,
                Height = Step - 4,
                Margin = t
            });

            if (r.Task is null) continue;

            t.Left = MARGIN + r.Task.Position.X * Step;
            t.Top = MARGIN + r.Task.Position.Y * Step;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 0,
                Fill = Brushes.Orange,
                Width = Step,
                Height = Step,
                Margin = t
            });
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
}
