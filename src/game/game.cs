using System.Numerics;

using SimulationFramework.Drawing;
using SimulationFramework;

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

    static List<Vector3> chunksToGenerate = new();


    public static void resize(int w, int h) {
        buffer.Dispose();
        buffer = Graphics.CreateTexture(w,h);
        buffer_c = buffer.GetCanvas();
    }

    public static void load() {
        shading = Graphics.LoadTexture("assets/shading.png");
        blockatlas = Graphics.LoadTexture("assets/block atlas.png");

        buffer = Graphics.CreateTexture(640,360);
        buffer_c = buffer.GetCanvas();

        atlassize = new Vector2(blockatlas.Width/32,blockatlas.Height/48);

        frag.shading = shading;
        frag.atlas = blockatlas;
        frag.atlassize = atlassize;

        camera.pos.Y = chunking.get_real_height_at(new(0,0))+2;
    }

    public static void render_world(ICanvas c, IDepthMask depth) {
        depth.Clear(1f);

        buffer_c.Clear(Color.Transparent);

        renderdist = global.render_dist;

        vert.vpmat = camera.vertprojmatrix;
        frag.campos = camera.pos;
        frag.fog = main.col;
        frag.fogdist = global.chk_size*renderdist;
        frag.fogintensity = global.fog? global.fog_density : -1;

        long minx = (long)math.round(camera.pos.X/global.chk_size)-renderdist,
            miny = (long)math.round(camera.pos.Y/global.chk_size)-renderdist,
            minz = (long)math.round(camera.pos.Z/global.chk_size)-renderdist,
            maxx = (long)math.round(camera.pos.X/global.chk_size)+renderdist,
            maxy = (long)math.round(camera.pos.Y/global.chk_size)+renderdist,
            maxz = (long)math.round(camera.pos.Z/global.chk_size)+renderdist;

        tris_rendered = 0;
        chunks_rendered = 0;

        Vector3 min, max;
        float csize;

        //List<Vector3> chunksToGenerate = new();

        for(long x = minx; x < maxx; x++)
            for(long y = miny; y < maxy; y++)
                for(long z = minz; z < maxz; z++) {
                    if(math.sqrdist(camera.pos, new(x*global.chk_size,y*global.chk_size,z*global.chk_size)) > math.sqr(renderdist*global.chk_size))
                        continue;

                    if(!map.scene.TryGetValue(new(x,y,z), out chunk? chk)) {  
                        //because chunk gen is expensive, frustum culling :D
                        min = new(x*global.chk_size-.5f,y*global.chk_size-.5f,z*global.chk_size-.5f);
                        max = new(x*global.chk_size+global.chk_size+.5f,y*global.chk_size+global.chk_size+.5f,z*global.chk_size+global.chk_size+.5f);

                        if(!frustum.IsAABBInFrustum(min,max,camera.view_frustum))
                            continue;

                        map.scene.TryAdd(new(x,y,z),null);
                        chunksToGenerate.Add(new(x,y,z));
                        continue;
                    } if(chk == null)
                        continue;
                    if(chk.m_inds.Length == 0)
                        continue;

                    min = new(x*global.chk_size-.5f,y*global.chk_size-.5f,z*global.chk_size-.5f);
                    max = new(x*global.chk_size+global.chk_size+.5f,y*global.chk_size+global.chk_size+.5f,z*global.chk_size+global.chk_size+.5f);

                    if(!frustum.IsAABBInFrustum(min,max,camera.view_frustum))
                        continue;
                    

                    csize = ease.oback(math.clamp01(Time.TotalTime-chk.starttime));

                    vert.wmat = Matrix4x4.CreateScale(csize) * Matrix4x4.CreateTranslation(x*global.chk_size,y*global.chk_size-(1-csize)*global.chk_size*2,z*global.chk_size);

                    tris_rendered += chk.m_inds.Length/3;
                    chunks_rendered++;

                    buffer_c.Stroke(frag, vert);
                    buffer_c.Mask(depth);
                    buffer_c.WriteMask(depth, null);
                    //c.DrawTriangles<vertex>(chk.m_verts, chk.m_inds);

                    // temp fix for simf bug, but its only here bc chunking is on a different thread and most graphics class functions dont work on a different thread (its also easy to remove)
                    if (chk.geom == null)
                        chk.geom = Graphics.CreateGeometry<vertex>(chk.m_verts, chk.m_inds);
                    buffer_c.DrawGeometry(chk.geom);

                    //c.Flush();
                }

        var tasks = chunksToGenerate.Select(pos => Task.Run(() => chunking.gen_chunk_thing(pos)));
        Task.WhenAll(tasks);

        chunksToGenerate.Clear();

        /*global.frames_waited_for_dispatch++;

        if(global.frames_waited_for_dispatch >= global.frames_between_dispatch) {
            int i = 0;
            while(i < global.max_chunk_gen_threads && i < chunksToGenerate.Count) {
                ThreadPool.UnsafeQueueUserWorkItem(_ => chunking.gen_chunk_thing(chunksToGenerate[i]), null);
                chunksToGenerate.RemoveAt(i); i++;
            }

            global.frames_waited_for_dispatch = 0;
        }*/

        buffer_c.Flush();

        post_process.tex = buffer;

        post_process.quant = global.color_quant? global.color_quant_amt : 0;
        post_process.oklab_quant = global.better_quant;

        c.Fill(post_process);
        c.DrawRect(0,0,c.Width,c.Height);
    }

    public static void update(ICanvas c) {
        camera.update(c);
    }
}