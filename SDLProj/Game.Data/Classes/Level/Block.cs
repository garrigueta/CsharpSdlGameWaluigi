using SdlDotNet.Graphics;
using System.Drawing;

namespace Game.Data.Classes.Level
{
    class Block
    {
        private readonly int height = 32;
        private readonly int width = 32;

        public Surface Background { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }

    }
}
