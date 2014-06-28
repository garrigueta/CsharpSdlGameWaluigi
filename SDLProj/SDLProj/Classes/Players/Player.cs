using System;
using System.Collections.Generic;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;
using System.Drawing;

namespace SDLProj.Classes.Players
{
    class Player:AnimatedSprite
    {
        public Point position = new Point(100, 100);
        //AnimationDictionary adAnimations = new AnimationDictionary();
        Dictionary<String,int> animations;
        string name;
        //gets external data to generate animationd
        public void fillData(String _name, Dictionary<String, int> _animations )
        {
            this.name = _name;
            this.animations = _animations;
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
                    surfCollect.Add("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + ".bmp", new Size(38, 51));
                }
                else
                {
                    for (int i = 0; i <= anim.Value; i++)
                    {
                        String zero = "";
                        if (i <= 9) zero = "0";
                        Console.WriteLine("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + zero + i + ".bmp");
                        surfCollect.Add("../../Data/" + this.name + "/" + anim.Key + "/" + anim.Key + zero + i + ".bmp", new Size(38, 51));
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

    }
}
