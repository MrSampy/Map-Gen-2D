namespace CourseWork.MapGen.Helpers;

public sealed class RgbColor
{
    public int Red { get; }
    public int Green { get; }
    public int Blue { get; }

    public RgbColor(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
}