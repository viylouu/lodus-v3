using SimulationFramework.Drawing.Shaders;
using SimulationFramework;
using SimulationFramework.Drawing;

using System.Numerics;

public class base_frag : CanvasShader {
    [VertexShaderOutput]
    Vector2 uv;

    public ITexture atlas;

    public override ColorF GetPixelColor(Vector2 pos) {
        return atlas.SampleUV(uv);
    }
}