using Prism.Mvvm;
using PromptDiff.Utils;

namespace PromptDiff.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new ();

    public string Title => appVersionInfo.Title;
}