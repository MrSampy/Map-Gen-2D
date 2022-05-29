namespace CourseWork.Map.Helpers;

public class PerlinNoise
{
    private const int GradientSizeTable = 256;
    private readonly Random _random;
    private readonly double[] _gradients = new double[GradientSizeTable * 3];
    private readonly byte[] _perm = new byte[256];
    private readonly double _widthDivisor;
    private readonly double _heightDivisor;
    private readonly double[] _decrCoeffs = {0.8, 0.2, 0.1};
    private readonly int _smoothCoef = 3;
    private readonly int _lerpCoef = 2;

    public PerlinNoise(int seed, int width, int height)
    {
        _widthDivisor = 1 / (double) width;
        _heightDivisor = 1 / (double) height;
        _random = new Random(seed);
        for (int i = 0; i < _perm.Length; i++)
            _perm[i] = Convert.ToByte(i);
        for (int i = _perm.Length - 1; i >= 1; i--)
        {
            int j = _random.Next(i + 1);
            (_perm[i], _perm[j]) = (_perm[j], _perm[i]);
        }

        InitGradients();
    }

    public double MakeNumber(int x, int y)
    {
        double tempNum = 0;
        double lerpCoef = -0.5;
        for (int counter = 1; counter < 4; counter++)
        {
            double newX = Math.Pow(_lerpCoef, counter) * x * _widthDivisor,
                newY = Math.Pow(_lerpCoef, counter) * y * _heightDivisor;
            tempNum += (Noise(newX, newY, lerpCoef) + 1) / 2 * _decrCoeffs[counter - 1];
            lerpCoef += 0.5;
        }

        tempNum = Math.Min(1, Math.Max(0, tempNum));
        tempNum = Math.Round(tempNum, _smoothCoef);
        return tempNum;
    }

    private double Noise(double x, double y, double z)
    {
        int ix = (int) Math.Floor(x), iy = (int) Math.Floor(y), iz = (int) Math.Floor(z);
        double dx0 = x - ix;
        double dy0 = y - iy;
        double dz0 = z - iz;
        double dx1 = dx0 - 1, wx = Smooth(dx0);
        double dy1 = dy0 - 1;
        double wy = Smooth(dy0);
        double dz1 = dz0 - 1;
        double wz = Smooth(dz0);
        double vx0 = Lattice(ix, iy, iz, dx0, dy0, dz0);
        double vx1 = Lattice(ix + 1, iy, iz, dx1, dy0, dz0);

        double vy0 = Lerp(wx, vx0, vx1);

        vx0 = Lattice(ix, iy + 1, iz, dx0, dy1, dz0);
        vx1 = Lattice(ix + 1, iy + 1, iz, dx1, dy1, dz0);
        double vy1 = Lerp(wx, vx0, vx1);
        double vz0 = Lerp(wy, vy0, vy1);

        vx0 = Lattice(ix, iy, iz + 1, dx0, dy0, dz1);
        vx1 = Lattice(ix + 1, iy, iz + 1, dx1, dy0, dz1);
        vy0 = Lerp(wx, vx0, vx1);

        vx0 = Lattice(ix, iy + 1, iz + 1, dx0, dy1, dz1);
        vx1 = Lattice(ix + 1, iy + 1, iz + 1, dx1, dy1, dz1);
        vy1 = Lerp(wx, vx0, vx1);

        double vz1 = Lerp(wy, vy0, vy1);
        return Lerp(wz, vz0, vz1);
    }

    private void InitGradients()
    {
        for (int i = 0; i < GradientSizeTable; i++)
        {
            double z = 1f - 2f * _random.NextDouble();
            double r = Math.Sqrt(1f - z * z);
            double theta = _lerpCoef * Math.PI * _random.NextDouble();
            _gradients[i * _smoothCoef] = r * Math.Cos(theta);
            _gradients[i * _smoothCoef + 1] = r * Math.Sin(theta);
            _gradients[i * _smoothCoef + _lerpCoef] = z;
        }
    }

    private double Lattice(int ix, int iy, int iz, double dx, double dy, double dz)
    {
        int index = Index(ix, iy, iz);
        int g = index * _smoothCoef;
        return _gradients[g] * dx + _gradients[g + 1] * dy + _gradients[g + 2] * dz;
    }

    private int Index(int ix, int iy, int iz) => Permutate(ix + Permutate(iy + Permutate(iz)));

    private int Permutate(int x)
    {
        const int mask = GradientSizeTable - 1;
        return _perm[x & mask];
    }

    private double Lerp(double t, double value0, double value1) => value0 + t * (value1 - value0);
    private double Smooth(double x) => Math.Pow(x, _lerpCoef) * (_smoothCoef - _lerpCoef * x);
}