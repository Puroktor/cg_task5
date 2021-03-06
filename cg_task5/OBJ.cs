using OpenTK.Mathematics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace cg_task5;

static class OBJ
{
    public static List<Triangle> Load(Stream stream)
    {
        List<Triangle> triangles = new();
        List<Vector3> v = new();
        List<Vector3> vt = new();
        List<Vector3> vn = new();
        float[] min = { float.MaxValue, float.MaxValue, float.MaxValue };
        float[] max = { float.MinValue, float.MinValue, float.MinValue };

        using StreamReader reader = new(stream);
        string inp;
        while ((inp = reader.ReadLine()) != null)
        {
            string[] str = Regex.Replace(inp.Trim(), @"\s+", " ").Split(' ', 2);
            switch (str[0])
            {
                case "v":
                    Vector3 vect = ReadVector(str[1]);
                    for (int i = 0; i < 3; i++)
                    {
                        max[i] = Math.Max(max[i], vect[i]);
                        min[i] = Math.Min(min[i], vect[i]);
                    }
                    v.Add(vect);
                    break;
                case "vt":
                    vt.Add(ReadVector(str[1]));
                    break;
                case "vn":
                    vn.Add(ReadVector(str[1]));
                    break;
                case "f":
                    string[] point = str[1].Split();
                    Triangle triangle = new();
                    for (int i = 0; i < 3; i++)
                    {
                        int[] ind = point[i].Split('/').Select(x => int.Parse(x) - 1).ToArray();
                        var p = triangle[i];
                        p.V = v[ind[0]];
                        p.VT = vt[ind[1]];
                        p.VN = vn[ind[2]];
                        triangle[i] = p;
                    }
                    triangles.Add(triangle);
                    break;
            }
        }
        Normalize(triangles, min, max);
        return triangles;
    }

    private static void Normalize(List<Triangle> triangles, float[] min, float[] max)
    {
        float maxV = Math.Max(Math.Abs(max.Max()), Math.Abs(min.Min()));
        float[] m = new float[3];
        for (int i = 0; i < 3; i++)
        {
            m[i] = (min[i] + max[i]) / 2f;
        }
        for (int i = 0; i < triangles.Count; i++)
        {
            var triangle = triangles[i];
            for (int j = 0; j < 3; j++)
            {
                var p = triangle[j];
                var cords = p.V;
                for (int k = 0; k < 3; k++)
                {
                    cords[k] -= m[k];
                }
                p.V = cords / maxV;
                triangle[j] = p;
            }
            triangles[i] = triangle;
        }
    }

    private static Vector3 ReadVector(string str)
    {
        string[] cords = str.Split();
        Vector3 v = new();
        for (int i = 0; i < 3; i++)
        {
            v[i] = float.Parse(cords[i], CultureInfo.InvariantCulture);
        }
        return v;
    }
}

