namespace DrawerTools
{
    public class DTMultiAssetProvider
    {
        public DTMultiAssetProvider(string pathFormat)
        {
            PathFormat = pathFormat;
        }

        public string PathFormat { get; protected set; }
        public string GetPath(params string[] nameParams) => string.Format(PathFormat, nameParams);
    }
}