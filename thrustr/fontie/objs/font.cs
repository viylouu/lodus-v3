using SimulationFramework.Drawing;

namespace thrustr.basic;

public class font {
    public ITexture tex;
    public string chars;
    public int charw, charh;
    public chardata[] data;

    public caseness fcase;
}

public enum caseness { 
    lower,
    both,
    upper
}