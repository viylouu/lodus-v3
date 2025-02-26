using System.Numerics;

using thrustr.utils;

public class chunking {
    static FastNoiseLite height;
    static FastNoiseLite bigheight;
    static FastNoiseLite caves;
    static FastNoiseLite bigcaves;
    static FastNoiseLite steeps;
    static FastNoiseLite stones;
    static FastNoiseLite dirtoff;

    static Random r;

    public static int seed;

    public static ulong max_actions_before_wait = 256;
    static ulong actions = 0;

    public static void load() {
        r = new();
        height = new();
        bigheight = new();
        caves = new();
        bigcaves = new();
        steeps = new();
        stones = new();
        dirtoff = new();

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

        steeps.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        steeps.SetFrequency(0.0075f);
        steeps.SetSeed(seed);
        steeps.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        steeps.SetDomainWarpAmp(126);
        steeps.SetFractalType(FastNoiseLite.FractalType.DomainWarpProgressive);
        steeps.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        steeps.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);

        stones.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        stones.SetFrequency(0.1f);
        stones.SetSeed(seed);
        stones.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);

        dirtoff.SetNoiseType(FastNoiseLite.NoiseType.Value);
        dirtoff.SetFrequency(1);
        dirtoff.SetSeed(seed);
    }

    public static void gen_chunk(Vector3 pos) {
        gen_chunk_thing(pos);
    }

    public static async void gen_chunk_thing(Vector3 pos) {
        chunk c = new();

        map.scene.TryAdd(pos, null);

        float cx = (int)pos.X*global.chk_size,
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

        /*Parallel.For(0, 16 * 16 * 16, index => {
            int x = index / (16 * 16);
            int y = (index / 16) % 16;
            int z = index % 16;

            block b = get_block_shaping(x+cx,y+cy,z+cz);
            c.data[x,y,z] = b;

            if(b != block.air)
                        empty = false;
        });*/

        global.chks_loaded++;

        if(empty)
            return;

        global.filled_chks_loaded++;

        // generate more data

        await Task.Delay(10);

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++)
                    if(c.data[x,y,z] == block.def) {
                        {
                            if(y+cy > get_noise_height(x+cx,z+cz)-1)
                                c.data[x,y,z] = block.grass;
                            else if(y+cy > get_noise_height(x+cx,z+cz)-4-dirtoff.GetNoise(x,z)*2)
                                c.data[x,y,z] = block.dirt;
                            else {
                                // stones

                                int type = (int)math.round(stones.GetNoise(x+cx,y+cy,z+cz)*3);

                                switch(type) {
                                    case 0:
                                        c.data[x,y,z] = block.shale; break;
                                    case 1:
                                        c.data[x,y,z] = block.andesite; break;
                                    case 2:
                                        c.data[x,y,z] = block.shale_bricks; break;
                                    case 3:
                                        c.data[x,y,z] = block.andesite_bricks; break;
                                }
                            }
                        }

                        actions++;

                        if(actions > max_actions_before_wait) {
                            actions = 0;
                            await Task.Delay(10);
                        }
                    }

        // generate mesh
        
        await Task.Delay(10);

        List<vertex> verts = new();
        List<uint> inds = new();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        for (int x = 0; x < global.chk_size; x++)
            for (int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++)
                    if (c.data[x, y, z] != block.air)
                        // Iterate through the 6 possible faces of the current block
                        for (int i = 0; i < 6; i++) {
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == global.chk_size - 1 && get_block_shaping(x + 1 + cx, y + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (x != global.chk_size - 1 && c.data[x + 1, y, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x - 1 + cx, y + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (x != 0 && c.data[x - 1, y, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == global.chk_size - 1 && get_block_shaping(x + cx, y + 1 + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (y != global.chk_size - 1 && c.data[x, y + 1, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x + cx, y - 1 + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (y != 0 && c.data[x, y - 1, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == global.chk_size - 1 && get_block_shaping(x + cx, y + cy, z + 1 + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (z != global.chk_size - 1 && c.data[x, y, z + 1] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x + cx, y + cy, z - 1 + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (z != 0 && c.data[x, y, z - 1] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                            }

                            if (shouldRenderFace) {
                                // Check if the current face can be merged with adjacent blocks (greedy meshing)
                                bool canMerge = false;
                                if (i == 0 && x < global.chk_size - 1 && c.data[x + 1, y, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +x face
                                else if (i == 1 && x > 0 && c.data[x - 1, y, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -x face
                                else if (i == 2 && y < global.chk_size - 1 && c.data[x, y + 1, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +y face
                                else if (i == 3 && y > 0 && c.data[x, y - 1, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -y face
                                else if (i == 4 && z < global.chk_size - 1 && c.data[x, y, z + 1] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +z face
                                else if (i == 5 && z > 0 && c.data[x, y, z - 1] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -z face

                                // If merging, skip adding the face and move on to the next block
                                if (canMerge) continue;

                                // Add the vertices for this face
                                for (int v = 0; v < 4; v++) {
                                    vertex vtx = cube.f_verts[i, v];
                                    vtx.pos += new Vector3(x, y, z); // Translate position based on the chunk's coordinates
                                    vtx.uv /= game.atlassize;
                                    vtx.uv.X += (c.data[x, y, z].index - 1) / game.atlassize.X;
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

                                actions++;

                                if (actions > max_actions_before_wait) {
                                    actions = 0;
                                    await Task.Delay(10); // Prevent the frame from freezing
                                }
                            }
                        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        await Task.Delay(10);

        c.m_inds = inds.ToArray();
        c.m_verts = verts.ToArray(); 

        c.size = 0;

        map.scene[pos] = c;
    }

    static float get_noise_height(float x, float z) {
        float bass = height.GetNoise(x,z)*12;
        float big = bigheight.GetNoise(x,z)*72;
        float xp = x, zp = z;
        steeps.DomainWarp(ref xp,ref zp);
        float steep = steeps.GetNoise(xp,zp)*16;

        //min is -12 - 16 = -76

        if(big < -32)
            return bass*0.25f - 32;

        return bass + big + steep +64;
    }

    static block get_block_shaping(float x, float y, float z) {
        if(y > get_noise_height(x,z))
            return block.air;

        float cave = caves.GetNoise(x, y, z);
        float depthFactor = 1.0f - (y/512f);
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

        for (int x = 0; x < global.chk_size; x++)
            for (int y = 0; y < global.chk_size; y++)
                for (int z = 0; z < global.chk_size; z++)
                    if (c.data[x, y, z] != block.air)
                        // Iterate through the 6 possible faces of the current block
                        for (int i = 0; i < 6; i++) {
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == global.chk_size - 1 && get_block_shaping(x + 1 + cx, y + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (x != global.chk_size - 1 && c.data[x + 1, y, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x - 1 + cx, y + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (x != 0 && c.data[x - 1, y, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == global.chk_size - 1 && get_block_shaping(x + cx, y + 1 + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (y != global.chk_size - 1 && c.data[x, y + 1, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x + cx, y - 1 + cy, z + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (y != 0 && c.data[x, y - 1, z] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == global.chk_size - 1 && get_block_shaping(x + cx, y + cy, z + 1 + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (z != global.chk_size - 1 && c.data[x, y, z + 1] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x + cx, y + cy, z - 1 + cz) == block.air) 
                                        shouldRenderFace = true;
                                    if (z != 0 && c.data[x, y, z - 1] == block.air) 
                                        shouldRenderFace = true;
                                    break;
                            }

                            if (shouldRenderFace) {
                                // Check if the current face can be merged with adjacent blocks (greedy meshing)
                                bool canMerge = false;
                                if (i == 0 && x < global.chk_size - 1 && c.data[x + 1, y, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +x face
                                else if (i == 1 && x > 0 && c.data[x - 1, y, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -x face
                                else if (i == 2 && y < global.chk_size - 1 && c.data[x, y + 1, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +y face
                                else if (i == 3 && y > 0 && c.data[x, y - 1, z] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -y face
                                else if (i == 4 && z < global.chk_size - 1 && c.data[x, y, z + 1] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with +z face
                                else if (i == 5 && z > 0 && c.data[x, y, z - 1] == c.data[x, y, z]) 
                                    canMerge = true; // Merge with -z face

                                // If merging, skip adding the face and move on to the next block
                                if (canMerge) continue;

                                // Add the vertices for this face
                                for (int v = 0; v < 4; v++) {
                                    vertex vtx = cube.f_verts[i, v];
                                    vtx.pos += new Vector3(x, y, z); // Translate position based on the chunk's coordinates
                                    vtx.uv /= game.atlassize;
                                    vtx.uv.X += (c.data[x, y, z].index - 1) / game.atlassize.X;
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