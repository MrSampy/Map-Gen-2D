namespace Course_work.Map.Helpers;

public abstract class TilesProperty
{
    public RgbColor _Color;

    public TilesProperty(RgbColor color)=>_Color = color;
    
    public void Darkify(double k)
    { 
        _Color = new RgbColor((int) (_Color.Red * k), (int) (_Color.Green * k), (int) (_Color.Blue * k));
    }
}