using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using System.Drawing;

namespace Game.Data.Classes.Level
{
    class Block:Sprite
    {
        private readonly int height = 32;
        private readonly int width = 32;
        public Point position = new Point(320, 100);

        public Surface Background { get; set; }

        public Block()
        {

        }
    }
}
