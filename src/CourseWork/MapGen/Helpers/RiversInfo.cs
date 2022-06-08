namespace CourseWork.MapGen.Helpers;

public sealed class RiversInfo
{
    public int MaxRiverCount { get; }
    public int MaxRiverWidth { get; }
    public double MaxRiverMoisture { get; }
    public double MinRiverMoisture { get; }
    public double StepMoisture { get; }
    public int MsxSteps { get; }


    private readonly Random _random;
    public RiversInfo(double width, double length)
    {
        var riverCof = 0.000015;
        _random = Random.Shared;
        MinRiverMoisture = 0.8;
        MaxRiverMoisture = 0.6;
        const int cofSteps = 3;
        const int cofDivision = 100;
        MsxSteps = (int)(cofSteps * width / cofDivision);
        StepMoisture = (MinRiverMoisture - MaxRiverMoisture) / MsxSteps;
        MaxRiverCount = _random.Next(1, 5);
        MaxRiverWidth = (int) Math.Ceiling(width * length * riverCof);
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
        arrBorders.Add(_random.Next(1, riverLength / 5));
        while (counter != 3)
        {
            var point = _random.Next(1, riverLength - 2);
            if (arrBorders.Contains(point))
                continue;
            arrBorders.Add(point);
            ++counter;
        }

        arrBorders.Sort();
        arrBorders.Add(0);
        for (int i = 0; i < 6; ++i)
            extend.Add(i != 5 ? new[] {_random.Next(border[i][1], border[i][0])} : arrBorders.ToArray());
    }
   
    
    
    
    
    
    
}