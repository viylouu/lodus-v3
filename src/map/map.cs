using System.Numerics;

public class map {
    public static Dictionary<Vector3, chunk> scene = new();
    public static (int startx, int starty, int startz, int endx, int endy, int endz) bounds = (-3,-1,-3, 4,2,4);

    public const byte chk_size = 16;

    public static void populate() {
        for(int x = bounds.startx; x < bounds.endx; x++)
            for(int y = bounds.starty; y < bounds.endy; y++)
                for(int z = bounds.startz; z < bounds.endz; z++) {
                    chunk c = chunking.gen_chunk(new(x,y,z));
                    scene.Add(new(x,y,z), c);
                }
    }
}