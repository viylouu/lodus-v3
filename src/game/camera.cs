using System.Numerics;

using thrustr.utils;

public class camera {
    static Vector3 up = Vector3.UnitY;
    static Vector3 front = -Vector3.UnitZ;
    static Vector3 right = Vector3.UnitX;

    public static Vector3 pos = new(0,0,-8);

    public static Matrix4x4 vertprojmatrix {
        get {
            pos = new(0,0,-8);
            return Matrix4x4.CreateLookAtLeftHanded(pos, pos + front, up) * Matrix4x4.CreatePerspectiveFieldOfView(math.rad(90), 640f/360, 0.1f, 1000f);
        }
    }
}