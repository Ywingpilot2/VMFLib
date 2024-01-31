namespace VMFLib.Objects
{
    public class RGB
    {
        public int Red;
        public int Green;
        public int Blue;

        public RGB(string str)
        {
            var property = str.Split(' ');
            Red = int.Parse(property[0]);
            Green = int.Parse(property[1]);
            Blue = int.Parse(property[2]);
        }

        public RGB()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }

        public RGB(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public override string ToString()
        {
            return $"{Red} {Green} {Blue}";
        }
    }
    
    public class Vertex
    {
        public double X;
        public double Y;
        public double Z;
        
        public Vertex(string str)
        {
            var property = str.Trim('[', ']').Split(' ');
            X = double.Parse(property[0]);
            Y = double.Parse(property[1]);
            Z = double.Parse(property[2]);
        }

        public Vertex(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vertex()
        {
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        /// <summary>
        /// Same as ToString() except is capable of returning special formats(e.g "({x} {y} {z})")
        /// </summary>
        /// <param name="flags">0 = ToString() 1 = surrounded () 2 = surrounded []</param>
        /// <returns></returns>
        public string ToSpecialString(int flags)
        {
            switch (flags)
            {
                case 2:
                {
                    return $"[{ToString()}]";
                }
                case 1:
                {
                    return $"({ToString()})";
                }
                default:
                {
                    return ToString();
                }
            }
        }
    }
}