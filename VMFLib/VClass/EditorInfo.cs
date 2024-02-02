using VMFLib.Objects;

namespace VMFLib.VClass;

public class ViewSettings : BaseVClass
{
    public override string ClassHeader => "viewsettings";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();

    public bool? SnapToGrid => Properties["bSnapToGrid"].Bool();
    public bool? ShowGrid => Properties["bShowGrid"].Bool();
    public bool? ShowLogicalGrid => Properties["bShowLogicalGrid"].Bool();
    public int? GridSpacing => Properties["nGridSpacing"].Int();
    public bool? Show3DGrid => Properties["bShow3DGrid"].Bool();
}

public class Group : BaseVClass
{
    public override string ClassHeader => "group";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();

    public Editor? Editor;
}

public class Hidden : BaseVClass
{
    public override string ClassHeader => "hidden";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();
    public BaseVClass? Class;
    public Editor? Editor;
}

public class Editor : BaseVClass
{
    public override string ClassHeader => "editor";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();

    public RGB? Color => Properties["color"].Rgb();
    public int? VisGroupId => Properties["visgroupid"].Int();
    public int? GroupId => Properties["groupid"].Int();
    public bool? VisGroupShown => Properties["visgroupshown"].Bool();
    public bool? VisGroupAutoShown => Properties["visgroupautoshown"].Bool();
    public string? Comment => Properties["comments"].Str();
    public Vec2? LogicalPosition => Properties["logicalpos"].Vec2();
}