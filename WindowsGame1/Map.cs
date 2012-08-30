using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Map
    {
        Planet parent;
        public int[,] tilemap;
        public List<int> heightMap;

        public Map(Planet parent_)
        {
            parent = parent_;
            heightMap = new List<int>();
            tilemap = new int[Convert.ToInt32(parent.size/3.14), parent.size];
        }
        public void generateHeights()
        {
            int d = 1;
            int y = 0;
            int w = 0;
            heightMap = new List<int>();
            for (int i = 0; i < parent.size; i++)
            {
                //y += d * (int)(roughness * SimplexNoise.noise(i, parent.seed, 0)); roughness will later be determined by planet type, or something
                y += d * (int)(parent.roughness * SimplexNoise.noise(i, parent.seed, 0));
                if (i < parent.size / 2)
                    w = (int)(40 * SimplexNoise.noise((float)(1.0 * i /80), parent.seed, 0));
                if (i >= parent.size / 2)
                    w = (int)(40 * SimplexNoise.noise((float)(1.0 * (parent.size-i) / 80), parent.seed, 0));

                heightMap.Add(w+y + 100);
                if (i % 768 == 0) d *= -1;
            }
            generateWorld(100);
        }
        public void generateWorld(int caves)
        {
            for (int i = 0; i < tilemap.GetUpperBound(1); i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                {
                    if (heightMap[i] > j) { tilemap[j, i] = 0; continue; }
                    tilemap[j, i] = (int)(Math.Abs(SimplexNoise.noise(
                        (float)(1.0 * i / tilemap.GetUpperBound(1)*30), 
                        (float)(1.0 * j / tilemap.GetUpperBound(0)*30),
                        parent.seed)) * 256);

                    //tilemap[j, i] = tilemap[j, i] + (int)(Math.Pow((0.001 * (tilemap.GetUpperBound(0)-j)), 2));
                    //tilemap[j, i] = tilemap[j, i] - (int)(Math.Pow((0.05 * j), 2));

                    if (j > tilemap.GetUpperBound(0)-200) tilemap[j, i] /= 4;
                    if (j == heightMap[i]) tilemap[j, i] = 0;
                    if (tilemap[j, i] < 50) tilemap[j, i] = 0;
                    if (tilemap[j, i] >= 50) tilemap[j, i] = 256;
                }
            }

        }
        public void drawLine(SpriteBatch spriteBatch, Rectangle tracedSize)
        {
            for (int i = 0; i < heightMap.Count(); i++)
            {
                //SpriteBatchHelper.DrawRectangle(spriteBatch, new Rectangle(i, planet.map.heightMap[i], 1, 1), Color.White);
                SpriteBatchHelper.DrawRectangle(spriteBatch,
                    new Rectangle(i, heightMap[i], 1, 1),
                    Color.White);
            }
        }
        public void drawWorld(SpriteBatch spriteBatch, Rectangle tracedSize)
        {


            for (int i = 0; i < tilemap.GetUpperBound(1); i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                {
                    SpriteBatchHelper.DrawRectangle(spriteBatch,
                        new Rectangle(i, j, 1, 1),
                        new Color(tilemap[j, i], tilemap[j, i], tilemap[j, i]));
                }
            }
        }
    }
}
