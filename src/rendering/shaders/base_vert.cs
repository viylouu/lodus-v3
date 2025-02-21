using SimulationFramework.Drawing.Shaders;

using System.Numerics;

public class base_vert : VertexShader {
    [VertexData]
    vertex vert;

    public Matrix4x4 wmat;
    public Matrix4x4 vpmat;

    [VertexShaderOutput]
    Vector2 uv;

    [VertexShaderOutput]
    Vector3 wspos;

    [UseClipSpace]
    public override Vector4 GetVertexPosition() {
        Vector4 v4pos = new Vector4(vert.pos, 1);
        Vector4 _wspos = Vector4.Transform(v4pos, wmat);
        Vector4 result = Vector4.Transform(_wspos, vpmat);
        
        uv = vert.uv;
        wspos = new(_wspos.X,_wspos.Y,_wspos.Z);

        return result;
    }
}