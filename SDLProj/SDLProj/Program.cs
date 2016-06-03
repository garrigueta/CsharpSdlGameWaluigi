using System;

using SDLProj;

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
            JoystickExample joystickExample = new JoystickExample();
            joystickExample.Go();
        }
    }
}
