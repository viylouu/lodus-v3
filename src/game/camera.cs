using System.Numerics;
using SimulationFramework;
using SimulationFramework.Input;
using thrustr.utils;

public class camera {
    public static float speed = 8f;
    public static float sens = 25f;
    public static float fov = 90f;

    public static Vector3 pos = new(0,0,8);

    public static float pitch, yaw = -90f;

    public static Vector2 center = new Vector2(Window.Width/2f, Window.Height/2f);

    static float aspectratio = 16/9f;

    static bool firstmove = true;
    static Vector2 lastPos;

    static Vector3 up = Vector3.UnitY;
    static Vector3 front = -Vector3.UnitZ;
    static Vector3 movefront = -Vector3.UnitZ;
    static Vector3 right = Vector3.UnitX;

    public static bool canlook = true;

    
    public static Matrix4x4 vertprojmatrix {
        get {
            return Matrix4x4.CreateLookAt(pos, pos + front, up) * Matrix4x4.CreatePerspectiveFieldOfView(math.rad(fov), aspectratio, 0.1f, 1000f);
        }
    }

    public static void update() {
        if(Keyboard.IsKeyDown(Key.W))
            pos += movefront * speed * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.A)) 
            pos -= right * speed * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.S))
            pos -= movefront * speed * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.D))
            pos += right * speed * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.Space))
            pos.Y += speed * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.LeftShift))
            pos.Y -= speed * Time.DeltaTime;

        if(canlook) {
            if(firstmove) {
                Mouse.Position = center;
                center = Mouse.Position;
                lastPos = center;

                firstmove = false;
            } else {
                var d = Mouse.Position - lastPos;
                lastPos = center;

                Mouse.Position = center;

                yaw += d.X * sens / Window.Width * aspectratio * Time.TotalTime;
                pitch -= d.Y * sens / Window.Height * Time.TotalTime;

                pitch = math.clamp(pitch,-89.99f,89.99f);
            }

            front.X = math.cos(math.rad(pitch)) * math.cos(math.rad(yaw));
            front.Y = math.sin(math.rad(pitch));
            front.Z = math.cos(math.rad(pitch)) * math.sin(math.rad(yaw));

            front = math.norm(front);

            movefront = new(front.X,0,front.Z);
            movefront = math.norm(movefront);

            right = math.norm(Vector3.Cross(front, Vector3.UnitY));
            up = math.norm(Vector3.Cross(right, front));
        }
    }
}