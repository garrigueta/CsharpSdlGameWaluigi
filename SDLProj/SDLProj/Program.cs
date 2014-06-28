using System;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Core;
using System.Collections.Generic;

namespace SdlDotNetExamples.SmallDemos
{
    public class JoystickExample : IDisposable
    {
        Point position = new Point(100, 100);
        int width = 640;
        int height = 480;

        private static Surface m_Background;
        private static Point m_BackgroundPosition;

        Joystick joystick;
        Surface screen;
        Surface cursor;

        bool loaded;

        bool _upArrowFired;
        bool _downArrowFired;
        bool _leftArrowFired;
        bool _rightArrowFired;

        bool _isJumping;
        int _jumpPosition;

        string _cursorStatus;

        SDLProj.Classes.Players.Player hero;
        /// <summary>

        /// 
        /// </summary>
        public JoystickExample()
        {
            this._jumpPosition = 0;
            this.loaded = false;
            this._upArrowFired=false;
            this._downArrowFired = false;
            this._leftArrowFired = false;
            this._rightArrowFired = false;
            this._isJumping = false;
            this._cursorStatus = "stopped_left";
            this.hero = new SDLProj.Classes.Players.Player();
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
            //background config
            m_Background = (new Surface(@"../../Data/background/background.png")).Convert(screen, true, false);
            m_BackgroundPosition = new Point(0, 0);
            Surface surf = screen.CreateCompatibleSurface(width, height, true);
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), System.Drawing.Color.Black);
            Events.Run();
        }

        private void configAnimations()
        {
            Console.WriteLine("configAnimations");
            List<String> animation_file_names = new List<String>();
            animation_file_names.Add("running_left");
            animation_file_names.Add("running_right");
            
            //add hero animation info 
            Dictionary<String, int> animations = new Dictionary<String, int>();
            animations.Add("running_left", 7);
            animations.Add("running_right", 7);
            animations.Add("stopped_right", 0);
            animations.Add("stopped_left", 0);
            Console.WriteLine("this.hero.fillData('hero',animations);");
            this.hero.fillData("hero",animations);
            this.loaded = true;
            
        }

        private void Tick(object sender, TickEventArgs e)
        {
            if(this.loaded){
                Console.WriteLine("Tick(object sender, TickEventArgs e)");
                screen.Fill(Color.Black);
                this.updatePosition();
                screen.Blit(m_Background, m_BackgroundPosition);
                screen.Blit(this.hero, this.hero.position);
                screen.Update();
            }
            
        }
        private void setCursor()
        {
            if (this._leftArrowFired)
            {
                if (this._isJumping)
                {
                    this.hero.CurrentAnimation = "running_left";// "jumping_left";
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
                    this.hero.CurrentAnimation = "running_right";// "jumping_right";
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

            if (this._isJumping)
            {
                if (this._jumpPosition <= 15 && this._jumpPosition >= 0)
                {
                    this._jumpPosition++;
                }else
                {
                    this._isJumping = false;
                    this._jumpPosition = -1;
                }

            }else{
                if (this._jumpPosition >= -17 && this._jumpPosition <= -1)
                {
                    this._jumpPosition--;
                }
                if (this._jumpPosition == -17)
                {
                    this._jumpPosition = 0;
                }
            }
            if (this._downArrowFired)
            {
                this.hero.position.Y = (int)this.hero.position.Y + 2;
            }
            if (this._leftArrowFired)
            {
                if (this._cursorStatus != "running_left")
                {
                    this._cursorStatus = "running_left";
                    modify = true;
                }
                this.hero.position.X = (int)this.hero.position.X - 2;
            }
            if (this._rightArrowFired)
            {
                if (this._cursorStatus != "running_right")
                {
                    this._cursorStatus = "running_right";
                    modify = true;
                }
                this.hero.position.X = (int)this.hero.position.X + 2;
            }
            Console.WriteLine("Jump Position: " + this._jumpPosition);

            this.hero.position.Y = (int)this.hero.position.Y - this._jumpPosition;
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
                if(this._jumpPosition==0){
                    this._isJumping = true;
                }
                
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
