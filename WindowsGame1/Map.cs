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
        }
        public void generateWorld(int caves)
        {
            Random rng = new Random(parent.seed);
            for (int i = 0; i < tilemap.GetUpperBound(1); i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                {
                    
                    int octaves = 5;
                    int octtotal = 0;

                    tilemap[j, i] = 0;
                    if (heightMap[i] > j) { continue; }

                    for (int k = 1; k < octaves+1; k++)
                    {
                        tilemap[j, i] += (int)(Math.Abs(SimplexNoise.noise(
                                                (float)(1.0 * i / tilemap.GetUpperBound(1) * 10*Math.Pow(2, k-1)),
                                                (float)(1.0 * j / tilemap.GetUpperBound(0) * 10*Math.Pow(2, k-1)),
                                                parent.seed)) * (256/k));
                        octtotal += 256 / k;
                    }

                    tilemap[j, i] =  (int)(1.0*tilemap[j,i]/octtotal*256);

                    tilemap[j, i] -= j/10;


                    if (tilemap[j, i] < caves) tilemap[j, i] = 0;
                    if (tilemap[j, i] >= caves) tilemap[j, i] = 256;

                }
            }
        }


        public void drawLine(SpriteBatch spriteBatch, Rectangle tracedSize, Vector2 pos)
        {
            for (int i = 0; i < heightMap.Count(); i++)
            {
                //SpriteBatchHelper.DrawRectangle(spriteBatch, new Rectangle(i, planet.map.heightMap[i], 1, 1), Color.White);
                SpriteBatchHelper.DrawRectangle(spriteBatch,
                    new Rectangle((int)(i+pos.X), (int)(heightMap[i]+pos.Y), 1, 1),
                    Color.White);
            }
        }
        public void drawWorld(SpriteBatch spriteBatch, Rectangle tracedSize, Vector2 pos, int zoom)
        {

            for (int i = 0; i < tilemap.GetUpperBound(1)/zoom; i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0)/zoom; j++)
                {
                    if ((i - pos.X) > tilemap.GetUpperBound(1) || (i - pos.X) < 0) continue;
                    if ((j - pos.Y) > tilemap.GetUpperBound(0) || (j - pos.Y) < 0) continue;
                    SpriteBatchHelper.DrawRectangle(spriteBatch,
                        new Rectangle(i*zoom, j*zoom, zoom, zoom),
                        new Color(tilemap[(int)(j-pos.Y), (int)(i-pos.X)], tilemap[(int)(j-pos.Y), (int)(i-pos.X)], tilemap[(int)(j-pos.Y), (int)(i-pos.X)]));
                }
            }
        }
    }
}
