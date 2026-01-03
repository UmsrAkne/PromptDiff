using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using PromptDiff.Core;
using PromptDiff.Utils;

namespace PromptDiff.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new ();

    public string Title => appVersionInfo.Title;

    public ObservableCollection<string> Paths { get; } = new ();

    public DelegateCommand CopyRequestToClipboardCommand => new (() =>
    {
        if (Paths.Count == 0)
        {
            return;
        }

        var requests = new List<string>();

        foreach (var path in Paths)
        {
            var rawMetadata = PngMetadataReader.ReadPngTextMetadata(path);
            var requestLine = PromptRequestFormatter.ConvertToPromptRequest(rawMetadata);
            var results = StepDiffGenerator.GenerateStepVariants(requestLine, 0, 5);
            requests.AddRange(results);
        }

        var requestText = string.Join(Environment.NewLine, requests);
        Clipboard.SetText(requestText);

        LogWriter.Write(requestText);
    });
}