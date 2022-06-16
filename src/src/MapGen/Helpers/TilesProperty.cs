namespace CourseWork.MapGen.Helpers;

public abstract class TilesProperty
{
    public RgbColor Color;
    protected TilesProperty(RgbColor color) => Color = color;
    public double NoiseValue { set; get; }
    public void Darkish(double darkCof)
    {
        Color = new RgbColor((int) (Color.Red * darkCof), (int) (Color.Green * darkCof), (int) (Color.Blue * darkCof));
    }
}