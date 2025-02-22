using System.Numerics;
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

using thrustr.basic;
using thrustr.utils;

partial class main {
    static IDepthMask depth;

    static (vertex[] verts, uint[] inds) skybox;
    static skybox_frag sbfrag;
    static skybox_vert sbvert;

    static bool focus;

    static ColorF skycol = ColorF.SkyBlue;
    static ColorF caveskycol = new(4/255f,12/255f,24/255f);
    public static ColorF col;

    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        col = math.lerp(caveskycol,skycol,math.clamp01((camera.pos.Y+32)/16));

        sbfrag.col = col;

        rend_skybox(c);

        game.update();

        game.render_world(c, depth); 

        rend_ui(c);

        terminal.render(c);


        if(Keyboard.IsKeyPressed(Key.Escape))
            Application.Exit(false);

        if(Keyboard.IsKeyPressed(Key.Tab))
            focus = !focus;

        camera.canlook = focus && !terminal.enabled;
        camera.canmove = !terminal.enabled;

        if(!focus) {
            ImGui.Begin("settings");

            if(ImGui.CollapsingHeader("camera")) {
                ImGui.SliderInt("fov", ref global.fov, 30, 170);

                ImGui.SliderInt("sensitivity", ref global.sens, 1, 150);
            }

            if(ImGui.CollapsingHeader("rendering")) {
                if(ImGui.CollapsingHeader("performance")) {
                    ImGui.SliderInt("render distance", ref global.render_dist, 2, 64);
                }

                if(ImGui.CollapsingHeader("post processing")) {
                    ImGui.Checkbox("color quantization", ref global.color_quant);

                    if(global.color_quant) {
                        ImGui.Checkbox("oklab quantization", ref global.better_quant);

                        ImGui.SliderInt("quantization amount", ref global.color_quant_amt, 1, 48);
                    }
                }
            }

            ImGui.End();
        }
    }

    static void rend_ui(ICanvas c) {
        fontie.rendertext(c, $"{math.round(1/Time.DeltaTime)} fps", 3,3);
        fontie.rendertext(c, $"seed: {chunking.seed}", 3,4+fontie.dfont.charh-fontie.dfont.chart);
        fontie.rendertext(c, $"window size: ({global.fr_intercept.BaseWindowProvider.Size.X}, {global.fr_intercept.BaseWindowProvider.Size.Y})", 3,5+fontie.dfont.charh*2-fontie.dfont.chart*2);
        fontie.rendertext(c, $"pos: ({camera.pos.X}, {camera.pos.Y}, {camera.pos.Z})", 3,6+fontie.dfont.charh*3-fontie.dfont.chart*3);
        fontie.rendertext(c, $"{global.chks_loaded} chunks loaded ({global.chks_loaded-global.filled_chks_loaded} air, {global.filled_chks_loaded} populated)", 3,7+fontie.dfont.charh*4-fontie.dfont.chart*4);
        fontie.rendertext(c, $"{game.chunks_rendered} chunks rendered", 3,8+fontie.dfont.charh*5-fontie.dfont.chart*5);
        fontie.rendertext(c, $"{game.tris_rendered} tris", 3,9+fontie.dfont.charh*6-fontie.dfont.chart*6);
    }

    static void rend_skybox(ICanvas c) {
        c.Fill(sbfrag, sbvert);

        sbvert.vpmat = camera.vertprojmatrix;
        sbvert.wmat = Matrix4x4.CreateTranslation(camera.pos);

        c.DrawTriangles<vertex>(skybox.verts, skybox.inds);
    }
}