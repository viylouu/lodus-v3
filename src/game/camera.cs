using System.ComponentModel;
using System.Net;
using System.Numerics;

using SimulationFramework;
using SimulationFramework.Input;

using thrustr.utils;

public class camera {
    public static float speed = 20f;
    public static float sprint_speed = 48f;
    public static float alt_speed = 256f;
    public static float sens = 12500f;
    public static float deffov = 90f;
    public static float zoomfov = 30f;
    static float fov = 1f;

    public static float zoomsmooth = 6f;

    public static Vector3 pos = new(0,256,0);
    public static Vector3 vel;

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
    public static bool canmove = true;

    static Vector2 last_window_size;

    static float jumpforce = 10;
    static float gravity = 42f;
    static float deccel = 24f;

    static float maxvel = 8f;
    static float maxvelsprint = 24f;
    static float maxvelalt = 64f;

    
    public static Matrix4x4 vertprojmatrix {
        get {
            return Matrix4x4.CreateLookAt(pos, pos + front, up) * Matrix4x4.CreatePerspectiveFieldOfView(math.rad(fov), aspectratio, 0.1f, 1000f);
        }
    }

    public static void update() {
        if(canmove) {
            float spd = Keyboard.IsKeyDown(Key.LeftAlt)? alt_speed : Keyboard.IsKeyDown(Key.LeftControl)? sprint_speed : speed;

            if(Keyboard.IsKeyDown(Key.W))
                vel += movefront * spd * Time.DeltaTime;
            if(Keyboard.IsKeyDown(Key.A)) 
                vel -= right * spd * Time.DeltaTime;
            if(Keyboard.IsKeyDown(Key.S))
                vel -= movefront * spd * Time.DeltaTime;
            if(Keyboard.IsKeyDown(Key.D))
                vel += right * spd * Time.DeltaTime;
            if(Keyboard.IsKeyDown(Key.Space))
                pos.Y += spd * Time.DeltaTime * 0.35f;
            if(Keyboard.IsKeyDown(Key.LeftShift))
                pos.Y -= spd * Time.DeltaTime * 0.35f;
        }

        if(vel.X != 0 || vel.Z != 0) {
            if((!Keyboard.IsKeyDown(Key.W) && !Keyboard.IsKeyDown(Key.S) && !Keyboard.IsKeyDown(Key.A) && !Keyboard.IsKeyDown(Key.D)) || !canmove) {
                Vector2 horizdec = math.norm(new Vector2(vel.X,vel.Z))*deccel*Time.DeltaTime;

                vel.X -= horizdec.X;
                vel.Z -= horizdec.Y;
            }

            float mv = Keyboard.IsKeyDown(Key.LeftAlt)? maxvelalt : Keyboard.IsKeyDown(Key.LeftControl)? maxvelsprint : maxvel;

            if(new Vector2(vel.X,vel.Z).Length() > mv) {
                Vector2 horiz = math.norm(new Vector2(vel.X,vel.Z));

                vel.X = horiz.X * mv;
                vel.Z = horiz.Y * mv;
            }
        }

        deffov = global.fov;
        sens = global.sens*500;

        fov += ease.dyn(fov, (canmove && Keyboard.IsKeyDown(Key.C))? zoomfov : deffov, zoomsmooth);
        fov = math.clamp(fov, 1,170);

        pos += vel * Time.DeltaTime;

        if(canmove && Keyboard.IsKeyPressed(Key.P))
            chunking.place_block(pos, block.rhyolite);

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