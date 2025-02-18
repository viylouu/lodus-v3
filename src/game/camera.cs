using System.Numerics;

using SimulationFramework;
using SimulationFramework.Input;

using thrustr.utils;

public class camera {
    public static float speed = 20f;
    public static float sprint_speed = 48f;
    public static float sens = 12500f;
    public static float fov = 90f;

    public static Vector3 pos = new(0,24,0);
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

    static Vector2 last_window_size;

    static float jumpforce = 10;
    static float gravity = 42f;
    static float deccel = 24f;

    static float maxvel = 8f;
    static float maxvelsprint = 24f;

    
    public static Matrix4x4 vertprojmatrix {
        get {
            return Matrix4x4.CreateLookAt(pos, pos + front, up) * Matrix4x4.CreatePerspectiveFieldOfView(math.rad(fov), aspectratio, 0.1f, 1000f);
        }
    }

    public static void update() {
        float spd = Keyboard.IsKeyDown(Key.LeftControl)? sprint_speed : speed;

        if(Keyboard.IsKeyDown(Key.W))
            vel += movefront * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.A)) 
            vel -= right * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.S))
            vel -= movefront * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.D))
            vel += right * spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.Space))
            pos.Y += spd * Time.DeltaTime;
        if(Keyboard.IsKeyDown(Key.LeftShift))
            pos.Y -= spd * Time.DeltaTime;

        if(vel.X != 0 || vel.Z != 0) {
            if(!Keyboard.IsKeyDown(Key.W) && !Keyboard.IsKeyDown(Key.S) && !Keyboard.IsKeyDown(Key.A) && !Keyboard.IsKeyDown(Key.D)) {
                Vector2 horizdec = math.norm(new Vector2(vel.X,vel.Z))*deccel*Time.DeltaTime;

                vel.X -= horizdec.X;
                vel.Z -= horizdec.Y;
            }

            float mv = Keyboard.IsKeyDown(Key.LeftControl)? maxvelsprint : maxvel;

            if(new Vector2(vel.X,vel.Z).Length() > mv) {
                Vector2 horiz = math.norm(new Vector2(vel.X,vel.Z));

                vel.X = horiz.X * mv;
                vel.Z = horiz.Y * mv;
            }
        }

        //vel.Y -= gravity * Time.DeltaTime;
        pos += vel * Time.DeltaTime;

        /*int wx = (int)math.floor(pos.X/global.chk_size),
            wy = (int)math.floor((pos.Y-2)/global.chk_size),
            wz = (int)math.floor(pos.Z/global.chk_size);

        if(map.scene.TryGetValue(new(wx,wy,wz), out chunk? chk))
            if(chk != null) 
                if(chk.data != null) 
                    for(int x = 0; x < global.chk_size; x++)
                        for(int y = 0; y < global.chk_size; y++)
                            for(int z = 0; z < global.chk_size; z++)
                                if(chk.data[x,y,z] != block.air)
                                    if(pos.Y < y+wy*global.chk_size+1.75f) {
                                        pos.Y = y+wy*global.chk_size+1.75f;
                                        vel.Y = 0;
                                    }*/

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