using System.Numerics;
using Silk.NET.Windowing;
using SimulationFramework.Drawing;

using thrustr.utils;

public class game {
    static base_frag frag = new();
    static base_vert vert = new();

    static ITexture shading, blockatlas;
    public static Vector2 atlassize;

    static int renderdist = 10;

    public static int tris_rendered = 0;
    public static int chunks_rendered = 0;

    static ITexture buffer;
    static ICanvas buffer_c;

    static post_frag post_process = new();


    public static void load() {
        shading = Graphics.LoadTexture("assets/shading.png");
        blockatlas = Graphics.LoadTexture("assets/block atlas.png");

        atlassize = new Vector2(blockatlas.Width/32,blockatlas.Height/48);

        frag.shading = shading;
        frag.atlas = blockatlas;
        frag.atlassize = atlassize;
    }

    public static void render_world(ICanvas c, IDepthMask depth) {
        depth.Clear(1f);

        buffer.trydispose();
        buffer = Graphics.CreateTexture(c.Width,c.Height);
        buffer_c = buffer.GetCanvas();

        renderdist = global.render_dist;

        vert.vpmat = camera.vertprojmatrix;
        frag.campos = camera.pos;
        frag.fog = main.col;
        frag.fogdist = global.chk_size*renderdist;

        long minx = (long)math.round(camera.pos.X/global.chk_size)-renderdist,
            miny = (long)math.round(camera.pos.Y/global.chk_size)-renderdist,
            minz = (long)math.round(camera.pos.Z/global.chk_size)-renderdist,
            maxx = (long)math.round(camera.pos.X/global.chk_size)+renderdist,
            maxy = (long)math.round(camera.pos.Y/global.chk_size)+renderdist,
            maxz = (long)math.round(camera.pos.Z/global.chk_size)+renderdist;

        tris_rendered = 0;
        chunks_rendered = 0;

        Plane[] frustumPlanes = frustum.GetFrustumPlanes(camera.vertprojmatrix);

        for(long x = minx; x < maxx; x++)
            for(long y = miny; y < maxy; y++)
                for(long z = minz; z < maxz; z++) {
                    if(math.sqrdist(camera.pos, new(x*global.chk_size,y*global.chk_size,z*global.chk_size)) > math.sqr(renderdist*global.chk_size))
                        continue;

                    if(!map.scene.TryGetValue(new(x,y,z), out chunk? chk)) {
                        chunking.gen_chunk(new(x,y,z));
                        continue;
                    } if(chk == null)
                        continue;
                    if(chk.m_inds.Length == 0)
                        continue;

                    Vector3 min = new(x*global.chk_size,y*global.chk_size,z*global.chk_size);
                    Vector3 max = new(x*global.chk_size+global.chk_size,y*global.chk_size+global.chk_size,z*global.chk_size+global.chk_size);

                    if(!frustum.IsAABBInFrustum(min,max,frustumPlanes))
                        continue;
                    

                    vert.wmat = Matrix4x4.CreateScale(chk.size) * Matrix4x4.CreateTranslation(x*global.chk_size,y*global.chk_size-(1-chk.size)*global.chk_size*2,z*global.chk_size);

                    tris_rendered += chk.m_inds.Length/3;
                    chunks_rendered++;

                    buffer_c.Stroke(frag, vert);
                    buffer_c.Mask(depth);
                    buffer_c.WriteMask(depth, null);
                    //c.DrawTriangles<vertex>(chk.m_verts, chk.m_inds);

                    // temp fix for simf bug, but its only here bc chunking is async and most graphics class functions dont work async (its also easy to remove)
                    if (chk.geom == null && chk.m_inds.Length > 0)
                        chk.geom = Graphics.CreateGeometry<vertex>(chk.m_verts, chk.m_inds);
                    if (chk.geom != null)
                        buffer_c.DrawGeometry(chk.geom);

                    chk.size += ease.dyn(chk.size, 1, 12);
                    chk.size = math.clamp01(chk.size);

                    //c.Flush();
                }

        buffer_c.Flush();

        post_process.tex = buffer;

        post_process.quant = global.color_quant? global.color_quant_amt : 0;
        post_process.oklab_quant = global.better_quant;

        c.Fill(post_process);
        c.DrawRect(0,0,c.Width,c.Height);
    }

    public static void update() {
        camera.update();
    }
}