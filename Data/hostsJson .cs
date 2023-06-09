namespace Dynamic_Grouping.Data
{
    public class Location
    {
        public string ElementId { get; set; }
        public string Port { get; set; }
    }

    public class Host
    {
        public string Id { get; set; }
        public string Mac { get; set; }
        public string Vlan { get; set; }
        public string InnerVlan { get; set; }
        public string OuterTpid { get; set; }
        public bool Configured { get; set; }
        public bool Suspended { get; set; }
        public List<string> IpAddresses { get; set; }
        public List<Location> Locations { get; set; }
        public string devicePort { get; set; }
        public string militaryPower { get; set; } = "Unknown";
        public string vpls { get; set; } = "Available";
        public string iface {get; set;} = "None";
    }

    public class HostsObject
    {
        public List<Host> Hosts { get; set; }
        public int test { get; set; }
    }

}