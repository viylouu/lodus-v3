using SimulationFramework.Drawing;
using SimulationFramework;

using System.Numerics;

namespace thrustr.basic;

public class fontie {
    public static ITexture dfonttex = Graphics.LoadTexture("thrustr/assets/fonts/dfont.png");
    public static font dfont = genfont_wpath(dfonttex, "thrustr/assets/fonts/dfont.txt", 4, 10);

    public static font genfont(ITexture _tex, string _chars, int chartop = 0, int charbot = -1) {
        font font = new();

        int _charw = _tex.Width/_chars.Length, _charh = _tex.Height;

        font.tex = _tex;
        font.charw = _charw;
        font.charh = _charh;
        font.chars = _chars;
        font.chart = chartop;
        font.charb = charbot==-1?_charh:charbot;
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

    public static font genfont_wpath(ITexture _tex, string path, int chartop = 0, int charbot = -1) {
        string _chars = new StreamReader(path).ReadToEnd();
        return genfont(_tex, _chars, chartop,charbot);
    }

    public static int predicttextwidth(string text, font? f = null) {
        if(f == null)
            f = dfont;

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

    public static void rendertext(ICanvas c, string text, Vector2 pos, Alignment align, font f, Color? col) => rendertext(c,text,pos.X,pos.Y,align,f,col?.ToColorF() ?? null);
    public static void rendertext(ICanvas c, string text, Vector2 pos, Alignment align = Alignment.TopLeft, font f = null, ColorF? col = null) => rendertext(c,text,pos.X,pos.Y,align,f,col);
    public static void rendertext(ICanvas c, string text, float px, float py, Alignment align, font f, Color col) => rendertext(c,text,px,py,align,f,col.ToColorF());

    public static void rendertext(ICanvas c, string text, float px, float py, Alignment align = Alignment.TopLeft, font f = null, ColorF? col = null) {
        if(f == null)
           f = dfont; 

        if(f.fcase == caseness.lower)
            text = text.ToLower();
        else if(f.fcase == caseness.upper)
            text = text.ToUpper();

        ColorF _c = col ?? ColorF.White; 

        switch(align) {
            case Alignment.TopLeft:
                py -= f.chart;
                break;
            case Alignment.BottomLeft:
                py += f.charh-f.charb;
                break;
            case Alignment.TopRight:
                py -= f.chart;
                px -= predicttextwidth(text,f)-f.charw;
                break;
            case Alignment.BottomRight:
                py += f.charh-f.charb;
                px -= predicttextwidth(text,f)-f.charw;
                break;
            case Alignment.CenterLeft:
                py += (f.charh-f.charb + -f.chart) * .5f;
                break;
            case Alignment.CenterRight:
                py += (f.charh-f.charb + -f.chart) * .5f;
                px -= predicttextwidth(text,f)-f.charw;
                break;
            case Alignment.Center:
                py += (f.charh-f.charb + -f.chart) * .5f;
                px -= (predicttextwidth(text,f)-f.charw)/2f;
                break;
            case Alignment.TopCenter:
                py -= f.chart;
                px -= (predicttextwidth(text,f)-f.charw)/2f;
                break;
            case Alignment.BottomCenter:
                py += f.charh-f.charb;
                px -= (predicttextwidth(text,f)-f.charw)/2f;
                break;
        }

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
                        new Rectangle(px+x,py,f.charw,f.charh,align),
                        _c
                    );
                    x += f.data[f.chars.IndexOf(' ')].width;
                } else { 
                    c.DrawTexture(
                        f.tex,
                        new Rectangle(ch*f.charw,0,f.charw,f.charh),
                        new Rectangle(px+x,py,f.charw,f.charh,align),
                        _c
                    );
                    x += f.data[f.chars.IndexOf(text[i])].width;
                }
            }
        }

        c.Flush();
    }
}