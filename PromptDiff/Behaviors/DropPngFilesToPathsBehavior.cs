using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using PromptDiff.ViewModels;

namespace PromptDiff.Behaviors
{
    public class DropPngFilesToPathsBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject == null)
            {
                return;
            }

            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DragOver -= OnDragOver;
                AssociatedObject.Drop -= OnDrop;
            }

            base.OnDetaching();
        }

        private static bool HasPngFiles(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return false;
            }

            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0)
            {
                return false;
            }

            return files.Any(f => string.Equals(Path.GetExtension(f), ".png", StringComparison.OrdinalIgnoreCase));
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (!HasPngFiles(e))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files == null || files.Length == 0)
                {
                    return;
                }

                var viewModel = AssociatedObject?.DataContext as MainWindowViewModel;
                if (viewModel == null)
                {
                    return;
                }

                var pngs = files
                    .Where(f => !string.IsNullOrWhiteSpace(f))
                    .Where(f => string.Equals(Path.GetExtension(f), ".png", StringComparison.OrdinalIgnoreCase))
                    .Select(Path.GetFullPath)
                    .ToList();

                foreach (var path in pngs)
                {
                    viewModel.Paths.Add(path);
                }

                e.Handled = true;
            }
            catch
            {
                // ignore unexpected errors to avoid crashing on a drop
                e.Handled = true;
            }
        }
    }
}