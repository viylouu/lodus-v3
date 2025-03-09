using System.Numerics;
using System.Collections.Concurrent;

using SimulationFramework;

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

    static ConcurrentDictionary<Vector2, float> generatedBlockHeights = new();
    static int blockHeightSamples = 32/5;


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

    public static void gen_chunk_thing(Vector3 pos) {
        chunk c = new();

        float cx = (int)pos.X*global.chk_size,
              cy = (int)pos.Y*global.chk_size,
              cz = (int)pos.Z*global.chk_size;

        bool empty = true;

        c.data = new block[global.chk_size,global.chk_size,global.chk_size];

        // generate data

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++) {
                    //////////////////////////////////////////////////////////////

                    block b = get_block_shaping(x+cx,y+cy,z+cz);
                    c.data[x,y,z] = b;

                    if(b != block.air)
                        empty = false;

                    //////////////////////////////////////////////////////////////

                    if(b == block.def) {
                        float nh = get_noise_height(x+cx,z+cz);
                        if(y+cy > nh-1)
                            c.data[x,y,z] = block.grass;
                        else if(y+cy > nh-4-dirtoff.GetNoise(x,z)*2)
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

                    //////////////////////////////////////////////////////////////
                }

        global.chks_loaded++;

        if(empty)
            return;

        global.filled_chks_loaded++;

        // generate more data

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++)
                    if(c.data[x,y,z] == block.def) {
                        float nh = get_noise_height(x+cx,z+cz);
                        if(y+cy > nh-1)
                            c.data[x,y,z] = block.grass;
                        else if(y+cy > nh-4-dirtoff.GetNoise(x,z)*2)
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

        (List<vertex> vertices, List<uint> indices) = generate_mesh(c,pos);

        c.m_inds = indices.ToArray();
        c.m_verts = vertices.ToArray();

        c.starttime = Time.TotalTime;

        map.scene[pos] = c;
    }

    public static float get_real_height_at(Vector2 pos) {
        float bass = height.GetNoise(pos.X,pos.Y)*12;
        float big = bigheight.GetNoise(pos.X,pos.Y)*72;
        float xp = pos.X, zp = pos.Y;
        steeps.DomainWarp(ref xp,ref zp);
        float steep = steeps.GetNoise(xp,zp)*16;

        return bass + big + steep +64;
    }

    public static float get_noise_height(float x, float z) {
        int sample1X = (int)math.floor(x/blockHeightSamples)*blockHeightSamples;
        int sample1Z = (int)math.floor(z/blockHeightSamples)*blockHeightSamples;
        int sample2X = sample1X + blockHeightSamples;
        int sample2Z = sample1Z + blockHeightSamples;

        if (sample1X == sample2X) sample2X += 1;
        if (sample1Z == sample2Z) sample2Z += 1;

        Vector2 pAA = new(sample1X,sample1Z);
        Vector2 pAB = new(sample1X,sample2Z);
        Vector2 pBA = new(sample2X,sample1Z);
        Vector2 pBB = new(sample2X,sample2Z);
        
        if(!generatedBlockHeights.TryGetValue(pAA, out float AA)) {
            generatedBlockHeights.TryAdd(pAA, get_real_height_at(pAA));
            AA = generatedBlockHeights[pAA];
            if(x == sample1X && z == sample1Z)
                return AA;
        }
        if(!generatedBlockHeights.TryGetValue(pBA, out float BA)) {
            generatedBlockHeights.TryAdd(pBA, get_real_height_at(pBA));
            BA = generatedBlockHeights[pBA];
            if(x == sample2X && z == sample1Z)
                return BA;
        }
        if(!generatedBlockHeights.TryGetValue(pAB, out float AB)) {
            generatedBlockHeights.TryAdd(pAB, get_real_height_at(pAB));
            AB = generatedBlockHeights[pAB];
            if(x == sample1X && z == sample2Z)
                return AB;
        }
        if(!generatedBlockHeights.TryGetValue(pBB, out float BB)) {
            generatedBlockHeights.TryAdd(pBB, get_real_height_at(pBB));
            BB = generatedBlockHeights[pBB];
            if(x == sample2X && z == sample2Z)
                return BB;
        }

        float diffx = (x-sample1X)/blockHeightSamples;
        float diffz = (z-sample1Z)/blockHeightSamples;

        return math.lerp(math.lerp(AA,BA,diffx),math.lerp(AB,BB,diffx),diffz);
    }

    static block get_block_shaping(float x, float y, float z) {
        if(y > get_noise_height(x,z))
            return block.air;

        float cave = caves.GetNoise(x,y,z);
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

        generate_mesh(pos);

        Console.WriteLine($"placed block in chunk ({pos.X}, {pos.Y}, {pos.Z})");
    }

    public static void generate_mesh(Vector3 pos) {
        map.scene.TryGetValue(pos, out chunk? c);

        if(c == null)
            return;

        if(c.data == null)
            return;

        (List<vertex> vertices, List<uint> indices) = generate_mesh(c,pos);

        c.m_inds = indices.ToArray();
        c.m_verts = vertices.ToArray();

        map.scene[pos] = c;
    }

    public static (List<vertex>, List<uint>) generate_mesh(chunk c, Vector3 pos) {
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

        return (verts,inds);
    }

    public static (Vector3 pos, Vector3 cpos) from_worldspace(Vector3 wspos) {
        Vector3 pos = math.nfloor(wspos/global.chk_size);
        Vector3 cpos = math.wmod(wspos,global.chk_size);

        return (pos, cpos);
    }
}