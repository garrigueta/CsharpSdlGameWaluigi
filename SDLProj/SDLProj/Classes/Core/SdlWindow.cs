using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SDLProj.Classes.Core;
using SDLProj.Classes.Level;
using SdlDotNet.Core;
using System;
using System.Collections.Generic;

public class SdlWindow : IDisposable
{
    Point position = new Point(100, 100);
    int width = 800;
    int height = 600;

    private static Surface m_Background;
    private static Point m_BackgroundPosition;

    Joystick joystick;
    Surface screen;
    Surface cursor;

    Collisions coll = new Collisions();
    Ground grd = new Ground();

    bool loaded;

    

    SDLProj.Classes.Players.Player hero;

    /* ctor */
    public SdlWindow()
    {
        Video.WindowCaption = "Sdl Window";
        this.loaded = false;        
        this.hero = new SDLProj.Classes.Players.Player();
    }

    public void Go()
    {
        Events.Tick += new EventHandler<TickEventArgs>(Tick);
        Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.hero.KeyboardDown);
        Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(this.hero.KeyboardUp);
        Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
        Events.JoystickAxisMotion += new EventHandler<JoystickAxisEventArgs>(this.JoystickAxisChanged);
        Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(this.JoystickButtonDown);

        this.configElements();

        if (Joysticks.IsInitialized)
        {
            joystick = Joysticks.OpenJoystick(0);
        }
        else
        {
            Console.WriteLine("Joy Not Initialized");
        }

        Video.WindowIcon();
        Video.WindowCaption = "SdlDotNet - Joystick Example";
        screen = Video.SetVideoMode(width, height, true);
        Mouse.ShowCursor = false; 
        m_Background = (new Surface(@"../../Data/background/background.png")).Convert(screen, true, false);
        m_BackgroundPosition = new Point(0, 0);
        Surface surf = screen.CreateCompatibleSurface(width, height, true);
        surf.Fill(new Rectangle(new Point(0, 0), surf.Size), System.Drawing.Color.Black);
        Events.Run();
    }

    private void configElements()
    {

        this.hero.configPlayer();
        this.loaded = true;

    }

    private void Tick(object sender, TickEventArgs e)
    {
        if (this.loaded)
        {
            //Console.WriteLine("Tick(object sender, TickEventArgs e)");
            screen.Fill(Color.Black);
            this.hero.apply_gravity = !coll.Sprite_Collide(this.hero, this.grd);
            this.hero.updatePosition();
            screen.Blit(grd.m_Background, grd.Position);
            //screen.Blit(m_Background, m_BackgroundPosition);
            screen.Blit(this.hero, this.hero.position);
            screen.Update();

        }

    }
    

    private void JoystickAxisChanged(object sender, JoystickAxisEventArgs e)
    {
        if (e.AxisIndex == 0)
        {
            position.X = (int)(joystick.GetAxisPosition(JoystickAxis.Horizontal) * width);
        }
        else if (e.AxisIndex == 1)
        {
            position.Y = (int)(joystick.GetAxisPosition(JoystickAxis.Vertical) * height);
        }
    }

    private void JoystickButtonDown(object sender, JoystickButtonEventArgs e)
    {
        Console.WriteLine("Joystick button was pressed");
    }

    private void Quit(object sender, QuitEventArgs e)
    {
        Events.QuitApplication();
    }

    /// <summary>
    /// Lesson Title
    /// </summary>
    public static string Title
    {
        get
        {
            return "JoystickExample: Move the cursor with a joystick";
        }
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
                if (this.joystick != null)
                {
                    this.joystick.Dispose();
                    this.joystick = null;
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