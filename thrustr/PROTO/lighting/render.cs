using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.basic;
using thrustr.utils;
using thrustr.PROTO;
using SimulationFramework.Input;

partial class _main {
    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        lightingshader shad = new();
        shad.mp = Mouse.Position;

        c.Fill(shad);
        c.DrawRect(0,0,640,360);

        fontie.rendertext(c, $"{math.round(1/Time.DeltaTime)} fps", 3,3);
    }
}