using System;
using System.Collections.Generic;
using VMFLib.Objects;

namespace VMFLib.VClass;

public class Entity : BaseVClass
{
    public override string ClassHeader => "entity";
    public override Dictionary<string, VProperty> Properties { get; set; }
    public List<Connection> Connections = new List<Connection>();
    public Solid Solid;
    public Editor Editor;

    public int Id => Properties["id"].Int();
    public string ClassName => Properties["classname"].Str();
    public int SpawnFlags => Properties["spawnflags"].Int();
    public Vertex Origin => Properties["origin"].Vertex();

    public override string ToString()
    {
        return ClassName ?? base.ToString();
    }
}

public class Connection
{
    public string OutName;
    public string TargetName;
    public string InputName;
    public VProperty Value;
    public double Delay;
    public int Refires;

    public Connection(string connection)
    {
        var conSplit = connection.Split(new[] { "\" \"" }, StringSplitOptions.RemoveEmptyEntries);
        OutName = conSplit[0].Trim('"');
        var properties = conSplit[1].Trim('"').Split(',');
        TargetName = properties[0];
        InputName = properties[1];
        Value = string.IsNullOrEmpty(properties[2]) ? null : new VProperty(null, properties[2]); //May not have property
        Delay = double.Parse(properties[3]);
        Refires = int.Parse(properties[4]);
    }

    public Connection(string outName, string targetName, string inputName, VProperty value, double delay, int refires)
    {
        OutName = outName;
        TargetName = targetName;
        InputName = inputName;
        Value = value;
        Delay = delay;
        Refires = refires;
    }
}