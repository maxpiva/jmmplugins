using System;
using System.Collections.Generic;

namespace JMMPlugins
{
    public partial class JMMPluginService : IJMMPluginService
    {
        public JMMPluginPreferenceResult SetPreferences(string pluginguid, string enable, List<JMMPluginPreferenceValue> values)
        {
            Guid guid;
            bool enbl;
            if (!Guid.TryParse(pluginguid, out guid))
                return new JMMPluginPreferenceResult { Error = "Invalid plugin Guid", Ok = false };
            if (!bool.TryParse(enable,out enbl))
                return new JMMPluginPreferenceResult { Error = "Enable parameter should be true or false", Ok = false };
            return SetPreferences(guid, values, enbl);
        }
        
        public JMMPluginResult<List<Guid>> GetPluginOrder(string type)
        {
            JMMPluginTypes pt;
            if (!Enum.TryParse(type, out pt))
                return new JMMPluginResult<List<Guid>> { Error = "Invalid Plugin Type", Ok = false };
            return GetPluginOrder(pt);
        }

        public JMMPluginResult SetPluginOrder(string type, List<Guid> order)
        {
            JMMPluginTypes pt;
            if (!Enum.TryParse(type, out pt))
                return new JMMPluginResult<List<Guid>> { Error = "Invalid Plugin Type", Ok = false };
            return SetPluginOrder(order, pt);
        }
        
        public JMMPluginResult<List<JMMPluginDownloadLink>> SearchByPlugin(string pluginguid, string text)
        {
            Guid guid;
            if (!Guid.TryParse(pluginguid, out guid))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid plugin Guid", Ok = false };
            return Search(text, guid);
        }

        public JMMPluginResult<List<JMMPluginDownloadLink>> SearchByDownloadType(string downloadtype, string text)
        {
            JMMPluginDownloadTypes pt;
            if (!Enum.TryParse(downloadtype, out pt))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid Download Type", Ok = false };
            return Search(text, pt);
        }
        

        public JMMPluginResult<List<JMMPluginDownloadLink>> News(string limit)
        {
            int lm;
            if (!int.TryParse(limit,out lm))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid Limit", Ok = false };
            return News(lm);
        }



        public JMMPluginResult<List<JMMPluginDownloadLink>> News(string pluginguid, string limit)
        {
            Guid guid;
            int lm;
            if (!Guid.TryParse(pluginguid, out guid))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid plugin Guid", Ok = false };
            if (!int.TryParse(limit, out lm))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid Limit", Ok = false };
            return News(guid, lm);
        }

        public JMMPluginResult Download(string pluginguid, JMMPluginDownloadLink link)
        {
            Guid guid;
            if (!Guid.TryParse(pluginguid, out guid))
                return new JMMPluginResult<List<JMMPluginDownloadLink>> { Error = "Invalid plugin Guid", Ok = false };
            return Download(guid, link);
        }

    }
}
