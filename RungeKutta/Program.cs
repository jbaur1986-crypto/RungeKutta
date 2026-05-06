namespace RungeKutta;

delegate double Derivative(double t, double[] y);

class ButcherTableau
{
    // _a sxs matrix, _b and _c length s
    private readonly double[,] _a;
    private readonly double[] _b;
    private readonly double[] _c;

    public double[,] A => (double[,])_a.Clone();
    public double[] B => (double[])_b.Clone(); 
    public double[] C => (double[])_c.Clone();

    public ButcherTableau(double[,] a, double[] b, double[] c)
    {
        _a = (double[,])a.Clone();
        _b= (double[])b.Clone();
        _c = (double[])c.Clone();
        Validate();
    }

    private void Validate()
    {
        if (_b.Length != _c.Length) throw new ArgumentException("Dimensions of b and c have to be equal.");
        if (_a.GetLength(0) != _a.GetLength(1) || _a.GetLength(0) != _b.Length)
            throw new ArgumentException("Dimensions of a are wrong.");
        // check for explicit runge kutta
        for (int i = 0; i < _a.GetLength(0)-1; i++)
        {
            for (int j = i; j < _a.GetLength(1); j++)
            {
                if (_a[i, j] != 0) throw new ArgumentException("Entries of a in right upper triangle have to be zero.");
            }
        }
    }
    
    // private void CheckColumnSum()
    
}
static class RungeKutta
{
    static double SolveExplicitRungeKutta(ButcherTableau b, Derivative f, double[] y, double t0, double tmax, int n)
    {
        //implement here
    }
}
class Program
{
    static void Main(string[] args)
    {
        Func<double, double> f = x => Math.Cos(10 * Math.PI * x);
                string fdescription = "Cos (10*Pi*x)";
    }
}