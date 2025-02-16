using SimulationFramework;

using thrustr.basic;

partial class main {
    static void Main() 
       => handle.init(init, rend);
    
    static void init() {
        Simulation.SetFixedResolution(640, 360, Color.Black);
    }
}