using System.Collections.Generic;

namespace JMMPlugins
{
    public interface IJMMPluginSearch : IJMMPluginLinkDownload
    {
        JMMPluginResult<List<JMMPluginDownloadLink>> Search(string search);
    }
}
