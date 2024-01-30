//Hammer++ Classes

using System.Collections.Generic;
using VMFLib.Objects;

namespace VMFLib.VClass;

public class VerticesPlus : BaseVClass
{
    public override string ClassHeader { get; }
    public override Dictionary<string, VProperty> Properties { get; set; }

    public List<Vertex> Vertices = new List<Vertex>();
}