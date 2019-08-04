namespace NetRt.Common
{
    internal class NetRtResources
    {
        public static string GetResource(string resName) => CliResources.ResourceManager.GetString(resName);
    }
}