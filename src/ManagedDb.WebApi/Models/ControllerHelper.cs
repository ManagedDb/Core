using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ManagedDb.WebApi.Models
{
    public static class ControllerHelper
    {
        public static IEdmModel RegisterODataEntities(Type[] modelTypes) 
        {
            // odata start
            var mb = new ODataConventionModelBuilder();

            foreach (var type in modelTypes)
            {
                mb.AddEntitySet(
                    type.Name,
                    mb.AddEntityType(type));
            }

            return mb.GetEdmModel();
        }
    }
}
