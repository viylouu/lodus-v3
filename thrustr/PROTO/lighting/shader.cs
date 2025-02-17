using System.Numerics;

using SimulationFramework;
using SimulationFramework.Drawing.Shaders;

using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

namespace thrustr.PROTO;

public class lightingshader : CanvasShader {
    public Vector2 mp;

    public float range = 480;
    public float intensity = 1;
    public float power = 1;

    public override ColorF GetPixelColor(Vector2 pos) {
        float bright = Pow((range-Distance(mp,pos))* intensity/range, power);

        Vector2 p = pos;
        Vector2 dir = (mp-p).Normalized();

        float d = 0;

        float r = 0;

        while(r > .125f) {
            r = map(p);
            p += dir * r;
            d += r;
        }

        if(Distance(mp,p) > 1)
            bright = 0;

        return ColorF.Lerp(ColorF.Transparent, ColorF.White, bright);
    }

    public float map(Vector2 p) {
        float val = Length(mp-p);

        val = Min(val, sdf_circ(p, Vector2.Zero, 4));

        return val;
    }

    public float sdf_circ(Vector2 raypos, Vector2 circpos, float radius) {
        return Length(raypos - circpos) - radius;
    }
}