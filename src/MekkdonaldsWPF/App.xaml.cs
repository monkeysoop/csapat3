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
    private ViewModel.ViewModel? _viewModel;

    public App()
    {
        Startup += OnStartup;
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        _viewModel = new SimulationViewModel();


        _simWindow = new SimulationWindow
        {
            WindowState = WindowState.Maximized,
            DataContext = _viewModel
        };

        _viewModel.Tick += (_, _) => Redraw(_simWindow.MapCanvas);

        _simWindow.SizeChanged += (_, _) => Redraw(_simWindow.MapCanvas);

        foreach (var r in _viewModel.Robots)
        {
            r.Assign(r.Position.X + 3, r.Position.Y + 4);
        }

        _simWindow.Show();
        Redraw(_simWindow.MapCanvas);
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

        XStep = (XLength - MARGIN) / _viewModel!.Size.W;
        YStep = (YLength - MARGIN) / _viewModel!.Size.H;

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
