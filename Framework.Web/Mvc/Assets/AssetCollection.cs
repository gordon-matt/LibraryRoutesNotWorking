using System.Collections.Generic;

namespace Framework.Web.Mvc.Assets
{
    public class AssetCollection
    {
        public IEnumerable<Asset> Scripts { get; set; }

        public IEnumerable<Asset> Styles { get; set; }
    }
}