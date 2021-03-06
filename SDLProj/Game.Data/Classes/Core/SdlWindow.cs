﻿using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using Game.Data.Classes.Core;
using Game.Data.Classes.Level;
using SdlDotNet.Core;
using System;
using Game.Data.Classes.Players;

public class SdlWindow : IDisposable
{
    Point position = new Point(100, 100);
    readonly int width = 800;
    readonly int height = 600;

    Surface screen;
    Surface cursor;

    Collisions coll = new Collisions();
    Level level;

    bool loaded;

    Player hero;

    public SdlWindow()
    {
        Video.WindowCaption = "Sdl Window";
        this.loaded = false;        
        this.hero = new Player();
    }
    /// <summary>
    /// Initial function called when game is started
    /// </summary>
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
        surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);
        Events.Run();
    }
    /// <summary>
    /// First time Configuration, called once
    /// </summary>
    private void ConfigElements()
    {
        this.hero.ConfigPlayer();
        this.level = new Level(this.hero);
        this.loaded = true;
    }
    /// <summary>
    /// Tick event called every frame
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Tick(object sender, TickEventArgs e)
    {
        if (this.loaded)
        {
            screen.Fill(Color.Black);
            this.level.CheckPlayerCollision();
            this.hero.UpdatePosition();
            this.level.Scroll();
            foreach (Block block in this.level.GetVisibleBlocks())
            {
                screen.Blit(block.Surface, block.position);
            }
            screen.Blit(this.hero, this.hero.position);
            screen.Update();
        }
    }
    /// <summary>
    /// Quit function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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