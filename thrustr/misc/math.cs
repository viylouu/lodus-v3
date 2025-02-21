using SimulationFramework;
using System.Numerics;

namespace thrustr.utils;

public class math {
    //consts

    /// <summary>half the circumference of a circle with a radius of 1 or 180 degrees in radians</summary>
    public const float pi = 3.14159265358979323846264f;
    /// <summary>the full circumference of a circle with a radius of 1 or 360 degrees in radians</summary>
    public const float tau = 6.28318530717958647692528f;
    /// <summary>represents positive infinity</summary>
    public const float inf = float.PositiveInfinity;
    /// <summary>represents negative infinity</summary>
    public const float ninf = float.NegativeInfinity;
    /// <summary>the value to multiply a number by to get the result in radians from degrees</summary>
    public const float d2r = 0.01745329251994329576923691f;
    /// <summary>the value to multiply a number by to get the result in degrees from radians</summary>
    public const float r2d = 57.29577951308232087679815f;
    /// <summary>euler's number</summary>
    public const float e = 2.71828182845904523536029f;
    /// <summary>a quarter the circumference of a circle with a radius of 1 or 90 degrees in radians</summary>
    public const float hpi = 1.57079632679489661923132f;

    //misc

    /// <summary>rounds a number</summary>
    public static float round(float a) => MathF.Round(a);
    /// <summary>rounds a number</summary>
    public static Vector2 round(Vector2 a) => new (MathF.Round(a.X), MathF.Round(a.Y));
    /// <summary>rounds a number</summary>
    public static Vector3 round(Vector3 a) => new (MathF.Round(a.X), MathF.Round(a.Y), MathF.Round(a.Z));

    /// <summary>rounds a number up</summary>
    public static float ceil(float a) => MathF.Ceiling(a);
    /// <summary>rounds a number up</summary>
    public static Vector2 ceil(Vector2 a) => new (MathF.Ceiling(a.X), MathF.Ceiling(a.Y));
    /// <summary>rounds a number up</summary>
    public static Vector3 ceil(Vector3 a) => new (MathF.Ceiling(a.X), MathF.Ceiling(a.Y), MathF.Ceiling(a.Z));

    /// <summary>rounds a number down</summary>
    public static float floor(float a) => MathF.Floor(a);
    /// <summary>rounds a number down</summary>
    public static Vector2 floor(Vector2 a) => new (MathF.Floor(a.X), MathF.Floor(a.Y));
    /// <summary>rounds a number down</summary>
    public static Vector3 floor(Vector3 a) => new (MathF.Floor(a.X), MathF.Floor(a.Y), MathF.Floor(a.Z));

    /// <summary>rounds a number up if it's positive and down if it's negative</summary>
    public static float nceil(float a) => a > 0? MathF.Ceiling(a) : MathF.Floor(a);
    /// <summary>rounds a number up if it's positive and down if it's negative</summary>
    public static Vector2 nceil(Vector2 a) => new (nceil(a.X), nceil(a.Y));
    /// <summary>rounds a number up if it's positive and down if it's negative</summary>
    public static Vector3 nceil(Vector3 a) => new (nceil(a.X), nceil(a.Y), nceil(a.Z));

    /// <summary>rounds a number down if it's positive and up if it's negative</summary>
    public static float nfloor(float a) => a > 0? MathF.Floor(a) : MathF.Ceiling(a);
    /// <summary>rounds a number down if it's positive and up if it's negative</summary>
    public static Vector2 nfloor(Vector2 a) => new (nfloor(a.X), nfloor(a.Y));
    /// <summary>rounds a number down if it's positive and up if it's negative</summary>
    public static Vector3 nfloor(Vector3 a) => new (nfloor(a.X), nfloor(a.Y), nfloor(a.Z));

    /// <summary>calculates the absolute value of a number</summary>
    public static float abs(float a) => MathF.Abs(a);
    /// <summary>calculates the absolute value of a number</summary>
    public static Vector2 abs(Vector2 a) => new (MathF.Abs(a.X), MathF.Abs(a.Y));
    /// <summary>calculates the absolute value of a number</summary>
    public static Vector3 abs(Vector3 a) => new (MathF.Abs(a.X), MathF.Abs(a.Y), MathF.Abs(a.Z));

    /// <summary>calculates the negative absolute value of a number</summary>
    public static float nabs(float a) => -MathF.Abs(a);
    /// <summary>calculates the negative absolute value of a number</summary>
    public static Vector2 nabs(Vector2 a) => new (-MathF.Abs(a.X), -MathF.Abs(a.Y));
    /// <summary>calculates the negative absolute value of a number</summary>
    public static Vector3 nabs(Vector3 a) => new (-MathF.Abs(a.X), -MathF.Abs(a.Y), -MathF.Abs(a.Z));
    
    /// <summary>clamps a number between two values (b and c)</summary>
    public static float clamp(float a, float b, float c) => Math.Clamp(a,b,c);
    /// <summary>clamps a number between two values (b and c)</summary>
    public static Vector2 clamp(Vector2 a, float b, float c) => new (Math.Clamp(a.X,b,c), Math.Clamp(a.Y,b,c));
    /// <summary>clamps a number between two values (b and c)</summary>
    public static Vector3 clamp(Vector3 a, float b, float c) => new (Math.Clamp(a.X,b,c), Math.Clamp(a.Y,b,c), Math.Clamp(a.Z,b,c));
    /// <summary>clamps a number between two values (b and c)</summary>
    public static Vector2 clamp(Vector2 a, Vector2 b, Vector2 c) => new (Math.Clamp(a.X,b.X,c.X), Math.Clamp(a.Y,b.Y,c.Y));
    /// <summary>clamps a number between two values (b and c)</summary>
    public static Vector3 clamp(Vector3 a, Vector3 b, Vector3 c) => new (Math.Clamp(a.X,b.X,c.X), Math.Clamp(a.Y,b.Y,c.Y), Math.Clamp(a.Z,b.Z,c.Z));

    /// <summary>clamps a number between 0 and 1</summary>
    public static float clamp01(float a) => clamp(a,0,1);
    /// <summary>clamps a number between 0 and 1</summary>
    public static Vector2 clamp01(Vector2 a) => clamp(a,0,1);
    /// <summary>clamps a number between 0 and 1</summary>
    public static Vector3 clamp01(Vector3 a) => clamp(a,0,1);

    /// <summary>calculates a^b</summary>
    public static float pow(float a, float b) => MathF.Pow(a,b);
    /// <summary>calculates the square root of a number</summary>
    public static float sqrt(float a) => MathF.Sqrt(a);
    /// <summary>calculates the cube root of a number</summary>
    public static float cbrt(float a) => MathF.Pow(a,.3333333333333333333333333333333333f);
    /// <summary>calculates a number to its nth root</summary>
    public static float nroot(float a, float n) => MathF.Pow(a,1f/n);
    /// <summary>calculates a number to the power of 2</summary>
    public static float sqr(float a) => a*a;
    /// <summary>calculates a number to the power of 3</summary>
    public static float cbe(float a) => a*a*a;
    /// <summary>calculates the distance between two points</summary>
    public static float dist(Vector2 a, Vector2 b) => sqrt(sqr(b.X-a.X)+sqr(b.Y-a.Y));
    /// <summary>calculates the distance between two points</summary>
    public static float dist(Vector2 a, float x, float y) => sqrt(sqr(x - a.X) + sqr(y - a.Y));
    /// <summary>calculates the distance between two points</summary>
    public static float dist(float u, float v, float x, float y) => sqrt(sqr(x - u) + sqr(y - v));
    /// <summary>calculates the distance between two points</summary>
    public static float dist(Vector3 a, Vector3 b) => sqrt(sqr(b.X - a.X) + sqr(b.Y - a.Y) + sqr(b.Z - a.Z));
    /// <summary>calculates the squared distance between two points</summary>
    public static float sqrdist(Vector2 a, Vector2 b) => sqr(b.X - a.X) + sqr(b.Y - a.Y);
    /// <summary>calculates the squared distance between two points</summary>
    public static float sqrdist(Vector2 a, float x, float y) => sqr(x - a.X) + sqr(y - a.Y);
    /// <summary>calculates the squared distance between two points</summary>
    public static float sqrdist(float u, float v, float x, float y) => sqr(x - u) + sqr(y - v);
    /// <summary>calculates the squared distance between two points</summary>
    public static float sqrdist(Vector3 a, Vector3 b) => sqr(b.X - a.X) + sqr(b.Y - a.Y) + sqr(b.Z - a.Z);
    ///<summary>returns the biggest number between 2 numbers</summary>
    public static float max(float a, float b) => MathF.Max(a, b);
    ///<summary>returns the smallest number between 2 numbers</summary>
    public static float min(float a, float b) => MathF.Min(a, b);
    /// <summary>returns the input value in degrees in radians</summary>
    public static float rad(float deg) => deg * d2r;
    /// <summary>returns the input value in radians in degrees</summary>
    public static float deg(float rad) => rad * r2d;
    /// <summary>returns e^input</summary>
    public static float exp(float a) => MathF.Exp(a);
    /// <summary>returns the logarithm of the input value</summary>
    public static float log(float a) => MathF.Log(a);
    /// <summary>returns the logarithm of the input value with base p</summary>
    public static float logp(float a, float p) => MathF.Log(a,p);
    /// <summary>returns the base 10 logarithm of the input value</summary>
    public static float log10(float a) => MathF.Log10(a);
    /// <summary>linearly interpolates between a and b at point t</summary>
    public static float lerp(float a, float b, float t) => float.Lerp(a, b, t);
    /// <summary>linearly interpolates between 2 angles (a and b) at point t</summary>
    public static float lerpang(float a, float b, float t) => Angle.Lerp(a, b, t);
    /// <summary>converts an angle (radians) to a vector2</summary>
    public static Vector2 tovec(float theta) => Angle.ToVector(theta);
    /// <summary>lerps between a and b based on x</summary>
    public static float sstep(float a, float b, float x) { x = clamp01(x); x = x*x*x*(x*(x*6-15)+10); return a + (b - a) * x; }
    /// <summary>if x is less than a, then return 0, else return 1</summary>
    public static float step(float a, float x) => x<a?0:1;
    /// <summary>if x is negative, then return -1, else return 1 (returns 0 if x is 0)</summary>
    public static float sign(float x) => MathF.Sign(x);

    /// <summary>normalizes a vector</summary>
    public static Vector2 norm(Vector2 x) => Vector2.Normalize(x);
    /// <summary>normalizes a vector</summary>
    public static Vector3 norm(Vector3 x) => Vector3.Normalize(x);
    /// <summary>returns a vector that is perpendicular to both of the input vectors</summary>
    public static Vector3 cross(Vector3 x, Vector3 y) => Vector3.Cross(x,y);

    /// <summary>returns a number, that when going beyond a specific value, resets to zero</summary>
    public static float mod(float a, float b) => a%b;
    /// <summary>returns a number, that when going beyond a specific value, resets to zero</summary>
    public static Vector2 mod(Vector2 a, float b) => new(a.X%b,a.Y%b);
    /// <summary>returns a number, that when going beyond a specific value, resets to zero</summary>
    public static Vector3 mod(Vector3 a, float b) => new(a.X%b,a.Y%b,a.Z%b);
    /// <summary>returns a number, that when going beyond a specific value, resets to zero</summary>
    public static Vector2 mod(Vector2 a, Vector2 b) => new(a.X%b.X,a.Y%b.Y);
    /// <summary>returns a number, that when going beyond a specific value, resets to zero</summary>
    public static Vector3 mod(Vector3 a, Vector3 b) => new(a.X%b.X,a.Y%b.Y,a.Z%b.Z);
    /// <summary>returns a number, that when going beyond a specific value, resets to zero, but if it goes below zero, it wraps around to the max</summary>
    public static float wmod(float a, float b) => ((a%b)+b)%b;
    /// <summary>returns a number, that when going beyond a specific value, resets to zero, but if it goes below zero, it wraps around to the max</summary>
    public static Vector2 wmod(Vector2 a, float b) => new(wmod(a.X,b),wmod(a.Y,b));
    /// <summary>returns a number, that when going beyond a specific value, resets to zero, but if it goes below zero, it wraps around to the max</summary>
    public static Vector3 wmod(Vector3 a, float b) => new(wmod(a.X,b),wmod(a.Y,b),wmod(a.Z,b));
    /// <summary>returns a number, that when going beyond a specific value, resets to zero, but if it goes below zero, it wraps around to the max</summary>
    public static Vector2 wmod(Vector2 a, Vector2 b) => new(wmod(a.X,b.X),wmod(a.Y,b.Y));
    /// <summary>returns a number, that when going beyond a specific value, resets to zero, but if it goes below zero, it wraps around to the max</summary>
    public static Vector3 wmod(Vector3 a, Vector3 b) => new(wmod(a.X,b.X),wmod(a.Y,b.Y),wmod(a.Z,b.Z));

    //trig

    /// <summary>calculates the cosine of a number</summary>
    public static float cos(float a) => MathF.Cos(a);
    /// <summary>calculates the sine of a number</summary>
    public static float sin(float a) => MathF.Sin(a);
    /// <summary>calculates the tangent of a number</summary>
    public static float tan(float a) => MathF.Tan(a);
    /// <summary>calculates the cosecant of a number</summary>
    public static float csc(float a) => 1f/MathF.Sin(a);
    /// <summary>calculates the secant of a number</summary>
    public static float sec(float a) => 1f/MathF.Cos(a);
    /// <summary>calculates the cotangent of a number</summary>
    public static float cot(float a) => 1f/MathF.Tan(a);
    /// <summary>calculates the arcsine (sin^-1(θ)) of a number</summary>
    public static float asin(float a) => MathF.Asin(a);
    /// <summary>calculates the arccosine (cos^-1(θ)) of a number</summary>
    public static float acos(float a) => MathF.Acos(a);
    /// <summary>calculates the arctangent (tan^-1(θ)) of a number</summary>
    public static float atan(float a) => MathF.Atan(a);
    /// <summary>calculates the arctangent (tan^-1(y/x)) of a number</summary>
    public static float atan2(float y, float x) => MathF.Atan(y/x);
    /// <summary>calculates the arccosecant (csc^-1(θ)) of a number</summary>
    public static float acsc(float a) => MathF.Asin(1f/a);
    /// <summary>calculates the arcsecant (sec^-1(θ)) of a number</summary>
    public static float asec(float a) => MathF.Acos(1f / a);
    /// <summary>calculates the arccotangent (cot^-1(θ)) of a number</summary>
    public static float acot(float a) => MathF.Atan(1f/a);
    /// <summary>calculates the hyperbolic arcsine (sinh^-1(θ)) of a number</summary>
    public static float asinh(float a) => MathF.Asinh(a);
    /// <summary>calculates the hyperbolic arccosine (cosh^-1(θ)) of a number</summary>
    public static float acosh(float a) => MathF.Acosh(a);
    /// <summary>calculates the hyperbolic arctangent (tanh^-1(θ)) of a number</summary>
    public static float atanh(float a) => MathF.Atanh(a);
    /// <summary>calculates the hyperbolic arccosecant (csch^-1(θ)) of a number</summary>
    public static float acsch(float a) => MathF.Log(1f/a+MathF.Sqrt(1f/(a*a)+1));
    /// <summary>calculates the hyperbolic arcsecant (sech^-1(θ)) of a number</summary>
    public static float asech(float a) => MathF.Log(1f/a+MathF.Sqrt(1f/(a*a)-1));
    /// <summary>calculates the hyperbolic arccotangent (coth^-1(θ)) of a number</summary>
    public static float acoth(float a) => .5f*MathF.Log((a+1)/(a-1));
    /// <summary>calculates the hyperbolic cosine of a number</summary>
    public static float cosh(float a) => MathF.Cosh(a);
    /// <summary>calculates the hyperbolic sine of a number</summary>
    public static float sinh(float a) => MathF.Sinh(a);
    /// <summary>calculates the hyperbolic tangent of a number</summary>
    public static float tanh(float a) => MathF.Tanh(a);
    /// <summary>calculates the hyperbolic cosecant of a number</summary>
    public static float csch(float a) => 1f / MathF.Sinh(a);
    /// <summary>calculates the hyperbolic secant of a number</summary>
    public static float sech(float a) => 1f / MathF.Cosh(a);
    /// <summary>calculates the hyperbolic cotangent of a number</summary>
    public static float coth(float a) => 1f / MathF.Tanh(a);
}