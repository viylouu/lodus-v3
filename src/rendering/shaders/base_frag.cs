using SimulationFramework.Drawing.Shaders;
using SimulationFramework;

using System.Numerics;

public class base_frag : CanvasShader {
    [VertexShaderOutput]
    Vector2 uv;

    public override ColorF GetPixelColor(Vector2 pos) {
        return new(uv.X,uv.Y,0,1);
    }
}