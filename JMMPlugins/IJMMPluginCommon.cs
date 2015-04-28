using System;
using System.Collections.Generic;

namespace JMMPlugins
{
    public interface IJMMPluginCommon
    {
        List<JMMPluginPreferenceDescription> GetPreferencesDescriptions();        
        JMMPluginPreferenceResult SetPreferences(List<JMMPluginPreferenceValue> values);
        string Name { get; set; }
        Guid Guid { get; set; }
        JMMPluginTypes SupportedPluginTypes { get; set; }

        JMMPluginDownloadTypes SupportedDownloadTypes { get; set; }

    }

    
}
