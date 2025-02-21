using System.Numerics;

public class frustum {
    public static bool IsAABBInFrustum(Vector3 min, Vector3 max, Plane[] frustumPlanes) {
        foreach (var plane in frustumPlanes)
            // check if all 8 corners of the bounding box are outside a plane
            if (PlaneOutside(plane, min, max))
                return false; // outside frustum
        return true; // at least one corner is inside
    }

    // checks if all corners of an AABB are outside a given plane
    public static bool PlaneOutside(Plane plane, Vector3 min, Vector3 max) {
        Vector3[] corners = {
            new Vector3(min.X, min.Y, min.Z),
            new Vector3(max.X, min.Y, min.Z),
            new Vector3(min.X, max.Y, min.Z),
            new Vector3(max.X, max.Y, min.Z),
            new Vector3(min.X, min.Y, max.Z),
            new Vector3(max.X, min.Y, max.Z),
            new Vector3(min.X, max.Y, max.Z),
            new Vector3(max.X, max.Y, max.Z)
        };

        foreach (var corner in corners)
            if (Vector3.Dot(plane.Normal, corner) + plane.D >= 0)
                return false; // at least one point is inside

        return true; // all points are outside
    }

    public static Plane[] GetFrustumPlanes(Matrix4x4 viewProjectionMatrix) {
        Plane[] planes = new Plane[6];

        // left
        planes[0] = new Plane(
            viewProjectionMatrix.M14 + viewProjectionMatrix.M11,
            viewProjectionMatrix.M24 + viewProjectionMatrix.M21,
            viewProjectionMatrix.M34 + viewProjectionMatrix.M31,
            viewProjectionMatrix.M44 + viewProjectionMatrix.M41
        );

        // right
        planes[1] = new Plane(
            viewProjectionMatrix.M14 - viewProjectionMatrix.M11,
            viewProjectionMatrix.M24 - viewProjectionMatrix.M21,
            viewProjectionMatrix.M34 - viewProjectionMatrix.M31,
            viewProjectionMatrix.M44 - viewProjectionMatrix.M41
        );

        // bottom
        planes[2] = new Plane(
            viewProjectionMatrix.M14 + viewProjectionMatrix.M12,
            viewProjectionMatrix.M24 + viewProjectionMatrix.M22,
            viewProjectionMatrix.M34 + viewProjectionMatrix.M32,
            viewProjectionMatrix.M44 + viewProjectionMatrix.M42
        );

        // top
        planes[3] = new Plane(
            viewProjectionMatrix.M14 - viewProjectionMatrix.M12,
            viewProjectionMatrix.M24 - viewProjectionMatrix.M22,
            viewProjectionMatrix.M34 - viewProjectionMatrix.M32,
            viewProjectionMatrix.M44 - viewProjectionMatrix.M42
        );

        // near
        planes[4] = new Plane(
            viewProjectionMatrix.M13,
            viewProjectionMatrix.M23,
            viewProjectionMatrix.M33,
            viewProjectionMatrix.M43
        );

        // far
        planes[5] = new Plane(
            viewProjectionMatrix.M14 - viewProjectionMatrix.M13,
            viewProjectionMatrix.M24 - viewProjectionMatrix.M23,
            viewProjectionMatrix.M34 - viewProjectionMatrix.M33,
            viewProjectionMatrix.M44 - viewProjectionMatrix.M43
        );

        // normalize planes
        for (int i = 0; i < 6; i++)
        {
            planes[i] = Plane.Normalize(planes[i]);
        }

        return planes;
    }
}