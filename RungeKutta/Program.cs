namespace RungeKutta;

delegate double[] Derivative(double t, double[] y);

class VectorHelper
{
    // Refactor put in all Helpers.
}

class ButcherTableau
{
    // _a sxs matrix, _b and _c length s
    private readonly double[,] _a;
    private readonly double[] _b;
    private readonly double[] _c;

    public double[,] A => (double[,])_a.Clone();
    public double[] B => (double[])_b.Clone(); 
    public double[] C => (double[])_c.Clone();

    public ButcherTableau(double[,] a, double[] b, double[] c, double eps)
    {
        _a = (double[,])a.Clone();
        _b= (double[])b.Clone();
        _c = (double[])c.Clone();
        Validate(eps);
    }

    private void Validate(double eps)
    {
        if (_b.Length != _c.Length) throw new ArgumentException("Dimensions of b and c have to be equal.");
        if (_a.GetLength(0) != _a.GetLength(1) || _a.GetLength(0) != _b.Length)
            throw new ArgumentException("Dimensions of a are wrong.");
        // check for explicit runge kutta
        for (int i = 0; i < _a.GetLength(0)-1; i++)
        {
            for (int j = i; j < _a.GetLength(1); j++)
            {
                if (Math.Abs(_a[i, j]) > eps) throw new ArgumentException("Entries of a in right upper triangle have to be zero.");
            }
        }
    }
    // private void CheckColumnSum()
}

static class RungeKutta
{
    public static double[] SolveExplicitRungeKutta(ButcherTableau b, Derivative f, double[] y0, double t0, double tmax,
        int n, double tol)
    {
        double h = (tmax - t0) / n;
        int s = b.B.Length;
        int dim = y0.Length;

        double[] yN = (double[])y0.Clone();
        double tN = t0;
        double[] yNext = new double[dim];
        double[] ki = new double [s * dim];
        double[] kHelp = new double[dim];

        ref double Ki(int i, int j) => ref ki[i * dim + j];

        double[] GetKiVector(int i)
        {
            double[] k = new double[dim];
            for (int j = 0; j < dim; j++)
            {
                k[j] = Ki(i, j);
            }

            return k;
        }

        double[] Scale(double[] vec, double alpha)
        {
            double[] res = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                res[i] = alpha * vec[i];
            }

            return res;
        }

        double[] Add(double[] first, double[] second)
        {
            double[] res = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                res[i] = first[i] + second[i];
            }

            return res;
        }

        double TimeTemp(int i)
        {
            return tN + h * b.C[i];
        }

        double[] YTemp(int i)
        {
            double[] ySnake = new double[dim];

            for (int j = 0; j < i; j++)
            {
                double[] yAdd = Scale(GetKiVector(j), b.A[i, j]);
                ySnake = Add(ySnake, yAdd);
            }

            ySnake = Scale(ySnake, h);
            return Add(yN, ySnake);
        }

        for (int k = 0; k < n; k++)
        {
            double[] phi = new double[dim];

            for (int i = 0; i < s; i++)
            {
                kHelp = f(TimeTemp(i), YTemp(i));
                for (int j = 0; j < dim; j++)
                {
                    Ki(i, j) = kHelp[j];
                }
            }

            for (int i = 0; i < s; i++)
            {
                phi = Add(phi, Scale(GetKiVector(i), b.B[i]));
            }

            phi = Scale(phi, h);

            yNext = Add(yN, phi);

            tN += h;
            yN = yNext;

        }

        return yN;
    }

    public static double ExperimentalConvergenceOrder(ButcherTableau b, Derivative f, double[] y0, double t0,
        double tEnd, int n)
    {
        double[] y1 = SolveExplicitRungeKutta(b, f, y0, t0, tEnd, n, -9.999);

        double[] y2 = SolveExplicitRungeKutta(b, f, y0, t0, tEnd, 2 * n, -9.999);

        double[] y3 = SolveExplicitRungeKutta(b, f, y0, t0, tEnd, 4 * n, -9.999);

        double value;

        double Norm(double[] y)
        {
            double n = 0;
            for (int i = 0; i < y.Length; i++)
            {
                n += Math.Pow(y[i], 2);
            }

            n = Math.Sqrt(n);
            return n;
        }

        double[] Subtract(double[] first, double[] second)
        {
            if (first.Length != second.Length)
                throw new ArgumentOutOfRangeException("Both arrays must match in dimension.");

            double[] res = new double[first.Length];

            for (int i = 0; i < first.Length; i++)
            {
                res[i] = first[i] - second[i];
            }

            return res;
        }

        value = Math.Log2(Norm(Subtract(y1, y2)) / Norm(Subtract(y2, y3)));
        return value;
    }

    class Program
    {
        static void Main(string[] args)
        {
            double[,] a = new double[,]
            {
                { 0, 0, 0 },
                { 1.0 / 3, 0, 0 },
                { 0, 2.0 / 3, 0 }
            };
            double[] be = new double[] { 1.0 / 4, 0, 3.0 / 4 };
            double[] c = new double[] { 0, 1.0 / 3, 2.0 / 3 };
            ButcherTableau b = new ButcherTableau(a, be, c, 0.1);
            Derivative f = (t, y) => new double[] { Math.Sin(t) + 4 - Math.Pow(y[0], 2) };
            double[] y0 = new double[] { 2 };
            double t0 = 0;
            double tmax = 0.4;
            int n = 1;
            double tol = -999;
            double[] solution = RungeKutta.SolveExplicitRungeKutta(b, f, y0, t0, tmax, n, tol);
            Console.WriteLine(string.Join(",", solution));
            double p = ExperimentalConvergenceOrder(b, f, y0, t0, tmax, 30);
            Console.WriteLine(p);
        }
    }
}