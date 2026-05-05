namespace RungeKutta;

delegate double Derivative(double t, double[] y);

class ButcherTableau
{
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
        //missing
    }
    
}
static class RungeKutta
{
    static bool CheckExplizitRungeKuttaStructure(double[] b, double[] c, double[,] a)
    {
        if (b.Length != c.Length)
            return false;
        for (int i = 0; i < c.GetLength(0)-1; i++)
        {
            for (int j = 0; j < c.GetLength(1) - 1; j++)
            {
                if (a[i, j] != 0) return false;
            }
        } 
        return true;
    }

    static double ComputeRungeKutta(double[] b, double[] c, double[,] a, Derivative f, double[] y, double t0, double tmax, int n)
    {
        
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