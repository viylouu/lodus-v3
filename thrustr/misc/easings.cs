using SimulationFramework;

namespace thrustr.utils;

public class ease {
    public const float _c1 = 1.70158f;
    public const float _c2 = 2.5949095f;
    public const float _c3 = 2.70158f;
    public const float _c4 = 2.09439510239f;
    public const float _c5 = 1.3962634f;
    public const float _n1 = 7.5625f;
    public const float _d1 = 2.75f;

    public static float dyn(float x, float t, float k) => (t-x)/(k/(Time.DeltaTime*60));
    public static float dynang(float x, float t, float k) => ((math.abs(t+math.tau-x)<math.abs(t-x)?t+math.tau:t)-x)/(k/(Time.DeltaTime*60))%math.pi;

    public static float isine(float x) => 1-math.cos(math.clamp01(x)*math.pi/2f);
    public static float osine(float x) => math.sin(math.clamp01(x)*math.pi/2f);
    public static float iosine(float x) => -(math.cos(math.pi*math.clamp01(x))-1)/2f;

    public static float isqr(float x) => ittn(x,2);
    public static float osqr(float x) => ottn(x,2);
    public static float iosqr(float x) => iottn(x,2);

    public static float icbe(float x) => ittn(x,3);
    public static float ocbe(float x) => ottn(x,3);
    public static float iocbe(float x) => iottn(x,3);

    public static float iqrt(float x) => ittn(x,4);
    public static float oqrt(float x) => ottn(x,4);
    public static float ioqrt(float x) => iottn(x,4);

    public static float iqnt(float x) => ittn(x,5);
    public static float oqnt(float x) => ottn(x,5);
    public static float ioqnt(float x) => iottn(x,5);

    public static float ittn(float x, float n) => math.pow(math.clamp01(x),n);
    public static float ottn(float x, float n) => 1-math.pow(1-math.clamp01(x),n);
    public static float iottn(float x, float n) => math.clamp01(x)<.5f?(math.pow(2,n-1)*math.pow(math.clamp01(x),n)):(1-math.pow(-2*math.clamp01(x)+2,n)/2f);

    public static float iexpo(float x) => math.clamp01(x)==0?0:math.pow(2,10*math.clamp01(x)-10);
    public static float oexpo(float x) => math.clamp01(x)==1?1:1-math.pow(2,-10*math.clamp01(x));
    public static float ioexpo(float x) => math.clamp01(x)==0?0:math.clamp01(x)==1?1:math.clamp01(x)<.5f?math.pow(2,20*math.clamp01(x)-10)/2f:(2-math.pow(2,-20*math.clamp01(x)+10))/2f;

    public static float icirc(float x) {
        x = math.clamp01(x);
        return 1f-math.sqrt(1f-x*x);
    }
    public static float ocirc(float x) {
        x = math.clamp01(x);
        x -= 1f;
        return math.sqrt(1f-x*x);
    }
    public static float iocirc(float x) {
        x = math.clamp01(x);
        if (x < 0.5f) {
            x *= 2f;
            return (1f - math.sqrt(1f - x*x)) /2f;
        }
        else {
            x = 2f*x -2f;
            return (math.sqrt(1f - x*x) +1f) /2f;
        }
    }
    
    public static float iback(float x) => _c3*math.cbe(math.clamp01(x))-_c1*math.sqr(math.clamp01(x));
    public static float oback(float x) => 1+_c3*math.cbe(math.clamp01(x)-1)+_c1*math.sqr(math.clamp01(x)-1);
    public static float ioback(float x) => math.clamp01(x)<.5f?(math.sqr(2*math.clamp01(x))*((_c2+1)*2*math.clamp01(x)-_c2))/2f:(math.sqr(2*math.clamp01(x)-2)*((_c2+1)*(math.clamp01(x)*2-2)+_c2)+2);
    
    public static float ielast(float x) => math.clamp01(x)==0?0:math.clamp01(x)==1?1:-math.pow(2,10*math.clamp01(x)-10)*math.sin((math.clamp01(x)*10-10.75f)*_c4);
    public static float oelast(float x) => math.clamp01(x)==0?0:math.clamp01(x)==1?1:math.pow(2,-10*math.clamp01(x))*math.sin((math.clamp01(x)*10-.75f)*_c4)+1;
    public static float ioelast(float x) => math.clamp01(x)==0?0:math.clamp01(x)==1?1:math.clamp01(x)<.5f?-(math.pow(2,20*math.clamp01(x)-10)*math.sin((20*math.clamp01(x)-11.125f)*_c5))/2f:(math.pow(2,-20*math.clamp01(x)+10)*math.sin((20*math.clamp01(x)-11.125f)*_c5))/2f+1;

    public static float iboun(float x) => 1-oboun(1-math.clamp01(x));
    public static float oboun(float x) { 
        x = math.clamp01(x);

        if(x < 1/_d1)
            return _n1 * x*x;
        else if(x < 2/_d1) {
            x -= 1.5f/_d1;
            return _n1 * x*x + .75f;
        }
        else if(x < 2.5f/_d1) {
            x -= 2.25f/_d1;
            return _n1 * x*x + .9375f;
        }
        else {
            x -= 2.625f/_d1;
            return _n1 * x*x + .984375f;
        }
    }
    public static float ioboun(float x) => math.clamp01(x)<.5f?(1-oboun(1-2*math.clamp01(x)))/2f:(1+oboun(2*math.clamp01(x)-1))/2f;
}