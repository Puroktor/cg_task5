using OpenTK.Mathematics;

namespace cg_task5;

struct Triangle
{
    public Triangle(Color4 color, Vector3 v1, Vector3 v2, Vector3 v3) => (Color, V1, V2, V3) = (color, v1, v2, v3);

    public Color4 Color { get; set; }
    public Vector3 V1 { get; set; }
    public Vector3 V2 { get; set; }
    public Vector3 V3 { get; set; }
}
