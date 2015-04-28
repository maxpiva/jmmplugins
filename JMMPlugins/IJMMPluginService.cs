using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace JMMPlugins
{
    [ServiceContract]
    public interface IJMMPluginService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Plugins/GetPlugins")]
        JMMPluginResult<List<JMMPlugin>> GetPlugins();
        [WebInvoke(UriTemplate = "Plugins/SetPreferences/{pluginguid}/{enabled}",Method="Post")]
        JMMPluginPreferenceResult SetPreferences(string pluginguid, string enable, List<JMMPluginPreferenceValue> values);
        [WebGet(UriTemplate = "Plugins/GetPluginOrder/{plugintype}")]
        JMMPluginResult<List<Guid>> GetPluginOrder(string type);
        [WebInvoke(UriTemplate = "Plugins/SetPluginOrder/{plugintype}", Method = "Post")]
        JMMPluginResult SetPluginOrder(string type, List<Guid> order);
        [WebGet(UriTemplate = "Plugins/Search/{text}")]
        JMMPluginResult<List<JMMPluginDownloadLink>> Search(string text);
        [WebGet(UriTemplate = "Plugins/SearchByPlugin/{pluginguid}/{text}")]
        JMMPluginResult<List<JMMPluginDownloadLink>> SearchByPlugin(string pluginguid, string text);
        [WebGet(UriTemplate = "Plugins/SearchByDownloadType/{type}/{text}")]
        JMMPluginResult<List<JMMPluginDownloadLink>> SearchByDownloadType(string downloadtype, string text);
        [WebGet(UriTemplate = "Plugins/News/{limit}")]
        JMMPluginResult<List<JMMPluginDownloadLink>> News(string limit);
        [WebGet(UriTemplate = "Plugins/NewsByPlugin/{pluginguid}/{limit}")]
        JMMPluginResult<List<JMMPluginDownloadLink>> News(string pluginguid, string limit);
        [WebInvoke(UriTemplate = "Plugins/Download", Method = "Post")]
        JMMPluginResult Download(JMMPluginDownloadLink link);
        [WebInvoke(UriTemplate = "Plugins/DownloadByPlugin/{pluginguid}", Method = "Post")]
        JMMPluginResult Download(string pluginguid, JMMPluginDownloadLink link);
    }


}
