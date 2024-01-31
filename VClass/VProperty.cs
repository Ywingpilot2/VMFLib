using System;
using VMFLib.Objects;

namespace VMFLib.VClass
{
    public class VProperty
    {
        public string Name;
        private string _property;

        public VProperty(string name, object value)
        {
            Name = name;
            _property = value.ToString();
        }

        public VProperty(string property)
        {
            property = property.Trim();

            int commentPosition = property.IndexOf("//"); //Remove comments
            if (commentPosition != -1 && commentPosition != 0)
            {
                property = property.Remove(commentPosition).Trim();
            }
            
            var parsed = property.Split(new []{"\" \""}, StringSplitOptions.RemoveEmptyEntries);
            Name = parsed[0].Trim(new []{' ', '"'});
            _property = parsed[1].Trim().Trim(new []{' ', '"'});
        }

        public int Int()
        {
            //Don't return null values; return defaults instead
            if (_property == null)
                return 0;
            return int.Parse(_property);
        }
        
        public float Float()
        {
            //Don't return null values; return defaults instead
            if (_property == null)
                return 0.0f;
            return float.Parse(_property);
        }
        
        public double Dec()
        {
            //Don't return null values; return defaults instead
            if (_property == null)
                return 0.0;
            return double.Parse(_property);
        }
        
        public string Str()
        {
            //Don't return null values; return defaults instead
            if (_property == null)
                return "";
            return _property;
        }
        
        public bool Bool()
        {
            //Don't return null values; return defaults instead
            if (_property == null)
                return false;
            
            bool value;
            if (bool.TryParse(_property, out value))
            {
                return value;
            }
            else //Probably an int
            {
                switch (Int())
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        return false;
                }
            }
        }
        
        public RGB Rgb()
        {
            return new RGB(_property);
        }

        public Vertex Vertex()
        {
            return new Vertex(_property);
        }
        
        public Vec2 Vec2()
        {
            return new Vec2(_property);
        }

        public Plane Plane()
        {
            return new Plane(_property);
        }

        public UVAxis UvAxis()
        {
            return new UVAxis(_property);
        }

        /// <summary>
        /// Sets the VProperty to the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether or not the operation was a success</returns>
        public bool Set(object value)
        {
            if (value == null)
                return false;
            
            _property = value.ToString();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The name of this property, otherwise the type's name</returns>
        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}