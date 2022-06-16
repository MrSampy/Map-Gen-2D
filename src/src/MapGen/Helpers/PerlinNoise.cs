namespace CourseWork.MapGen.Helpers;

public sealed class PerlinNoise
{
    private const int GradientSizeTable = 256;
    private readonly Random _random;
    private readonly byte[] _perm = new byte[256];
    private readonly double _widthDivisor;
    private readonly double _heightDivisor;
    private readonly double[] _decrCof = {0.8, 0.2, 0.1};
    private const int SmoothCof = 3;
    private const int LerpCof = 2;
    private readonly double[] _gradients = new double[GradientSizeTable * SmoothCof];

    public PerlinNoise(int seed, int width, int height)
    {
        _widthDivisor = 1 / (double) width;
        _heightDivisor = 1 / (double) height;
        _random = new Random(seed);
        for (var i = 0; i < _perm.Length; i++)
            _perm[i] = Convert.ToByte(i);
        for (var i = _perm.Length - 1; i >= 1; i--)
        {
            var j = _random.Next(i + 1);
            (_perm[i], _perm[j]) = (_perm[j], _perm[i]);
        }

        InitGradients();
    }

    public double MakeNumber(int x, int y)
    {
        double tempNum = 0;
        var lerpCof = -0.5;
        const int maxLength = 4;
        const double changeLerpCof = 0.5;
        for (var counter = 1; counter < maxLength; counter++)
        {
            double newX = Math.Pow(LerpCof, counter) * x * _widthDivisor,
                newY = Math.Pow(LerpCof, counter) * y * _heightDivisor;
            tempNum += (Noise(newX, newY, lerpCof) + 1) / LerpCof * _decrCof[counter - 1];
            lerpCof += changeLerpCof;
        }

        tempNum = Math.Min(1, Math.Max(0, tempNum));
        tempNum = Math.Round(tempNum, SmoothCof);
        return tempNum;
    }

    private double Noise(double x, double y, double z)
    {
        var ix = (int) Math.Floor(x);
        var iy = (int) Math.Floor(y);
        var iz = (int) Math.Floor(z);
        var dx0 = x - ix;
        var dy0 = y - iy;
        var dz0 = z - iz;
        var dx1 = dx0 - 1;
        var wx = Smooth(dx0);
        var dy1 = dy0 - 1;
        var wy = Smooth(dy0);
        var dz1 = dz0 - 1;
        var wz = Smooth(dz0);
        var vx0 = Lattice(ix, iy, iz, dx0, dy0, dz0);
        var vx1 = Lattice(ix + 1, iy, iz, dx1, dy0, dz0);
        var vy0 = Lerp(wx, vx0, vx1);

        vx0 = Lattice(ix, iy + 1, iz, dx0, dy1, dz0);
        vx1 = Lattice(ix + 1, iy + 1, iz, dx1, dy1, dz0);
        var vy1 = Lerp(wx, vx0, vx1);
        var vz0 = Lerp(wy, vy0, vy1);

        vx0 = Lattice(ix, iy, iz + 1, dx0, dy0, dz1);
        vx1 = Lattice(ix + 1, iy, iz + 1, dx1, dy0, dz1);
        vy0 = Lerp(wx, vx0, vx1);

        vx0 = Lattice(ix, iy + 1, iz + 1, dx0, dy1, dz1);
        vx1 = Lattice(ix + 1, iy + 1, iz + 1, dx1, dy1, dz1);
        vy1 = Lerp(wx, vx0, vx1);

        var vz1 = Lerp(wy, vy0, vy1);
        return Lerp(wz, vz0, vz1);
    }

    private void InitGradients()
    {
        for (var i = 0; i < GradientSizeTable; i++)
        {
            var sizeGradient = 1f - 2f * _random.NextDouble();
            var roundSqrt = Math.Sqrt(1f - sizeGradient * sizeGradient);
            var theta = LerpCof * Math.PI * _random.NextDouble();
            _gradients[i * SmoothCof] = roundSqrt * Math.Cos(theta);
            _gradients[i * SmoothCof + 1] = roundSqrt * Math.Sin(theta);
            _gradients[i * SmoothCof + LerpCof] = sizeGradient;
        }
    }

    private double Lattice(int ix, int iy, int iz, double dx, double dy, double dz)
    {
        var index = Index(ix, iy, iz);
        var gradient = index * SmoothCof;
        return _gradients[gradient] * dx + _gradients[gradient + 1] * dy + _gradients[gradient + LerpCof] * dz;
    }

    private int Index(int ix, int iy, int iz) => Permutation(ix + Permutation(iy + Permutation(iz)));

    private int Permutation(int x)
    {
        const int mask = GradientSizeTable - 1;
        return _perm[x & mask];
    }

    private double Lerp(double t, double value0, double value1) => value0 + t * (value1 - value0);
    private double Smooth(double x) => Math.Pow(x, LerpCof) * (SmoothCof - LerpCof * x);
}