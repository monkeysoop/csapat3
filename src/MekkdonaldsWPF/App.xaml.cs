
using Microsoft.Win32;

namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const int MARGIN = 20;
    private const int BORDERTHICKNESS = 4;
    private const int SIDELENGTH = 20;

    private double XLength;
    private double YLength;

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

    private void DisposeStartWindow()
    {
        _startWindow!.Close(); // can't be null
        _startWindow.SimButton.Click -= SimButton_Click;
        _startWindow.ReplayButton.Click -= ReplayButton_Click;
        _startWindow = null;
    }

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

    private void Calculate(Canvas c)
    {
        var (w, h) = _viewModel!.Size;

        XLength = (w + 1) * SIDELENGTH;
        YLength = (h + 1) * SIDELENGTH;

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

    private void DrawGrid(Canvas c)
    {
        for (var i = 0; i <= _viewModel!.Size.W; i++)
        {
            c.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                X1 = MARGIN + i * SIDELENGTH,
                Y1 = MARGIN,
                X2 = MARGIN + i * SIDELENGTH,
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
                Y1 = MARGIN + i * SIDELENGTH,
                X2 = XLength,
                Y2 = MARGIN + i * SIDELENGTH,
            });
        }
    }

    private void DrawRobots(Canvas c)
    {
        foreach (var r in _viewModel!.Robots)
        {
            Thickness t;

            t.Left = MARGIN + 2 + r.Position.X * SIDELENGTH;
            t.Top = MARGIN + 2 + r.Position.Y * SIDELENGTH;

            c.Children.Add(new Ellipse()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Blue,
                Width = SIDELENGTH - 4,
                Height = SIDELENGTH - 4,
                Margin = t
            });

            if (r.Task is null) continue;

            t.Left = MARGIN + r.Task.Position.X * SIDELENGTH;
            t.Top = MARGIN + r.Task.Position.Y * SIDELENGTH;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 0,
                Fill = Brushes.Orange,
                Width = SIDELENGTH,
                Height = SIDELENGTH,
                Margin = t
            });
        }
    }

    private void DrawWalls(Canvas c)
    {
        foreach (var w in _viewModel!.Walls)
        {
            Thickness t;

            t.Left = MARGIN + w.Position.X * SIDELENGTH;
            t.Top = MARGIN + w.Position.Y * SIDELENGTH;

            c.Children.Add(new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Black,
                Width = SIDELENGTH,
                Height = SIDELENGTH,
                Margin = t
            });
        }
    }
}
