using System.Numerics;

using thrustr.utils;

public class chunking {
    static FastNoiseLite height;
    static FastNoiseLite bigheight;
    static FastNoiseLite caves;
    static FastNoiseLite bigcaves;

    static Random r;

    public static int seed;

    public static ulong max_actions_before_wait = 128;
    static ulong actions = 0;

    public static void load() {
        r = new();
        height = new();
        bigheight = new();
        caves = new();
        bigcaves = new();

        seed = r.Next(int.MinValue, int.MaxValue);

        height.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        height.SetFrequency(0.025f);
        height.SetFractalType(FastNoiseLite.FractalType.FBm);
        height.SetSeed(seed);

        bigheight.SetSeed(seed);
        bigheight.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        bigheight.SetFrequency(0.0035f);

        caves.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        caves.SetFrequency(0.0125f);
        caves.SetSeed(seed);
        caves.SetFractalType(FastNoiseLite.FractalType.FBm);
        caves.SetFractalOctaves(5);

        bigcaves.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        bigcaves.SetFrequency(0.0035f);
        bigcaves.SetSeed(seed);
        bigcaves.SetFractalType(FastNoiseLite.FractalType.FBm);
        bigcaves.SetFractalOctaves(5);
    }


    public static async void gen_chunk(Vector3 pos) {
        chunk c = new();

        lock(map.scene)
            map.scene.Add(pos, null);

        int cx = (int)pos.X*global.chk_size,
            cy = (int)pos.Y*global.chk_size,
            cz = (int)pos.Z*global.chk_size;

        bool empty = true;

        c.data = new block[global.chk_size,global.chk_size,global.chk_size];

        // generate data

        await Task.Delay(1);

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++) {
                    block b = get_block_shaping(x+cx,y+cy,z+cz);
                    c.data[x,y,z] = b;

                    if(b != block.air)
                        empty = false;

                    actions++;

                    if(actions > max_actions_before_wait) {
                        actions = 0;
                        await Task.Delay(1);
                    }
                }

        global.chks_loaded++;

        if(empty)
            return;

        // generate more data

        await Task.Delay(1);

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++)
                    if(c.data[x,y,z] == block.def) {
                        {
                            if(y+cy > get_noise_height(x+cx,z+cz)-1)
                                c.data[x,y,z] = block.grass;
                            else if(y+cy > get_noise_height(x+cx,z+cz)-6)
                                c.data[x,y,z] = block.dirt;
                            else
                                c.data[x,y,z] = block.cobble;
                        }

                        actions++;

                        if(actions > max_actions_before_wait) {
                            actions = 0;
                            await Task.Delay(1);
                        }
                    }

        // generate mesh
        
        await Task.Delay(1);

        List<vertex> verts = new();
        List<uint> inds = new();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        for (int x = 0; x < global.chk_size; x++)
            for (int y = 0; y < global.chk_size; y++)
                for (int z = 0; z < global.chk_size; z++)
                    if(c.data[x,y,z] != block.air)
                        for (int i = 0; i < 6; i++) {
                            // Determine if the face should be rendered
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == global.chk_size - 1 && get_block_shaping(x+1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != global.chk_size - 1 && c.data[x + 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x-1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != 0 && c.data[x - 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == global.chk_size - 1 && get_block_shaping(x+cx,y+1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != global.chk_size - 1 && c.data[x, y + 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x+cx,y-1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != 0 && c.data[x, y - 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == global.chk_size - 1 && get_block_shaping(x+cx,y+cy,z+1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != global.chk_size - 1 && c.data[x, y, z + 1] == block.air) shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x+cx,y+cy,z-1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != 0 && c.data[x, y, z - 1] == block.air) shouldRenderFace = true;
                                    break;
                            }

                            if (shouldRenderFace) {
                                // Add the vertices for this face
                                for (int v = 0; v < 4; v++) {
                                    vertex vtx = cube.f_verts[i, v];
                                    vtx.pos += new Vector3(x, y, z); // Translate position based on the chunk's coordinates
                                    vtx.uv /= game.atlassize;
                                    vtx.uv.X += (c.data[x,y,z].index-1) / game.atlassize.X;
                                    verts.Add(vtx); // Add the vertex to the list
                                }

                                // Indices for drawing the face (two triangles per face)
                                uint baseIndex = (uint)(verts.Count - 4);
                                inds.Add(baseIndex);
                                inds.Add(baseIndex + 1);
                                inds.Add(baseIndex + 2);

                                inds.Add(baseIndex);
                                inds.Add(baseIndex + 2);
                                inds.Add(baseIndex + 3);
                            }

                            actions++;

                            if(actions > max_actions_before_wait) {
                                actions = 0;
                                await Task.Delay(1);
                            }
                        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        await Task.Delay(1);

        c.m_inds = inds.ToArray();
        c.m_verts = verts.ToArray(); 

        c.size = 0;

        lock(map.scene)
            map.scene[pos] = c;
    }

    static float get_noise_height(float x, float z) {
        float bass = height.GetNoise(x,z)*12;
        float big = bigheight.GetNoise(x,z)*24;

        return bass + big*big +64;
    }

    static block get_block_shaping(int x, int y, int z) {
        if(y > get_noise_height(x,z))
            return block.air;

        float cave = caves.GetNoise(x, y, z);
        float depthFactor = 1.0f - (y/1024f);
        float lowerThreshold = 0.25f / depthFactor;
        float upperThreshold = 0.75f * depthFactor;

        if (cave < upperThreshold && cave > lowerThreshold)
            return block.air;

        return block.def;
    }

    public static void place_block(Vector3 wspos, block b) {
        (Vector3 pos, Vector3 cpos) = from_worldspace(wspos);

        if(!map.scene.TryGetValue(pos, out chunk? c)) {
            Console.WriteLine("tried to place block in ungenerated chunk!");
            return;
        }

        if(c == null) {
            c = new();
            c.data = new block[global.chk_size,global.chk_size,global.chk_size];
        }

        int x = (int)cpos.X,
            y = (int)cpos.Y,
            z = (int)cpos.Z;

        c.data[x,y,z] = b;

        c.geom = null;

        //map.scene[pos] = c;

        regenerate_mesh(pos);

        Console.WriteLine($"placed block in chunk ({pos.X}, {pos.Y}, {pos.Z})");
    }

    public static void regenerate_mesh(Vector3 pos) {
        map.scene.TryGetValue(pos, out chunk? c);

        if(c == null)
            return;

        if(c.data == null)
            return;

        int cx = (int)pos.X*global.chk_size,
            cy = (int)pos.Y*global.chk_size,
            cz = (int)pos.Z*global.chk_size;

        List<vertex> verts = new();
        List<uint> inds = new();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        for (int x = 0; x < global.chk_size; x++)
            for (int y = 0; y < global.chk_size; y++)
                for (int z = 0; z < global.chk_size; z++)
                    if(c.data[x,y,z] != block.air)
                        for (int i = 0; i < 6; i++) {
                            // Determine if the face should be rendered
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == global.chk_size - 1 && get_block_shaping(x+1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != global.chk_size - 1 && c.data[x + 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x-1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != 0 && c.data[x - 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == global.chk_size - 1 && get_block_shaping(x+cx,y+1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != global.chk_size - 1 && c.data[x, y + 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x+cx,y-1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != 0 && c.data[x, y - 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == global.chk_size - 1 && get_block_shaping(x+cx,y+cy,z+1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != global.chk_size - 1 && c.data[x, y, z + 1] == block.air) shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x+cx,y+cy,z-1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != 0 && c.data[x, y, z - 1] == block.air) shouldRenderFace = true;
                                    break;
                            }

                            if (shouldRenderFace) {
                                // Add the vertices for this face
                                for (int v = 0; v < 4; v++) {
                                    vertex vtx = cube.f_verts[i, v];
                                    vtx.pos += new Vector3(x, y, z); // Translate position based on the chunk's coordinates
                                    vtx.uv /= game.atlassize;
                                    vtx.uv.X += (c.data[x,y,z].index-1) / game.atlassize.X;
                                    verts.Add(vtx); // Add the vertex to the list
                                }

                                // Indices for drawing the face (two triangles per face)
                                uint baseIndex = (uint)(verts.Count - 4);
                                inds.Add(baseIndex);
                                inds.Add(baseIndex + 1);
                                inds.Add(baseIndex + 2);

                                inds.Add(baseIndex);
                                inds.Add(baseIndex + 2);
                                inds.Add(baseIndex + 3);
                            }
                        }

        c.m_inds = inds.ToArray();
        c.m_verts = verts.ToArray();

        map.scene[pos] = c;
    }

    public static (Vector3 pos, Vector3 cpos) from_worldspace(Vector3 wspos) {
        Vector3 pos = math.nfloor(wspos/global.chk_size);
        Vector3 cpos = math.wmod(wspos,global.chk_size);

        return (pos, cpos);
    }
}