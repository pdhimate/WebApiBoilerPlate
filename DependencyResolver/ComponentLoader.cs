using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver
{
    /// <summary>
    /// Loads the reigistered types/components present in the assemblies present in the specified directory.
    /// </summary>
    public static class ComponentLoader
    {
        public static void LoadContainer(IUnityContainer unityContainer, string directoryPath, string fileNamePattern)
        {
            var directoryCatalog = new DirectoryCatalog(directoryPath, fileNamePattern);
            var importDefinition = BuildImportDefinition();
            try
            {
                using (var aggregateCatalog = new AggregateCatalog())
                {
                    aggregateCatalog.Catalogs.Add(directoryCatalog);

                    using (var componsitionContainer = new CompositionContainer(aggregateCatalog))
                    {
                        IEnumerable<Export> exports = componsitionContainer.GetExports(importDefinition);
                        IEnumerable<IComponent> modules = exports.Select(export => export.Value as IComponent).Where(m => m != null);

                        var registerComponent = new RegisterComponent(unityContainer);
                        foreach (IComponent module in modules)
                        {
                            module.Setup(registerComponent);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException typeLoadException)
            {
                var builder = new StringBuilder();
                foreach (Exception loaderException in typeLoadException.LoaderExceptions)
                {
                    builder.AppendFormat("{0}\n", loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(
                exportDefinition => true,       // No constraints on Export
                typeof(IComponent).FullName,    // Contract to be imported
                ImportCardinality.ZeroOrMore,   // 0+ objects can be imported
                false,
                false);
        }
    }

}
