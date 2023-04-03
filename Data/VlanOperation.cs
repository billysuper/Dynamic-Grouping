 namespace Dynamic_Grouping.Data.Service
{
    public class VlanOperation
    {
        public string test;
        private List<string> temp1 { get; set; } = new List<string>();
        private List<string> temp2 { get; set; } = new List<string>();

        public Dictionary<string, string>  getVlanCfg(RootObject data,Dictionary<string,string> InputValues)
        {
                foreach (var vlan in data.apps.orgonosprojectvpls.vpls.vplsList)
                {
                    //Vlan
                    foreach (var viface in vlan.interfaces)
                    {
                        temp1.Add(viface);
                    }
                    //Port
                    foreach (var kvp in data.ports)
                    {
                        foreach (Interfaces piface in kvp.Value.interfaces)
                        {
                            if (temp1.Contains(piface.name))
                            {
                                InputValues[piface.name] = vlan.name;
                                temp2.Add(piface.name);
                                temp1.Remove(piface.name);
                            }
                            else if (temp2.Contains(piface.name)) break;
                            else
                            {
                                InputValues[piface.name] = "none";
                                temp1.Remove(piface.name);
                            }
                        }
                    }
                }
            return InputValues;
        }

        public RootObject setVlanCfg(RootObject data, Dictionary<string, string> InputValues)
        {
            List<string> temp1 = new List<string>();
            List<string> temp2 = new List<string>();
                data.apps.orgonosprojectvpls.vpls.vplsList.Clear();
            //collect all vlan
            foreach (var port in data.ports)
            {
                foreach (var piface in port.Value.interfaces)
                {
                    if (InputValues[piface.name] == "")
                    {
                        InputValues[piface.name] = "none";
                    }
                    else if (InputValues[piface.name] != "none")
                    {
                        temp1.Add(InputValues[piface.name]);
                    }
                }
            }

                data.apps.orgonosprojectvpls.vpls.vplsList.Clear();
            temp2 = temp1.Distinct().ToList();
            //create vlan objects
            foreach (var vlan in temp2)
            {
                VplsInfo newObj = new VplsInfo();
                newObj.name = vlan;
                newObj.encapsulation = "NONE";
                // add interfaces belong the vlan
                newObj.interfaces = new List<string>();
                foreach (var viface in InputValues)
                {
                    if (viface.Value == vlan)
                    {
                        newObj.interfaces.Add(viface.Key);
                    }
                }
                    data.apps.orgonosprojectvpls.vpls.vplsList.Add(newObj);
            }
            return data;
        }
    }
}