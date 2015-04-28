using System;
using System.Collections.Generic;
using System.Linq;
using JMMPlugins.Properties;

namespace JMMPlugins
{
    public partial class JMMPluginService
    {
        public JMMPluginResult<List<JMMPluginDownloadLink>> Search(string text)
        {
            JMMPluginResult<List<JMMPluginDownloadLink>> res = new JMMPluginResult<List<JMMPluginDownloadLink>>();
            res.Result = new List<JMMPluginDownloadLink>();
            res.Ok = true;
            Dictionary<Guid, int> d = pluginorder[JMMPluginTypes.Search].OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            foreach (Guid g in d.Keys.ToList())
            {
                JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == g);
                if (!prefs.Enabled)
                    d.Remove(g);
            }
            if (d.Count == 0)
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "There is no search plugins active", Ok = false };
            foreach (Guid g in d.Keys)
            {
                IJMMPluginSearch se = plugins.List.FirstOrDefault(a => a.Guid == g) as IJMMPluginSearch;
                if (se != null)
                {
                    JMMPluginResult<List<JMMPluginDownloadLink>> m = se.Search(text);
                    if (!m.Ok)
                        return m;
                    res.Result.AddRange(m.Result);
                }
            }
            return res;
        }
        public JMMPluginResult<List<JMMPluginDownloadLink>> Search(string text, Guid pluginguid)
        {
            IJMMPluginSearch se = plugins.List.FirstOrDefault(a => a.Guid == pluginguid) as IJMMPluginSearch;
            JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == pluginguid);
            if ((se == null) || (!prefs.Enabled))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "The search plugin asked is not active", Ok = false };
            return se.Search(text);
        }
        public JMMPluginResult<List<JMMPluginDownloadLink>> Search(string text, JMMPluginDownloadTypes type)
        {
            JMMPluginResult<List<JMMPluginDownloadLink>> res = new JMMPluginResult<List<JMMPluginDownloadLink>>();
            res.Result = new List<JMMPluginDownloadLink>();
            res.Ok = true;
            Dictionary<Guid, int> d = pluginorder[JMMPluginTypes.Search].OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            foreach (Guid g in d.Keys.ToList())
            {
                JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == g);
                if (!prefs.Enabled)
                    d.Remove(g);
                IJMMPluginSearch se = plugins.List.FirstOrDefault(a => a.Guid == g) as IJMMPluginSearch;
                if ((se == null) || (se.SupportedDownloadTypes & type) == 0)
                    d.Remove(g);
            }
            if (d.Count == 0)
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "There is no search plugins active or matching the download types", Ok = false };
            foreach (Guid g in d.Keys)
            {
                IJMMPluginSearch se = plugins.List.FirstOrDefault(a => a.Guid == g) as IJMMPluginSearch;
                if (se != null)
                {
                    JMMPluginResult<List<JMMPluginDownloadLink>> m = se.Search(text);
                    if (!m.Ok)
                        return m;
                    res.Result.AddRange(m.Result);
                }
            }
            return res;
        }

    }
}
