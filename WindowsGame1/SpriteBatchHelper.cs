using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    static class SpriteBatchHelper
    {

        static Texture2D pixel;

        private static void LoadPixel(GraphicsDevice graphicsDevice)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(graphicsDevice, 1, 1);
                pixel.SetData<Color>(new Color[] { Color.White });
            }
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            LoadPixel(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(pixel, rectangle, color);
        }
        public static void DrawPixel(this SpriteBatch spriteBatch, int x, int y, Color color)
        {
            DrawRectangle(spriteBatch, new Rectangle(x, y, 1, 1), color);
        } 
    }
}
