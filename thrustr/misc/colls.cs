using SimulationFramework;
using System.Numerics;

namespace thrustr.utils;

public class coll {
    /// <summary>checks if 2 points are colliding (why would you need this function)</summary>
    public static bool poipoi(Vector2 x, Vector2 y) => x == y;
    public static bool poipoi(float x, float y, float u, float v) => x == u && y == v;
    public static bool poipoi(Vector2 a, float x, float y) => a.X == x && a.Y == y;
    public static bool poipoi(float x, float y, Vector2 a) => a.X == x && a.Y == y;

    /// <summary>checks if a point is colliding with a circle</summary>
    public static bool poicirc(Vector2 cp, float cr, Vector2 pos) => math.sqrdist(cp, pos) <= cr * cr;
    public static bool poicirc(Vector2 cp, float cr, float x, float y) => math.sqrdist(cp, x, y) <= cr * cr;
    public static bool poicirc(float u, float v, float cr, float x, float y) => math.sqrdist(u, v, x, y) <= cr * cr;
    public static bool poicirc(float u, float v, float cr, Vector2 pos) => math.sqrdist(pos, u, v) <= cr * cr;

    /// <summary>checks if 2 circles are colliding</summary>
    public static bool circcirc(Vector2 p1, float r1, Vector2 p2, float r2) => math.sqrdist(p1, p2) <= r1 * r1 + r2 * r2;
    public static bool circcirc(float x1, float y1, float r1, Vector2 p2, float r2) => math.sqrdist(p2, x1, y1) <= r1 * r1 + r2 * r2;
    public static bool circcirc(Vector2 p1, float r1, float x2, float y2, float r2) => math.sqrdist(p1, x2, y2) <= r1 * r1 + r2 * r2;
    public static bool circcirc(float x1, float y1, float r1, float x2, float y2, float r2) => math.sqrdist(x1, y1, x2, y2) <= r1 * r1 + r2 * r2;

    /// <summary>checks if a point is colliding with a rectangle</summary>
    public static bool poirect(float px, float py, float x, float y, float w, float h, Alignment align = Alignment.TopLeft) {
        switch(align) {
            default:
                return px >= x && py >= y && px <= x + w && py <= y + h;
            case Alignment.BottomLeft:
                return px >= x && py >= y - h && px <= x + w && py <= y;
            case Alignment.TopRight:
                return px >= x - w && py >= y && px <= x && py <= y + h;
            case Alignment.BottomRight:
                return px >= x - w && py >= y - h && px <= x && py <= y;
            case Alignment.TopCenter:
                return px >= x - w / 2 && py >= y && px <= x + w / 2 && py <= y + h;
            case Alignment.BottomCenter:
                return px >= x - w / 2 && py >= y - h && px <= x + w / 2 && py <= y;
            case Alignment.CenterLeft:
                return px >= x && py >= y - h / 2 && px <= x + w && py <= y + w / 2;
            case Alignment.CenterRight:
                return px >= x - w && py >= y - h / 2 && px <= x && py <= y + w / 2;
            case Alignment.Center:
                return px >= x - w / 2 && py >= y - h / 2 && px <= x + w / 2 && py <= y + w / 2;
        }
    }
    public static bool poirect(Vector2 p, float x, float y, float w, float h, Alignment align = Alignment.TopLeft) => poirect(p.X, p.Y, x, y, w, h, align);
    public static bool poirect(float px, float py, Vector2 pos, float w, float h, Alignment align = Alignment.TopLeft) => poirect(px, py, pos.X, pos.Y, w, h, align);
    public static bool poirect(Vector2 p, Vector2 pos, float w, float h, Alignment align = Alignment.TopLeft) => poirect(p.X, p.Y, pos.X, pos.Y, w, h, align);
    public static bool poirect(float px, float py, float x, float y, Vector2 size, Alignment align = Alignment.TopLeft) => poirect(px, py, x, y, size.X, size.Y, align);
    public static bool poirect(Vector2 p, float x, float y, Vector2 size, Alignment align = Alignment.TopLeft) => poirect(p.X, p.Y, x, y, size.X, size.Y, align);
    public static bool poirect(float px, float py, Vector2 pos, Vector2 size, Alignment align = Alignment.TopLeft) => poirect(px, py, pos.X, pos.Y, size.X, size.Y, align);
    public static bool poirect(Vector2 p, Vector2 pos, Vector2 size, Alignment align = Alignment.TopLeft) => poirect(p.X, p.Y, pos.X, pos.Y, size.X, size.Y, align);
    public static bool poirect(float px, float py, Rectangle rect) => poirect(px, py, rect.X, rect.Y, rect.Width, rect.Height);
    public static bool poirect(Vector2 p, Rectangle rect) => poirect(p.X, p.Y, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>checks if 2 rectangles are colliding</summary>
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, float r2x, float r2y, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) {
        float r1xl = 0, //x left
              r2xl = 0,
              r1yt = 0, //y top
              r2yt = 0,
              r1xr = 0, //x right
              r2xr = 0,
              r1yb = 0, //y bot
              r2yb = 0;

        switch(r1a) {
            default:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomLeft:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y + r1h; r1yb = r1y; break;
            case Alignment.TopRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y + r1h; r1yb = r1y; break;
            case Alignment.TopCenter:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomCenter:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y - r1h; r1yb = r1y; break;
            case Alignment.CenterLeft:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
            case Alignment.CenterRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
            case Alignment.Center:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
        }

        switch(r2a) {
            default:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomLeft:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y + r2h; r2yb = r2y; break;
            case Alignment.TopRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y + r2h; r2yb = r2y; break;
            case Alignment.TopCenter:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomCenter:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y - r2h; r2yb = r2y; break;
            case Alignment.CenterLeft:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
            case Alignment.CenterRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
            case Alignment.Center:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
        }

        return r1xr >= r2xl && r1xl <= r2xr && r1yb >= r2yt && r1yt <= r2yb;
    }
    public static bool rectrect(Vector2 r1p, float r1w, float r1h, float r2x, float r2y, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1w, r1h, r2x, r2y, r2w, r2h, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, Vector2 r1s, float r2x, float r2y, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1s.X, r1s.Y, r2x, r2y, r2w, r2h, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, Vector2 r2p, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1w, r1h, r2p.X, r2p.Y, r2w, r2h, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, float r2x, float r2y, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1w, r1h, r2x, r2y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(Vector2 r1p, Vector2 r1s, float r2x, float r2y, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1s.X, r1s.Y, r2x, r2y, r2w, r2h, r1a, r2a);
    public static bool rectrect(Vector2 r1p, float r1w, float r1h, Vector2 r2p, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1w, r1h, r2p.X, r2p.Y, r2w, r2h, r1a, r2a);
    public static bool rectrect(Vector2 r1p, float r1w, float r1h, float r2x, float r2y, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1w, r1h, r2x, r2y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, Vector2 r1s, Vector2 r2p, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1s.X, r1s.Y, r2p.X, r2p.Y, r2w, r2h, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, Vector2 r1s, float r2x, float r2y, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1s.X, r1s.Y, r2x, r2y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, Vector2 r2p, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1w, r1h, r2p.X, r2p.Y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(Vector2 r1p, Vector2 r1s, Vector2 r2p, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1s.X, r1s.Y, r2p.X, r2p.Y, r2w, r2h, r1a, r2a);
    public static bool rectrect(Vector2 r1p, Vector2 r1s, float r2x, float r2y, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1s.X, r1s.Y, r2x, r2y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(Vector2 r1p, float r1w, float r1h, Vector2 r2p, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1w, r1h, r2p.X, r2p.Y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(float r1x, float r1y, Vector2 r1s, Vector2 r2p, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1x, r1y, r1s.X, r1s.Y, r2p.X, r2p.Y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(Vector2 r1p, Vector2 r1s, Vector2 r2p, Vector2 r2s, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1s.X, r1s.Y, r2p.X, r2p.Y, r2s.X, r2s.Y, r1a, r2a);
    public static bool rectrect(Rectangle r1, float r2x, float r2y, float r2w, float r2h, Alignment r2a = Alignment.TopLeft) => rectrect(r1.X, r1.Y, r1.Width, r1.Height, r2x, r2y, r2w, r2h, Alignment.TopLeft, r2a);
    public static bool rectrect(Rectangle r1, Vector2 r2p, float r2w, float r2h, Alignment r2a = Alignment.TopLeft) => rectrect(r1.X, r1.Y, r1.Width, r1.Height, r2p.X, r2p.Y, r2w, r2h, Alignment.TopLeft, r2a);
    public static bool rectrect(Rectangle r1, float r2x, float r2y, Vector2 r2s, Alignment r2a = Alignment.TopLeft) => rectrect(r1.X, r1.Y, r1.Width, r1.Height, r2x, r2y, r2s.X, r2s.Y, Alignment.TopLeft, r2a);
    public static bool rectrect(Rectangle r1, Vector2 r2p, Vector2 r2s, Alignment r2a = Alignment.TopLeft) => rectrect(r1.X, r1.Y, r1.Width, r1.Height, r2p.X, r2p.Y, r2s.X, r2s.Y, Alignment.TopLeft, r2a);
    public static bool rectrect(Vector2 r1p, float r1w, float r1h, Rectangle r2, Alignment r1a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1w, r1h, r2.X, r2.Y, r2.Width, r2.Height, r1a);
    public static bool rectrect(float r1x, float r1y, Vector2 r1s, Rectangle r2, Alignment r1a = Alignment.TopLeft) => rectrect(r1x, r1y, r1s.X, r1s.Y, r2.X, r2.Y, r2.Width, r2.Height, r1a);
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, Rectangle r2, Alignment r1a = Alignment.TopLeft) => rectrect(r1x, r1y, r1w, r1h, r2.X, r2.Y, r2.Width, r2.Height, r1a);
    public static bool rectrect(Vector2 r1p, Vector2 r1s, Rectangle r2, Alignment r1a = Alignment.TopLeft) => rectrect(r1p.X, r1p.Y, r1s.X, r1s.Y, r2.X, r2.Y, r2.Width, r2.Height, r1a);
    public static bool rectrect(Rectangle r1, Rectangle r2) => rectrect(r1.X, r1.Y, r1.Width, r1.Height, r2.X, r2.Y, r2.Width, r2.Height);

    /// <summary>checks if a circle and a rectangle are colliding</summary>
    public static bool circrect(float cx, float cy, float cr, float rx, float ry, float rw, float rh, Alignment ra = Alignment.TopLeft) {
        float rxl = 0, // x left
              ryt = 0, // y top
              rxr = 0, // x right
              ryb = 0; // y bottom

        switch(ra) {
            default:
                rxl = rx; rxr = rx + rw; ryt = ry; ryb = ry + rh; break;
            case Alignment.BottomLeft:
                rxl = rx; rxr = rx + rw; ryt = ry + rh; ryb = ry; break;
            case Alignment.TopRight:
                rxl = rx - rw; rxr = rx; ryt = ry; ryb = ry + rh; break;
            case Alignment.BottomRight:
                rxl = rx - rw; rxr = rx; ryt = ry + rh; ryb = ry; break;
            case Alignment.TopCenter:
                rxl = rx - rw / 2; rxr = rx + rw / 2; ryt = ry; ryb = ry + rh; break;
            case Alignment.BottomCenter:
                rxl = rx - rw / 2; rxr = rx + rw / 2; ryt = ry - rh; ryb = ry; break;
            case Alignment.CenterLeft:
                rxl = rx; rxr = rx + rw; ryt = ry - rh / 2; ryb = ry + rh / 2; break;
            case Alignment.CenterRight:
                rxl = rx - rw; rxr = rx; ryt = ry - rh / 2; ryb = ry + rh / 2; break;
            case Alignment.Center:
                rxl = rx - rw / 2; rxr = rx + rw / 2; ryt = ry - rh / 2; ryb = ry + rh / 2; break;
        }

        float closestX = math.max(rxl, math.min(cx, rxr));
        float closestY = math.max(ryt, math.min(cy, ryb));

        float distanceX = cx - closestX;
        float distanceY = cy - closestY;

        return (distanceX * distanceX + distanceY * distanceY) < (cr * cr);
    }
    public static bool circrect(Vector2 cp, float cr, float x, float y, float w, float h, Alignment a = Alignment.TopLeft) => circrect(cp.X, cp.Y, cr, x, y, w, h, a);
    public static bool circrect(float cx, float cy, float cr, Vector2 p, float w, float h, Alignment a = Alignment.TopLeft) => circrect(cx, cy, cr, p.X, p.Y, w, h, a);
    public static bool circrect(float cx, float cy, float cr, float x, float y, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(cx, cy, cr, x, y, s.X, s.Y, a);
    public static bool circrect(Vector2 cp, float cr, Vector2 p, float w, float h, Alignment a = Alignment.TopLeft) => circrect(cp.X, cp.Y, cr, p.X, p.Y, w, h, a);
    public static bool circrect(Vector2 cp, float cr, float x, float y, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(cp.X, cp.Y, cr, x, y, s.X, s.Y, a);
    public static bool circrect(float cx, float cy, float cr, Vector2 p, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(cx, cy, cr, p.X, p.Y, s.X, s.Y, a);
    public static bool circrect(Vector2 cp, float cr, Vector2 p, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(cp.X, cp.Y, cr, p.X, p.Y, s.X, s.Y, a);
    public static bool circrect(Circle c, float x, float y, float w, float h, Alignment a = Alignment.TopLeft) => circrect(c.Position.X, c.Position.Y, c.Radius, x, y, w, h, a);
    public static bool circrect(Circle c, Vector2 p, float w, float h, Alignment a = Alignment.TopLeft) => circrect(c.Position.X, c.Position.Y, c.Radius, p.X, p.Y, w, h, a);
    public static bool circrect(Circle c, float x, float y, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(c.Position.X, c.Position.Y, c.Radius, x, y, s.X, s.Y, a);
    public static bool circrect(Circle c, Vector2 p, Vector2 s, Alignment a = Alignment.TopLeft) => circrect(c.Position.X, c.Position.Y, c.Radius, p.X, p.Y, s.X, s.Y, a);
    public static bool circrect(Vector2 cp, float cr, Rectangle r) => circrect(cp.X, cp.Y, cr, r.X, r.Y, r.Width, r.Height);
    public static bool circrect(float cx, float cy, float cr, Rectangle r) => circrect(cx, cy, cr, r.X, r.Y, r.Width, r.Height);
    public static bool circrect(Circle c, Rectangle r) => circrect(c.Position.X, c.Position.Y, c.Radius, r.X, r.Y, r.Width, r.Height);

    /// <summary>checks if a point is on a line</summary>
    public static bool linepoi(float lx1, float ly1, float lx2, float ly2, float px, float py) => math.sqrdist(lx1,ly1,lx2,ly2) == math.sqrdist(px,py,lx1,ly1)+math.sqrdist(px,py,lx2,ly2);
    public static bool linepoi(Vector2 l1, float lx2, float ly2, float px, float py) => linepoi(l1.X,l1.Y,lx2,ly2,px,py);
    public static bool linepoi(float lx1, float ly1, Vector2 l2, float px, float py) => linepoi(lx1,ly1,l2.X,l2.Y,px,py);
    public static bool linepoi(float lx1, float ly1, float lx2, float ly2, Vector2 p) => linepoi(lx1,ly1,lx2,ly2,p.X,p.Y);
    public static bool linepoi(Vector2 l1, Vector2 l2, float px, float py) => linepoi(l1.X,l1.Y,l2.X,l2.Y,px,py);
    public static bool linepoi(float lx1, float ly1, Vector2 l2, Vector2 p) => linepoi(lx1,ly1,l2.X,l2.Y,p.X,p.Y);
    public static bool linepoi(Vector2 l1, float lx2, float ly2, Vector2 p) => linepoi(l1.X,l1.Y,lx2,ly2,p.X,p.Y);

    /// <summary>checks if a circle is touching a line</summary>
    public static bool linecirc(float lx1, float ly1, float lx2, float ly2, float x, float y, float r) { 
        if(poicirc(lx1,ly1,x,y,r)) return true;
        if(poicirc(lx2,ly2,x,y,r)) return true;

        float len = math.sqr(lx1-lx2)+math.sqr(ly1-ly2);
        float dot = (((x-lx1)*(lx2-lx1))+((y-ly1)*(ly2-ly1))) / len;

        float cx = lx1 + dot * (lx2-lx1);
        float cy = ly1 + dot * (ly2-ly1);

        if(!linepoi(lx1,ly1,lx2,ly2,cx,cy)) return false;

        if(math.sqrdist(cx,cy,x,y) <= r*r) return true;

        return false;
    }
    public static bool linecirc(Vector2 l1, float lx2, float ly2, float x, float y, float r) => linecirc(l1.X,l1.Y,lx2,ly2,x,y,r);
    public static bool linecirc(float lx1, float ly1, Vector2 l2, float x, float y, float r) => linecirc(lx1,ly1,l2.X,l2.Y,x,y,r);
    public static bool linecirc(float lx1, float ly1, float lx2, float ly2, Vector2 p, float r) => linecirc(lx1,ly1,lx2,ly2,p.X,p.Y,r);
    public static bool linecirc(Vector2 l1, Vector2 l2, float x, float y, float r) => linecirc(l1.X,l1.Y,l2.X,l2.Y,x,y,r);
    public static bool linecirc(Vector2 l1, float lx2, float ly2, Vector2 p, float r) => linecirc(l1.X,l1.Y,lx2,ly2,p.X,p.Y,r);
    public static bool linecirc(float lx1, float ly1, Vector2 l2, Vector2 p, float r) => linecirc(lx1,ly1,l2.X,l2.Y,p.X,p.Y,r);
    public static bool linecirc(Vector2 l1, Vector2 l2, Vector2 p, float r) => linecirc(l1.X,l1.Y,l2.X,l2.Y,p.X,p.Y,r);
    public static bool linecirc(Vector2 l1, float lx2, float ly2, Circle c) => linecirc(l1.X,l1.Y,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static bool linecirc(float lx1, float ly1, Vector2 l2, Circle c) => linecirc(lx1,ly1,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);
    public static bool linecirc(float lx1, float ly1, float lx2, float ly2, Circle c) => linecirc(lx1,ly1,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static bool linecirc(Vector2 l1, Vector2 l2, Circle c) => linecirc(l1.X,l1.Y,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);

    /// <summary>checks if a circle is touching a line and returns the collision point</summary>
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, float lx2, float ly2, float x, float y, float r) {
        float len = math.sqr(lx1-lx2)+math.sqr(ly1-ly2);
        float dot = (((x-lx1)*(lx2-lx1))+((y-ly1)*(ly2-ly1))) / len;

        float cx = lx1 + dot * (lx2-lx1);
        float cy = ly1 + dot * (ly2-ly1);

        if(!linepoi(lx1,ly1,lx2,ly2,cx,cy)) return (0,0,false);

        if(math.sqrdist(cx,cy,x,y) <= r*r) return (cx,cy,true);

        return (0,0,false);
    }
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, float lx2, float ly2, float x, float y, float r) => linecircxy(l1.X,l1.Y,lx2,ly2,x,y,r);
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, Vector2 l2, float x, float y, float r) => linecircxy(lx1,ly1,l2.X,l2.Y,x,y,r);
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, float lx2, float ly2, Vector2 p, float r) => linecircxy(lx1,ly1,lx2,ly2,p.X,p.Y,r);
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, Vector2 l2, float x, float y, float r) => linecircxy(l1.X,l1.Y,l2.X,l2.Y,x,y,r);
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, float lx2, float ly2, Vector2 p, float r) => linecircxy(l1.X,l1.Y,lx2,ly2,p.X,p.Y,r);
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, Vector2 l2, Vector2 p, float r) => linecircxy(lx1,ly1,l2.X,l2.Y,p.X,p.Y,r);
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, Vector2 l2, Vector2 p, float r) => linecircxy(l1.X,l1.Y,l2.X,l2.Y,p.X,p.Y,r);
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, float lx2, float ly2, Circle c) => linecircxy(l1.X,l1.Y,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, Vector2 l2, Circle c) => linecircxy(lx1,ly1,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);
    public static (float x, float y, bool ret) linecircxy(float lx1, float ly1, float lx2, float ly2, Circle c) => linecircxy(lx1,ly1,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static (float x, float y, bool ret) linecircxy(Vector2 l1, Vector2 l2, Circle c) => linecircxy(l1.X,l1.Y,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);

    /// <summary>checks if a circle is touching a line and returns the collision point</summary>
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, float lx2, float ly2, float x, float y, float r) {
        float len = math.sqr(lx1-lx2)+math.sqr(ly1-ly2);
        float dot = (((x-lx1)*(lx2-lx1))+((y-ly1)*(ly2-ly1))) / len;

        float cx = lx1 + dot * (lx2-lx1);
        float cy = ly1 + dot * (ly2-ly1);

        if(!linepoi(lx1,ly1,lx2,ly2,cx,cy)) return (Vector2.Zero,false);

        if(math.sqrdist(cx,cy,x,y) <= r*r) return (new Vector2(cx,cy),true);

        return (Vector2.Zero,false);
    }
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, float lx2, float ly2, float x, float y, float r) => linecircp(l1.X,l1.Y,lx2,ly2,x,y,r);
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, Vector2 l2, float x, float y, float r) => linecircp(lx1,ly1,l2.X,l2.Y,x,y,r);
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, float lx2, float ly2, Vector2 p, float r) => linecircp(lx1,ly1,lx2,ly2,p.X,p.Y,r);
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, Vector2 l2, float x, float y, float r) => linecircp(l1.X,l1.Y,l2.X,l2.Y,x,y,r);
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, float lx2, float ly2, Vector2 p, float r) => linecircp(l1.X,l1.Y,lx2,ly2,p.X,p.Y,r);
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, Vector2 l2, Vector2 p, float r) => linecircp(lx1,ly1,l2.X,l2.Y,p.X,p.Y,r);
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, Vector2 l2, Vector2 p, float r) => linecircp(l1.X,l1.Y,l2.X,l2.Y,p.X,p.Y,r);
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, float lx2, float ly2, Circle c) => linecircp(l1.X,l1.Y,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, Vector2 l2, Circle c) => linecircp(lx1,ly1,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);
    public static (Vector2 p, bool ret) linecircp(float lx1, float ly1, float lx2, float ly2, Circle c) => linecircp(lx1,ly1,lx2,ly2,c.Position.X,c.Position.Y,c.Radius);
    public static (Vector2 p, bool ret) linecircp(Vector2 l1, Vector2 l2, Circle c) => linecircp(l1.X,l1.Y,l2.X,l2.Y,c.Position.X,c.Position.Y,c.Radius);

    /// <summary>checks if a line is intersecting with another line</summary>
    public static bool lineline(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
        float a = ((x4-x3)*(y1-y3) - (y4-y3)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));
        float b = ((x2-x1)*(y1-y3) - (y2-y1)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));

        if(a >= 0 && a <= 1 && b >= 0 && b <= 1)
            return true;

        return false;
    }
    public static bool lineline(Vector2 p1, float x2, float y2, float x3, float y3, float x4, float y4) => lineline(p1.X,p1.Y,x2,y2,x3,y3,x4,y4);
    public static bool lineline(float x1, float y1, Vector2 p2, float x3, float y3, float x4, float y4) => lineline(x1,y1,p2.X,p2.Y,x3,y3,x4,y4);
    public static bool lineline(float x1, float y1, float x2, float y2, Vector2 p3, float x4, float y4) => lineline(x1,y1,x2,y2,p3.X,p3.Y,x4,y4);
    public static bool lineline(float x1, float y1, float x2, float y2, float x3, float y3, Vector2 p4) => lineline(x1,y1,x2,y2,x3,y3,p4.X,p4.Y);
    public static bool lineline(Vector2 p1, Vector2 p2, float x3, float y3, float x4, float y4) => lineline(p1.X,p1.Y,p2.X,p2.Y,x3,y3,x4,y4);
    public static bool lineline(Vector2 p1, float x2, float y2, Vector2 p3, float x4, float y4) => lineline(p1.X,p1.Y,x2,y2,p3.X,p3.Y,x4,y4);
    public static bool lineline(Vector2 p1, float x2, float y2, float x3, float y3, Vector2 p4) => lineline(p1.X,p1.Y,x2,y2,x3,y3,p4.X,p4.Y);
    public static bool lineline(float x1, float y1, Vector2 p2, Vector2 p3, float x4, float y4) => lineline(x1,y1,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static bool lineline(float x1, float y1, Vector2 p2, float x3, float y3, Vector2 p4) => lineline(x1,y1,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static bool lineline(float x1, float y1, float x2, float y2, Vector2 p3, Vector2 p4) => lineline(x1,y1,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static bool lineline(Vector2 p1, Vector2 p2, Vector2 p3, float x4, float y4) => lineline(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static bool lineline(Vector2 p1, Vector2 p2, float x3, float y3, Vector2 p4) => lineline(p1.X,p1.Y,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static bool lineline(Vector2 p1, float x2, float y2, Vector2 p3, Vector2 p4) => lineline(p1.X,p1.Y,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static bool lineline(float x1, float y1, Vector2 p2, Vector2 p3, Vector2 p4) => lineline(x1,y1,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);
    public static bool lineline(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) => lineline(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);

    /// <summary>checks if a line is intersecting with another line and returns the intersection pos</summary>
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
        float a = ((x4-x3)*(y1-y3) - (y4-y3)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));
        float b = ((x2-x1)*(y1-y3) - (y2-y1)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));

        if(a >= 0 && a <= 1 && b >= 0 && b <= 1)
            return (x1 + (a * (x2-x1)),y1 + (a * (y2-y1)),true);

        return (0,0,false);
    }
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, float x2, float y2, float x3, float y3, float x4, float y4) => linelinexy(p1.X,p1.Y,x2,y2,x3,y3,x4,y4);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, Vector2 p2, float x3, float y3, float x4, float y4) => linelinexy(x1,y1,p2.X,p2.Y,x3,y3,x4,y4);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, float x2, float y2, Vector2 p3, float x4, float y4) => linelinexy(x1,y1,x2,y2,p3.X,p3.Y,x4,y4);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, float x2, float y2, float x3, float y3, Vector2 p4) => linelinexy(x1,y1,x2,y2,x3,y3,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, Vector2 p2, float x3, float y3, float x4, float y4) => linelinexy(p1.X,p1.Y,p2.X,p2.Y,x3,y3,x4,y4);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, float x2, float y2, Vector2 p3, float x4, float y4) => linelinexy(p1.X,p1.Y,x2,y2,p3.X,p3.Y,x4,y4);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, float x2, float y2, float x3, float y3, Vector2 p4) => linelinexy(p1.X,p1.Y,x2,y2,x3,y3,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, Vector2 p2, Vector2 p3, float x4, float y4) => linelinexy(x1,y1,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, Vector2 p2, float x3, float y3, Vector2 p4) => linelinexy(x1,y1,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, float x2, float y2, Vector2 p3, Vector2 p4) => linelinexy(x1,y1,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, Vector2 p2, Vector2 p3, float x4, float y4) => linelinexy(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, Vector2 p2, float x3, float y3, Vector2 p4) => linelinexy(p1.X,p1.Y,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, float x2, float y2, Vector2 p3, Vector2 p4) => linelinexy(p1.X,p1.Y,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(float x1, float y1, Vector2 p2, Vector2 p3, Vector2 p4) => linelinexy(x1,y1,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);
    public static (float x, float y, bool ret) linelinexy(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) => linelinexy(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);

    /// <summary>checks if a line is intersecting with another line and returns the intersection pos</summary>
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
        float a = ((x4-x3)*(y1-y3) - (y4-y3)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));
        float b = ((x2-x1)*(y1-y3) - (y2-y1)*(x1-x3)) / ((y4-y3)*(x2-x1) - (x4-x3)*(y2-y1));

        if(a >= 0 && a <= 1 && b >= 0 && b <= 1)
            return (new Vector2(x1 + (a * (x2-x1)),y1 + (a * (y2-y1))),true);

        return (Vector2.Zero,false);
    }
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, float x2, float y2, float x3, float y3, float x4, float y4) => linelinep(p1.X,p1.Y,x2,y2,x3,y3,x4,y4);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, Vector2 p2, float x3, float y3, float x4, float y4) => linelinep(x1,y1,p2.X,p2.Y,x3,y3,x4,y4);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, float x2, float y2, Vector2 p3, float x4, float y4) => linelinep(x1,y1,x2,y2,p3.X,p3.Y,x4,y4);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, float x2, float y2, float x3, float y3, Vector2 p4) => linelinep(x1,y1,x2,y2,x3,y3,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, Vector2 p2, float x3, float y3, float x4, float y4) => linelinep(p1.X,p1.Y,p2.X,p2.Y,x3,y3,x4,y4);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, float x2, float y2, Vector2 p3, float x4, float y4) => linelinep(p1.X,p1.Y,x2,y2,p3.X,p3.Y,x4,y4);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, float x2, float y2, float x3, float y3, Vector2 p4) => linelinep(p1.X,p1.Y,x2,y2,x3,y3,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, Vector2 p2, Vector2 p3, float x4, float y4) => linelinep(x1,y1,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, Vector2 p2, float x3, float y3, Vector2 p4) => linelinep(x1,y1,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, float x2, float y2, Vector2 p3, Vector2 p4) => linelinep(x1,y1,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, Vector2 p2, Vector2 p3, float x4, float y4) => linelinep(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,x4,y4);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, Vector2 p2, float x3, float y3, Vector2 p4) => linelinep(p1.X,p1.Y,p2.X,p2.Y,x3,y3,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, float x2, float y2, Vector2 p3, Vector2 p4) => linelinep(p1.X,p1.Y,x2,y2,p3.X,p3.Y,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(float x1, float y1, Vector2 p2, Vector2 p3, Vector2 p4) => linelinep(x1,y1,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);
    public static (Vector2 p, bool ret) linelinep(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) => linelinep(p1.X,p1.Y,p2.X,p2.Y,p3.X,p3.Y,p4.X,p4.Y);
}