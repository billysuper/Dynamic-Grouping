 namespace Dynamic_Grouping.Data
{
    public class VlanOperation
    {
        public Dictionary<string, List<string>> getVlanCfg(HostsObject hostdata, RootObject data, Dictionary<string, List<string>> vlanIfaces,Dictionary<string,string>hostifaces)
        {
            List<string> existifaces = new List<string>();
            bool exist=false;
            //add Available
            /*if (data.apps.orgonosprojectvpls.vpls.vplsList == null)
            {
                vlanIfaces   ["Available"] = new List<string>();
                //add available iface.
                foreach (var hostiface in hostifaces)
                {
                    foreach (var host in hostdata.Hosts)
                    {
                        if (host.devicePort == hostiface.Key)
                        {
                            vlanIfaces["Available"].Add(hostiface.Value);
                        }
                    }
                }
            }
            else
            {
                existifaces.Clear();*/
                foreach (var vlan in data.apps.orgonosprojectvpls.vpls.vplsList)
                {
                    vlanIfaces[vlan.name] = vlan.interfaces;
                }
                /*if (!vlanIfaces.ContainsKey("Available"))
                {
                    vlanIfaces["Available"] = new List<string>();
                    foreach (var hostiface in hostifaces)
                    {
                        foreach (var host in hostdata.Hosts)
                        {
                            if (host.devicePort == hostiface.Key)
                            {
                                foreach (var vlan in vlanIfaces)
                                {
                                    foreach (var iface in vlan.Value)
                                    {
                                        existifaces.Add(iface);
                                    }
                                }
                            }
                        }
                    }
                    foreach (var host in hostdata.Hosts)
                    {
                        foreach (var hostiface in hostifaces)
                        {
                            if(host.devicePort == hostiface.Key)
                            {
                                exist = false;
                                foreach (var iface in existifaces)
                                {
                                    if (iface == hostiface.Value)
                                    {
                                        exist = true;
                                    }
                                }
                                if (!exist)
                                {
                                    vlanIfaces["Available"].Add(hostiface.Value);
                                }
                            }
                        }
                    }
                }
            }  */
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