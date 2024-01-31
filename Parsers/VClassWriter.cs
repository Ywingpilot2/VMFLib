using System;
using System.Collections.Generic;
using System.IO;
using VMFLib.Objects;
using VMFLib.VClass;

namespace VMFLib.Parsers;

public class VClassWriter : IDisposable
{
    private protected StreamWriter Writer;
    private protected int Level; //Indentation level; how deep in a class cycle we are

    public VClassWriter(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException();
        }
        Writer = new StreamWriter(filePath);
    }

    public VClassWriter(StreamWriter writer)
    {
        Writer = writer;
    }

    public virtual void WriteClass(BaseVClass vClass)
    {
        switch (vClass.ClassHeader)
        {
            case "vertices_plus":
            {
                WriteVPlus((VerticesPlus)vClass);
            } break;
            case "solid":
            {
                WriteSolidClass((Solid)vClass);
            } break;
            case "entity":
            {
                WriteEntityClass((Entity)vClass);
            } break;
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
    
    /// <summary>
    /// Writes a property
    /// </summary>
    /// <param name="property"></param>
    private protected void WriteProperty(VProperty property)
    {
        WriteIndentedLine($"\"{property.Name}\" \"{property.Str()}\"");
    }

    #endregion

    #region Special Classes

    #region World Info

    #region Solids

    /// <summary>
    /// Writes a solid class
    /// </summary>
    /// <param name="solid"></param>
    private protected void WriteSolidClass(Solid solid)
    {
        WriteIndentedLine(solid.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        //Properties get written first
        foreach (VProperty property in solid.Properties.Values)
        {
            WriteProperty(property);
        }

        if (solid.Sides.Count != 0)
        {
            foreach (Side side in solid.Sides)
            {
                WriteSideClass(side);
            }
        }

        if (solid.Editor != null)
        {
            WriteClass(solid.Editor);
        }

        foreach (BaseVClass subClass in solid.SubClasses)
        {
            WriteClass(subClass); //Pass this back to WriteClass() in case this is a special class
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }
    
    /// <summary>
    /// Writes the side of a solid
    /// </summary>
    private protected void WriteSideClass(Side side)
    {
        WriteIndentedLine(side.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        //Properties get written first
        foreach (VProperty property in side.Properties.Values)
        {
            WriteProperty(property);
        }

        if (side.DisplacementInfo != null)
        {
            WriteDispInfo(side.DisplacementInfo);
        }

        foreach (BaseVClass subClass in side.SubClasses)
        {
            WriteClass(subClass); //Pass this back to WriteClass() in case this is a special class
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }

    /// <summary>
    /// Hammer++ has its own way of storing vertexes, these cannot be written to like normal classes though due to special ways of storing verts
    /// </summary>
    /// <param name="vPlus"></param>
    private protected void WriteVPlus(VerticesPlus vPlus)
    {
        WriteIndentedLine(vPlus.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        foreach (Vertex vertex in vPlus.Vertices)
        {
            WriteIndentedLine($"\"v\" \"{vertex}\"");
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }

    #region Displacements

    /// <summary>
    /// Writes a generic class
    /// </summary>
    private protected void WriteDispInfo(Displacement displacement)
    {
        WriteIndentedLine(displacement.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        //Properties get written first
        foreach (VProperty property in displacement.Properties.Values)
        {
            WriteProperty(property);
        }

        if (displacement.Rows != null)
        {
            WriteRows(displacement.Rows.RowNormals, "normals");
            WriteRows(displacement.Rows.RowDistances, "distances");
            WriteRows(displacement.Rows.RowOffsets, "offsets");
            WriteRows(displacement.Rows.RowOffsetNormals, "offset_normals");
            WriteRows(displacement.Rows.RowAlphas, "alpha");
            WriteRows(displacement.Rows.RowTriangleTags, "triangle_tags");
            WriteRows(displacement.Rows.AllowedVerts, "allowed_verts");
        }

        foreach (BaseVClass subClass in displacement.SubClasses)
        {
            WriteClass(subClass); //Pass this back to WriteClass() in case this is a special class
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }

    private protected void WriteRows(object rowsobj, string header)
    {
        Dictionary<int, object> rows = (Dictionary<int, object>)rowsobj;
        WriteIndentedLine(header);
        WriteIndentedLine("{");
        NextLevel();
        for (int i = 0; i < rows.Count; i++)
        {
            var currentRow = rows[0];
            WriteRow(i, currentRow.ToString());
        }
        PreviousLevel();
        WriteIndentedLine("}");
    }

    private protected void WriteRow(int idx, string values)
    {
        WriteIndentedLine($"\"row{idx}\" \"{values}\"");
    }

    #endregion

    #endregion

    #endregion

    #region Entities

    /// <summary>
    /// Writes an entity class
    /// </summary>
    private protected void WriteEntityClass(Entity entity)
    {
        WriteIndentedLine(entity.ClassHeader);
        WriteIndentedLine("{"); // Need our brackets!
        NextLevel(); //Heading into our properties
        
        //Properties get written first
        foreach (VProperty property in entity.Properties.Values)
        {
            WriteProperty(property);
        }

        if (entity.Connections.Count != 0)
        {
            WriteIndentedLine("connections");
            WriteIndentedLine("{");
            NextLevel();
            foreach (Connection connection in entity.Connections)
            {
                WriteConnection(connection);
            }
            PreviousLevel();
            WriteIndentedLine("}");
        }

        if (entity.Solid != null)
        {
            WriteClass(entity.Solid);
        }

        if (entity.Hidden != null)
        {
            WriteClass(entity.Hidden);
        }

        if (entity.Editor != null)
        {
            WriteClass(entity.Editor);
        }

        foreach (BaseVClass subClass in entity.SubClasses)
        {
            WriteClass(subClass); //Pass this back to WriteClass() in case this is a special class
        }
        
        PreviousLevel();
        WriteIndentedLine("}");
    }
    
    private protected void WriteConnection(Connection connection)
    {
        WriteIndentedLine($"\"{connection.OutName}\" \"{connection.TargetName},{connection.InputName},{connection.Value.Str()},{connection.Delay},{connection.Refires}\"");
    }

    #endregion

    #endregion

    #region Indentation management/Writing

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