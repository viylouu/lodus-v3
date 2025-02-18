using System.Numerics;

using SimulationFramework.Drawing;
using thrustr.utils;

public class game {
    static base_frag frag = new();
    static base_vert vert = new();

    static ITexture atlas;

    static int renderdist = 8;

    public static int tris_rendered = 0;
    public static int chunks_rendered = 0;

    public static void load() {
        atlas = Graphics.LoadTexture("assets/atlas.png");

        frag.atlas = atlas;
    }

    public static void render_world(ICanvas c, IDepthMask depth) {
        c.ResetState();

        depth.Clear(1f);

        vert.vpmat = camera.vertprojmatrix;

        int render_dist = renderdist/2;

        int minx = (int)math.round(camera.pos.X/global.chk_size)-render_dist,
            miny = (int)math.round(camera.pos.Y/global.chk_size)-render_dist,
            minz = (int)math.round(camera.pos.Z/global.chk_size)-render_dist,
            maxx = (int)math.round(camera.pos.X/global.chk_size)+render_dist,
            maxy = (int)math.round(camera.pos.Y/global.chk_size)+render_dist,
            maxz = (int)math.round(camera.pos.Z/global.chk_size)+render_dist;

        tris_rendered = 0;
        chunks_rendered = 0;

        for(int x = minx; x < maxx; x++)
            for(int y = miny; y < maxy; y++)
                for(int z = minz; z < maxz; z++) {
                    if(!map.scene.TryGetValue(new(x,y,z), out chunk? chk)) {
                        chunking.gen_chunk(new(x,y,z));
                        continue;
                    } if(chk == null)
                        continue;
                    if(chk.m_inds.Length == 0)
                        continue;

                    vert.wmat = Matrix4x4.CreateTranslation(x*global.chk_size,y*global.chk_size,z*global.chk_size);

                    tris_rendered += chk.m_inds.Length/3;
                    chunks_rendered++;

                    c.Fill(frag, vert);
                    c.Mask(depth);
                    c.WriteMask(depth, null);
                    c.DrawTriangles<vertex>(chk.m_verts, chk.m_inds);

                    //c.Flush();
                }

        c.ResetState();
    }

    public static void update() {
        camera.update();
    }
}