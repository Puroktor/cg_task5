using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace cg_task5;

class Window : GameWindow
{
    private List<Triangle> model;
    private double frameTime = 0;
    private int fps = 0;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        VSync = VSyncMode.On;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        using Stream stream = File.OpenRead("dart.obj");
        model = OBJ.Load(stream);

        GL.ClearColor(Color4.LightBlue);
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        frameTime += args.Time;
        fps++;
        if (frameTime >= 1)
        {
            string sep = ", FPS: ";
            Title = Title.Split(sep).First() + sep + fps;
            fps = 0;
            frameTime = 0;
        }

        CheckKeys();

        base.OnUpdateFrame(args);
    }

    private void CheckKeys()
    {
        if (KeyboardState.IsKeyDown(Keys.Up))
        {
            GL.Rotate(-1, 1, 0, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.Down))
        {
            GL.Rotate(1, 1, 0, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.Left))
        {
            GL.Rotate(-1, 0, 1, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.Right))
        {
            GL.Rotate(1, 0, 1, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            GL.Rotate(-1, 0, 0, 1);
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            GL.Rotate(1, 0, 0, 1);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.Begin(PrimitiveType.Triangles);
        foreach(var triangle in model)
        {
            GL.Color4(triangle.Color);
            GL.Vertex3(triangle.V1);
            GL.Vertex3(triangle.V2);
            GL.Vertex3(triangle.V3);
        }
        GL.End();
        SwapBuffers();
        base.OnRenderFrame(args);
    }
}
