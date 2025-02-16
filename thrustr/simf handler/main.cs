using SimulationFramework;
using SimulationFramework.Drawing;

namespace thrustr.basic;

public class handle {
    static Action _start;
    static Action<ICanvas> _render;

    // thrustr settings

    public static bool do_intro = true;
    public static font? intro_font = null;


    public static void init(Action start, Action<ICanvas> render) {
        _start = start;
        _render = render;

        Simulation sim = Simulation.Create(ini, rend);
        sim.Run();
    }

    static void ini() {
        intro.initintro();

        Window.SetIcon(Graphics.LoadTexture("thrustr/assets/sprites/icons/engine small.png"));

        Window.Title = "thrustr engine";

        _start();
    }

    static void rend(ICanvas c) {
        _render(c);

        if(do_intro)
            intro.dostuff(c, intro_font);
    }
}