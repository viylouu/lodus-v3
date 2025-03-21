using SimulationFramework;


public class global {
    public static FixedResolutionInterceptor fr_intercept = Application.GetComponent<FixedResolutionInterceptor>();

    //STAY AT 32!!!!!!!!!!!!!!!!!!!!!!!!!
    public const byte chk_size = 32;
    public static long chks_loaded = 0;
    public static long filled_chks_loaded = 0;
    public static int frames_waited_for_dispatch = 0;

    //settings

    public static bool color_quant = true;
    public static int color_quant_amt = 18;
    public static bool better_quant = true;

    public static int fov = 90;

    public static int sens = 25;

    public static int render_dist = 10;

    public static bool fog = true;
    public static float fog_density = 0.5f;

    public static bool pixelate = true;

    public static int max_chunk_gen_threads = 20;
    public static int frames_between_dispatch = 120;


    public static void window_resized(int w, int h) {
        main.resize(w,h);
        game.resize(w,h);
        camera.resize(w,h);
    }
}