using System;
using System.Collections.Generic;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;
using System.Drawing;
using System.Collections;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace Game.Data.Classes.Players
{
    class Player:AnimatedSprite
    {
        private readonly int height = 51;
        private readonly int width = 38;
        public Point position = new Point(320, 100);
        Dictionary<String,int> animations;
        string name;
        private bool apply_gravity;
        bool _upArrowFired;
        bool _downArrowFired;
        bool _leftArrowFired;
        bool _rightArrowFired;
        bool _isJumping;
        int _jumpPosition;
        string _cursorStatus;
        public Joystick joystick;

        public bool Apply_gravity { get => apply_gravity; set => apply_gravity = value; }

        public void ConfigPlayer()
        {
            this._upArrowFired = false;
            this._downArrowFired = false;
            this._leftArrowFired = false;
            this._rightArrowFired = false;
            this._isJumping = false;
            this._cursorStatus = "stopped_left";
            Console.WriteLine("configAnimations");
            List<String> animation_file_names = new List<String>();
            animation_file_names.Add("running_left");
            animation_file_names.Add("running_right");


            if (Joysticks.IsInitialized)
            {
                joystick = Joysticks.OpenJoystick(0);
            }
            else
            {
                Console.WriteLine("Joy Not Initialized");
            }

            //add hero animation info 
            Dictionary<String, int> animations = new Dictionary<String, int>();
            animations.Add("running_left", 7);
            animations.Add("running_right", 7);
            animations.Add("stopped_right", 0);
            animations.Add("stopped_left", 0);
            Console.WriteLine("this.hero.fillData('hero',animations);");
            this.name = "hero";
            this.animations = animations;
            this.loadAnimations();
        }

        //Generates animations on player, getting images from Data folder
        private void loadAnimations()
        {
            string defaultState = "";
            foreach (KeyValuePair<String, int> anim in this.animations)
            {
                
                AnimationCollection animCollect = new AnimationCollection();
                SurfaceCollection surfCollect = new SurfaceCollection();
                if (anim.Value == 0)
                {
                    Console.WriteLine("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + ".bmp");
                    surfCollect.Add("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + ".bmp", new Size(width, height));
                }
                else
                {
                    for (int i = 0; i <= anim.Value; i++)
                    {
                        String zero = "";
                        if (i <= 9) zero = "0";
                        Console.WriteLine("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + zero + i + ".bmp");
                        surfCollect.Add("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + zero + i + ".bmp", new Size(width, height));
                    }
                }
                animCollect.Add(surfCollect);
                animCollect.Delay = 200;
                defaultState = anim.Key;
                base.Animations.Add(anim.Key, animCollect);
            }
            base.CurrentAnimation = defaultState;
            base.Animate = true;
        }

        public Size GetSize()
        {
            return new Size(height, width);
        }

        public void UpdatePosition()
        {
            bool modify = false;

            if (this._isJumping)
            {
                if (this._jumpPosition <= 16 )
                {
                    this._jumpPosition += 2;
                }
                else{
                    _isJumping = _downArrowFired;
                }

            }else{
                if (this._jumpPosition > 0)
                {
                    this._jumpPosition -= 1;
                }
            }


            if (this._leftArrowFired)
            {
                if (this._cursorStatus != "running_left")
                {
                    this._cursorStatus = "running_left";
                    modify = true;
                }
                this.position.X = (int)this.position.X - 2;
            }
            if (this._rightArrowFired)
            {
                if (this._cursorStatus != "running_right")
                {
                    this._cursorStatus = "running_right";
                    modify = true;
                }
                this.position.X = (int)this.position.X + 2;
               
            }

            this.position.Y -= _jumpPosition;

            if (this.Apply_gravity && !this._isJumping) { this.position.Y += 10; }

            if (modify) { this.setCursor(); }
        }

        public void KeyboardUp(object sender, KeyboardEventArgs e)
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
            if (modify)
            {
                this._cursorStatus = "stopped" + stop_direction;
                this.setCursor();
            }

        }

        public void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape ||
                e.Key == Key.Q)
            {
                Events.QuitApplication();
            }

            if (e.Key == Key.UpArrow)
            {
                
                if (this._jumpPosition <= 0 && !Apply_gravity)
                {
                    this._upArrowFired = true;
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

        public void JoystickAxisChanged(object sender, JoystickAxisEventArgs e)
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

        public void JoystickButtonDown(object sender, JoystickButtonEventArgs e)
        {
            Console.WriteLine("Joystick button was pressed");
        }

        private void setCursor()
        {
            if (this._leftArrowFired)
            {
                if (this._isJumping)
                {
                    this.CurrentAnimation = "running_left";// "jumping_left";
                }
                else
                {
                    this.CurrentAnimation = "running_left";
                }

            }
            else if (this._rightArrowFired)
            {
                if (this._isJumping)
                {
                    this.CurrentAnimation = "running_right";// "jumping_right";
                }
                else
                {
                    this.CurrentAnimation = "running_right";
                }
            }
            else
            {
                this.CurrentAnimation = this._cursorStatus;
            }
        }
    }
}
