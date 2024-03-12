
namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const int MARGIN = 20;
    private const int BORDERTHICKNESS = 4;

    private double XLength;
    private double YLength;
    private double XStep;
    private double YStep;

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
        OpenReplay();
        Current.MainWindow = _replayWindow;
        DisposeStartWindow();
    }

    private void SimButton_Click(object sender, RoutedEventArgs e)
    {
        OpenSim();
        Current.MainWindow = _simWindow;
        DisposeStartWindow();
    }

    private void DisposeStartWindow()
    {
        _startWindow!.Close(); // can't be null
        _startWindow.SimButton.Click -= SimButton_Click;
        _startWindow.ReplayButton.Click -= ReplayButton_Click;
        _startWindow = null;
    }

    private void OpenReplay()
    {
        _viewModel = new ReplayViewModel();

        _replayWindow = new ReplayWindow
        {
            WindowState = WindowState.Maximized,
            DataContext = _viewModel
        };

        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw(_replayWindow.MapCanvas)); // UI elemts have to be updated with this call when it is called from another thread

        _replayWindow.SizeChanged += (_, _) => { Calculate(_replayWindow.MapCanvas.ActualWidth, _replayWindow.MapCanvas.ActualHeight); Redraw(_replayWindow.MapCanvas); };

        foreach (var r in _viewModel.Robots)
        {
            r.Assign(r.Position.X + 3, r.Position.Y + 4);
        }

        _replayWindow.Show();

        Calculate(_replayWindow.MapCanvas.ActualWidth, _replayWindow.MapCanvas.ActualHeight);
        Redraw(_replayWindow.MapCanvas);
    }

    private void OpenSim()
    {
        _viewModel = new SimulationViewModel();


        _simWindow = new SimulationWindow
        {
            WindowState = WindowState.Maximized,
            DataContext = _viewModel
        };

        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw(_simWindow.MapCanvas)); // UI elemts have to be updated with this call when it is called from another thread

        _simWindow.SizeChanged += (_, _) => { Calculate(_simWindow.MapCanvas.ActualWidth, _simWindow.MapCanvas.ActualHeight); Redraw(_simWindow.MapCanvas); };

        foreach (var r in _viewModel.Robots)
        {
            r.Assign(r.Position.X + 3, r.Position.Y + 4);
        }

        _simWindow.Show();

        Calculate(_simWindow.MapCanvas.ActualWidth, _simWindow.MapCanvas.ActualHeight);
        Redraw(_simWindow.MapCanvas);
    }

    private void Calculate(double width, double height)
    {
        var (w, h) = _viewModel!.Size;

        if (w > h)
        {
            XLength = width - 2 * MARGIN;
            YLength = XLength * h / w;
        }
        else if (w < h)
        {
            YLength = height - 2 * MARGIN;
            XLength = YLength * w / h;
        }
        else
        {
            XLength = YLength = Math.Min(height, width) - 2 * MARGIN;
        }

        XStep = (XLength - MARGIN) / _viewModel!.Size.W;
        YStep = (YLength - MARGIN) / _viewModel!.Size.H;
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

    private void DrawGrid(Canvas c)
    {
        for (var i = 0; i <= _viewModel!.Size.W; i++)
        {
            c.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                X1 = MARGIN + i * XStep,
                Y1 = MARGIN,
                X2 = MARGIN + i * XStep,
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
                Y1 = MARGIN + i * YStep,
                X2 = XLength,
                Y2 = MARGIN + i * YStep,
            });
        }
    }

    private void DrawRobots(Canvas c)
    {        
        foreach (var r in _viewModel!.Robots)
        {
            Thickness t;

            t.Left = MARGIN + 2 + r.Position.X * XStep;
            t.Top = MARGIN + 2 + r.Position.Y * YStep;

            c.Children.Add(new Ellipse()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Blue,
                Width = XStep - 4,
                Height = YStep - 4,
                Margin = t
            });

            if (r.Task is null) continue;

            t.Left = MARGIN + r.Task.Position.X * XStep;
            t.Top = MARGIN + r.Task.Position.Y * YStep;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 0,
                Fill = Brushes.Orange,
                Width = XStep,
                Height = YStep,
                Margin = t
            });
        }
    }

    private void DrawWalls(Canvas c)
    {
        foreach (var w in _viewModel!.Walls)
        {
            Thickness t;

            t.Left = MARGIN + w.Position.X * XStep;
            t.Top = MARGIN + w.Position.Y * YStep;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Black,
                Width = XStep,
                Height = YStep,
                Margin = t
            });
        }
    }
}
