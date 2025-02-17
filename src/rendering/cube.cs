public class cube {
    //faces

    public static vertex[,] f_verts = new vertex[6, 4] {
        { /////////////////////////////////////////////////////////// +x
            new() { pos = new (0.5f, 0.5f, 0.5f), uv = new (0, .3333f), normal = new (1f, 0f, 0f) },
            new() { pos = new (0.5f, -0.5f, 0.5f), uv = new (0, .6667f), normal = new (1f, 0f, 0f) },
            new() { pos = new (0.5f, -0.5f, -0.5f), uv = new (.5f, .6667f), normal = new (1f, 0f, 0f) },
            new() { pos = new (0.5f, 0.5f, -0.5f), uv = new (.5f, .3333f), normal = new (1f, 0f, 0f) }
        }, { //////////////////////////////////////////////////////// -x
            new() { pos = new (-0.5f, 0.5f, -0.5f), uv = new (.5f, .3333f), normal = new (-1f, 0f, 0f) },
            new() { pos = new (-0.5f, -0.5f, -0.5f), uv = new (.5f, .6667f), normal = new (-1f, 0f, 0f) },
            new() { pos = new (-0.5f, -0.5f, 0.5f), uv = new (1f, .6667f), normal = new (-1f, 0f, 0f) },
            new() { pos = new (-0.5f, 0.5f, 0.5f), uv = new (1f, .3333f), normal = new (-1f, 0f, 0f) }
        }, { //////////////////////////////////////////////////////// +y
            new() { pos = new (-0.5f, 0.5f, 0.5f), uv = new (0, 0), normal = new (0f, 1f, 0f) },
            new() { pos = new (-0.5f, 0.5f, -0.5f), uv = new (0, .3333f), normal = new (0f, 1f, 0f) },
            new() { pos = new (0.5f, 0.5f, -0.5f), uv = new (.5f, .3333f), normal = new (0f, 1f, 0f) },
            new() { pos = new (0.5f, 0.5f, 0.5f), uv = new (.5f, 0), normal = new (0f, 1f, 0f) }
        }, { //////////////////////////////////////////////////////// -y
            new() { pos = new (0.5f, -0.5f, 0.5f), uv = new (.5f, 0), normal = new (0f, -1f, 0f) },
            new() { pos = new (0.5f, -0.5f, -0.5f), uv = new (.5f, .3333f), normal = new (0f, -1f, 0f) },
            new() { pos = new (-0.5f, -0.5f, -0.5f), uv = new (1f, .3333f), normal = new (0f, -1f, 0f) },
            new() { pos = new (-0.5f, -0.5f, 0.5f), uv = new (1f, 0), normal = new (0f, -1f, 0f) }
        }, { //////////////////////////////////////////////////////// +z
            new() { pos = new (-0.5f, 0.5f, 0.5f), uv = new (0, .6667f), normal = new (0f, 0f, 1f) },
            new() { pos = new (0.5f, 0.5f, 0.5f), uv = new (.5f, .6667f), normal = new (0f, 0f, 1f) },
            new() { pos = new (0.5f, -0.5f, 0.5f), uv = new (.5f, 1f), normal = new (0f, 0f, 1f) },
            new() { pos = new (-0.5f, -0.5f, 0.5f), uv = new (0, 1f), normal = new (0f, 0f, 1f) }
        }, { //////////////////////////////////////////////////////// -z
            new() { pos = new (0.5f, 0.5f, -0.5f), uv = new (.5f, .6667f), normal = new (0f, 0f, -1f) },
            new() { pos = new (-0.5f, 0.5f, -0.5f), uv = new (1f, .6667f), normal = new (0f, 0f, -1f) },
            new() { pos = new (-0.5f, -0.5f, -0.5f), uv = new (1f, 1f), normal = new (0f, 0f, -1f) },
            new() { pos = new (0.5f, -0.5f, -0.5f), uv = new (.5f, 1f), normal = new (0f, 0f, -1f) }
        }
    };
}