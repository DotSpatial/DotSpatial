﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Dialog for performing calculations on attributes.
    /// </summary>
    public partial class AttributeCalculator : Form
    {
        #region Fields

        private const double E = Math.E;
        private List<string> _fields = new List<string>();

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCalculator"/> class.
        /// This form will show to user to algebra calculation
        /// with attribute fields and user can save it back in attribute Table.
        /// </summary>
        public AttributeCalculator()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the user Added new field to the Table
        /// </summary>
        public event EventHandler NewFieldAdded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Expression of computation.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the feature set.
        /// </summary>
        public IFeatureSet FeatureSet { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This will display the Expression in RichText box
        /// </summary>
        public void DisplyExpression()
        {
            rtxtComputaion.Text = Expression;
        }

        /// <summary>
        /// This will load the attribute field in Table Field.
        /// </summary>
        /// <param name="list">List of fields.</param>
        public void LoadTableField(List<string> list)
        {
            _fields = list;
            comDestFieldComboBox.Items.Add("New Field");
            foreach (string st in list)
            {
                lstViewFields.Items.Add(st + "\n");
                comDestFieldComboBox.Items.Add(st);
            }
        }

        /// <summary>
        /// This will activate the NewFieldAdded event.
        /// </summary>
        protected virtual void OnNewFieldAdded()
        {
            NewFieldAdded?.Invoke(this, EventArgs.Empty);
        }

        private static double AcosFunction(double a)
        {
            if (a == 1)
                return 0;
            return a == -1 ? Math.PI : Math.Atan(-a / Math.Sqrt((-a * a) + 1)) + (2 * Math.Atan(1));
        }

        private static double AsinFunction(double a)
        {
            if (Math.Abs(a) == 1)
                return Math.Sign(a) * Math.PI / 2;
            return Math.Atan(a / Math.Sqrt((-a * a) + 1));
        }

        /// <summary>
        /// beta function
        /// </summary>
        /// <param name="z">z is not used</param>
        /// <param name="w">w is not used</param>
        /// <returns>The calculated value.</returns>
        private static double Beta(double z, double w)
        {
            return Math.Exp(GammaLn() + GammaLn() - GammaLn());
        }

        /// <summary>
        /// This will return true if string is Exponetial.eg 1.2E+2,
        /// </summary>
        /// <param name="arg">String  that is checked.</param>
        /// <returns>True, if t he string is exponential.</returns>
        private static bool CheckExpo(string arg)
        {
            bool exponentioal = false;
            string eguTrim = arg.Trim();
            if (arg.Length < 2)
                return false;

            if ((eguTrim.IndexOf("E", 0, 1, StringComparison.Ordinal) == 0) || (eguTrim.IndexOf("e", 0, 1, StringComparison.Ordinal) == 0))
            {
                exponentioal = true;
            }

            return exponentioal;
        }

        /// <summary>
        /// Combination n objects, k classes
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The resulting value.</returns>
        private static double Comb(double a, double b)
        {
            double y = 0.0;
            int n = Convert.ToInt32(a);
            int k = Convert.ToInt32(b);
            if (n < 0) return 0.0;
            if (k < 1 || k > n) k = 0;
            if (n == 0 || k == 0 || k == n) return 1.0;
            if (k > Convert.ToInt32(n / 2)) k = n - k;
            for (int i = 2; i < k + 1; i++)
                y = y * (n + 1 - i) / i;
            return y;
        }

        /// <summary>
        /// digamma function
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>The resulting value.</returns>
        private static double Digamma(double x)
        {
            double[] b1 = new double[12];
            double[] b2 = new double[12];
            double z;
            const short LimLow = 8;

            // Bernoulli's numbers
            b1[0] = 1;
            b2[0] = 1;
            b1[1] = 1;
            b2[1] = 6;
            b1[2] = -1;
            b2[2] = 30;
            b1[3] = 1;
            b2[3] = 42;
            b1[4] = -1;
            b2[4] = 30;
            b1[5] = 5;
            b2[5] = 66;
            b1[6] = -691;
            b2[6] = 2730;
            b1[7] = 7;
            b2[7] = 6;
            b1[8] = -3617;
            b2[8] = 360;
            b1[9] = 43867;
            b2[9] = 798;
            b1[10] = -174611;
            b2[10] = 330;
            b1[11] = 854513;
            b2[11] = 138;
            if (x < LimLow)
                z = x - 1 + LimLow;
            else
                z = x + 1;
            double s = 0;
            for (int k = 1; k < 12; k++)
            {
                double tem = b1[k] / b2[k] / k / Math.Pow(z, 2 * k);
                s += tem;
            }

            double y = Math.Log(z, E) + (0.5 * (1 / (z - s)));
            if (x <= LimLow)
            {
                s = 0;
                for (int k = 0; k < LimLow; k++)
                    s = (s + 1) / (x + k);
                y -= s;
            }

            return y;
        }

        /// <summary>
        /// Error distribution function.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>The resulting value.</returns>
        private static double Erf(double x)
        {
            const int Maxloop = 400;
            const double Tiny = 0.000000000000001;
            double y;
            double f2 = 0.0;
            if (x <= 2)
            {
                double t = 2 * x * x;
                double p = 1;
                double s = 1;
                for (int i = 3; i < Maxloop; i = i + 2)
                {
                    p = p * t / i;
                    s = s + p;
                    if (p < Tiny) break;
                }

                y = 2 * s * x * Math.Exp(-x * x) / Math.Sqrt(Math.PI);
            }
            else
            {
                double a0 = 1;
                double b0 = 0;
                double a1 = 0;
                double b1 = 1;
                double f1 = 0;
                for (int i = 1; i < Maxloop; i++)
                {
                    double g = 2 - (i % 2);
                    double a2 = (g * x * a1) + (i * a0);
                    double b2 = (g * x * b1) + (i * b0);
                    f2 = a2 / b2;
                    double d = Math.Abs(f2 - f1);
                    if (d < Tiny) break;
                    a0 = a1;
                    b0 = b1;
                    a1 = a2;
                    b1 = b2;
                    f1 = f2;
                }

                y = 1 - (2 * Math.Exp(-x * x) / ((2 * x) + f2) / Math.Sqrt(Math.PI));
            }

            return y;
        }

        // This will perform single function calculation.
        private static double EvaluateFunction(int index, double a, double b)
        {
            const double CvAngleCoeff = 1.0; // This has to assign proper value accoding to the unit as Radiaous or Degree.
            double result;
            switch (index)
            {
                case 0:
                    result = a + b;
                    break;
                case 1:
                    result = a - b;
                    break;
                case 2:
                    result = a * b;
                    break;
                case 3:
                    result = a / b;
                    break;
                case 4:
                    result = a * (b / 100);
                    break;
                case 5:
                    result = Math.Floor(a / b);
                    break;
                case 6:
                    result = Math.Pow(a, b);
                    break;
                case 7:
                    result = Math.Abs(a);
                    break;
                case 8:
                    result = Math.Atan(a) / CvAngleCoeff;
                    break;
                case 9:
                    result = Math.Cos(a * CvAngleCoeff);
                    break;
                case 10:
                    result = Math.Sin(a * CvAngleCoeff);
                    break;
                case 11:
                    result = Math.Exp(a);
                    break;
                case 13:
                    result = Math.Floor(a);
                    break;
                case 14:
                    result = Math.Floor(a);
                    break;
                case 67:
                    result = a - Math.Floor(a);
                    break;
                case 15:
                    result = Math.Log(a, Math.E);
                    break;
                case 16:
                    result = Math.Log10(a);
                    break;

                // case 17: result = Math.r // return a random number of  type single
                // break;
                case 18:
                    result = Math.Sign(a);
                    break;
                case 19:
                    result = Math.Sqrt(a);
                    break;
                case 65:
                    result = Math.Sign(a) * Math.Pow(Math.Abs(a), 1 / 3.0);
                    break;
                case 20:
                    result = Math.Tan(a * CvAngleCoeff);
                    break;
                case 21:
                    result = Math.Acos(a) / CvAngleCoeff;
                    break;
                case 22:
                    result = Math.Asin(a) / CvAngleCoeff;
                    break;
                case 23:
                    result = Math.Cosh(a);
                    break;
                case 24:
                    result = Math.Sinh(a);
                    break;
                case 25:
                    result = Math.Tanh(a);
                    break;
                case 26:
                    result = Math.Log(a + Math.Sqrt((a * a) - 1)); // what?
                    break;
                case 27:
                    result = Math.Log(a + Math.Sqrt((a * a) + 1));
                    break;
                case 28:
                    result = Math.Log((1 + a) / (1 - a) / 2);
                    break;

                case 66:
                    {
                        if (a > 0)
                        {
                            result = Math.Pow(a, 1 / b);
                        }
                        else
                        {
                            if ((b % 2) != 0)
                                result = Math.Pow(a, 1 / b);
                            else
                                throw new Exception(SymbologyFormsMessageStrings.AttributeCalculator_RootNthRootIncorrect);
                        }
                    }

                    break;
                case 29:
                    result = a % b; // Math.IEEERemainder(a, b); // check no
                    break;
                case 30:
                    result = Fact(a);
                    break;
                case 31:
                    result = Comb(a, b);
                    break;
                case 32:
                    result = Math.Min(a, b);
                    break;
                case 33:
                    result = Math.Max(a, b);
                    break;
                case 34:
                    result = Mcd(a, b);
                    break;
                case 35:
                    result = Mcm(a, b);
                    break;
                case 36:
                    {
                        result = a > b ? 1.0 : 0.0;
                    }

                    break;
                case 37:
                    {
                        result = a >= b ? 1.0 : 0.0;
                    }

                    break;
                case 38:
                    {
                        result = a < b ? 1.0 : 0.0;
                    }

                    break;
                case 39:
                    {
                        result = a <= b ? 1.0 : 0.0;
                    }

                    break;
                case 40:
                    {
                        result = a == b ? 1.0 : 0.0;
                    }

                    break;
                case 41:
                    {
                        result = a != b ? 1.0 : 0.0;
                    }

                    break;
                case 42:
                    {
                        if ((a != 0) && (b != 0))
                            result = 1.0;
                        else
                            result = 0.0;
                    }

                    break;
                case 43:
                    {
                        if ((a != 0) || (b != 0))
                            result = 1.0;
                        else
                            result = 0.0;
                    }

                    break;
                case 44:
                    {
                        result = a == 0 ? 1.0 : 0.0;
                    }

                    break;
                case 45:
                    {
                        result = (a != 0) != (b != 0) ? 1.0 : 0.0;
                    }

                    break;
                case 72:
                    {
                        if ((a == 0) || (b == 0))
                            result = 1.0;
                        else
                            result = 0.0;
                    }

                    break;
                case 73:
                    {
                        if ((a == 0) && (b == 0))
                            result = 1.0;
                        else
                            result = 0.0;
                    }

                    break;
                case 74:
                    {
                        result = (a != 0) == (b != 0) ? 1.0 : 0.0;
                    }

                    break;
                case 46:
                    result = Erf(a);
                    break;
                case 47:
                    result = Gamma();
                    break;
                case 48:
                    result = GammaLn();
                    break;
                case 49:
                    result = Digamma(a);
                    break;
                case 50:
                    result = Beta(a, b);
                    break;
                case 51:
                    result = Zeta(a);
                    break;
                case 52:
                    result = ExpIntegr(a);
                    break;
                case 53:
                    result = 1 / Math.Sin(a * CvAngleCoeff);
                    break;
                case 54:
                    result = 1 / Math.Cos(a * CvAngleCoeff);
                    break;
                case 55:
                    result = 1 / Math.Tan(a * CvAngleCoeff);
                    break;
                case 56:
                    result = AsinFunction(a);
                    break;
                case 57:
                    result = AcosFunction(a);
                    break;
                case 58:
                    result = Math.PI / (2 - Math.Atan(a)) / CvAngleCoeff;
                    break;
                case 59:
                    result = 1 / ((Math.Exp(a) - Math.Exp(-a)) / 2);
                    break;
                case 60:
                    result = 1 / ((Math.Exp(a) + Math.Exp(-a)) / 2);
                    break;
                case 61:
                    result = 1 / ((Math.Exp(a) - Math.Exp(-a)) / (Math.Exp(a) + Math.Exp(-a)));
                    break;
                case 62:
                    result = Math.Log((1 / a) + Math.Sqrt((1 / a * 1 / a) + 1), E);
                    break;
                case 63:
                    result = Math.Log((1 / a) + Math.Sqrt((1 / a * 1 / a) - 1), E);
                    break;
                case 64:
                    result = Math.Log((1 + (1 / a)) / (1 - (1 / a)), E) / 2;
                    break;
                case 68:
                    result = a / CvAngleCoeff;
                    break;
                case 69:
                    result = a / CvAngleCoeff * Math.PI / 180;
                    break;
                case 71:
                    result = a / CvAngleCoeff * Math.PI / 200;
                    break;
                case 70:
                    result = RoundFunction(a, b);
                    break;
                default:
                    throw new Exception(SymbologyFormsMessageStrings.AttributeCalculator_NonStandardFunction);
            }

            return result;
        }

        /// <summary>
        /// exponential integral Ei(x) for x >0.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>The return value.</returns>
        private static double ExpIntegr(double x)
        {
            const double Eps = 0.000000000000001;
            const double Euler = 0.577215664901532;
            const int Maxit = 100;
            const double Fpmin = 1E-30;
            if (x <= 0)
                return 0;
            if (x < Fpmin) // Special case: avoid failure of convergence test because of under ow
                return Math.Log(x, E) + Euler;
            double sum;
            double term;
            if (x <= -Math.Log(Eps, E))
            {
                sum = 0;
                double fact = 1;
                for (int k = 1; k < Maxit + k; k++)
                {
                    fact = fact * x / k;
                    term = fact / k;
                    sum += term;
                    if (term < Eps * sum)
                        break;
                }

                return Math.Log(Eps, E); // cause of under ow.
            }

            sum = 0; // Start with second term.
            term = 1;
            for (int k = 1; k < Maxit + 1; k++)
            {
                double prev = term;
                term = term * k / x;
                if (term < Eps) break; // Since al sum is greater than one, term itself ap-
                if (term < prev)
                {
                    sum += term; // Still converging: add new term.
                }
                else
                {
                    sum -= prev;
                    break;
                }
            }

            return Math.Exp(x) * (1 + sum) / x;
        }

        /// <summary>
        /// Calculate the Factorial in given number.
        /// </summary>
        /// <param name="n">The number.</param>
        /// <returns>The calculated value.</returns>
        private static double Fact(double n)
        {
            if (n < 0) throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_FactGreaterThanZero);
            if (n == 1.0 || n == 0.0)
            {
                return 1.0;
            }

            double res = 1.0;
            for (int i = 0; i < Math.Floor(n); i++)
                res = res * (i + 1);
            return res;
        }

        /// <summary>
        /// gamma function
        /// </summary>
        /// <returns>The calculated value.</returns>
        private static double Gamma()
        {
            double mantissa;
            int expo;
            GammaSplit(out mantissa, out expo);
            return mantissa * Math.Pow(10, expo);
        }

        /// <summary>
        /// logarithm gamma function
        /// </summary>
        /// <returns>The calculated value.</returns>
        private static double GammaLn()
        {
            double mantissa;
            int expo;
            GammaSplit(out mantissa, out expo);
            return Math.Log(mantissa, E) + (expo * Math.Log(10, E));
        }

        /// <summary>
        /// gamma  - Lanczos approximation algorithm for gamma function
        /// </summary>
        /// <param name="mantissa">The mantissa.</param>
        /// <param name="expo">The exponent.</param>
        private static void GammaSplit(out double mantissa, out int expo)
        {
            const double Z = 0.0;
            double[] cf = new double[15];
            const double Doublepi = 6.28318530717959;
            const double G = 4.7421875;
            cf[0] = 0.999999999999997;
            cf[1] = 57.1562356658629;
            cf[2] = -59.5979603554755;
            cf[3] = 14.1360979747417;
            cf[4] = -0.49191381609762;
            cf[5] = 0.0000339946499848119;
            cf[6] = 0.0000465236289270486;
            cf[7] = -0.0000983744753048796;
            cf[8] = 0.000158088703224912;
            cf[9] = -0.000210264441724105;
            cf[10] = 0.000217439618115213;
            cf[11] = -0.000164318106536764;
            cf[12] = 0.0000844182239838528;
            cf[13] = -0.0000261908384015814;
            cf[14] = 0.00000368991826595316;

            double w = Math.Exp(G) / Math.Sqrt(Doublepi);
            double s = cf[0];
            for (int i = 1; i < 15; i++)
            {
                s += cf[i] / (Z + 1);
            }

            s /= w;
            double p = Math.Log((Z + G + 0.5) / Math.Exp(1), E) * (Z + 0.5) / Math.Log(10, E);

            // split in mantissa and exponent to avoid overflow
            expo = Convert.ToInt32(Math.Floor(p));
            p -= expo;
            mantissa = Math.Pow(10, p) * s;

            // rescaling
            p = Math.Floor(Math.Log(mantissa, E) / Math.Log(10, E));
            mantissa = mantissa * Math.Pow(10, -p);
            expo += Convert.ToInt32(p);
        }

        /// <summary>
        /// This give the relavent token no.
        /// </summary>
        /// <param name="subExp">Function Name</param>
        /// <returns>The calculated value.</returns>
        private static int GetTokenNo(string subExp)
        {
            subExp = subExp.Trim();
            int tok;
            switch (subExp.ToLower())
            {
                case "+":
                    tok = 0;
                    break;
                case "-":
                    tok = 1;
                    break;
                case "*":
                    tok = 2;
                    break;
                case "/":
                    tok = 3;
                    break;
                case "%":
                    tok = 4;
                    break;

                // case ("\") : tok=5;
                // break;
                case "^":
                    tok = 6;
                    break;
                case "abs(":
                    tok = 7;
                    break;
                case "atan(":
                    tok = 8;
                    break;
                case "cos(":
                    tok = 9;
                    break;
                case "sin(":
                    tok = 10;
                    break;
                case "exp(":
                    tok = 11;
                    break;

                // case ("exp(") : tok=12;
                //    break;
                case "fix(":
                    tok = 13;
                    break;
                case "int(":
                    tok = 14;
                    break;
                case "dec(":
                    tok = 67;
                    break;
                case "ln(":
                    tok = 15;
                    break;
                case "log(":
                    tok = 16;
                    break;
                case "rnd(":
                    tok = 17;
                    break;
                case "sgn(":
                    tok = 18;
                    break;
                case "sqr(":
                    tok = 19;
                    break;
                case "cbr(":
                    tok = 65;
                    break;
                case "tan(":
                    tok = 20;
                    break;
                case "acos(":
                    tok = 21;
                    break;
                case "asin(":
                    tok = 22;
                    break;
                case "cosh(":
                    tok = 23;
                    break;
                case "sinh(":
                    tok = 24;
                    break;
                case "tanh(":
                    tok = 25;
                    break;
                case "acosh(":
                    tok = 26;
                    break;
                case "asinh(":
                    tok = 27;
                    break;
                case "atanh(":
                    tok = 28;
                    break;
                case "root(":
                    tok = 66;
                    break;
                case "mod(":
                    tok = 29;
                    break;
                case "!":
                case "fact(":
                    tok = 30;
                    break;
                case "comb(":
                    tok = 31;
                    break;
                case "min(":
                    tok = 32;
                    break;
                case "max(":
                    tok = 33;
                    break;
                case "gcd(":
                case "mcd(":
                    tok = 34;
                    break;
                case "mcm(":
                case "lcm(":
                    tok = 35;
                    break;
                case ">":
                    tok = 36;
                    break;
                case ">=":
                case "=>":
                    tok = 37;
                    break;
                case "<":
                    tok = 38;
                    break;
                case "<=":
                case "=<":
                    tok = 39;
                    break;
                case "=":
                    tok = 40;
                    break;
                case "<>":
                    tok = 41;
                    break;
                case "and":
                    tok = 42;
                    break;
                case "or":
                    tok = 43;
                    break;
                case "not":
                    tok = 44;
                    break;
                case "xor":
                    tok = 45;
                    break;
                case "nand":
                    tok = 72;
                    break;
                case "nor":
                    tok = 73;
                    break;
                case "nxor":
                    tok = 74;
                    break;
                case "erf(":
                    tok = 46;
                    break;
                case "gamma(":
                    tok = 47;
                    break;
                case "gammaln(":
                    tok = 48;
                    break;
                case "digamma(":
                    tok = 49;
                    break;
                case "beta(":
                    tok = 50;
                    break;
                case "zeta(":
                    tok = 51;
                    break;
                case "ei(":
                    tok = 52;
                    break;
                case "csc(":
                    tok = 53;
                    break;
                case "sec(":
                    tok = 54;
                    break;
                case "cot(":
                    tok = 55;
                    break;
                case "acsc(":
                    tok = 56;
                    break;
                case "asec(":
                    tok = 57;
                    break;
                case "acot(":
                    tok = 58;
                    break;
                case "csch(":
                    tok = 59;
                    break;
                case "sech(":
                    tok = 60;
                    break;
                case "coth(":
                    tok = 61;
                    break;
                case "acsch(":
                    tok = 62;
                    break;
                case "asech(":
                    tok = 63;
                    break;
                case "acoth(":
                    tok = 64;
                    break;
                case "rad(":
                    tok = 68;
                    break;
                case "deg(":
                    tok = 69;
                    break;
                case "grad(":
                    tok = 71;
                    break;
                case "round(":
                    tok = 70;
                    break;

                default:
                    tok = -1;
                    break;
            }

            return tok;
        }

        /// <summary>
        /// Checks if the given argument is a number.
        /// x must have always the decimal point "123.756", ".0056", "1.3455E-12"
        /// </summary>
        /// <param name="arg">Argument checked for being a number.</param>
        /// <returns>True, if the argument is a number.</returns>
        private static bool IsNumeric(IEnumerable<char> arg)
        {
            foreach (char c in arg)
            {
                if (!char.IsNumber(c)) return false;
            }

            return true;
        }

        /// <summary>
        /// FIdx the MCD between two integer numbers
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>The calculated value.</returns>
        private static double Mcd(double a, double b)
        {
            double y = a;
            double x = b;
            while (x > 0)
            {
                double r = y % x;
                y = x;
                x = r;
            }

            return y;
        }

        /// <summary>
        /// FIdx the mcm between two integer numbers
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>The calculated value.</returns>
        private static double Mcm(double a, double b)
        {
            return a * b / Mcd(a, b);
        }

        private static double RoundFunction(double x, double d)
        {
            double b = Math.Pow(10, d);
            x = x * b;
            double xi = Convert.ToInt32(Math.Floor(x));
            double xd = x - xi;
            if (xd > 0.5) xi++;
            return xi / b;
        }

        /// <summary>
        /// Riemman's zeta function
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>The calculated value.</returns>
        private static double Zeta(double x)
        {
            double coeff = 1.0;
            const int NMax = 1000;
            const double Tiny = 0.0000000000000001;
            int n = 0;
            double s = 0;
            while (Math.Abs(coeff) < Tiny || n > NMax)
            {
                double s1 = 0;
                double cnk = 1;
                for (int k = 0; k < n + 1; k++)
                {
                    if (k > 0)
                        cnk = cnk * (n - k + 1) / k;
                    s1 = s1 + (Math.Pow(-1, k) * cnk / Math.Pow(k + 1, x));
                }

                coeff = s1 / Math.Pow(2, 1 + n);
                s += coeff;
                n++;
            }

            return s / (1 - Math.Pow(2, 1 - x));
        }

        private void BtnClaculateClick(object sender, EventArgs e)
        {
            string expression = rtxtComputaion.Text;
            ParExp(expression);
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnDivideClick(object sender, EventArgs e)
        {
            const string Division = " / ";
            Expression = rtxtComputaion.Text;
            Expression = Expression + Division;
            DisplyExpression();
        }

        private void BtnMinusClick(object sender, EventArgs e)
        {
            const string Minus = " - ";
            Expression = rtxtComputaion.Text;
            Expression = Expression + Minus;
            DisplyExpression();
        }

        private void BtnMultiplyClick(object sender, EventArgs e)
        {
            const string Times = " * ";
            Expression = rtxtComputaion.Text;
            Expression = Expression + Times;
            DisplyExpression();
        }

        private void BtnPlusClick(object sender, EventArgs e)
        {
            const string Plus = " + ";
            Expression = rtxtComputaion.Text;
            Expression = Expression + Plus;
            DisplyExpression();
        }

        private bool CreateNewColumn()
        {
            using (var addCol = new AddNewColum())
            {
                if (addCol.ShowDialog(this) == DialogResult.OK)
                {
                    FeatureSet.DataTable.Columns.Add(addCol.Name, addCol.Type);
                    OnNewFieldAdded();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This will check and return the column index as given field name or -1 for not find.
        /// </summary>
        /// <param name="arg">Name of the field that should be found.</param>
        /// <returns>The index of the field.</returns>
        private int FindFieldInDataTable(string arg)
        {
            for (int j = 0; j < _fields.Count; j++)
            {
                // check in the field of datatable
                if (arg == _fields[j])
                {
                    return j;
                }
            }

            return -1;
        }

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CalculatorUserGuide calUseGui = new CalculatorUserGuide();
            calUseGui.ShowDialog();
        }

        /// <summary>
        /// Tral: This will divide the string to relavent functions level
        /// </summary>
        /// <param name="exp">The exponent.</param>
        private void ParExp(string exp)
        {
            const string Fun1Full = "Abs( Atan( Cos( Exp( Fix( Int( Ln( Log( Rnd( Sgn( Sin( Sqr( Cbr (Tan( Acos( Asin( " + "Cosh( Sinh( Tanh( Acosh( Asinh( Atanh( Fact( Not( Erf( Gamma( Gammaln( Digamma( Zeta( Ei( " + "csc( sec( cot( acsc( asec( acot( csch( sech( coth( acsch( asech( acoth( Dec( Rad( Deg( Grad(";
            const string Fun2Full = "Comb( Max( Min( Mcm( Mcd( Lcm( Gcd( Mod( And( Or( Xor( Beta( Root( Round( Nand( Nor( NXor(";
            int numPart = 0;
            char[] seperators = { '[', ']', ' ', ',' };
            string[] split = exp.Split(seperators, StringSplitOptions.None);
            int noOfSplit = split.Length;
            FunClass[] functionClsArr = new FunClass[noOfSplit]; // assign no of arg to no of split string
            FunClass tempClass;
            bool isFunctionEndingBracket = false;
            for (int i = 0; i < noOfSplit; i++)
            {
                bool isField = false;
                var st = split[i];
                st = st.Trim();
                if (string.IsNullOrEmpty(st)) continue;
                int num;
                switch (st)
                {
                    case " ":
                        break;
                    case ",":
                        break;
                    case "(": // process Open Bracket
                    case "{":
                        numPart++;
                        break;

                    case ")": // process Closed Bracket
                    case "}":
                        {
                            if (st == ")" && i > 1)
                            {
                                // consider closing functions
                                // check it is normal closing bracket or function closing bracket
                                num = i;
                                for (int l = i - 1; l > -1; l--)
                                {
                                    if (functionClsArr[l] != null)
                                    {
                                        num--;
                                        if (functionClsArr[l] != null)
                                        {
                                            if (functionClsArr[l].NoOfArg == 1 && num == i - 2)
                                            {
                                                // mono function
                                                isFunctionEndingBracket = true;
                                                break;
                                            }

                                            if (functionClsArr[l].NoOfArg == 3 && num == i - 3)
                                            {
                                                // di function
                                                isFunctionEndingBracket = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (isFunctionEndingBracket == false)
                                numPart -= 1;
                            else // No need to subtract
                                isFunctionEndingBracket = false;
                            if (numPart < 0)
                                throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_TooManyClosingBrackets);
                        }

                        break;

                    case "+":
                    case "-":
                        string preSt = split[i - 1];
                        if (CheckExpo(preSt))
                        {
                            tempClass = new FunClass("exp", i)
                            {
                                PriorityLevel = 8 + (numPart * 10)
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 1;
                        }
                        else
                        {
                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = 2 + (numPart * 10)
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 2;
                        }

                        break;

                    case "*":
                    case "/":
                        tempClass = new FunClass(st, i)
                        {
                            PriorityLevel = 3 + (numPart * 10)
                        };
                        functionClsArr[i] = tempClass;
                        functionClsArr[i].NoOfArg = 2;
                        break;

                    case "^":
                        tempClass = new FunClass(st, i)
                        {
                            PriorityLevel = 4 + (numPart * 10)
                        };
                        functionClsArr[i] = tempClass;
                        functionClsArr[i].NoOfArg = 2;
                        break;

                    case "%":
                        tempClass = new FunClass(st, i)
                        {
                            PriorityLevel = 4 + (numPart * 10)
                        };
                        functionClsArr[i] = tempClass;
                        functionClsArr[i].NoOfArg = 2;
                        break;

                    case "!":
                        tempClass = new FunClass("fact(", i)
                        {
                            PriorityLevel = 7 + (numPart * 10)
                        };
                        functionClsArr[i] = tempClass;
                        functionClsArr[i].NoOfArg = 1;
                        functionClsArr[i].NextArgument = functionClsArr[i - 1].FunctionName;
                        break;

                    case ";":
                        // comes from bivariate function f(x;y)
                        break;
                    case "|":
                        // absolute symbol |.|
                        if (IsNumeric(split[i - 1]))
                        {
                            // ending absolte symbol
                            numPart--;
                            if (numPart < 0)
                            {
                                throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_TooManyClosingBrackets);
                            }
                        }
                        else
                        {
                            // start absolute
                            numPart++;
                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = numPart * 10
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 1;
                        }

                        break;

                    case "=":
                    case "<":
                    case ">": // Logical operators
                        tempClass = new FunClass(st, i)
                        {
                            PriorityLevel = 1 + (numPart * 10)
                        };
                        functionClsArr[i] = tempClass;
                        functionClsArr[i].NoOfArg = 2;
                        break;

                    case "X": // Not use full in practical
                    case "x":
                    case "Y":
                    case "y":
                    case "Z": // monomial coeff.
                    case "z": // Ex: 7x  is converted into product 7*x
                        if (IsNumeric(split[i + 1]))
                        {
                            tempClass = new FunClass("*", i)
                            {
                                PriorityLevel = 3 + (numPart * 10)
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 2;
                        }

                        break;

                    default:
                        if (IsNumeric(st))
                        {
                            // Check with Numeric number
                            if (st.IndexOf(")", st.Length - 1, StringComparison.Ordinal) == st.Length - 1 && st.Length > 1)
                            {
                                // this will check the following 24).closing bracket with out space in between
                                st = st.Remove(st.Length - 1, 1);
                                st = st.Trim();

                                // check it is normal closing bracket or function closing bracket
                                num = i;
                                for (int l = i - 1; l > -1; l--)
                                {
                                    if (functionClsArr[l] != null)
                                    {
                                        num--;
                                        if (functionClsArr[l] != null)
                                        {
                                            if (functionClsArr[l].NoOfArg == 1 && num == i - 1)
                                            {
                                                // mono function
                                                isFunctionEndingBracket = true;
                                                break;
                                            }

                                            if (functionClsArr[l].NoOfArg == 3 && num == i - 2)
                                            {
                                                // di function
                                                isFunctionEndingBracket = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (isFunctionEndingBracket == false)
                                    numPart--;
                                else
                                    isFunctionEndingBracket = false;
                            }

                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = 0
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 0;
                            break;
                        }

                        if (st.IndexOf("(", 0, StringComparison.Ordinal) == 0 && st.Length > 1)
                        {
                            // this will check the following 24).starting bracket with out space in between
                            st = st.Remove(0, 1);
                            st = st.Trim();
                            numPart++;

                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = 0
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 0;
                            break;
                        }

                        if (Fun1Full.ToLower().Contains(st.ToLower()))
                        {
                            // MonoVariable function
                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = 9 + (numPart * 10)
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 1;
                            break;
                        }

                        if (Fun2Full.ToLower().Contains(st.ToLower()))
                        {
                            // BiVariable function
                            tempClass = new FunClass(st, i)
                            {
                                PriorityLevel = 9 + (numPart * 10)
                            };
                            functionClsArr[i] = tempClass;
                            functionClsArr[i].NoOfArg = 3; // mean it need to take next two arguments but in between ";" sysmbol
                            break;
                        }

                        foreach (string field in _fields)
                        {
                            if (st == field)
                            {
                                tempClass = new FunClass(st, i)
                                {
                                    PriorityLevel = 0
                                };
                                functionClsArr[i] = tempClass;
                                functionClsArr[i].NoOfArg = 0;
                                isField = true;
                                break;
                            }
                        }

                        if (isField == false)
                        {
                            MessageBox.Show("Unrecognized format.");
                            return;
                        }

                        break;
                }
            }

            int count = 0; // index after filtered

            // sort the array.
            List<FunClass> templist1 = new List<FunClass>();
            foreach (FunClass funcls in functionClsArr)
            {
                if (funcls != null)
                {
                    funcls.Index = count;
                    templist1.Add(funcls);
                    count++;
                }
            }

            int noOfArr = templist1.Count;

            // assign the relavent variables
            for (int i = 0; i < noOfArr; i++)
            {
                if (templist1[i].NoOfArg == 1)
                {
                    if (i < noOfArr - 1 && templist1[i].NextArgument == null)
                        templist1[i].NextArgument = templist1[i + 1].FunctionName;
                }
                else if (templist1[i].NoOfArg == 2)
                {
                    if (i < noOfArr - 1)
                        templist1[i].NextArgument = templist1[i + 1].FunctionName;
                    if (i > 0)
                        templist1[i].PreviousArgument = templist1[i - 1].FunctionName;
                }
                else if (templist1[i].NoOfArg == 3)
                {
                    // need to modify to handle continues two arguments
                    if (i < noOfArr - 2)
                    {
                        templist1[i].PreviousArgument = templist1[i + 1].FunctionName;
                        templist1[i].NextArgument = templist1[i + 2].FunctionName;
                    }
                }
            }

            // sort according to Priority Level
            for (int a = 1; a < noOfArr; a++)
            {
                if (templist1[a].PriorityLevel > templist1[a - 1].PriorityLevel)
                {
                    // int b = a;
                    for (int c = a; c > 0; c--)
                    {
                        if (templist1[c].PriorityLevel > templist1[c - 1].PriorityLevel)
                        {
                            tempClass = templist1[c];
                            templist1[c] = templist1[c - 1];
                            templist1[c - 1] = tempClass;
                        }
                    }
                }
            }

            // arrange operation next to previous priority level operation in same prority level oprations
            for (int a = 2; a < noOfArr; a++)
            {
                if (templist1[a].PriorityLevel == templist1[a - 1].PriorityLevel && templist1[a].PriorityLevel > 0)
                {
                    if (a > 1)
                    {
                        // At least 3 operations need, 1 higher prority and 2 same prorities
                        if (templist1[a - 2].Index > templist1[a].Index)
                        {
                            if (templist1[a].Index > templist1[a - 1].Index)
                            {
                                tempClass = templist1[a];
                                templist1[a] = templist1[a - 1];
                                templist1[a - 1] = tempClass;
                            }
                        }
                        else if (templist1[a - 2].Index < templist1[a].Index)
                        {
                            if (templist1[a].Index < templist1[a - 1].Index)
                            {
                                tempClass = templist1[a];
                                templist1[a] = templist1[a - 1];
                                templist1[a - 1] = tempClass;
                            }
                        }
                    }
                }
            }

            // take selected destination field from combo box
            int destinationCol = comDestFieldComboBox.SelectedIndex;
            if (destinationCol == -1)
            {
                // set to new field if user not selected
                destinationCol = 0;
            }

            if (destinationCol == 0)
            {
                // create new field
                if (CreateNewColumn() == false)
                {
                    return;
                }

                destinationCol = FeatureSet.DataTable.Columns.Count; // point the last field
            }

            // execute the equation through records
            for (int i = 0; i < FeatureSet.DataTable.Rows.Count; i++)
            {
                double finanalVal = 1.0;
                string arg1;
                if (noOfArr > 1)
                {
                    for (int a = 0; a < noOfArr; a++)
                    {
                        if (templist1[a].NoOfArg == 0)
                        {
                            // check it is argument.
                            continue;
                        }

                        bool isField1 = false;
                        bool isField2 = false;
                        bool isArgAssign = false;
                        bool noNeedToAssign = false;
                        double argVal1 = 0.0;
                        double argVal2 = 0.0;
                        double val;
                        int colField1 = 0; // save the colum index of the field

                        if (templist1[a].NoOfArg == 1)
                        {
                            // mono argument function
                            arg1 = templist1[a].NextArgument;
                            if (IsNumeric(arg1) == false)
                            {
                                // Get the field from data Table
                                for (int j = 0; j < _fields.Count; j++)
                                {
                                    if (arg1 == _fields[j])
                                    {
                                        colField1 = j;
                                        isField1 = true;
                                        break;
                                    }
                                }
                            }

                            if (isField1)
                            {
                                argVal1 = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                            }
                            else if (IsNumeric(arg1))
                            {
                                argVal1 = Convert.ToDouble(arg1);
                            }
                            else
                            {
                                throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_ParameterInvalid);
                            }

                            val = EvaluateFunction(GetTokenNo(templist1[a].FunctionName), argVal1, 0.0); // EXECUTE operation
                            templist1[a].Value = val;
                            finanalVal = finanalVal * val;
                        }
                        else
                        {
                            string arg2;
                            if (templist1[a].NoOfArg == 2)
                            {
                                // operator
                                arg1 = templist1[a].PreviousArgument;
                                arg2 = templist1[a].NextArgument;
                                if (IsNumeric(arg1) == false)
                                {
                                    // Get the field from data Table
                                    for (int j = 0; j < _fields.Count; j++)
                                    {
                                        // check in the field of datatable
                                        if (arg1 == _fields[j])
                                        {
                                            colField1 = j;
                                            isField1 = true;
                                            break;
                                        }
                                    }
                                }

                                if (a > 0)
                                {
                                    // take the value from previous operation
                                    for (int r = a; r > -1; r--)
                                    {
                                        // go through previous calculated functions
                                        if (Equals(noNeedToAssign, true))
                                            break;
                                        if (templist1[a].Index > templist1[r].Index)
                                        {
                                            // check the prority in left side
                                            for (int b = a; b < noOfArr; b++)
                                            {
                                                // this will check the rest operation that in between current and Previous prority operation
                                                if (templist1[a].Index - 2 == templist1[b].Index && templist1[b].NoOfArg > 0)
                                                {
                                                    noNeedToAssign = true;
                                                    break;
                                                }
                                            }

                                            if (templist1[r].Value != 0 && noNeedToAssign == false)
                                            {
                                                argVal1 = templist1[r].Value;
                                                isArgAssign = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (isField1 && isArgAssign == false)
                                {
                                    argVal1 = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                                }
                                else if (IsNumeric(arg1) && isArgAssign == false)
                                {
                                    argVal1 = Convert.ToDouble(arg1);
                                }
                                else if (isArgAssign == false)
                                {
                                    throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_ParameterInvalid);
                                }

                                isArgAssign = false; // reset flag
                                noNeedToAssign = false;

                                if (IsNumeric(arg2) == false)
                                {
                                    // Get the field from data Table
                                    // Caroso add code to determine the second field
                                    int j = 0;
                                    foreach (string t in _fields)
                                    {
                                        if (arg2 == t)
                                        {
                                            colField1 = j;
                                            isField2 = true;
                                            break;
                                        }

                                        j++;
                                    }
                                }

                                if (a > 0)
                                {
                                    // take the value from previous operation
                                    for (int r = a; r > -1; r--)
                                    {
                                        // go through previous calculated functions
                                        if (noNeedToAssign) break;
                                        if (templist1[a].Index < templist1[r].Index)
                                        {
                                            // check the prority in right side
                                            // Take only if it is next opration
                                            for (int b = a; b < noOfArr; b++)
                                            {
                                                // this will check the rest operation that in between current and Previous prority operation
                                                if (templist1[a].Index == templist1[b].Index - 2 && templist1[b].NoOfArg > 0)
                                                {
                                                    noNeedToAssign = true;
                                                    break;
                                                }
                                            }

                                            if (templist1[r].Value != 0 && noNeedToAssign == false)
                                            {
                                                argVal2 = templist1[r].Value;
                                                isArgAssign = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (isField2 && isArgAssign == false)
                                {
                                    argVal2 = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                                }
                                else if (IsNumeric(arg2) && isArgAssign == false)
                                {
                                    argVal2 = Convert.ToDouble(arg2);
                                }
                                else if (isArgAssign == false)
                                {
                                    argVal2 = 0.0;
                                }

                                // execute the operation
                                val = EvaluateFunction(GetTokenNo(templist1[a].FunctionName), argVal1, argVal2);
                                templist1[a].Value = val;
                                finanalVal = val;
                            }
                            else if (templist1[a].NoOfArg == 3)
                            {
                                // di argument function
                                arg1 = templist1[a].PreviousArgument;
                                arg2 = templist1[a].NextArgument;
                                if (IsNumeric(arg1) == false)
                                {
                                    // Get the field from data Table
                                    for (int j = 0; j < _fields.Count; j++)
                                    {
                                        // check in the field of datatable
                                        if (arg1 == _fields[j])
                                        {
                                            colField1 = j;
                                            isField1 = true;
                                            break;
                                        }
                                    }
                                }

                                if (isField1)
                                {
                                    argVal1 = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                                }
                                else if (IsNumeric(arg1))
                                {
                                    argVal1 = Convert.ToDouble(arg1);
                                }
                                else
                                {
                                    throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_ParameterInvalid);
                                }

                                if (IsNumeric(arg2) == false)
                                {
                                    // Get the field from data Table
                                    foreach (string t in _fields)
                                    {
                                        if (arg2 == t)
                                        {
                                            isField2 = true;
                                            break;
                                        }
                                    }
                                }

                                if (isField2)
                                {
                                    argVal2 = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                                }
                                else if (IsNumeric(arg2))
                                {
                                    argVal2 = Convert.ToDouble(arg2);
                                }
                                else
                                {
                                    throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_ParameterInvalid);
                                }

                                // execute the operation
                                val = EvaluateFunction(GetTokenNo(templist1[a].FunctionName), argVal1, argVal2);
                                templist1[a].Value = val;
                                finanalVal = val;
                            }
                        }
                    }

                    // loop through all operation
                }
                else if (noOfArr == 1 && templist1[0].NoOfArg == 0)
                {
                    // Simple case only one variable or one argument
                    arg1 = templist1[0].FunctionName;
                    int colField1 = FindFieldInDataTable(arg1);
                    if (colField1 != -1)
                    {
                        // Field inside the Table
                        finanalVal = Convert.ToDouble(FeatureSet.DataTable.Rows[i][colField1]);
                    }
                    else if (IsNumeric(arg1))
                    {
                        finanalVal = Convert.ToDouble(arg1);
                    }
                    else
                    {
                        throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_ParameterInvalid);
                    }
                }
                else
                {
                    MessageBox.Show("There is no argument");
                    return;
                }

                // Save the final value to the field under attribute
                if (FeatureSet.DataTable.Columns[destinationCol - 1].DataType == typeof(double))
                {
                    FeatureSet.DataTable.Rows[i][destinationCol - 1] = finanalVal;
                }
                else if (FeatureSet.DataTable.Columns[destinationCol - 1].DataType == typeof(string))
                {
                    FeatureSet.DataTable.Rows[i][destinationCol - 1] = Convert.ToString(finanalVal);
                }
                else if (FeatureSet.DataTable.Columns[destinationCol - 1].DataType == typeof(int))
                {
                    FeatureSet.DataTable.Rows[i][destinationCol - 1] = Convert.ToInt32(finanalVal);
                }
                else
                {
                    throw new ArgumentException(SymbologyFormsMessageStrings.AttributeCalculator_IncompatibleDestinationField);
                }
            }

            // Loop through records
            Hide();
        }

        #endregion
    }
}