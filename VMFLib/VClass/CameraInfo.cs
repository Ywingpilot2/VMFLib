using VMFLib.VClass;

public class CamerasHolder : BaseVClass
{
    public override string ClassHeader => "cameras";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();
}

public class Camera : BaseVClass
{
    public override string ClassHeader => "camera";
    public override Dictionary<string, VProperty> Properties { get; set; } = new Dictionary<string, VProperty>();
}