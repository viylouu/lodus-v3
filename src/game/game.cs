using System.Numerics;

using SimulationFramework.Drawing;

public class game {
    static base_frag frag = new();
    static base_vert vert = new();

    static ITexture atlas;

    public static void load() {
        atlas = Graphics.LoadTexture("assets/atlas.png");

        frag.atlas = atlas;
    }

    public static void render_world(ICanvas c, IDepthMask depth) {
        c.ResetState();

        depth.Clear(1f);

        vert.vpmat = camera.vertprojmatrix;

        for(int x = map.bounds.startx; x < map.bounds.endx; x++)
            for(int y = map.bounds.starty; y < map.bounds.endy; y++)
                for(int z = map.bounds.startz; z < map.bounds.endz; z++) {
                    if(!map.scene.TryGetValue(new(x,y,z), out chunk chk))
                        continue;
                    if(chk == null)
                        continue;

                    vert.wmat = Matrix4x4.CreateTranslation(x*map.chk_size,y*map.chk_size,z*map.chk_size);

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