namespace Mekkdonalds;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainWindow? _mainWindow;
    public App()
    {
        Startup += OnStartup;
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        _mainWindow = new MainWindow();

        _mainWindow.Show();
    }
}
