namespace Course_work.Map.Additional_classes;

public class PerlinNoise
{
    private const int GradientSizeTable = 256;
    private readonly Random _random;
    private readonly double[] _gradients = new double[GradientSizeTable * 3];
    private readonly byte[] _perm = new byte[256];
    
    
    
    private int Index(int ix, int iy, int iz) => Permutate(ix + Permutate(iy + Permutate(iz)));
    private int Permutate(int x)
    {
        const int mask = GradientSizeTable - 1;
        return _perm[x & mask];
    }
    private double Lerp(double t, double value0, double value1) => value0 + t * (value1 - value0);
    private double Smooth(double x) => x * x * (3 - 2 * x);
    
    
    
}