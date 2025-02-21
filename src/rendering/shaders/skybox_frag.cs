using SimulationFramework.Drawing.Shaders;
using SimulationFramework;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

using System.Numerics;

public class skybox_frag : CanvasShader {
    [VertexShaderOutput]
    Vector3 frag_dir;

    public ColorF col;

    public override ColorF GetPixelColor(Vector2 pos) {
        //Vector3 ray_dir = Normalize(frag_dir);
        //return new(ray_dir.X,ray_dir.Y,ray_dir.Z,1);

        return col;
    }
}