using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StoryTimeFramework.Utils;

namespace StoryTimeFramework.Resources.Loaders
{
    public class ComponentLoader
    {
        private List<Assembly> _contentAssemblies;
 
        public ComponentLoader()
        {
            _contentAssemblies = ApplicationUtils.GetAllAssemblyFiles().Select(af => Assembly.LoadFrom(af.FullName)).ToList();
        }

        public IEnumerable<Type> LoadTypesAssignableFrom<T>(bool allowAbstractNotInstantiable = false)
        {
            var types =
                AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t =>
                        typeof (T).IsAssignableFrom(t) &&
                        !t.ContainsGenericParameters);

            if (!allowAbstractNotInstantiable)
            {
                types = types.Where(t => !t.IsAbstract && !t.IsInterface);
            }

            return types;
        }
    }
}
