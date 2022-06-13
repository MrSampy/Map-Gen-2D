namespace CourseWork.MapGen.Helpers;


public abstract class TilesProperty
{
    public RgbColor Color;
    protected TilesProperty(RgbColor color) => Color = color;

    public void Darkish(double darckCof)
    {
        Color = new RgbColor((int) (Color.Red * darckCof), (int) (Color.Green * darckCof), (int) (Color.Blue * darckCof));
    }
}