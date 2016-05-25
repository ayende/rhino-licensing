using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public interface ILicenseHolder
    {
        License CurrentLicense { get; set; } 
    }
}