using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SdlDotNet.Graphics.Sprites;
using SDLProj.Classes.Level;
using SDLProj.Classes.Players;

namespace SDLProj.Classes.Core
{
    class Collisions
    {
        private ArrayList elmements;
        public void addElement(ArrayList elm)
        {
            elmements.Add(elm);
        }
        public bool checkCollisions()
        {
            for (int i = 0; i < elmements.Count;i++)
            {
                int[] elm1 = (int[])elmements[i];
                int[] elm2 = (int[])elmements[i+1];
                if (elm1[0] < ((int)elm2[0] + (int)elm2[2]) &&
                           elm1[0] + elm1[2] > elm2[0] &&
                           elm1[1] < elm2[1] + elm2[3] &&
                           elm1[3] + elm1[1] > elm2[1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            return false;     
        }

       public bool Sprite_Collide(Player object1, Ground object2)
        {

	        int left1, left2;
	        int right1, right2;
	        int top1, top2;
	        int bottom1, bottom2;

	        left1 = object1.position.X;
            left2 = object2.Position.X;
            right1 = object1.position.X + object1.GetSize().Width;
            right2 = object2.Position.X + object2.GetSize().Width;
            top1 = object1.position.Y;
            top2 = object2.Position.Y;
            bottom1 = object1.position.Y + object1.GetSize().Height;
            bottom2 = object2.Position.Y + object2.GetSize().Height;

            if (bottom1 < top2)
            {
                Console.WriteLine("bottom1 < top2");
                return false;
            }
            if (top1 > bottom2)
            {
                Console.WriteLine("top1 > bottom2");
                return false;
            }

            if (right1 < left2)
            {
                Console.WriteLine("right1 < left2");
                return false;
            }
            if (left1 > right2)
            {
                Console.WriteLine("left1 > right2");
                return false;
            }

            Console.WriteLine("no collision");
            return true;

        }
    }
}
