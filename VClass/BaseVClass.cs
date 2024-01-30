using System.Collections.Generic;
namespace VMFLib.VClass;

public abstract class BaseVClass
{
    public abstract string ClassHeader { get; }
    public abstract Dictionary<string, VProperty> Properties { get; set; }

    public void AddProperty(VProperty property)
    {
        Properties.Add(property.Name, property);
    }

    public BaseVClass()
    {
        Properties = new Dictionary<string, VProperty>();
    }
}