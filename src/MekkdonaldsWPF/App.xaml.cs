namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const int MARGIN = 20;
    private const int BORDERTHICKNESS = 4;
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

        _mainWindow.Show();
        Draw(_mainWindow.MapCanvas);
    }

    /// <summary>
    /// Clears the canvas and redraws every element
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private void Redraw(Canvas c)
    {
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
    }

    /// <summary>
    /// Draws the frame of the map
    /// </summary>
    /// <param name="c">The currently open window's canvas</param>
    private static void DrawFrame(Canvas c)
    {
        var length = Math.Min(c.ActualHeight, c.ActualWidth) - MARGIN;

        List<Line> l = [];

        l.Add(
            new Line() // top
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN - 2,
                Y1 = MARGIN,
                X2 = length + 2,
                Y2 = MARGIN,
            }
        );

        l.Add(
            new Line() // bottom
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN - 2,
                Y1 = length,
                X2 = length + 2,
                Y2 = length,
            }
         );

        l.Add(
            new Line() // left
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = MARGIN,
                Y1 = MARGIN,
                X2 = MARGIN,
                Y2 = length,
            }
        );

        l.Add(
            new Line() // right
            {
                Stroke = Brushes.Black,
                StrokeThickness = BORDERTHICKNESS,
                X1 = length,
                Y1 = MARGIN,
                X2 = length,
                Y2 = length,
            }
        );

        l.ForEach(x => c.Children.Add(x));
    }

    private static void DrawRobots(List<Robot> r)
    {

    }
}
