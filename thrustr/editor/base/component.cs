namespace thrustr.obj;

public class component {
    public bool hasscripts;

    public Action? init;
    public Action? update;
    public Action? fixupdate;

    public object[] aparams;
}