using OpenTK.Mathematics;
using System.Globalization;

namespace cg_task5;

static class OBJ
{
    public static List<Triangle> Load(Stream stream)
    {
        List<Triangle> triangles = new();
        List<Vector3> vList = new();
        Dictionary<String, Color4> dict = new();
        Color4 cuurentColor = Color4.White;
        float[] max = { -1f, -1f, -1f };
        float[] min = { float.MaxValue, float.MaxValue, float.MaxValue };
        bool normilized = false;

        using StreamReader reader = new(stream);
        string inp;
        while ((inp = reader.ReadLine()) != null)
        {
            string[] str = inp.Trim().Split(' ', 2);
            switch (str[0])
            {
                case "v":
                    string[] cords = str[1].Split();
                    Vector3 v = new();
                    for (int i = 0; i < 3; i++)
                    {
                        v[i] = float.Parse(cords[i], CultureInfo.InvariantCulture);
                        max[i] = Math.Max(max[i], v[i]);
                        min[i] = Math.Min(min[i], v[i]);
                    }
                    vList.Add(v);
                    break;
                case "mtllib":
                    using (Stream mtlStream = File.OpenRead(str[1]))
                    {
                        dict = LoadMTL(mtlStream);
                    }
                    break;
                case "usemtl":
                    cuurentColor = dict[str[1]];
                    break;
                case "f":
                    if (!normilized)
                    {
                        float maxV = Math.Max(Math.Abs(max.Max()), Math.Abs(min.Min()));
                        float[] m = new float[3];
                        for (int i = 0; i < 3; i++)
                        {
                            m[i] = (min[i] + max[i]) / 2f;
                        }
                        for (int i = 0; i < vList.Count; i++)
                        {
                            var vert = vList[i];
                            for (int j = 0; j < 3; j++)
                            {
                                vert[j] -= m[j];
                            }
                            vert /= maxV;
                            vList[i] = vert;
                        }
                        normilized = true;
                    }
                    int[] ind = str[1].Split().Select(x => int.Parse(x) - 1).ToArray();
                    triangles.Add(new Triangle(cuurentColor, vList[ind[0]],
                             vList[ind[1]], vList[ind[2]]));
                    break;
            }
        }
        return triangles;
    }

    private static Dictionary<String, Color4> LoadMTL(Stream stream)
    {
        Dictionary<String, Color4> dict = new();
        using StreamReader reader = new(stream);
        string inp;
        while ((inp = reader.ReadLine()) != null)
        {
            string[] str = inp.Trim().Split(' ', 2);
            if (str[0] == "newmtl")
            {
                float[] color = reader.ReadLine().Trim()[4..]
                    .Split()
                    .Select(x => float.Parse(x, CultureInfo.InvariantCulture))
                    .ToArray();
                dict.Add(str[1], new Color4(color[0], color[1], color[2], 1f));
            }
        }
        return dict;
    }
}

