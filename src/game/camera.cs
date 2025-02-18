using System.Numerics;
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Input;
using thrustr.utils;

public class camera {
    public static float speed = 8f;
    public static float sprint_speed = 16f;
    public static float sens = 50000f;
    public static float fov = 90f;

    public static Vector3 pos = new(0,24,0);

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

    static Vector2 last_window_size;

    
    public static Matrix4x4 vertprojmatrix {
        get {
            return Matrix4x4.CreateLookAt(pos, pos + front, up) * Matrix4x4.CreatePerspectiveFieldOfView(math.rad(fov), aspectratio, 0.1f, 1000f);
        }
    }

    public static void update() {
        float spd = Keyboard.IsKeyDown(Key.LeftControl)? sprint_speed : speed;

        if(Keyboard.IsKeyDown(Key.W))
            pos += movefront * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.A)) 
            pos -= right * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.S))
            pos -= movefront * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.D))
            pos += right * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.Space))
            pos.Y += spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.LeftShift))
            pos.Y -= spd * Time.DeltaTime;

        if(canlook) {
            if(global.fr_intercept.BaseWindowProvider.Size != last_window_size) {
                firstmove = true;
                last_window_size = global.fr_intercept.BaseWindowProvider.Size;
            }

            if(firstmove) {
                Mouse.Position = center;
                center = Mouse.Position;
                lastPos = center;

                firstmove = false;
            } else {
                var d = Mouse.Position - lastPos;
                lastPos = center;

                Mouse.Position = center;

                yaw += d.X * sens / Window.Width * aspectratio * Time.DeltaTime;
                pitch -= d.Y * sens / Window.Height * Time.DeltaTime;

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

        Mouse.Visible = !canlook;
    }
}