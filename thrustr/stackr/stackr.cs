using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using thrustr.utils;

namespace thrustr.stackr;

public struct stk {
    public ssobj obj { get; set; }
    public Vector3 pos { get; set; }
    public float rot { get; set; }

    public stk(ssobj o, Vector3 p, float r) {
        obj = o;
        pos = p;
        rot = r;
    }
}

partial class stackr {
    public static Vector3 cam;
    public static float camrot;

    public static int obj_count => objs.Count;

    public static int layers_rendered;

    static List<stk> objs = new();
    static List<(stk a, Vector2 b)> s_objs = new();

    public static ssobj createobj(ITexture stack, Vector2 size)
        => new ssobj() { stack = stack, size = size, layers = (int)math.round(stack.Height/size.Y) };
    
    public static ssobj createobj(ITexture stack, Vector2 size, Color tint)
        => new ssobj() { stack = stack, size = size, layers = (int)math.round(stack.Height/size.Y), tint = tint.ToColorF() };

    public static void addobj(ssobj obj, Vector3? pos = null, float rot = 0) {
        if(pos == null)
            pos = Vector3.Zero;

        objs.Add(new stk(obj, (Vector3)pos, rot));
    }

    public static void render(ICanvas c) {
        layers_rendered = 0;

        s_objs.Clear();

        if(objs.Count != 1) {
            Matrix3x2 rot = Matrix3x2.CreateRotation(camrot);

            foreach (var item in objs) {
                bool inserted = false;

                Vector2 pos2 = new (item.pos.X -cam.X,item.pos.Z -cam.Z);
                pos2 = Vector2.Transform(pos2, rot);
                pos2 += new Vector2(c.Width/2, c.Height/2 -item.pos.Y);

                for (int i = 0; i < s_objs.Count; i++) {
                    Vector2 pos1 = new (s_objs[i].a.pos.X -cam.X,s_objs[i].a.pos.Z -cam.Z);
                    pos1 = Vector2.Transform(pos1, rot);
                    pos1 += new Vector2(c.Width/2,c.Height/2 -s_objs[i].a.pos.Y);

                    bool res = item.pos.Z - s_objs[i].a.pos.Z < float.Epsilon? (pos2.Y < pos1.Y) : (item.pos.Z > s_objs[i].a.pos.Z);

                    if (res)  {
                        s_objs.Insert(i, (item, pos2));
                        inserted = true;
                        break;
                    }
                }

                if (!inserted)
                    s_objs.Add((item, pos2));
            }
        } else
            s_objs.Add((objs[0], new Vector2(c.Width/2,c.Height/2)));

        for(int i = 0; i < s_objs.Count; i++)
            for(int j = 0; j < s_objs[i].a.obj.layers; j++) {
                c.Translate(s_objs[i].b.X,s_objs[i].b.Y -j -cam.Y);
                c.Rotate(camrot+s_objs[i].a.rot);

                c.DrawTexture(
                    objs[i].obj.stack,
                    new Rectangle(
                        0, (s_objs[i].a.obj.layers-(j+1))*s_objs[i].a.obj.size.Y,
                        s_objs[i].a.obj.size.X, s_objs[i].a.obj.size.Y,
                        Alignment.TopLeft
                    ),
                    new Rectangle(Vector2.Zero,s_objs[i].a.obj.size,Alignment.Center),
                    s_objs[i].a.obj.tint
                );

                c.ResetState();

                layers_rendered++;
            }
    }

    public static void render_with_clear(ICanvas c) {
        render(c);
        objs.Clear();
    }
}