using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.basic;

partial class main {
    static void Main()
       => handle.init(init, rend);
    
    static void init() {
        Simulation.SetFixedResolution(640, 360, Color.Black, false, true, false);

        depth = Graphics.CreateDepthMask(640,360);

        map.populate();

        game.load();
    }
}