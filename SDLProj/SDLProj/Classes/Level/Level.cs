using System.Collections;
using System.Drawing;

namespace SDLProj.Classes.Level
{
    class Level
    {
        public SdlDotNet.Graphics.Surface m_Background;
        private readonly int height = 100;
        private readonly int width = 900;
        private readonly ArrayList area = new ArrayList();
        public Point Position = new Point(0, 400);
        public Size Size = new Size(10,800);
        public Level()
        {
            m_Background = new SdlDotNet.Graphics.Surface(ResourceData.grass);
           // m_Background.Height = height;
        }
        public Size GetSize()
        {
            return new Size(width,height );
        }
    }
}
