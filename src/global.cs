using SimulationFramework;

public class global {
    public static FixedResolutionInterceptor fr_intercept = Application.GetComponent<FixedResolutionInterceptor>();
    public const byte chk_size = 16;
    public static long chks_loaded = 0;
}