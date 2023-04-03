using Newtonsoft.Json;

namespace Dynamic_Grouping.Data
{
    public class Interfaces
    {
        public string name { get; set; }
    }

    public class Port
    {
        public List<Interfaces> interfaces { get; set; }
    }

    public class Device
    {
        public List<object> classifiers { get; set; }
    }

    //Vlan
    public class Vpls
    {
        public long lastUpdateTime { get; set; }
        public List<VplsInfo> vplsList { get; set; }
    }

    public class VplsInfo
    {
        public string name { get; set; }
        public List<string> interfaces { get; set; }
        public string encapsulation { get; set; }
    }

    public class Suppression
    {
        public List<string> deviceTypes { get; set; }
        public string annotation { get; set; }
    }

    public class OrgOnosprojectVpls
    {
        public Vpls vpls { get; set; }
    }

    public class OrgOnosprojectProviderLldp
    {
        public Suppression suppression { get; set; }
    }

    public class App
    {
        [JsonProperty("org.onosproject.vpls")]
        public OrgOnosprojectVpls orgonosprojectvpls { get; set; }  
        //public OrgOnosprojectVpls orgonosprojectvpls { get; set; }
        [JsonProperty("org.onosproject.lldp")]
        public OrgOnosprojectProviderLldp orgonosprojectproviderlldp { get; set; }
    }

    public class RootObject
    {
        public Dictionary<string, Port>? ports { get; set; }
        public Dictionary<string, Device> devices { get; set; }
        public App apps { get; set; }
        public Dictionary<string, object> hosts { get; set; }
        public Dictionary<string, object> links { get; set; }
        public Dictionary<string, object> layouts { get; set; }
        public Dictionary<string, object> regions { get; set; }
    }
}