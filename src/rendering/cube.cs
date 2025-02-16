using System.Numerics;

public class cube {
    //faces

    public static Vector3[] f_normals = {
        new (1, 0, 0),  // +x
        new (-1, 0, 0), // -x
        new (0, 1, 0),  // +y
        new (0, -1, 0), // -y
        new (0, 0, 1),  // +z
        new (0, 0, -1)  // -z
    };

    public static uint[,] f_inds = {
        { 0, 1, 2, 2, 3, 0 }, // +x
        { 0, 1, 2, 2, 3, 0 }, // -x
        { 0, 1, 2, 2, 3, 0 }, // +y
        { 0, 1, 2, 2, 3, 0 }, // -y
        { 0, 1, 2, 2, 3, 0 }, // +z
        { 0, 1, 2, 2, 3, 0 }  // -z
    };

    public static vertex[,] f_verts = new vertex[6, 4] {
        { /////////////////////////////////////////////////////////// +x
            new() { pos = new (1, 1, 1), uv = new (0, .3333f) }, 
            new() { pos = new (1, 0, 1), uv = new (0, .6667f) }, 
            new() { pos = new (1, 0, 0), uv = new (.5f, .6667f) }, 
            new() { pos = new (1, 1, 0), uv = new (.5f, .3333f) }
        }, { //////////////////////////////////////////////////////// -x
            new() { pos = new (0, 1, 0), uv = new (.5f, .3333f) }, 
            new() { pos = new (0, 0, 0), uv = new (.5f, .6667f) }, 
            new() { pos = new (0, 0, 1), uv = new (1f, .6667f) }, 
            new() { pos = new (0, 1, 1), uv = new (1f, .3333f) }
        }, { //////////////////////////////////////////////////////// +y
            new() { pos = new (0, 1, 1), uv = new (0, 0) }, 
            new() { pos = new (0, 1, 0), uv = new (0, .3333f) }, 
            new() { pos = new (1, 1, 0), uv = new (.5f, .3333f) }, 
            new() { pos = new (1, 1, 1), uv = new (.5f, 0) } 
        }, { //////////////////////////////////////////////////////// -y
            new() { pos = new (1, 0, 1), uv = new (.5f, 0) }, 
            new() { pos = new (1, 0, 0), uv = new (.5f, .3333f) }, 
            new() { pos = new (0, 0, 0), uv = new (1f, .3333f) }, 
            new() { pos = new (0, 0, 1), uv = new (1f, 0) }
        }, { //////////////////////////////////////////////////////// +z
            new() { pos = new (0, 1, 1), uv = new (0, .6667f) }, 
            new() { pos = new (1, 1, 1), uv = new (.5f, .6667f) }, 
            new() { pos = new (1, 0, 1), uv = new (.5f, 1f) }, 
            new() { pos = new (0, 0, 1), uv = new (0, 1f) }
        }, { //////////////////////////////////////////////////////// -z
            new() { pos = new (1, 1, 0), uv = new (.5f, .6667f) }, 
            new() { pos = new (0, 1, 0), uv = new (1f, .6667f) }, 
            new() { pos = new (0, 0, 0), uv = new (1f, 1f) }, 
            new() { pos = new (1, 0, 0), uv = new (.5f, 1f) } 
        }  
    };

    // actual cube (not just faces)

    public static Vector3[] normals = {
        new (1, 0, 0),
        new (-1, 0, 0),
        new (0, 1, 0),
        new (0, -1, 0),
        new (0, 0, 1),
        new (0, 0, -1)
    };

    public static uint[] inds = {
        0,1,2, 2,3,0,
        4,5,6, 6,7,4,
        8,9,10, 10,11,8,
        12,13,14, 14,15,12,
        16,17,18, 18,19,16,
        20,21,22, 22,23,20
    };

    public static vertex[] verts = {
        new() { pos = new (1, 1, 1), uv = new (0, .3333f) }, 
        new() { pos = new (1, 0, 1), uv = new (0, .6667f) }, 
        new() { pos = new (1, 0, 0), uv = new (.5f, .6667f) }, 
        new() { pos = new (0, 1, 0), uv = new (.5f, .3333f) }, 
        new() { pos = new (0, 0, 0), uv = new (.5f, .6667f) }, 
        new() { pos = new (0, 0, 1), uv = new (1f, .6667f) }, 
        new() { pos = new (0, 1, 1), uv = new (1f, .3333f) },
        new() { pos = new (0, 1, 1), uv = new (0, 0) }, 
        new() { pos = new (0, 1, 0), uv = new (0, .3333f) }, 
        new() { pos = new (1, 1, 0), uv = new (.5f, .3333f) }, 
        new() { pos = new (1, 1, 1), uv = new (.5f, 0) } ,
        new() { pos = new (1, 0, 1), uv = new (.5f, 0) }, 
        new() { pos = new (1, 0, 0), uv = new (.5f, .3333f) }, 
        new() { pos = new (0, 0, 0), uv = new (1f, .3333f) }, 
        new() { pos = new (0, 0, 1), uv = new (1f, 0) },
        new() { pos = new (0, 1, 1), uv = new (0, .6667f) }, 
        new() { pos = new (1, 1, 1), uv = new (.5f, .6667f) }, 
        new() { pos = new (1, 0, 1), uv = new (.5f, 1f) }, 
        new() { pos = new (0, 0, 1), uv = new (0, 1f) },
        new() { pos = new (1, 1, 0), uv = new (.5f, .6667f) }, 
        new() { pos = new (0, 1, 0), uv = new (1f, .6667f) }, 
        new() { pos = new (0, 0, 0), uv = new (1f, 1f) }, 
        new() { pos = new (1, 0, 0), uv = new (.5f, 1f) } 
    };
}