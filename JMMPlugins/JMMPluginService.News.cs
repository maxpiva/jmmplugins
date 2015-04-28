using System;
using System.Collections.Generic;
using System.Linq;
using JMMPlugins.Properties;

namespace JMMPlugins
{
    public partial class JMMPluginService
    {
        public JMMPluginResult<List<JMMPluginDownloadLink>> News(int limit = 50)
        {
            JMMPluginResult<List<JMMPluginDownloadLink>> res = new JMMPluginResult<List<JMMPluginDownloadLink>>();
            res.Result = new List<JMMPluginDownloadLink>();
            res.Ok = true;
            Dictionary<Guid, int> d = pluginorder[JMMPluginTypes.News].OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            foreach (Guid g in d.Keys.ToList())
            {
                JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == g);
                if (!prefs.Enabled)
                    d.Remove(g);
            }
            if (d.Count == 0)
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "There is no news plugins active", Ok = false };
            int[] limits = new int[d.Count];
            int bs = limit/d.Count;
            for (int x = 0; x < d.Count; x++)
                limits[x] = bs;
            int left = d.Count - bs;
            for (int x = 0; x < left; x++)
                limits[x]++;
            int cnt = 0;
            foreach (Guid g in d.Keys)
            {
                IJMMPluginNews se = plugins.List.FirstOrDefault(a => a.Guid == g) as IJMMPluginNews;
                if (se != null)
                {
                    JMMPluginResult<List<JMMPluginDownloadLink>> m = se.GetNews(limits[cnt]);
                    if (!m.Ok)
                        return m;
                    res.Result.AddRange(m.Result);
                }
                cnt++;
            }
            res.Result = res.Result.OrderByDescending(a => a.Date).ToList();
            return res;
        }
        public JMMPluginResult<List<JMMPluginDownloadLink>> News(Guid pluginguid, int limit = 50)
        {
            IJMMPluginNews se = plugins.List.FirstOrDefault(a => a.Guid == pluginguid) as IJMMPluginNews;
            JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == pluginguid);
            if ((se == null) || (!prefs.Enabled))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "The news plugin asked is not active", Ok = false };
            return se.GetNews(limit);
        }
    }
}
