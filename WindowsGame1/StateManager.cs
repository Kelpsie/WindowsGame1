using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame
{

    public static class StateManager
    {
        static List<State> stack;
        static RenderTarget2D renderToScreen;

        public static void push(State toPush) { stack.Add(toPush); }

        public static void pop() { stack.RemoveAt(stack.Count - 1); }

        public static void pushAndPop(State toPush) { pop(); push(toPush); }

        public static void update()
        {
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                if (!stack[i].update()) break;
            }
        }

        public static void draw(SpriteBatch spriteBatch, Rectangle tracedSize)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack[i].drawUnderneath || i == stack.Count - 1)
                    stack[i].draw(spriteBatch);
            }
        }
    }
}
