using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;

using Assimp;

namespace thrustr.utils;

public static class misc_extentions {
    public static void trydispose(this ITexture todisp) {
        if(todisp != null)
            todisp.Dispose();
    }

    public static void trydispose(this ISound todisp) {
        if(todisp != null)
            todisp.Dispose();
    }

    public static void trydispose(this SoundPlayback todisp) {
        if(todisp != null)
            todisp.Dispose();
    }

    public static void DrawText(this ICanvas c, string text, float size, float x, float y, Alignment align = Alignment.TopLeft) => c.DrawAlignedText(text, size, x,y, align);
    public static void DrawText(this ICanvas c, string text, float size, Vector2 pos, Alignment align = Alignment.TopLeft) => c.DrawAlignedText(text, size, pos, align);
}

public class misc {
    static AssimpContext imp = new();

    /// <summary>loads an fbx file as a set of vertices (Vector3[]) indices (uint[]) and texture coordinates (Vector2[])</summary>
    public static (Vector3[], uint[], Vector2[])? loadfbx(string path) {
        Scene scene = imp.ImportFile(path, PostProcessSteps.Triangulate);

        Console.WriteLine($"loading fbx file at \"{path}\"");

        List<Vector3> verts = new();
        List<uint> inds = new();
        List<Vector2> texcoords = new();

        if(scene != null && scene.HasMeshes)
            foreach(var mesh in scene.Meshes) {
                Console.WriteLine($"found \"{mesh.Name}\"");
                Console.WriteLine($"{mesh.VertexCount} vertices");
                Console.WriteLine($"{mesh.FaceCount} faces");

                foreach(var vert in mesh.Vertices)
                    verts.Add(new(vert.X, vert.Y, vert.Z));

                foreach(var face in mesh.Faces)
                    if(face.IndexCount == 3) {
                        inds.Add((uint)face.Indices[0]);
                        inds.Add((uint)face.Indices[1]);
                        inds.Add((uint)face.Indices[2]);
                    } else {
                        Console.WriteLine($"error! invalid face count ({face.IndexCount}) expected (3)");
                        return null;
                    }

                if(mesh.TextureCoordinateChannels[0] != null)
                    for(int i = 0; i < mesh.VertexCount; i++)
                        texcoords.Add(new(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y));
                else {
                    Console.WriteLine("error! no texture coordinates found");
                    return null;
                }
            }
        else
            Console.WriteLine("no meshes found / failed to load file");

        return (verts.ToArray(), inds.ToArray(), texcoords.ToArray());
    }
}