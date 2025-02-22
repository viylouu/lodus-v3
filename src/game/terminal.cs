using SimulationFramework.Drawing;
using SimulationFramework;
using SimulationFramework.Input;

using thrustr.basic;

using TextCopy;


public class terminal {
    static char command_char = '~';

    static bool show_pipe = true;
    static float pipe_time = 0.5f;
    static float pipeing = 0;

    public static bool enabled = false;

    static string cur = "";


    public static void render(ICanvas c) {
        if(enabled) {
            c.Fill(Color.Black); 
            c.DrawRect(0,c.Height/2,c.Width,fontie.dfont.charh+2, Alignment.CenterLeft);

            fontie.rendertext(c, cur + (show_pipe?"|":""), 3,c.Height/2, Alignment.CenterLeft);

            pipeing += Time.DeltaTime;

            if(pipeing > pipe_time) {
                pipeing = 0;
                show_pipe = !show_pipe;
            }

            kb_input();
        }

        if(Keyboard.IsKeyPressed(Key.Enter)) {
            if(enabled && cur.Length > 0 && cur[0] == command_char)
                do_commands();

            enabled = !enabled;
            show_pipe = true;
            pipeing = 0;
            cur = "";
        }
    }

    static Dictionary<Key, string> keymap = new() {
        // alpathetic keys

        { Key.A, "Aa" }, { Key.B, "Bb" }, { Key.C, "Cc" },
        { Key.D, "Dd" }, { Key.E, "Ee" }, { Key.F, "Ff" },
        { Key.G, "Gg" }, { Key.H, "Hh" }, { Key.I, "Ii" },
        { Key.J, "Jj" }, { Key.K, "Kk" }, { Key.L, "Ll" },
        { Key.M, "Mm" }, { Key.N, "Nn" }, { Key.O, "Oo" },
        { Key.P, "Pp" }, { Key.Q, "Qq" }, { Key.R, "Rr" },
        { Key.S, "Ss" }, { Key.T, "Tt" }, { Key.U, "Uu" },
        { Key.V, "Vv" }, { Key.W, "Ww" }, { Key.X, "Xx" },
        { Key.Y, "Yy" }, { Key.Z, "Zz" },

        // symbols

        { Key.Tilde, "~`" }, { Key.Minus, "_-" }, { Key.Plus, "+=" },
        { Key.OpenBracket, "{[" }, { Key.CloseBracket, "}]" }, { Key.BackSlash, @"|\" },
        { Key.Semicolon, ":;" }, { Key.Apostrophe, "\"'" }, { Key.Comma, "<," },
        { Key.Period, ">." }, { Key.Slash, "?/" },
        
        // numbs

        { Key.Key1, "!1" }, { Key.Key2, "@2" }, { Key.Key3, "#3" },
        { Key.Key4, "$4" }, { Key.Key5, "%5" }, { Key.Key6, "^6" },
        { Key.Key7, "&7" }, { Key.Key8, "*8" }, { Key.Key9, "(9" },
        { Key.Key0, ")0" },

        // misc

        { Key.Space, "  " },
    };

    static void kb_input() {
        int shift = (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))? 0:1;

        if(!Keyboard.IsKeyDown(Key.LeftControl))
            foreach(var key in keymap)
                if(Keyboard.IsKeyPressed(key.Key))
                    cur += key.Value[shift];

        // special cases

        if(Keyboard.IsKeyDown(Key.LeftControl) && Keyboard.IsKeyPressed(Key.C))
            ClipboardService.SetText(cur);

        if(Keyboard.IsKeyDown(Key.LeftControl) && Keyboard.IsKeyPressed(Key.V))
            cur += ClipboardService.GetText();

        if(cur.Length > 0 && Keyboard.IsKeyPressed(Key.Backspace))
            cur = cur.Remove(cur.Length-1);
    }

    static void do_commands() {
        string[] command = cur.Split(" ");
        command[0] = command[0].Remove(0,1);

        switch(command[0]) {
            case "tp":
                if(command.Length != 4) {
                    Console.WriteLine("invalid command!");
                    break;
                }

                camera.pos = new(
                    Convert.ToSingle(command[1]),
                    Convert.ToSingle(command[2]),
                    Convert.ToSingle(command[3])
                );
                break; 
        }
    }
}