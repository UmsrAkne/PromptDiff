using System;
using System.IO;
using System.Windows;
using Prism.Ioc;
using PromptDiff.Views;

namespace PromptDiff;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Ensure a folder named "local_data" exists in the executable directory at startup
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var localDataPath = Path.Combine(baseDir, "local_data");
            Directory.CreateDirectory(localDataPath);
        }
        catch
        {
            // Swallow exceptions to avoid blocking app startup; optionally log here in the future.
        }
        finally
        {
            base.OnStartup(e);
        }
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }
}