using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace stanclova_knihovna__matematickych_funkci
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<MathFunction> functions = new List<MathFunction>()
            {
                new Linear(-2, -3),
                new Linear(0, 3),
                new LinearAbs(0, 3),
                new LinearAbs(2, 3),
                new LinearFract(0, 1, 1, 0),
                new LinearFract(-2, 1, 1, 0),
                new LinearFract(0, 0, 0, 0),
                new LinearFract(1, 1, 0, 1),
                new Quadratic (1, -2, 1),
                new Quadratic (-1, 2, -1),
                new Quadratic(0, 1, 2),
                new Quadratic(0, 0, 6),
            };

            double x = 2;

            foreach (var f in functions)
            {
                //Console.WriteLine(f.Description);
                //Console.WriteLine(f.Name);
                f.PrintInfo(x);
                //Console.WriteLine($"f({x}) = {y}");
                Console.WriteLine("");
                /*if (f is IDifferentiable)
                    Console.WriteLine(((IDifferentiable)f).OutputDerivative());*/
            }

            Console.ReadLine();
        }

        //int a = int.MaxValue;
        //if (a == int.MaxValue) ... neodecitam
    }
}


struct Interval
{
    public double UpperLimit { get; }
    public double LowerLimit { get; }
    public char UpperLimitBracket { get; }
    public char LowerLimitBracket { get; }
    public double ExceptionValue { get; }

    public Interval(char lowbracket, double lowlimit, double uplimit, char upbracket, double exceptionvalue)
    {
        UpperLimit = uplimit;
        LowerLimit = lowlimit;
        UpperLimitBracket = upbracket;
        LowerLimitBracket = lowbracket;
        ExceptionValue = exceptionvalue;
    }


    public string PrintToString()
    {
        string text = "";
        /*string lttext = "";
        string uttext = "";

        if (LowerLimit == double.NegativeInfinity)
        {
            lttext = "- nekonecno";
        }
        else 
        {
            lttext = LowerLimit.ToString();
        }

        if (UpperLimit == double.PositiveInfinity)
        {
            uttext = "+ nekonecno";
        }
        else
        {
            uttext = UpperLimit.ToString();
        }
        */
        text = "" + LowerLimitBracket + LowerLimit + " ; " + UpperLimit + UpperLimitBracket;

        if (!double.IsNaN(ExceptionValue))
        {
            text += " - {" + ExceptionValue + "}";
        }

        return text;
    }
}



abstract class MathFunction
{
    public string Name { get; protected set; }
    public string Description { get; set; }
    public string Derivation { get; set; }
    public Interval Domain { get; protected set; } //definicni obor
    public Interval Range { get; protected set; } //obor hodnot


    public MathFunction(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract double CalculateFunction(double x);


    public virtual void PrintInfo(double x) 
    {
        string text = Domain.PrintToString();
        Console.WriteLine($"{Name}: {Description} na D(f) = {text}");

        text = Range.PrintToString();
        Console.WriteLine($"H(f) = {text}");

        double y = CalculateFunction(x);
        Console.WriteLine($"f({x}) = {y}");

        Console.WriteLine(Derivation);
    }
}


//----------------------------------------------------------------------//
//                           LINEÁRNÍ FUNKCE                            //
//----------------------------------------------------------------------//
class Linear : MathFunction //osetrit ze a = 0 ... obor hodnot je [b,b]
{
    private double a;
    private double b;

    public Linear(double a, double b) : base("Lineární funkce", $"f(x) = {a}x + {b}") // ax + b
    {
        //mám 2 proměnné a (co dostanu a co mám jako proměnou -> this říkám, že to bude proměná té třídy)
        string descB;

        this.a = a;

        this.b = b;
        descB = (b < 0) ? $"{b}" : $"+ {b}";

        Description = $"f(x) = {a}x {descB}";


        Derivation = $"f'(x) = {a}";


        //definiční obor
        Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);

        //obor hodnot
        if (a == 0) 
        { 
            Range = new Interval('[', b, b, ']', double.NaN);
            Name = "Konstantní funkce";
        }
        else { Range = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN); }
    }

    public override double CalculateFunction(double x)
    {
        return (a * x + b);
    }
}


//----------------------------------------------------------------------//
//                 LINEÁRNÍ FUNKCE S ABSOLUTNÍ HODNOTOU                 //
//----------------------------------------------------------------------//
class LinearAbs : MathFunction
{
    private double a;
    private double b;

    public LinearAbs(double a, double b) : base("Lineární funkce s absolutní hodnotou", $"f(x) = {a}|x| + {b}") // a|x| + b
    {
        string descB;

        this.a = a;

        this.b = b;
        descB = (b < 0) ? $"{b}" : $"+ {b}";

        Description = $"f(x) = {a}|x| {descB}";

        double i = a * (-1);
        Derivation = $"f'(x) = {a} pro x > 0   &   f'(x) = {i} pro x < 0";


        //definiční obor
        Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);

        //obor hodnot
        if (a == 0) 
        { 
            Range = new Interval('[', b, b, ']', double.NaN);
            Name = "Konstantní funkce";
        }
        else 
        { 
            Range = new Interval('[', 0, double.PositiveInfinity, ')', double.NaN); 
        }
    }

    public override double CalculateFunction(double x)
    {
        return (a * Math.Abs(x) + b);
    }
}


//----------------------------------------------------------------------//
//                         LINEÁRNÍ LOMENÁ FUNKCE                       //
//----------------------------------------------------------------------//
class LinearFract : MathFunction
{
    private double a;
    private double b;
    private double c;
    private double d;

    public LinearFract(int a, int b, int c, int d) : base("Lineární lomená funkce (hyperbola)", "") // (ax + b)/(cx + d)
    {
        string descB;
        string descD;

        this.a = a;

        this.b = b;
        descB = (b < 0) ? $"{b}" : $"+ {b}";

        this.c = c;

        this.d = d;
        descD = (d < 0) ? $"{d}" : $"+ {d}";

        Description = $"f(x) = ({a}x {descB}) / ({c}x {descD})";


        double help;
        double helphelp;

        help = ((a * d) - (b * c));

        if (c != 0 & help != 0)
        {
            helphelp = ((-1) * (d / c));
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', helphelp);

            helphelp = (a / c);
            Range = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', helphelp);


            Derivation = $"f'(x) = ({a} * {d} - {b} * {c}) / ({c} * x + {d})^2";
        }

        else if (c != 0 & help == 0)
        {
            helphelp = (a / c);
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Range = new Interval('[', helphelp, helphelp, ']', double.NaN);
            Name = "Konstantní funkce";


            Derivation = $"f'(x) = 0";
        }

        else if (c == 0 & d == 0)
        {
            Domain = new Interval('(', double.NaN, double.NaN, ')', double.NaN);
            Range = new Interval('(', double.NaN, double.NaN, ')', double.NaN);
            Name = "Neexistující funkce!!!";


            Derivation = $"f'(x) = NaN";
        }

        else if (c == 0 & a != 0)
        {
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Range = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Name = "Lineární funkce";


            Derivation = $"f'(x) = {a} / {d}";
        }

        else if (c == 0 & a == 0)
        {
            helphelp = (b / d);
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Range = new Interval('[', helphelp, helphelp, ']', double.NaN);
            Name = "Konstantní funkce";


            Derivation = $"f'(x) = 0";
        }

        else 
        {
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Range = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Name = "Lineární funkce";


            Derivation = $"f'(x) = {a} / {d}";
        }
    }

    public override double CalculateFunction(double x)
    {
        if (x == -(d / c))
        {
            return double.NaN; //OŠETŘIT V HL. PROGRAMU - když se vratí NaN (not a number), nic nevypisovat!!!
        }

        return ((a * x + b) / (c * x + d));
    }
}


//----------------------------------------------------------------------//
//                          KVADRATICKÁ FUNKCE                          //
//----------------------------------------------------------------------//
class Quadratic : MathFunction
{
    private double a;
    private double b;
    private double c;

    public Quadratic(int a, int b, int c) : base("Kvadratická funkce (parabola)", "") //ax^2 + bx + c
    {
        string descB;
        string descC;

        this.a = a;

        this.b = b;
        descB = (b < 0) ? $"{b}" : $"+ {b}";

        this.c = c;
        descC = (c < 0) ? $"{c}" : $"+ {c}";

        Description = $"f(x) = {a}x^2 {descB}x {descC}";


        Derivation = $"f'(x) = {a} * 2 * x {descB}";


        //definiční obor
        Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);

        //obor hodnot
        double help;
        if (a == 0 & b == 0) 
        { 
            Range = new Interval('[', c, c, ']', double.NaN);
            Name = "Konstantní funkce";

            Derivation = $"f'(x) = 0";
        }
        else if (a == 0 & b != 0) 
        { 
            Range = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')', double.NaN);
            Name = "Lineární funkce";

            Derivation = $"f'(x) = {b}";
        }
        else if (a > 0)
        {
            help = (((-1) * Math.Pow(b, 2) + 4 * a * c) / (4 * a));
            Range = new Interval('[', help, double.PositiveInfinity, ')', double.NaN);
        }
        else 
        {
            help = (((-1) * Math.Pow(b, 2) + 4 * a * c) / (4 * a));
            Range = new Interval('(', double.NegativeInfinity, help, ']', double.NaN);
        }
    }

    public override double CalculateFunction(double x)
    {
        return (a * Math.Pow(x, 2) + b * x + c);
    }
}
