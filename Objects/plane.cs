using System;
using System.Collections.Generic;
using System.Linq;

namespace VMFLib.Objects;

public class Plane
{
    public Vertex[] Vertices;

    public Plane(string plane)
    {
        string[] planeVerts = plane.Split(new []{'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
        
        //TODO: FIXME!!!! THIS IS FUCKING AWFUL!!!
        Vertices = new[]
        {
            new Vertex(planeVerts[0]),
            new Vertex(planeVerts[1]),
            new Vertex(planeVerts[2]),
        };
    }
}

public class UVAxis
{
    public double X;
    public double Y;
    public double Z;

    public double Translation;
    public double Scaling;

    public UVAxis(string UVAxis)
    {
        var points = UVAxis.Replace("[", "").Replace("]", "").Split(' ');
        X = double.Parse(points[0]);
        Y = double.Parse(points[1]);
        Z = double.Parse(points[2]);
        Translation = double.Parse(points[3]);
        Scaling = double.Parse(points[4]);
    }
}

//TODO: Implement proper format for normal parsing(instead of just holding strings)
public class DispRows
{
    public Dictionary<int, List<string>> RowNormals = new Dictionary<int, List<string>>();
    public Dictionary<int, List<string>> RowDistances = new Dictionary<int, List<string>>(); //TODO: List<decimal>
    public Dictionary<int, List<string>> RowOffsets = new Dictionary<int, List<string>>(); //TODO: List<decimal>
    public Dictionary<int, List<string>> RowAlphas = new Dictionary<int, List<string>>(); //TODO: List<decimal>
    public Dictionary<int, List<string>> RowTriangleTags = new Dictionary<int, List<string>>(); //TODO: List<int>
    
    public Dictionary<int, List<string>> AllowedVerts = new Dictionary<int, List<string>>(); //TODO: List<int>
    
    public DispRows()
    {
    }

    public DispRows(string normals, string distances, string offsets, string alphas, string tritags, string allowedVerts)
    {
        ParseNormals(normals.Trim());
        ParseDistances(distances.Trim());
        ParseOffsets(offsets.Trim());
        ParseAlpha(alphas.Trim());
        ParseTriangleTag(tritags.Trim());
        ParseAllowedVerts(allowedVerts.Trim());
    }

    public void ParseNormals(string normals)
    {
        string[] rows = normals.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            string currentNorm = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentNorm = currentNorm.Trim('"');
            RowNormals.Add(row, currentNorm.Split(' ').ToList());
        }
    }
    
    public void ParseDistances(string distances)
    {
        string[] rows = distances.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            string currentDist = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentDist = currentDist.Trim('"');
            RowDistances.Add(row, currentDist.Split(' ').ToList());
        }
    }

    public void ParseOffsets(string offsets)
    {
        string[] rows = offsets.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            string currentOffset = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentOffset = currentOffset.Trim('"');
            RowOffsets.Add(row, currentOffset.Split(' ').ToList());
        }
    }
    
    public void ParseAlpha(string alpha)
    {
        string[] rows = alpha.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            string currentAlpha = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAlpha = currentAlpha.Trim('"');
            RowAlphas.Add(row, currentAlpha.Split(' ').ToList());
        }
    }
    
    public void ParseTriangleTag(string alpha)
    {
        string[] rows = alpha.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            string currentAlpha = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAlpha = currentAlpha.Trim('"');
            RowTriangleTags.Add(row, currentAlpha.Split(' ').ToList());
        }
    }
    
    public void ParseAllowedVerts(string av)
    {
        string[] rows = av.Split('\n');
        for (int row = 0; row <= rows.Length; row++)
        {
            int idx = int.Parse(rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[0]); //TODO: Figure out what this actually is?
            string currentAv = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAv = currentAv.Trim('"');
            RowTriangleTags.Add(idx, currentAv.Split(' ').ToList());
        }
    }
}

//TODO: offset normals