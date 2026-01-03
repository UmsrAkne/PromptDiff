using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public StepVariantRequest StepVariantRequest { get; set; } = new ()
    {
        StartOffset = 0,
        Count = 5,
    };

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
            var results = StepDiffGenerator.GenerateStepVariants(new StepVariantRequest
            {
                RequestLine = requestLine,
                StartOffset = StepVariantRequest.StartOffset,
                Count = StepVariantRequest.Count,
            });
            requests.AddRange(results);
        }

        var requestText = string.Join(Environment.NewLine, requests);
        Clipboard.SetText(requestText);

        LogWriter.Write(requestText);
    });
}