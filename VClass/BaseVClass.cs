using System.Collections.Generic;
using System.Linq;

namespace VMFLib.VClass;

public abstract class BaseVClass
{
    /// <summary>
    /// The header a class uses in a vmf file, e.g "world" or "entity"
    /// </summary>
    public abstract string ClassHeader { get; }
    /// <summary>
    /// This classes properties
    /// </summary>
    public abstract Dictionary<string, VProperty> Properties { get; set; }
    
    /// <summary>
    /// Classes that are inside of this class
    /// </summary>
    public List<BaseVClass> SubClasses { get; set; }

    /// <summary>
    /// Add a new property to this class
    /// </summary>
    /// <param name="property"></param>
    public void AddProperty(VProperty property)
    {
        if (Properties.Keys.Contains(property.Name))
            return;
        Properties.Add(property.Name, property);
    }

    public BaseVClass()
    {
        Properties = new Dictionary<string, VProperty>();
        SubClasses = new List<BaseVClass>();
    }

    public override string ToString()
    {
        return ClassHeader ?? base.ToString();
    }
}

/// <summary>
/// A generic class, used when a class is detected but there is no native reading
/// </summary>
public class GenericVClass : BaseVClass
{
    public override string ClassHeader { get; }
    public override Dictionary<string, VProperty> Properties { get; set; }

    public GenericVClass(string classHeader)
    {
        ClassHeader = classHeader;
    }
}