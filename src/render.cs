using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using thrustr.basic;
using thrustr.utils;

partial class main {
    static IDepthMask depth;

    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        game.update();

        game.render_world(c, depth);

        fontie.rendertext(c, fontie.dfont, $"{math.round(1/Time.DeltaTime)} fps", 3,3, ColorF.White);

        if(Keyboard.IsKeyPressed(Key.Escape))
            Application.Exit(false);
    }
}