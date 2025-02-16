using SimulationFramework.Drawing;
using System.Numerics;
using SimulationFramework;

namespace thrustr.basic;

public class fontie {
    public static ITexture dfonttex = Graphics.LoadTexture("thrustr/assets/fonts/def font.png");
    public static font dfont = genfont_wpath(dfonttex, "thrustr/assets/fonts/def font.txt");

    public static font genfont(ITexture _tex, string _chars) {
        font font = new();

        int _charw = _tex.Width/_chars.Length, _charh = _tex.Height;

        font.tex = _tex;
        font.charw = _charw;
        font.charh = _charh;
        font.chars = _chars;
        font.data = new chardata[_chars.Length];
        font.fcase = caseness.both;

        for(int i = 0; i < _chars.Length; i++) {
            font.data[i] = new chardata();
            
            int w = 0;

            for(int x = 0; x < _charw; x++)
                for(int y = 0; y < _charh; y++)
                    if(_tex[i*_charw+x,y].A > 0)
                        w = x;
            
            font.data[i].width = w+2;
        }

        return font;
    }

    public static font genfont_wpath(ITexture _tex, string path) {
        string _chars = new StreamReader(path).ReadToEnd();
        return genfont(_tex, _chars);
    }

    public static int predicttextwidth(font f, string text) {
        int w = 0;
        for (int i = 0; i < text.Length; i++) { 
            if (text[i] == ' ')
                w += f.data[f.chars.IndexOf(' ')].width;
            else {
                int ch = f.chars.IndexOf(text[i]);

                if (ch == -1)
                    w += f.data[f.chars.IndexOf(' ')].width;
                else
                    w += f.data[f.chars.IndexOf(text[i])].width;
            }
        }

        return w-1;
    }

    public static void rendertext(ICanvas c, font f, string text, Vector2 pos, Color col) => rendertext(c,f,text,pos.X,pos.Y,col.ToColorF());
    public static void rendertext(ICanvas c, font f, string text, Vector2 pos, ColorF col) => rendertext(c,f,text,pos.X,pos.Y,col);
    public static void rendertext(ICanvas c, font f, string text, float px, float py, Color col) => rendertext(c,f,text,px,py,col.ToColorF());

    public static void rendertext(ICanvas c, font f, string text, float px, float py, ColorF col) {
        if(f.fcase == caseness.lower)
            text = text.ToLower();
        else if(f.fcase == caseness.upper)
            text = text.ToUpper();

        int x = 0;
        for (int i = 0; i < text.Length; i++) {
            if (text[i] == ' ')
                x += f.data[f.chars.IndexOf(' ')].width;
            else {
                int ch = f.chars.IndexOf(text[i]);

                if (ch == -1) {
                    c.DrawTexture(
                        f.tex,
                        new Rectangle(0,0,f.charw,f.charh),
                        new Rectangle(px+x,py,f.charw,f.charh),
                        col
                    );
                    x += f.data[f.chars.IndexOf(' ')].width;
                } else { 
                    c.DrawTexture(
                        f.tex,
                        new Rectangle(ch*f.charw,0,f.charw,f.charh),
                        new Rectangle(px+x,py,f.charw,f.charh),
                        col
                    );
                    x += f.data[f.chars.IndexOf(text[i])].width;
                }
            }
        }

        c.Flush();
    }
}