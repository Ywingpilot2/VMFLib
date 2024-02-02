namespace VMFLib.VClass;

public class VisGroups : BaseVClass
{
    public override string ClassHeader => "visgroups";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();
}
    
public class VisGroup : BaseVClass
{
    public override string ClassHeader => "visgroup";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();

    public string? Name => Properties["name"].Str();
}