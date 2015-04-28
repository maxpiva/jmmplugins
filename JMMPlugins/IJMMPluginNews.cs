using System.Collections.Generic;

namespace JMMPlugins
{
    public interface IJMMPluginNews : IJMMPluginCommon
    {
        JMMPluginResult<List<JMMPluginDownloadLink>> GetNews(int quantity);
     
    }


}
