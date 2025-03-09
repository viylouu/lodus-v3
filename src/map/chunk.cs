using SimulationFramework.Drawing;

public class chunk {
    public block[,,] data;

    public vertex[] m_verts;
    public uint[] m_inds;

    public IGeometry? geom;

    public float starttime;
}