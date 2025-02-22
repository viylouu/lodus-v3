public class block {
    public byte index;

    public static block air = new() { index = 0 };
    public static block def = new() { index = 1 };

    public static block shale = new() { index = 1 };
    public static block andesite = new() { index = 6 };
    public static block rhyolite = new() { index = 2 };

    public static block andesite_bricks = new() { index = 5 };
    public static block shale_bricks = new() { index = 7 };

    public static block dirt = new() { index = 3 };
    public static block grass = new() { index = 4 };
}