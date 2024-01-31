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

    public override string ToString()
    {
        return $"{Vertices[0].ToSpecialString(1)} {Vertices[1].ToSpecialString(1)} {Vertices[2].ToSpecialString(1)} ";
    }
}

public class UVAxis
{
    public Vertex XYZ;

    public double Translation;
    public double Scaling;

    public UVAxis(string UVAxis)
    {
        var points = UVAxis.Replace("[", "").Replace("]", "").Split(' ');
        XYZ = new Vertex(double.Parse(points[0]), double.Parse(points[1]), double.Parse(points[2]));
    }

    public override string ToString()
    {
        return $"[{XYZ.ToString()} {Translation}] {Scaling}";
    }
}

public class DispRows
{
    public Dictionary<int, List<Vertex>> RowNormals = new Dictionary<int, List<Vertex>>();
    public Dictionary<int, List<Vertex>> RowOffsetNormals = new Dictionary<int, List<Vertex>>();
    
    public Dictionary<int, List<double>> RowDistances = new Dictionary<int, List<double>>();
    public Dictionary<int, List<Vertex>> RowOffsets = new Dictionary<int, List<Vertex>>();
    public Dictionary<int, List<double>> RowAlphas = new Dictionary<int, List<double>>();
    public Dictionary<int, List<int>> RowTriangleTags = new Dictionary<int, List<int>>();
    
    public Dictionary<int, List<int>> AllowedVerts = new Dictionary<int, List<int>>();
    
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
        string[] rows = normals.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            string currentNorm = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentNorm = currentNorm.Trim('"');

            //Please forgive me for what I am about to do
            List<Vertex> normalVerts = new List<Vertex>();
            var splitNorm = currentNorm.Split(' ');

            for (int i = 0; i < splitNorm.Length;)
            {
                double x = double.Parse(splitNorm[0 + i]);
                double y = double.Parse(splitNorm[1 + i]);
                double z = double.Parse(splitNorm[2 + i]);
                Vertex vert = new Vertex(x, y, z);
                normalVerts.Add(vert);
                i += 3;
            }

            RowNormals.Add(row, normalVerts);
        }
    }
    
    public void ParseOffsetNormals(string normals)
    {
        string[] rows = normals.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            string currentNorm = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentNorm = currentNorm.Trim('"');
            
            //Please forgive me for what I am about to do
            List<Vertex> normalVerts = new List<Vertex>();
            var splitNorm = currentNorm.Split(' ');

            for (int i = 0; i < splitNorm.Length;)
            {
                double x = double.Parse(splitNorm[0 + i]);
                double y = double.Parse(splitNorm[1 + i]);
                double z = double.Parse(splitNorm[2 + i]);
                Vertex vert = new Vertex(x, y, z);
                normalVerts.Add(vert);
                i += 3;
            }
            RowOffsetNormals.Add(row, normalVerts);
        }
    }
    
    public void ParseDistances(string distances)
    {
        string[] rows = distances.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            string currentDist = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentDist = currentDist.Trim('"');

            //TODO: This is an awful way to do this!
            List<double> distanceRows = new List<double>();
            foreach (string dist in currentDist.Split(' '))
            {
                distanceRows.Add(double.Parse(dist));
            }

            RowDistances.Add(row, distanceRows);
        }
    }

    public void ParseOffsets(string offsets)
    {
        string[] rows = offsets.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            string currentOffset = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentOffset = currentOffset.Trim('"');
            
            //Please forgive me for what I am about to do
            List<Vertex> offsetRows = new List<Vertex>();
            var splitOffset = currentOffset.Split(' ');

            for (int i = 0; i < splitOffset.Length;)
            {
                double x = double.Parse(splitOffset[0 + i]);
                double y = double.Parse(splitOffset[1 + i]);
                double z = double.Parse(splitOffset[2 + i]);
                Vertex vert = new Vertex(x, y, z);
                offsetRows.Add(vert);
                i += 3;
            }
            
            RowOffsets.Add(row, offsetRows);
        }
    }
    
    public void ParseAlpha(string alpha)
    {
        string[] rows = alpha.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            string currentAlpha = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAlpha = currentAlpha.Trim('"');
            
            //TODO: This is an awful way to do this!
            List<double> alphaRows = new List<double>();
            foreach (string dist in currentAlpha.Split(' '))
            {
                alphaRows.Add(double.Parse(dist));
            }
            
            RowAlphas.Add(row, alphaRows);
        }
    }
    
    public void ParseTriangleTag(string alpha)
    {
        string[] rows = alpha.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);;
        for (int row = 0; row != rows.Length; row++)
        {
            string currentAlpha = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAlpha = currentAlpha.Trim('"');
            
            //TODO: This is an awful way to do this!
            List<int> distanceRows = new List<int>();
            foreach (string dist in currentAlpha.Split(' '))
            {
                distanceRows.Add(int.Parse(dist));
            }
            
            RowTriangleTags.Add(row, distanceRows);
        }
    }
    
    public void ParseAllowedVerts(string av)
    {
        string[] rows = av.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
        for (int row = 0; row != rows.Length; row++)
        {
            int idx = int.Parse(rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[0].Trim('"')); //TODO: Figure out what this actually is?
            string currentAv = rows[row].Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries)[1];
            currentAv = currentAv.Trim('"');
            
            //TODO: This is an awful way to do this!
            List<int> distanceRows = new List<int>();
            foreach (string dist in currentAv.Split(' '))
            {
                distanceRows.Add(int.Parse(dist));
            }
            
            AllowedVerts.Add(idx, distanceRows);
        }
    }
}