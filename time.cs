using System;


public class Comm
{
    public static long Time()
    {
        DateTime dt1 = new DateTime(2019, 1, 1);
        TimeSpan ts = DateTime.Now - dt1;
        return (long)ts.TotalMilliseconds;
    }
    public enum Direction
    {
        UP = 4,
        DOWN = 1,
        RIGHT = 3,
        LEFT = 2,
    }
}
