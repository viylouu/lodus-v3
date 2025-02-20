using SimulationFramework.Drawing.Shaders;
using SimulationFramework;
using SimulationFramework.Drawing;

using System.Numerics;

public class base_frag : CanvasShader {
    [VertexShaderOutput]
    Vector2 uv;

    public ITexture atlas, shading;
    public Vector2 atlassize;

    public override ColorF GetPixelColor(Vector2 pos) {
        return atlas.SampleUV(uv) * shading.SampleUV(uv*atlassize);
    }
}