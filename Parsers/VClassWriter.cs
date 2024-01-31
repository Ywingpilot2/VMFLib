using System;
using System.IO;
using VMFLib.VClass;

namespace VMFLib.Parsers;

public class VClassWriter : IDisposable
{
    private protected StreamWriter Writer;
    private protected int Level; //Indentation level; how deep in a class cycle we are

    public VClassWriter(string filePath)
    {
        Writer = new StreamWriter(filePath);
    }

    public virtual void WriteClass(BaseVClass vClass)
    {
        switch (vClass.ClassHeader)
        {
            default:
            {
                WriteGenericClass(vClass);
            } break;
        }
    }

    #region Generic classes

    /// <summary>
    /// Writes a generic class
    /// </summary>
    private protected void WriteGenericClass(BaseVClass vClass)
    {
        WriteIndentedLine(vClass.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        //Properties get written first
        foreach (VProperty property in vClass.Properties.Values)
        {
            WriteProperty(property);
        }

        foreach (BaseVClass subClass in vClass.SubClasses)
        {
            WriteClass(subClass); //Pass this back to WriteClass() in case this is a special class
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }

    #endregion

    #region Indentation management/Writing

    /// <summary>
    /// Writes a property
    /// </summary>
    /// <param name="property"></param>
    private protected void WriteProperty(VProperty property)
    {
        WriteIndentedLine($"{property.Name} {property.Str()}");
    }

    /// <summary>
    /// Writes a line with proper indentation
    /// </summary>
    /// <param name="line"></param>
    private protected void WriteIndentedLine(string line)
    {
        Writer.WriteLine($"{GetIndentation()}{line}");
    }

    private protected void PreviousLevel()
    {
        if (Level == 0)
            return;
        Level--;
    }

    private protected void NextLevel()
    {
        Level++;
    }

    /// <summary>
    /// Get the amount of indentations we need based on the current level. TODO: Is this really the best way to do this?
    /// </summary>
    /// <returns>Indentations for class properties on this level</returns>
    public string GetIndentation()
    {
        string indentations = "";
        for (int i = 0; i < Level; i++)
        {
            indentations += "\t";
        }

        return indentations;
    }

    #endregion
    
    public void Dispose()
    {
        Writer.Close();
        Writer.Dispose();
    }
}