using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace JMMPlugins
{
    internal class PluginFactory
    {
        private static PluginFactory _instance;
        public static PluginFactory Instance { get { return _instance ?? (_instance = new PluginFactory()); } }
            
        [ImportMany(typeof(IJMMPluginCommon))]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IEnumerable<IJMMPluginCommon> List { get; set; }
        protected PluginFactory()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string dirname = Path.GetDirectoryName(assembly.GetName().CodeBase);
            AggregateCatalog catalog = new AggregateCatalog();
            if (!string.IsNullOrEmpty(dirname))
            {
                if (dirname.StartsWith(@"file:\"))
                    dirname = dirname.Substring(6);
                string pluginname = Path.Combine(dirname, "plugins");
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                catalog.Catalogs.Add(new DirectoryCatalog(dirname));
                if (Directory.Exists(pluginname))
                    catalog.Catalogs.Add(new DirectoryCatalog(pluginname));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
