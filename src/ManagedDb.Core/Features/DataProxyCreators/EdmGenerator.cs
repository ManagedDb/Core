
using Microsoft.OData.Edm;
using System.Reflection;
using System.Linq;
using Microsoft.OData.ModelBuilder;
using System.CodeDom;

namespace ManagedDb.Core.Features.DataProxyCreators;

public static class EdmGenerator
{
    public static IEdmModel GetEdmModel() 
    {
        var modelTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && t.IsSubclassOf(typeof(MdbBaseEntity)));

        // odata start
        var mb = new ODataConventionModelBuilder();
        
        foreach(var modelType in modelTypes)
        {
            mb.AddEntitySet(
                modelType.Name, 
                mb.AddEntityType(modelType));
        }

        return mb.GetEdmModel();
    }
}
