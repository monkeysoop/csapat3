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

    private MainWindow? _mainWindow;
    private SimViewModel? _viewModel;

    public App()
    {
        Startup += OnStartup;
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        _mainWindow = new MainWindow
        {
            WindowState = WindowState.Maximized
        };

        _mainWindow.SizeChanged += (_, _) => Redraw(_mainWindow.MapCanvas);

        _viewModel = new MainWindowViewModel();

        _mainWindow.Show();
        Redraw(_mainWindow.MapCanvas); // calling redraw to calculate XLength and YLength
    }

    /// <summary>
    /// Clears the canvas and redraws every element
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Redraw(Canvas c)
    {
        var (w, h) = _viewModel!.Size;

        if (w > h)
        {
            XLength = c.ActualWidth - 2 * MARGIN;
            YLength = XLength * h / w;
        }
        else if (w < h)
        {
            YLength = c.ActualHeight - 2 * MARGIN;
            XLength = YLength * w / h;
        }
        else
        {
            XLength = YLength = Math.Min(c.ActualHeight, c.ActualWidth) - 2 * MARGIN;
        }

        c.Children.Clear();
        Draw(c);
    }

    /// <summary>
    /// Handels drawing
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Draw(Canvas c)
    {
        DrawFrame(c);
        DrawGrid(c);
    }

    /// <summary>
    /// Draws the frame of the map
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void DrawFrame(Canvas c)
    {
        List<Line> l = [];

        // TOP
        l.Add(
            new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN - 2,
                Y1 = MARGIN,
                X2 = XLength + 2,
                Y2 = MARGIN,
            }
        );

        // BOTTOM
        l.Add(
            new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN - 2,
                Y1 = YLength,
                X2 = XLength + 2,
                Y2 = YLength,
            }
         );

        // LEFT
        l.Add(
            new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN,
                Y1 = MARGIN,
                X2 = MARGIN,
                Y2 = YLength,
            }
        );

        // RIGHT
        l.Add(
            new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = XLength,
                Y1 = MARGIN,
                X2 = XLength,
                Y2 = YLength,
            }
        );

        l.ForEach(x => c.Children.Add(x));
    }


    private void DrawGrid(Canvas c)
    {
        var xStep = (XLength - MARGIN) / _viewModel!.Size.W;
        var ystep = (YLength - MARGIN) / _viewModel!.Size.H;

        for (var i = 1; i < _viewModel!.Size.W; i++)
        {
            c.Children.Add(
                new Line()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    X1 = MARGIN + i * xStep,
                    Y1 = MARGIN,
                    X2 = MARGIN + i * xStep,
                    Y2 = YLength,
                }
            );
        }

        for (var i = 1; i < _viewModel!.Size.H; i++)
        {
            c.Children.Add(
                new Line()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    X1 = MARGIN,
                    Y1 = MARGIN + i * ystep,
                    X2 = XLength,
                    Y2 = MARGIN + i * ystep,
                }
            );
        }
    }

    private void DrawRobots()
    {
        _ = _viewModel!.Robots[0];
        throw new NotImplementedException();
    }
}
