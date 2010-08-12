using Rhino.Licensing.AdminTool.Model;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.Models
{
    public class ProductModelTests
    {
        [Fact]
        public void Product_Properties_Fires_PropertyChanged()
        {
            var product = new Product();

            product.AssertThatAllProperties()
                   .RaiseChangeNotification();
        }
    }
}