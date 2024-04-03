using Microsoft.Win32;

using System.Collections;
using System.ComponentModel;

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

using Brushes = System.Drawing.Brushes;
using Point = System.Windows.Point;

namespace Mekkdonalds;

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

        if (fd.ShowDialog() is false)
            return false;

        throw new NotImplementedException("Replay is not implemented yet");
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

        _viewModel.Loaded += (_, _) => Dispatcher.Invoke(() =>
        {
            Calculate(_simWindow.MapCanvas);
            DrawGrid(_simWindow.MapCanvas);
            InitRobots(_simWindow.MapCanvas);
            Redraw();
            _simWindow.Cursor = Cursors.Arrow;
        });

        _viewModel.Tick += (_, _) => Dispatcher.Invoke(() => Redraw());
        _viewModel.PropertyChanged += OnPropertyChanged;

        _simWindow.SizeChanged += (_, _) => { Calculate(_simWindow.MapCanvas); Redraw(); };

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

        c.Width = XLength = w * Step;
        c.Height = YLength = h * Step;

        var fontSize = 12 * Math.Sqrt(_viewModel.Zoom);

        TextBlock? t;

        foreach (var g in _robots.Values)
        {
            g.Width = g.Height = Step - 2;
            t = g.Children[0] as TextBlock ?? throw new System.Exception();
            t.FontSize = fontSize;
        }

        foreach (var g in _targets.Values)
        {
            g.Width = g.Height = Step;
            t = g.Children[0] as TextBlock ?? throw new System.Exception();
            t.FontSize = fontSize;
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

    ///// <summary>
    ///// Draws the robots and their targets (if they have one) to the canvas
    ///// </summary>
    ///// <param name="c">The currently open window's canvas</param>
    //private void DrawRobots(Canvas c)
    //{
    //    var fontSize = 12 * Math.Sqrt(_viewModel!.Zoom);

    //    foreach (var r in _viewModel.Robots)
    //    {
    //        Thickness t;

    //        t.Left = MARGIN + 1 + r.Position.X * Step;
    //        t.Top = MARGIN + 1 + r.Position.Y * Step;

    //        var grid = new Grid
    //        {
    //            Width = Step - 2,
    //            Height = Step - 2,
    //            Margin = t,
    //            Background = _ellipse
    //        };

    //        grid.Children.Add(new TextBlock()
    //        {
    //            Text = r.ID.ToString(),
    //            HorizontalAlignment = HorizontalAlignment.Center,
    //            VerticalAlignment = VerticalAlignment.Center,
    //            FontSize = fontSize
    //        });

    //        c.Children.Add(grid);

    //        switch (r.Direction)
    //        {
    //            case Direction.North:
    //                c.Children.Add(new Line()
    //                {
    //                    Stroke = System.Windows.Media.Brushes.Red,
    //                    StrokeThickness = 3,
    //                    X1 = MARGIN + r.Position.X * Step + Step / 2,
    //                    Y1 = MARGIN + r.Position.Y * Step + 1,
    //                    X2 = MARGIN + r.Position.X * Step + Step / 2,
    //                    Y2 = MARGIN + r.Position.Y * Step + 1 + 7 * _viewModel.Zoom
    //                });
    //                break;
    //            case Direction.East:
    //                c.Children.Add(new Line()
    //                {
    //                    Stroke = System.Windows.Media.Brushes.Red,
    //                    StrokeThickness = 3,
    //                    X1 = MARGIN + (r.Position.X + 1) * Step - 1 - 7 * _viewModel.Zoom,
    //                    Y1 = MARGIN + r.Position.Y * Step + Step / 2,
    //                    X2 = MARGIN + (r.Position.X + 1) * Step - 1,
    //                    Y2 = MARGIN + r.Position.Y * Step + Step / 2
    //                });
    //                break;
    //            case Direction.South:
    //                c.Children.Add(new Line()
    //                {
    //                    Stroke = System.Windows.Media.Brushes.Red,
    //                    StrokeThickness = 3,
    //                    X1 = MARGIN + r.Position.X * Step + Step / 2,
    //                    Y1 = MARGIN + (r.Position.Y + 1) * Step - 1 - 7 * _viewModel.Zoom,
    //                    X2 = MARGIN + r.Position.X * Step + Step / 2,
    //                    Y2 = MARGIN + (r.Position.Y + 1) * Step - 1
    //                });
    //                break;
    //            case Direction.West:
    //                c.Children.Add(new Line()
    //                {
    //                    Stroke = System.Windows.Media.Brushes.Red,
    //                    StrokeThickness = 3,
    //                    X1 = MARGIN + r.Position.X * Step + 1 + 7 * _viewModel.Zoom,
    //                    Y1 = MARGIN + r.Position.Y * Step + Step / 2,
    //                    X2 = MARGIN + r.Position.X * Step + 1,
    //                    Y2 = MARGIN + r.Position.Y * Step + Step / 2
    //                });
    //                break;
    //            default:
    //                throw new System.Exception();
    //        }

    //        if (r.Task is null) continue;

    //        t.Left = MARGIN + r.Task.Position.X * Step;
    //        t.Top = MARGIN + r.Task.Position.Y * Step;

    //        grid = new Grid
    //        {
    //            Width = Step,
    //            Height = Step,
    //            Margin = t,
    //            Background = _rectangel
    //        };

    //        grid.Children.Add(new TextBlock()
    //        {
    //            Text = r.ID.ToString(),
    //            HorizontalAlignment = HorizontalAlignment.Center,
    //            VerticalAlignment = VerticalAlignment.Center,
    //            FontSize = fontSize
    //        });

    //        c.Children.Add(grid);
    //    }
    //}

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
            var bm = new Bitmap(500, 500);
            using var g = Graphics.FromImage(bm);
            
            g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(9, 194, 248)), 0, 250, 500, 250);
            g.FillPie(new SolidBrush(System.Drawing.Color.FromArgb(9, 194, 248)), 0, 0, 500, 500, 180, 180);
            g.FillEllipse(Brushes.Black, 150, 80, 75, 75);
            g.FillEllipse(Brushes.Black, 275, 80, 75, 75);

            for (int i = 0; i < 4; i++)
            {
                using var memory = new MemoryStream();
                bm.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var r = new BitmapImage();
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
                Redraw();
                break;
        }
    }

    private static void RotateBitmap(ref Bitmap bmp, float angle)
    {
        Bitmap tempBmp = new(bmp.Width, bmp.Height);
        using var g = Graphics.FromImage(tempBmp);
        
        g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
        g.RotateTransform(angle);
        g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
        g.DrawImage(bmp, new PointF(0, 0));        

        bmp.Dispose(); //dispose old bitmap
        bmp = tempBmp; //assign new bitmap to reference
    }
}
