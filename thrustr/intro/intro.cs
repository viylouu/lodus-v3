using System.Numerics;

using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.utils;

namespace thrustr.basic;

public class intro {
    static ITexture enginetex;
    static ITexture sftex;

    static ITexture smallenginetex;
    static ITexture smallsftex;

    static ISound fadesfx;

    public static bool introplayed = true;

    static float introstart = 0;

    public static void initintro() {
        enginetex.trydispose();
        sftex.trydispose();
        smallenginetex.trydispose();
        smallsftex.trydispose();
        fadesfx.trydispose();

        enginetex = Graphics.LoadTexture("thrustr/assets/sprites/icons/engine logo.png");
        sftex = Graphics.LoadTexture("thrustr/assets/sprites/icons/sf logo.png");
        smallenginetex = Graphics.LoadTexture("thrustr/assets/sprites/icons/engine small.png");
        smallsftex = Graphics.LoadTexture("thrustr/assets/sprites/icons/sf logo small.png");

        fadesfx = Audio.LoadSound("thrustr/assets/audio/introfade.wav");

        introplayed = false;
        introstart = -1;
    }
    
    public static void dostuff(ICanvas c, font? f) {
        if((introstart -10f)*1.25f >= 1)
            return;

        if(f == null)
            f = fontie.dfont;

        introstart += Time.DeltaTime;

        // bg

        float start = ease.ocirc(introstart*1.25f) *-100 +100;
        float end = ease.icirc((introstart -10f)*1.25f) *100;

        c.Fill(Color.Black);
        c.DrawPolygon(
            new Vector2[] {
                new(0-start-end,Window.Height-32),
                new(0-start-end,Window.Height+24),
                new(120-start-end,Window.Height+24),
                new(72-start-end,Window.Height-32)
            }
        );

        c.Stroke(Color.White);
        c.DrawPolygon(
            new Vector2[] {
                new(0-start-end,Window.Height-32),
                new(0-start-end,Window.Height+24),
                new(120-start-end,Window.Height+24),
                new(72-start-end,Window.Height-32)
            }
        );

        // thrustr engine

        float ease1 = ease.oelast(introstart*.25f) *-72 +72;
        float ease2 = ease.oelast((introstart-.25f)*.25f) *-52 +52;
        float ease3 = ease.oelast((introstart-.5f)*.25f) *-52 +52;
        float ease4 = ease.oelast((introstart-.75f)*.25f) *-52 +52;

        float down1 = ease.iback((introstart -4f)*2f) *32;
        float down2 = ease.iback((introstart-.25f -4f)*2f) *32;
        float down3 = ease.iback((introstart-.5f -4f)*2f) *32;
        float down4 = ease.iback((introstart-.75f -4f)*2f) *32;

        c.DrawTexture(smallenginetex, new Vector2(48-ease1,Window.Height+down1), Alignment.BottomLeft);

        fontie.rendertext(c, "thrustr", new(6-ease2, Window.Height-28+down2));
        fontie.rendertext(c, "engine", new(12-ease3, Window.Height-22+down3));
        fontie.rendertext(c, "v0.1.4.1", new(4-ease4, Window.Height-2+down4), Alignment.BottomLeft);

        // simulationframework logo

        float ease5 = ease.oelast((introstart -5f)*.25f) *-72 +72;
        float ease6 = ease.oelast((introstart-.25f -5f)*.25f) *-52 +52;
        float ease7 = ease.oelast((introstart-.5f -5f)*.25f) *-52 +52;
        float ease8 = ease.oelast((introstart-.75f -5f)*.25f) *-52 +52;

        float down5 = ease.iback((introstart -9f)*2f) *32;
        float down6 = ease.iback((introstart-.25f -9f)*2f) *32;
        float down7 = ease.iback((introstart-.5f -9f)*2f) *32;
        float down8 = ease.iback((introstart-.75f -9f)*2f) *32;

        c.DrawTexture(smallsftex, new Vector2(48-ease5,Window.Height+down5), Alignment.BottomLeft);

        fontie.rendertext(c, "simulation", new(6-ease6, Window.Height-28+down6));
        fontie.rendertext(c, "framework", new(8-ease7, Window.Height-22+down7));
        fontie.rendertext(c, "v0.3.0 a12", new(4-ease8, Window.Height-2+down8), Alignment.BottomLeft);
    }
}