using System.Numerics;

using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

public class post_frag : CanvasShader {
    public int quant;

    public ITexture tex;

    public bool oklab_quant;

    public override ColorF GetPixelColor(Vector2 position) {
        ColorF fin = tex.Sample(position);

        if(fin.A == 0)
            Discard();

        if(quant == 0)
            return fin;

        if(oklab_quant) {
            Vector3 precol_srgb = new(fin.R,fin.G,fin.B);
            Vector3 precol_lrgb = SRGBToLinear(precol_srgb);
            Vector3 precol_lms = LinearRGBToLMS(precol_lrgb);
            Vector3 oklab = LMSToOKLab(precol_lms);

            oklab = Round(oklab *quant) /quant;

            Vector3 poscol_lms = OKLabToLMS(oklab);
            Vector3 poscol_lrgb = LMSToLinearRGB(poscol_lms);
            Vector3 poscol_srgb = LinearRGBToSRGB(poscol_lrgb);
            
            return new(poscol_srgb.X, poscol_srgb.Y, poscol_srgb.Z, 1);
        } else {
            Vector3 col = new(fin.R,fin.G,fin.B);

            col = Round(col *quant) /quant;

            return new(col.X,col.Y,col.Z,1);
        }
    }


    float Cbrt(float x) => Sign(x) * Pow(Abs(x), 0.3333333333333f);

    Vector3 SRGBToLinear(Vector3 srgb) {
        return new Vector3(
            srgb.X <= 0.04045f ? srgb.X / 12.92f : Pow((srgb.X + 0.055f) / 1.055f, 2.4f),
            srgb.Y <= 0.04045f ? srgb.Y / 12.92f : Pow((srgb.Y + 0.055f) / 1.055f, 2.4f),
            srgb.Z <= 0.04045f ? srgb.Z / 12.92f : Pow((srgb.Z + 0.055f) / 1.055f, 2.4f)
        );
    }

    Vector3 LinearRGBToLMS(Vector3 rgb) {
        return new Vector3(
            0.38971f * rgb.X + 0.68898f * rgb.Y + -0.07868f * rgb.Z,
            -0.22981f * rgb.X + 1.18340f * rgb.Y + 0.04641f * rgb.Z,
            0.00000f * rgb.X + 0.00000f * rgb.Y + 1.00000f * rgb.Z
        );
    }

    Vector3 LMSToOKLab(Vector3 lms) {
        Vector3 lmsRoot = new Vector3(
            Cbrt(lms.X),
            Cbrt(lms.Y),
            Cbrt(lms.Z)
        );

        return new Vector3(
            0.210454f * lmsRoot.X + 0.793617f * lmsRoot.Y - 0.004072f * lmsRoot.Z,
            1.977998f * lmsRoot.X - 2.428592f * lmsRoot.Y + 0.450593f * lmsRoot.Z,
            0.025904f * lmsRoot.X + 0.782771f * lmsRoot.Y - 0.808675f * lmsRoot.Z
        );
    }

    Vector3 OKLabToLMS(Vector3 oklab) {
        Vector3 lmsRoot = new Vector3(
            oklab.X + 0.3963377774f * oklab.Y + 0.2158037573f * oklab.Z,
            oklab.X - 0.1055613458f * oklab.Y - 0.0638541728f * oklab.Z,
            oklab.X - 0.0894841775f * oklab.Y - 1.2914855480f * oklab.Z
        );

        return new Vector3(
            lmsRoot.X * lmsRoot.X * lmsRoot.X,
            lmsRoot.Y * lmsRoot.Y * lmsRoot.Y,
            lmsRoot.Z * lmsRoot.Z * lmsRoot.Z
        );
    }

    Vector3 LMSToLinearRGB(Vector3 lms) {
        return new Vector3(
            1.91020f * lms.X + -1.11212f * lms.Y + 0.20191f * lms.Z,
            0.37095f * lms.X + 0.62905f * lms.Y + 0.00000f * lms.Z,
            0.00000f * lms.X + 0.00000f * lms.Y + 1.00000f * lms.Z
        );
    }

    Vector3 LinearRGBToSRGB(Vector3 linearRGB) {
        return new Vector3(
            linearRGB.X <= 0.0031308f ? 12.92f * linearRGB.X : 1.055f * Pow(linearRGB.X, 1 / 2.4f) - 0.055f,
            linearRGB.Y <= 0.0031308f ? 12.92f * linearRGB.Y : 1.055f * Pow(linearRGB.Y, 1 / 2.4f) - 0.055f,
            linearRGB.Z <= 0.0031308f ? 12.92f * linearRGB.Z : 1.055f * Pow(linearRGB.Z, 1 / 2.4f) - 0.055f
        );
    }
}