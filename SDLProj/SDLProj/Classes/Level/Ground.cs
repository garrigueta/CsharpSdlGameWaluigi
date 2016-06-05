using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace SDLProj.Classes.Level
{
    class Ground
    {
        public SdlDotNet.Graphics.Surface m_Background;
        private int height = 100, width = 900;
        private ArrayList area = new ArrayList();
        public Point Position = new Point(0, 400);
        public Size Size = new Size(10,800);
        public Ground()
        {
            m_Background = new SdlDotNet.Graphics.Surface(ResourceData.grass);
           // m_Background.Height = height;
        }
        public Size getSize()
        {
            return new Size(width,height );
        }
        public void increaseX()
        {
            this.Position.X++;
            this.Position.X++;
        }
        public void decreaseX()
        {
            this.Position.X--;
            this.Position.X--;
        }
    }
}
