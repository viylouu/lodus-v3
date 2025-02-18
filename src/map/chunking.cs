using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public class chunking {
    static FastNoiseLite fnl;

    static Random r;

    public static int seed;

    public static ulong max_actions_before_wait = 2048;
    static ulong actions = 0;

    public static void load() {
        r = new();
        fnl = new();

        seed = r.Next(int.MinValue, int.MaxValue);

        fnl.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        fnl.SetFrequency(0.025f);
        fnl.SetFractalType(FastNoiseLite.FractalType.FBm);
        fnl.SetSeed(seed);
    }


    public static async void gen_chunk(Vector3 pos) {
        chunk c = new();

        map.scene.Add(pos, c);

        int cx = (int)pos.X*global.chk_size,
            cy = (int)pos.Y*global.chk_size,
            cz = (int)pos.Z*global.chk_size;

        Console.WriteLine($"making chunk at ({cx}, {cy}, {cz})");

        bool empty = true;

        c.data = new block[global.chk_size,global.chk_size,global.chk_size];

        // generate data

        await Task.Delay(1);

        for(int x = 0; x < global.chk_size; x++)
            for(int y = 0; y < global.chk_size; y++)
                for(int z = 0; z < global.chk_size; z++) {
                    block b = get_block_shaping(x+cx,y+cy,z+cz);
                    c.data[x,y,z] = b;

                    if(b == block.def)
                        empty = false;

                    actions++;

                    if(actions > max_actions_before_wait) {
                        actions = 0;
                        await Task.Delay(1);
                    }
                }

        if(empty)
            map.scene[pos] = null;

        // generate mesh
        
        await Task.Delay(1);

        (List<vertex> verts, List<uint> inds) = generate_mesh(c.data, cx,cy,cz);

        await Task.Delay(1);

        c.m_inds = inds.ToArray();
        c.m_verts = verts.ToArray();

        global.chks_loaded++;

        map.scene[pos] = c;
    }

    static block get_block_shaping(int x, int y, int z) {
        if(fnl.GetNoise(x,z)*12+16 > y)
            return block.def;
        else
            return block.air;
    }

    static (List<vertex>, List<uint>) generate_mesh(block[,,] data, int cx, int cy, int cz) {
        List<vertex> verts = new();
        List<uint> inds = new();

        for (int x = 0; x < global.chk_size; x++)
            for (int y = 0; y < global.chk_size; y++)
                for (int z = 0; z < global.chk_size; z++)
                    if(data[x,y,z].index == 1)
                        for (int i = 0; i < 6; i++) {
                            // Determine if the face should be rendered
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == global.chk_size - 1 && get_block_shaping(x+1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != global.chk_size - 1 && data[x + 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x-1+cx,y+cy,z+cz) == block.air) shouldRenderFace = true;
                                    if(x != 0 && data[x - 1, y, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == global.chk_size - 1 && get_block_shaping(x+cx,y+1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != global.chk_size - 1 && data[x, y + 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x+cx,y-1+cy,z+cz) == block.air) shouldRenderFace = true; 
                                    if(y != 0 && data[x, y - 1, z] == block.air) shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == global.chk_size - 1 && get_block_shaping(x+cx,y+cy,z+1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != global.chk_size - 1 && data[x, y, z + 1] == block.air) shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x+cx,y+cy,z-1+cz) == block.air) shouldRenderFace = true; 
                                    if(z != 0 && data[x, y, z - 1] == block.air) shouldRenderFace = true;
                                    break;
                            }

                            if (shouldRenderFace) {
                                // Add the vertices for this face
                                for (int v = 0; v < 4; v++) {
                                    vertex vtx = cube.f_verts[i, v];
                                    vtx.pos += new Vector3(x, y, z); // Translate position based on the chunk's coordinates
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

        return (verts, inds);
    }
}