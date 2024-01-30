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
    }
    
    public class Vertex
    {
        public int X;
        public int Y;
        public int Z;
        
        public Vertex(string str)
        {
            var property = str.Trim('[', ']').Split(' ');
            X = int.Parse(property[0]);
            Y = int.Parse(property[1]);
            Z = int.Parse(property[2]);
        }

        public Vertex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}