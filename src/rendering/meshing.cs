using System.Numerics;
using System.Runtime.InteropServices;
using thrustr.utils;

public class meshing {
    public (vertex[], uint[]) load_fbx(string path) {
        (Vector3[] verts, uint[] inds, Vector2[] uvs) = misc.loadfbx(path, out bool suc);

        if(!suc)
            return (Array.Empty<vertex>(), Array.Empty<uint>());

        vertex[] v = new vertex[verts.Length];

        for(int i = 0; i < verts.Length; i++)
            v[i] = new() { pos = verts[i], uv = uvs[i] };

        return (v, inds);
    }
}