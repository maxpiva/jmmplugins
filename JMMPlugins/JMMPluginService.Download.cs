using System;
using System.Collections.Generic;
using System.Linq;
using JMMPlugins.Properties;

namespace JMMPlugins
{
    public partial class JMMPluginService
    {

        public JMMPluginResult Download(JMMPluginDownloadLink link)
        {
            JMMPluginResult res = new JMMPluginResult();
            res.Ok = true;
            Dictionary<Guid, int> d = pluginorder[JMMPluginTypes.Download].OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
            foreach (Guid g in d.Keys.ToList())
            {
                JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == g);
                if (!prefs.Enabled)
                    d.Remove(g);

                IJMMPluginDownload se = plugins.List.FirstOrDefault(a => a.Guid == g) as IJMMPluginDownload;
                if ((se == null) || (se.SupportedDownloadTypes & link.Type) == 0)
                    d.Remove(g);
            }
            if (d.Count == 0)
                return new JMMPluginResult { Error = "There is no download plugins active or matching the download types", Ok = false };
            Guid m = d.First().Key;
            IJMMPluginLinkDownload pdownload = plugins.List.FirstOrDefault(a => a.Guid == link.PluginGuid) as IJMMPluginLinkDownload;
            if (pdownload != null)
            {
                JMMPluginPreferences prefss = Settings.Default.Preferences.First(a => a.Guid == link.PluginGuid);
                if (!prefss.Enabled)
                    pdownload = null;
            }
            if (pdownload == null)
                return new JMMPluginResult { Error = "The link originator plugin is not active anymore, unable to download", Ok = false };

            JMMPluginDownload down = pdownload.GetDownload(link);
            if (down.Data == null)
                return new JMMPluginResult { Error = "Unable to download archive", Ok = false };
            IJMMPluginDownload seq = plugins.List.FirstOrDefault(a => a.Guid == m) as IJMMPluginDownload;
            if (seq==null)
                return new JMMPluginResult { Error = "Unable to find download plugin", Ok = false };
            return seq.Download(down);
        }



        public JMMPluginResult Download(Guid pluginguid, JMMPluginDownloadLink link)
        {
            IJMMPluginDownload seq = plugins.List.FirstOrDefault(a => a.Guid == pluginguid) as IJMMPluginDownload;
            JMMPluginPreferences prefs = Settings.Default.Preferences.First(a => a.Guid == pluginguid);
            if ((seq == null) || (!prefs.Enabled))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "The download plugin asked is not active", Ok = false };
            IJMMPluginLinkDownload pdownload = plugins.List.FirstOrDefault(a => a.Guid == link.PluginGuid) as IJMMPluginLinkDownload;
            if (pdownload != null)
            {
                JMMPluginPreferences prefss = Settings.Default.Preferences.First(a => a.Guid == link.PluginGuid);
                if (!prefss.Enabled)
                    pdownload = null;
            }
            if (pdownload == null)
                return new JMMPluginResult { Error = "The link originator plugin is not active anymore, unable to download", Ok = false };
            JMMPluginDownload down = pdownload.GetDownload(link);
            if (down.Data == null)
                return new JMMPluginResult { Error = "Unable to download archive", Ok = false };
            return seq.Download(down);
        }

    }
}
