using System;
using System.Collections.Generic;
using System.Linq;
using JMMPlugins.Properties;

namespace JMMPlugins
{
    public partial class JMMPluginService
    {
        private static PluginFactory plugins = PluginFactory.Instance;
        static JMMPluginService()
        {
            CheckPreferences();
            //News loading then
        }
        private static Dictionary<JMMPluginTypes, Dictionary<Guid, int>> pluginorder = new Dictionary<JMMPluginTypes, Dictionary<Guid, int>>();


        private static void CheckPreferences()
        {
            foreach (IJMMPluginCommon j in plugins.List)
            {
                JMMPluginPreferences pp = Settings.Default.Preferences.FirstOrDefault(a => a.Guid == j.Guid);
                if (pp == null)
                {
                    pp = new JMMPluginPreferences();
                    pp.Guid = j.Guid;
                    pp.Orders = new Dictionary<JMMPluginTypes, int>();
                    pp.Preferences = j.GetPreferencesDescriptions();
                    pp.Valid = false;
                    pp.Enabled = true;
                    Settings.Default.Preferences.Add(pp);
                }
                foreach (JMMPluginTypes p in EnumerateTypes(j.SupportedPluginTypes))
                    AddOrderToPreferences(j.Guid, pp, p);
            }
            foreach (JMMPluginTypes p in Enum.GetValues(typeof(JMMPluginTypes)))
            {
                List<Guid> order = pluginorder[p].OrderBy(a => a.Value).Select(a => a.Key).ToList();
                int x = 0;
                foreach (Guid g in order)
                {
                    pluginorder[p][g] = x;
                    x++;
                }
            }
            foreach (IJMMPluginCommon j in plugins.List)
            {
                JMMPluginPreferences pp = Settings.Default.Preferences.First(a => a.Guid == j.Guid);
                foreach (JMMPluginTypes p in EnumerateTypes(j.SupportedPluginTypes))
                {
                    int val = pluginorder[p][j.Guid];
                    if (!pp.Orders.ContainsKey(p))
                        pp.Orders.Add(p, val);
                    else
                        pp.Orders[p] = val;
                }
                if (pp.Valid)
                {
                    List<JMMPluginPreferenceValue> v = pp.Preferences.Cast<JMMPluginPreferenceValue>().ToList();
                    pp.Valid = j.SetPreferences(v).Ok;
                }
                Settings.Default.Save();

            }
            Settings.Default.Save();
        }

        private static void AddOrderToPreferences(Guid guid, JMMPluginPreferences pp, JMMPluginTypes p)
        {
            int val = Int32.MaxValue;
            if (pp.Orders.ContainsKey(p))
                val = pp.Orders[p];
            if (!pluginorder.ContainsKey(p))
                pluginorder.Add(p, new Dictionary<Guid, int>());
            Dictionary<Guid, int> kk = pluginorder[p];
            if (!kk.ContainsKey(guid))
                kk.Add(guid, val);
            else
                kk[guid] = val;
        }
        private static List<JMMPluginTypes> EnumerateTypes(JMMPluginTypes type)
        {
            List<JMMPluginTypes> l = new List<JMMPluginTypes>();
            foreach (JMMPluginTypes p in Enum.GetValues(typeof(JMMPluginTypes)))
            {
                if ((type & p) == p)
                    l.Add(p);
            }
            return l;
        }
    }
}
