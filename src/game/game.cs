using System.Numerics;
using SimulationFramework.Drawing;

using thrustr.utils;

public class game {
    static base_frag frag = new();
    static base_vert vert = new();

    public static void render_world(ICanvas c) {
        c.ResetState();

        vert.vpmat = camera.vertprojmatrix;

        for(int x = map.bounds.startx; x < map.bounds.endx; x++)
            for(int y = map.bounds.starty; y < map.bounds.endy; y++)
                for(int z = map.bounds.startz; z < map.bounds.endz; z++) {
                    vert.wmat = Matrix4x4.CreateTranslation(x,y,z);

                    c.Fill(frag, vert);
                    c.DrawTriangles<vertex>(cube.verts, cube.inds);
                }

        c.ResetState();
    }
}