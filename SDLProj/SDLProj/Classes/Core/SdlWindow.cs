using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SDLProj.Classes.Core;
using SDLProj.Classes.Level;
using SdlDotNet.Core;
using System;

public class SdlWindow : IDisposable
{
    Point position = new Point(100, 100);
    readonly int width = 800;
    readonly int height = 600;

    Surface screen;
    Surface cursor;

    Collisions coll = new Collisions();
    Level grd = new Level();

    bool loaded;

    SDLProj.Classes.Players.Player hero;

    public SdlWindow()
    {
        Video.WindowCaption = "Sdl Window";
        this.loaded = false;        
        this.hero = new SDLProj.Classes.Players.Player();
    }

    public void Start()
    {
        Events.Tick += new EventHandler<TickEventArgs>(Tick);
        Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.hero.KeyboardDown);
        Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(this.hero.KeyboardUp);
        Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
        Events.JoystickAxisMotion += new EventHandler<JoystickAxisEventArgs>(this.hero.JoystickAxisChanged);
        Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(this.hero.JoystickButtonDown);

        this.ConfigElements();

        Video.WindowIcon();
        Video.WindowCaption = "SdlDotNet - Waluigi 2D Game";
        screen = Video.SetVideoMode(width, height, true);
        Mouse.ShowCursor = false; 
        Surface surf = screen.CreateCompatibleSurface(width, height, true);
        surf.Fill(new Rectangle(new Point(0, 0), surf.Size), System.Drawing.Color.Black);
        Events.Run();
    }

    private void ConfigElements()
    {
        this.hero.ConfigPlayer();
        this.loaded = true;
    }

    private void Tick(object sender, TickEventArgs e)
    {
        if (this.loaded)
        {
            screen.Fill(Color.Black);
            this.hero.Apply_gravity = !coll.Sprite_Collide(this.hero, this.grd);
            this.hero.UpdatePosition();
            screen.Blit(grd.m_Background, grd.Position);
            screen.Blit(this.hero, this.hero.position);
            screen.Update();
        }
    }

    private void Quit(object sender, QuitEventArgs e)
    {
        Events.QuitApplication();
    }


    #region IDisposable Members

    private bool disposed;

    /// <summary>

    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                if (this.screen != null)
                {
                    this.screen.Dispose();
                    this.screen = null;
                }
                if (this.cursor != null)
                {
                    this.cursor.Dispose();
                    this.cursor = null;
                }
                if (this.hero.joystick != null)
                {
                    this.hero.joystick.Dispose();
                    this.hero.joystick = null;
                }
            }
            this.disposed = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Close()
    {
        Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    ~SdlWindow()
    {
        Dispose(false);
    }

    #endregion
}