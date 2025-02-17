using System.Numerics;

public class chunking {
    static FastNoiseLite fnl;

    public static void load() {
        fnl = new();
        fnl.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        fnl.SetFrequency(0.05f);
    }


    public static chunk gen_chunk(Vector3 pos) {
        chunk c = new();

        int cx = (int)pos.X,
            cy = (int)pos.Y,
            cz = (int)pos.Z;

        c.data = new block[map.chk_size,map.chk_size,map.chk_size];

        // generate data

        for(int x = 0; x < map.chk_size; x++)
            for(int y = 0; y < map.chk_size; y++)
                for(int z = 0; z < map.chk_size; z++) {
                    c.data[x,y,z] = get_block_shaping(x+cx,y+cy,z+cz);
                }

        // generate mesh
        
        (List<vertex> verts, List<uint> inds) = generate_mesh(c.data);

        c.m_inds = inds.ToArray();
        c.m_verts = verts.ToArray();

        return c;
    }

    static block get_block_shaping(int x, int y, int z) {
        if(fnl.GetNoise(x,y,z) > 0)
            return block.def;
        else
            return block.air;
    }

    static (List<vertex>, List<uint>) generate_mesh(block[,,] data) {
        List<vertex> verts = new();
        List<uint> inds = new();

        for (int x = 0; x < map.chk_size; x++)
            for (int y = 0; y < map.chk_size; y++)
                for (int z = 0; z < map.chk_size; z++)
                    if(data[x,y,z].index == 1)
                        for (int i = 0; i < 6; i++) {
                            // Determine if the face should be rendered
                            bool shouldRenderFace = false;

                            switch (i) {
                                case 0: // +x face
                                    if (x == map.chk_size - 1 && get_block_shaping(x+1,y,z).index == 0) shouldRenderFace = true;
                                    else if(x != map.chk_size - 1 && data[x + 1, y, z].index == 0) shouldRenderFace = true;
                                    break;
                                case 1: // -x face
                                    if (x == 0 && get_block_shaping(x-1,y,z).index == 0) shouldRenderFace = true;
                                    else if(x != 0 && data[x - 1, y, z].index == 0) shouldRenderFace = true;
                                    break;
                                case 2: // +y face
                                    if (y == map.chk_size - 1 && get_block_shaping(x,y+1,z).index == 0) shouldRenderFace = true; 
                                    else if(y != map.chk_size - 1 && data[x, y + 1, z].index == 0) shouldRenderFace = true;
                                    break;
                                case 3: // -y face
                                    if (y == 0 && get_block_shaping(x,y-1,z).index == 0) shouldRenderFace = true; 
                                    else if(y != 0 && data[x, y - 1, z].index == 0) shouldRenderFace = true;
                                    break;
                                case 4: // +z face
                                    if (z == map.chk_size - 1 && get_block_shaping(x,y,z+1).index == 0) shouldRenderFace = true; 
                                    else if(z != map.chk_size - 1 && data[x, y, z + 1].index == 0) shouldRenderFace = true;
                                    break;
                                case 5: // -z face
                                    if (z == 0 && get_block_shaping(x,y,z-1).index == 0) shouldRenderFace = true; 
                                    else if(z != 0 && data[x, y, z - 1].index == 0) shouldRenderFace = true;
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