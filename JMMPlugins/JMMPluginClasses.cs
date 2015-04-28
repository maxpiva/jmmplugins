using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Serialization;


namespace JMMPlugins
{

    [DataContract]
    [Flags]
    public enum JMMPluginTypes
    {
        [EnumMember]
        News = 1,
        [EnumMember]
        Search = 2,
        [EnumMember]
        Download = 4,
        [EnumMember]
        Follow = 8
    }
    [DataContract]
    [Flags]
    public enum JMMPluginDownloadTypes
    {
        [EnumMember]
        Torrent = 1,
        [EnumMember]
        NZB = 2,
        [EnumMember]
        XDCC = 4,
        [EnumMember]
        DirectDownload = 8
    }
    [DataContract]
    public class JMMPluginPreferenceResult : JMMPluginResult
    {
        [DataMember]
        public Dictionary<string, string> Errors { get; set; }
    }

    [DataContract]
    public class JMMPluginResult<T> : JMMPluginResult
    {
        [DataMember]
        public T Result { get; set; }
    }
    [DataContract]
    public class JMMPluginResult
    {
        [DataMember]
        public bool Ok { get; set; }
        [DataMember]
        public string Error { get; set; }
    }
    [DataContract]
    public class JMMPluginPreferenceDescription : JMMPluginPreferenceValue
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public JMMPluginPreferenceType Type { get; set; }
    }
    [DataContract]
    public class JMMPluginPreferenceValue
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
    [DataContract]
    public enum JMMPluginPreferenceType
    {
        [EnumMember]
        String,
        [EnumMember]
        Integer,
        [EnumMember]
        Float,
    }
    [DataContract]
    public class JMMPluginDownload
    {
        [DataMember]
        public Guid PluginGuid { get; set; }
        [DataMember]
        public JMMPluginDownloadTypes Type { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public byte[] Data { get; set; }
    }

    [DataContract]
    public class JMMPluginDownloadLink
    {
        [DataMember]
        public Guid PluginGuid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public JMMPluginDownloadTypes Type { get; set; }
        [DataMember]
        public string DownloadLink { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }

    [DataContract]
    public class JMMPlugin
    {
        [DataMember]
        public Guid Guid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<JMMPluginPreferenceDescription> PluginPreferencesDescriptions { get; set; }
        [DataMember]
        public bool PreferencesValid { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public JMMPluginTypes SupportedPluginTypes { get; set; }
        [DataMember]
        public JMMPluginDownloadTypes SupportedDownloadTypes { get; set; }
    }


    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    internal class Preferences : List<JMMPluginPreferences>
    {
    }
    public class JMMPluginPreferences
    {
        public bool Valid { get; set; }
        public bool Enabled { get; set; }
        public Guid Guid { get; set; }
        public List<JMMPluginPreferenceDescription> Preferences { get; set; }
        public Dictionary<JMMPluginTypes, int> Orders { get; set; }
    }
}
