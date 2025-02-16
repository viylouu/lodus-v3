using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.basic;
using thrustr.utils;

partial class main {
    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        game.render_world(c);

        fontie.rendertext(c, fontie.dfont, $"{math.round(1/Time.DeltaTime)} fps", 3,3, ColorF.White);
    }
}