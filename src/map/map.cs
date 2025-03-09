using System.Numerics;
using System.Collections.Concurrent;

public class map {
    public static ConcurrentDictionary<Vector3, chunk?> scene = new();

    public static void populate() {
        chunking.gen_chunk_thing(new(0,0,0));
    }
}