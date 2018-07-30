using Extenso.Data.Entity;
using Newtonsoft.Json;

namespace Framework.Security.Membership
{
    public class FrameworkRole : IEntity
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        #region IEntity Members

        [JsonIgnore]
        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}