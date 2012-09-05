using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public class State
    {
        RenderTarget2D renderToStateManager;
        public bool drawUnderneath = false;

        public State() { }

        public bool update() { return true; }

        public bool draw(SpriteBatch spriteBatch) {  return true; }

        public void cleanUp() { StateManager.pop(); }
    }
}
