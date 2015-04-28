using System;
using System.Collections.Generic;
using System.Linq;
using JMMPlugins.Properties;

namespace JMMPlugins
{
    public partial class JMMPluginService
    {

        public JMMPluginResult<List<JMMPlugin>> GetPlugins()
        {
            JMMPluginResult<List<JMMPlugin>> result = new JMMPluginResult<List<JMMPlugin>>();
            result.Result = new List<JMMPlugin>();
            try
            {
                foreach (IJMMPluginCommon p in plugins.List)
                {
                    JMMPlugin j = new JMMPlugin();
                    j.Guid = p.Guid;
                    j.Name = p.Name;
                    j.SupportedPluginTypes = p.SupportedPluginTypes;
                    j.SupportedDownloadTypes = p.SupportedDownloadTypes;
                    JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == p.Guid);
                    j.PluginPreferencesDescriptions = prefs.Preferences;
                    j.PreferencesValid = prefs.Valid;
                    result.Result.Add(j);
                }
                result.Ok = true;
            }
            catch (Exception e)
            {
                result.Result = null;
                result.Error = e.ToString();
                result.Ok = false;
            }
            return result;
        }
        public JMMPluginPreferenceResult SetPreferences(Guid pluginguid, List<JMMPluginPreferenceValue> values, bool enabled)
        {
            JMMPluginPreferenceResult res = plugins.List.First(a => a.Guid == pluginguid).SetPreferences(values);
            if (res.Ok)
            {
                JMMPluginPreferences ll = Settings.Default.Preferences.First(a => a.Guid == pluginguid);
                foreach (JMMPluginPreferenceValue v in values)
                {
                    JMMPluginPreferenceDescription pd = ll.Preferences.First(a => a.Name == v.Name);
                    pd.Value = v.Value;
                }
                ll.Valid = true;
                ll.Enabled = enabled;
                Settings.Default.Save();
            }
            return res;

        }
        public JMMPluginResult<List<Guid>> GetPluginOrder(JMMPluginTypes type)
        {
            if (!pluginorder.ContainsKey(type))
                return new JMMPluginResult<List<Guid>> { Error = "There is no plugin of type " + type + " loaded", Ok = false };
            return new JMMPluginResult<List<Guid>> { Ok = true, Result = pluginorder[type].OrderBy(a => a.Value).Select(a => a.Key).ToList() };
        }

        public JMMPluginResult SetPluginOrder(List<Guid> order, JMMPluginTypes type)
        {
            if (!pluginorder.ContainsKey(type))
                return new JMMPluginResult { Error = "There is no plugin of type " + type + " loaded", Ok = false };
            Dictionary<Guid, int> d = pluginorder[type];
            int xx = 0;
            foreach (Guid g in order)
            {
                d[g] = xx;
                JMMPluginPreferences ll = Settings.Default.Preferences.First(a => a.Guid == g);
                if (!ll.Orders.ContainsKey(type))
                    ll.Orders.Add(type, xx);
                else
                    ll.Orders[type] = xx;
                xx++;
            }
            Settings.Default.Save();
            return new JMMPluginResult() { Ok = true };
        }
    }
}
