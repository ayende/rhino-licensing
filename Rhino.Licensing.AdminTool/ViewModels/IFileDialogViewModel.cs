using System.Collections.Generic;
using Caliburn.PresentationFramework.Screens;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public interface IFileDialogViewModel : IScreen
    {
        bool AddExtension { get; set; }
        bool CheckFileExists { get; set; }
        bool CheckPathExists { get; set; }
        string DefaultExtension { get; set; }
        string FileName { get; set; }
        string Filter { get; set; }
        string InitialDirectory { get; set; }
        string Title { get; set; }
        IEnumerable<string> FileNames { get; set; }
        bool? Result { get; set; }
    }
}