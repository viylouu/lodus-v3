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

        fontie.rendertext(c, $"{math.round(1/Time.DeltaTime)} fps", 3,3);
        fontie.rendertext(c, $"seed: {chunking.seed}", 3,4+fontie.dfont.charh-fontie.dfont.chart);

        if(Keyboard.IsKeyPressed(Key.Escape))
            Application.Exit(false);
    }
}