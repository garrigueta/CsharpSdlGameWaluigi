using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Graphics.Sprites;

namespace SDLProj.Classes.Players
{
    class fillData:AnimatedSprite
    {
        Dictionary<String,int> animations;
        string name;

        public fillData(String _name, Dictionary<String, int> _animations)
        {
            this.name = _name;
            this.animations = _animations;
        }

        public void init()
        {
            this.loadAnimations();
        }

        private void loadAnimations()
        {
            throw new NotImplementedException();
        }

    }
}
