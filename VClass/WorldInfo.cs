using System.Collections.Generic;
using VMFLib.Objects;

namespace VMFLib.VClass;

public class VersionInfo : BaseVClass
{
    public override string ClassHeader => "versioninfo";
    public override Dictionary<string, VProperty> Properties { get; set; }

    public bool Prefab => Properties["prefab"].Bool();
    public int EditorVersion => Properties["editorversion"].Int();
    public int EditorBuild => Properties["editorbuild"].Int();
    public int MapVersion => Properties["mapversion"].Int();
    public int FormatVersion => Properties["formatversion"].Int();
}

public class World : BaseVClass
{
    public override string ClassHeader => "world";
    public override Dictionary<string, VProperty> Properties { get; set; }
    public List<Solid> Solids { get; set; } = new List<Solid>();
    public List<Group> Groups { get; set; } = new List<Group>();
    public List<Hidden> HiddenClasses { get; set; } = new List<Hidden>();

    public int Id => Properties["id"].Int();
    public int MapVersion => Properties["mapversion"].Int();
    public string ClassName => Properties["classname"].Str();
    public string SkyName => Properties["skyname"].Str();
}

public class Solid : BaseVClass
{
    public override string ClassHeader => "solid";
    public override Dictionary<string, VProperty> Properties { get; set; }
    
    public int Id => Properties["id"].Int();
    public List<Side> Sides = new List<Side>();
    public Editor Editor;
}

public class Side : BaseVClass
{
    public override string ClassHeader => "side";
    public override Dictionary<string, VProperty> Properties { get; set; }
    
    public int Id => Properties["id"].Int();
    public Plane Plane => Properties["plane"].Plane();
    public Displacement DisplacementInfo;
    public string Material => Properties["material"].Str();

    public UVAxis UAxis => Properties["uaxis"].UvAxis();
    public UVAxis VAxis => Properties["vaxis"].UvAxis();

    public double Rotation => Properties["rotation"].Dec();
    public int LightmapScale => Properties["lightmapscale"].Int();
    public int SmoothingGroups => Properties["smoothing_groups"].Int();
    
    //TODO: Flag helpers
    public int Contents => Properties["contents"].Int();
    public int Flags => Properties["flags"].Int();
}

public class Displacement : BaseVClass
{
    public override string ClassHeader => "dispinfo";
    public override Dictionary<string, VProperty> Properties { get; set; }

    public int Power => Properties["power"].Int();
    public Vertex StartPosition => Properties["startposition"].Vertex();
    public float Elevation => Properties["elevation"].Float();
    public bool IsSubdivided => Properties["subdiv"].Bool();
    public DispRows Rows;

    public Displacement()
    {
        Rows = new DispRows();
    }
}