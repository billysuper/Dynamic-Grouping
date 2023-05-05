 namespace Dynamic_Grouping.Data
{
    public class VlanOperation
    {
        public Dictionary<string, List<string>> getVlanCfg(HostsObject hostdata, RootObject data, Dictionary<string, List<string>> vlanIfaces)
        {
            if (data.apps.orgonosprojectvpls.vpls.vplsList == null)
            {
                vlanIfaces["Available"] = new List<string>();
            }
            else
            {
                foreach (var vlan in data.apps.orgonosprojectvpls.vpls.vplsList)
                {
                    vlanIfaces[vlan.name] = vlan.interfaces;
                }
                if (!vlanIfaces.ContainsKey("Available"))
                {
                    vlanIfaces["Available"] = new List<string>();
                }
            } 
            return vlanIfaces;
        }

        public RootObject setVlanCfg(RootObject data, Dictionary<string, List<string>> InputValues)
        {
            data.apps.orgonosprojectvpls.vpls.vplsList.Clear();
            foreach (var vlan in InputValues)
            {
                VplsInfo newvpls = new VplsInfo();
                newvpls.name = vlan.Key;
                newvpls.interfaces = vlan.Value;
                data.apps.orgonosprojectvpls.vpls.vplsList.Add(newvpls);
            }
            return data;
        }
    }
}