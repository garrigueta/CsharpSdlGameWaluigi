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
        public Size Size = new Size(10,300);
        public Ground()
        {
            m_Background = new SdlDotNet.Graphics.Surface("../../Data/level1/grass.jpg");
           // m_Background.Height = height;
        }
        public Size getSize()
        {
            return new Size(width,height );
        }
    }
}
