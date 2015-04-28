namespace JMMPlugins
{
    public interface IJMMPluginLinkDownload : IJMMPluginCommon
    {
        JMMPluginDownload GetDownload(JMMPluginDownloadLink link);
    }

   
}
