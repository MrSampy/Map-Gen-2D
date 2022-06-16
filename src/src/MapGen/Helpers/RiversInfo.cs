namespace CourseWork.MapGen.Helpers;

public sealed class RiversInfo
{
    public int MaxRiverCount { get; }
    public int MaxRiverWidth { get; }

    private readonly Random _random;
    public RiversInfo(double width, double length)
    {
        const double riverCof = 0.000015;
        const int minRiverCount = 1;
        const int maxRiverCount = 5;
        _random = Random.Shared;
        MaxRiverCount = _random.Next(minRiverCount, maxRiverCount);
        MaxRiverWidth = (int) Math.Ceiling(width * length * riverCof);
    }
    
    public void Extend(int riverLength, out List<int[]> extend)
    {  
        extend = new List<int[]>();
        var border = new List<int[]>();
        const double k1 = 1.9;
        const double k2 = 2.5;
        const int maxExtension = 5;
        const int maxLength = 3;
        const int range = 2;
        border.Add(new[] {MaxRiverWidth, (int) Math.Ceiling(MaxRiverWidth / k2)});
        
        for (var i = 1; i < maxExtension; ++i)
        {
            var elem = (int) Math.Ceiling(border[i - 1][0] / k1);
            var elemMin = (int) Math.Ceiling(elem / k2);
            border.Add(new[] {elem, elemMin});
        }

        var arrBorders = new List<int>();
        var counter = 0;
        arrBorders.Add(_random.Next(1, riverLength / maxExtension));
        while (counter != maxLength)
        {
            var point = _random.Next(1, riverLength - range);
            if (arrBorders.Contains(point))
                continue;
            arrBorders.Add(point);
            ++counter;
        }

        arrBorders.Sort();
        arrBorders.Add(0);
        for (var i = 0; i < maxExtension; ++i)
        {
            var elem =  new[] {_random.Next(border[i][1], border[i][0])};
            extend.Add(elem);
        }
        extend.Add(arrBorders.ToArray());
    }
}