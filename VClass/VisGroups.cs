using System.Collections.Generic;

namespace VMFLib.VClass;

public class VisGroups : BaseVClass
{
    public override string ClassHeader => "visgroups";
    public List<VisGroup> Groups = new List<VisGroup>();
    public override Dictionary<string, VProperty> Properties { get; set; }
}
    
public class VisGroup : BaseVClass
{
    public override string ClassHeader => "visgroup";
    public override Dictionary<string, VProperty> Properties { get; set; }
    public List<VisGroup> Groups = new List<VisGroup>();

    public string Name => Properties["name"].Str();
}