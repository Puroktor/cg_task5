using OpenTK.Mathematics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace cg_task5;

static class OBJ
{
    public static (List<Triangle>, float) Load(Stream stream)
    {
        List<Triangle> triangles = new();
        List<Vector3> v = new();
        List<Vector3> vt = new();
        List<Vector3> vn = new();
        float max = -1;

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
                        max = Math.Max(max, Math.Abs(vect[i]));
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
        return (triangles, 1/max);
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

