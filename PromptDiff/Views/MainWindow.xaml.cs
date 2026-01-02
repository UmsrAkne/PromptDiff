using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using PromptDiff.ViewModels;

namespace PromptDiff.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ListBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Delete)
        {
            return;
        }

        if (sender is not ListBox listBox)
        {
            return;
        }

        // Get the view model bound to this window
        if (DataContext is not MainWindowViewModel vm)
        {
            return;
        }

        // Collect selected items to remove (avoid modifying a collection while enumerating)
        var toRemove = listBox.SelectedItems.Cast<string>().ToList();
        if (toRemove.Count == 0)
        {
            return;
        }

        foreach (var item in toRemove)
        {
            vm.Paths.Remove(item);
        }

        e.Handled = true;
    }
}