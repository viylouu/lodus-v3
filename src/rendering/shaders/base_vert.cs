using SimulationFramework.Drawing.Shaders;

using System.Numerics;

public class base_vert : VertexShader {
    [VertexData]
    vertex vertex;

    public Matrix4x4 wmat;
    public Matrix4x4 vpmat;

    [VertexShaderOutput]
    Vector2 uv;

    [UseClipSpace]
    public override Vector4 GetVertexPosition() {
        Vector4 result = new Vector4(vertex.pos, 1);
        result = Vector4.Transform(result, wmat);
        result = Vector4.Transform(result, vpmat);
        
        uv = vertex.uv;
        return result;
    }
}