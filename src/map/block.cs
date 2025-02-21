public class block {
    public ushort index;

    public static block air = new() { index = 0 };
    public static block def = new() { index = 1 };

    public static block cobble = new() { index = 1 };
    public static block rhyolite = new() { index = 2 };
    public static block dirt = new() { index = 3 };
}