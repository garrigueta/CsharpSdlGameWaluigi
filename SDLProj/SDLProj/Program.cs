using System;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;
using System.Collections.Generic;

namespace SdlDotNetExamples.SmallDemos
{
    public class JoystickExample : IDisposable
    {
        Point position = new Point(100, 100);
        int width = 640;
        int height = 480;
        Joystick joystick;
        Surface screen;
        Surface cursor;

        bool _upArrowFired;
        bool _downArrowFired;
        bool _leftArrowFired;
        bool _rightArrowFired;

        bool _isJumping;

        string _cursorStatus;

        AnimatedSprite hero;
        /// <summary>

        /// 
        /// </summary>
        public JoystickExample()
        {
            this._upArrowFired=false;
            this._downArrowFired = false;
            this._leftArrowFired = false;
            this._rightArrowFired = false;
            this._isJumping = false;
            this._cursorStatus = "stopped";
            this.hero = new AnimatedSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            Events.Tick += new EventHandler<TickEventArgs>(Tick);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(this.KeyboardUp);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.JoystickAxisMotion += new EventHandler<JoystickAxisEventArgs>(this.JoystickAxisChanged);
            Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(this.JoystickButtonDown);

            this.configAnimations();


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
            Mouse.ShowCursor = false; // hide the cursor

            Surface surf = screen.CreateCompatibleSurface(width, height, true);
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), System.Drawing.Color.Black);

            Events.Run();
        }

        private void configAnimations()
        {
            List<String> animation_file_names = new List<String>();
            animation_file_names.Add("running_left");
            animation_file_names.Add("running_right");
            AnimationDictionary animations = new AnimationDictionary(); 
            // Load the frames into the animation

            foreach (string item in animation_file_names)
            {

                AnimationCollection walk = new AnimationCollection();
                SurfaceCollection dummy_walk = new SurfaceCollection();
                for (int i = 0; i <= 7; i++)
                {              
                    dummy_walk.Add("Data/hero/running/" + item + "0" + i + ".bmp", new Size(38, 51));                   
                }
                walk.Add(dummy_walk);
                // Change the delay between frames
                walk.Delay = 200;  // wait 1 second each frame.
                this.hero.Animations.Add(item, walk);
            }

            AnimationCollection stop_right = new AnimationCollection();
            SurfaceCollection dummy_stop_right = new SurfaceCollection();
            dummy_stop_right.Add("Data/hero/stopped_right.bmp", new Size(38, 51));
            stop_right.Add(dummy_stop_right);
            this.hero.Animations.Add("stopped_right", stop_right);

            AnimationCollection stop_left = new AnimationCollection();
            SurfaceCollection dummy_stop_left = new SurfaceCollection();
            dummy_stop_left.Add("Data/hero/stopped_left.bmp", new Size(38, 51));
            stop_left.Add(dummy_stop_left);
            this.hero.Animations.Add("stopped_left", stop_left);

            this.hero.CurrentAnimation = "stopped_right";
            this.hero.Animate = true;
        }

        private void Tick(object sender, TickEventArgs e)
        {
            screen.Fill(Color.Black);
            this.updatePosition();
            screen.Blit(this.hero,position);
            screen.Update();
        }
        private void setCursor()
        {
            if (this._leftArrowFired)
            {
                if (this._isJumping)
                {
                    this.hero.CurrentAnimation = "jumping_left";
                }
                else
                {
                    this.hero.CurrentAnimation = "running_left";
                }
                
            }
            else if (this._rightArrowFired)
            {
                if (this._isJumping)
                {
                    this.hero.CurrentAnimation = "jumping_right";
                }
                else
                {
                this.hero.CurrentAnimation = "running_right";
                }
            }
            else
            {
                this.hero.CurrentAnimation = this._cursorStatus;
            }
        }
        private void KeyboardUp(object sender, KeyboardEventArgs e)
        {
            bool modify = false;
            string stop_direction = "";
            if (e.Key == Key.UpArrow)
            {
                this._upArrowFired = false;
            }
            if (e.Key == Key.DownArrow)
            {
                this._downArrowFired = false;
            }
            if (e.Key == Key.LeftArrow)
            {
                stop_direction = "_left";
                this._leftArrowFired = false;
                modify = true;
            }
            if (e.Key == Key.RightArrow)
            {
                stop_direction = "_right";
                this._rightArrowFired = false;
                modify = true;
            }
            if(modify){
                this._cursorStatus = "stopped" + stop_direction;
                this.setCursor();
            }
            
        }
        private void updatePosition()
        {
            bool modify = false;

            if (this._upArrowFired)
            {
                position.Y = (int)position.Y - 2;

            }
            if (this._downArrowFired)
            {
                position.Y = (int)position.Y + 2;
            }
            if (this._leftArrowFired)
            {
                if (this._cursorStatus != "running_left")
                {
                    this._cursorStatus = "running_left";
                    modify = true;
                }
                position.X = (int)position.X - 2;
            }
            if (this._rightArrowFired)
            {
                if (this._cursorStatus != "running_right")
                {
                    this._cursorStatus = "running_right";
                    modify = true;
                }
                position.X = (int)position.X + 2;
            }

            if (modify) { this.setCursor(); }

        }
        
        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape ||
                e.Key == Key.Q)
            {
                Events.QuitApplication();
            }

            if (e.Key == Key.UpArrow)
            {
                this._upArrowFired = true;
            }
            if (e.Key == Key.DownArrow)
            {
                this._downArrowFired = true;
            }
            if (e.Key == Key.LeftArrow)
            {
                this._leftArrowFired = true;
            }
            if (e.Key == Key.RightArrow)
            {
                this._rightArrowFired = true;
            }
            
            
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
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

        /// <summary>

        /// Application EntryPoint.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            JoystickExample joystickExample = new JoystickExample();
            joystickExample.Go();
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
        ~JoystickExample()
        {
            Dispose(false);
        }

        #endregion

        
    }
}
