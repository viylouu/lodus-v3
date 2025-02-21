using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.basic;

partial class main {
    static void Main()
       => handle.init(init, rend);
    
    static void init() {
        Simulation.SetFixedResolution(640,360, Color.Black, false,true,false);

        depth = Graphics.CreateDepthMask(640,360);

        chunking.load();
        game.load();

        map.populate();

        skybox = meshing.load_fbx("assets/skybox.fbx");
        sbfrag = new();
        sbvert = new();
    }
}