using System.Numerics;

public class map {
    public static Dictionary<Vector3, chunk?> scene = new();

    public static void populate() {
        chunking.gen_chunk(new(0,0,0));
    }
}