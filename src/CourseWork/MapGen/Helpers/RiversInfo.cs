namespace CourseWork.MapGen.Helpers;

public class RiversInfo
{
    public int MaxRiverCount { get; }
    public int MaxRiverWidth { get; }
    private Random Random;
    public RiversInfo(double width, double length)
    {
        var riverCof = 0.000015;
        Random rnd = Random.Shared;
        MaxRiverCount = rnd.Next(1, 5);
        MaxRiverWidth = (int) Math.Ceiling(width * length * riverCof);
        Random = Random.Shared;
    }
    
    public void Extend(int riverLength, out List<int[]> extend)
    {  
        extend = new List<int[]>();
        var border = new List<int[]>();
        const double k1 = 1.9;
        const double k2 = 2.5;
        border.Add(new[] {MaxRiverWidth, (int) Math.Ceiling(MaxRiverWidth / k2)});
        for (int i = 1; i < 5; ++i)
        {
            var elem = (int) Math.Ceiling(border[i - 1][0] / k1);
            var elemMin = (int) Math.Ceiling(elem / k2);
            border.Add(new[] {elem, elemMin});
        }

        var arrBorders = new List<int>();
        var counter = 0;
        arrBorders.Add(Random.Next(1, riverLength / 5));
        while (counter != 3)
        {
            var point = Random.Next(1, riverLength - 2);
            if (arrBorders.Contains(point))
                continue;
            arrBorders.Add(point);
            ++counter;
        }

        arrBorders.Sort();
        arrBorders.Add(0);
        for (int i = 0; i < 6; ++i)
            extend.Add(i != 5 ? new[] {Random.Next(border[i][1], border[i][0])} : arrBorders.ToArray());
    }
   
    
    
    
    
    
    
}