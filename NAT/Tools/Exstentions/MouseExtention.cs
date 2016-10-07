using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NAT.Tools.Exstentions {
    public static class MouseExtentios  {

        public static Point GetScreenPos(this MouseState state, GraphicsDeviceManager graphicsDevice) {
            float rw = Screen.PrimaryScreen.Bounds.Width;
            float rh = Screen.PrimaryScreen.Bounds.Height;

            float h = graphicsDevice.PreferredBackBufferHeight;
            float w = graphicsDevice.PreferredBackBufferWidth;

            return new Point((int)(state.X * (w/rw)),(int)(state.Y * (h/rh)));
        }

    }
}
