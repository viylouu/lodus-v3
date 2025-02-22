using SimulationFramework.Drawing.Shaders;
using SimulationFramework;
using SimulationFramework.Drawing;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

using System.Numerics;

public class base_frag : CanvasShader {
    [VertexShaderOutput]
    Vector2 uv;

    [VertexShaderOutput]
    Vector3 wspos;

    public ITexture atlas, shading;
    public Vector2 atlassize;

    public ColorF fog;

    public Vector3 campos;

    public float fogdist;

    public float fogintensity;

    public float quant;

    public ColorF Mix(ColorF a, ColorF b, float t)
        => new (Lerp(a.R,b.R,t),Lerp(a.G,b.G,t),Lerp(a.B,b.B,t),Lerp(a.A,b.A,t));

    public override ColorF GetPixelColor(Vector2 pos) {
        float dist = 0;

        if(fogintensity > 0) {
            dist = Distance(wspos,campos);
            dist /= fogdist;
            dist = Pow(dist,fogintensity);
        }

        if(dist > 1)
            Discard();

        ColorF fin = Mix(atlas.SampleUV(uv) * shading.SampleUV(uv*atlassize), fog, dist);

        return fin;
    }
}