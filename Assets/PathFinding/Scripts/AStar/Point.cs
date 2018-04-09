namespace PathFinding
{
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("Point:({0},{1})", X, Y);
        }
    }
}
