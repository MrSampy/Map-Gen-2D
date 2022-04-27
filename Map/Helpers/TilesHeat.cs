namespace Course_work.Map.Helpers;

public class TilesHeat
{
    public Constants.HeatType THeat { get; set; }
        public RgbColor _Color;
        public TilesHeat(Constants.HeatType heat, RgbColor color)
        {
            THeat = heat;
            _Color = color;
        }
        public void Darkify(double k)
        { 
            _Color = new RgbColor((int) (_Color.Red * k), (int) (_Color.Green * k), (int) (_Color.Blue * k));
        }
}