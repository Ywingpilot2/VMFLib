using System.Collections.Generic;
using VMFLib.VClass;

public class CamerasHolder : BaseVClass
{
    public override string ClassHeader => "cameras";
    public override Dictionary<string, VProperty> Properties { get; set; }
}

public class Camera : BaseVClass
{
    public override string ClassHeader => "camera";
    public override Dictionary<string, VProperty> Properties { get; set; }
}