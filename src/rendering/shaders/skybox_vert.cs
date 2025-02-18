using SimulationFramework;
using SimulationFramework.Drawing.Shaders;

using System.Numerics;

public class skybox_vert : VertexShader {
    [VertexData]
    vertex vert;

    public Matrix4x4 wmat;
    public Matrix4x4 vpmat;

    [VertexShaderOutput]
    Vector3 frag_dir;

    [UseClipSpace]
    public override Vector4 GetVertexPosition() {
        Vector4 result = new Vector4(vert.pos, 1);
        result = Vector4.Transform(result, wmat);
        result = Vector4.Transform(result, vpmat);
        
        frag_dir = vert.pos;
        return result;
    }
}