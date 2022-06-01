namespace CourseWork.Map.Helpers;

public abstract class TilesProperty
{
    public RgbColor Color;

    protected TilesProperty(RgbColor color) => Color = color;

    public void Darkish(double k)
    {
        Color = new RgbColor((int) (Color.Red * k), (int) (Color.Green * k), (int) (Color.Blue * k));
    }
}