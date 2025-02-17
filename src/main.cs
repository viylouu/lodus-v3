using SimulationFramework;
using SimulationFramework.Drawing.Shaders.Compiler;

using thrustr.basic;

partial class main {
    static void Main() {
        ShaderCompiler.DumpShaders = true;
       handle.init(init, rend);
    }
    
    static void init() {
        Simulation.SetFixedResolution(640, 360, Color.Black);
        
    }
}