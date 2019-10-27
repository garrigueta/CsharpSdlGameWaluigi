using Game.Data.Classes.Core;
using Game.Data.Classes.Players;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Game.Data.Classes.Level
{
    class Level
    {
        public SdlDotNet.Graphics.Surface m_Background;
        private readonly int height = 100;
        private readonly int width = 900;
        private readonly ArrayList area = new ArrayList();
        public Point Position = new Point(0, 400);
        public Size Size = new Size(10,800);
        Player hero;
        Collection<Block> BlockList = new Collection<Block>();
        Collection<Block> VisibleBlockList = new Collection<Block>();

        public Level(Player hero)
        {
            this.hero = hero;        
        }

        public Size GetSize()
        {
            return new Size(width,height);
        }

        public void CheckPlayerCollision()
        {
            this.hero.ApplyGravity = false;
            this.hero.AllowMoveLeft = false;
            this.hero.AllowMoveRight = false;
        }

        public Collection<Block> GetVisibleBlocks()
        {
            Collection<Block> visibleBlocks = new Collection<Block>();

            return visibleBlocks;
        }

        public void Scroll()
        {
            this.Position.X += this.hero.Scroll;
        }
    }
}
