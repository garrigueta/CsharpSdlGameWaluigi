using System;

using Game.Data;
using System.Drawing;

namespace SdlDotNetExamples.SmallDemos
{
    class Program
    {
        /// <summary>

        /// Application EntryPoint.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            SdlWindow window = new SdlWindow();
            window.Start();
        }
    }
}
