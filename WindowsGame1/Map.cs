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
            tilemap = new int[Convert.ToInt32(parent.size/3.5), parent.size];
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
                    w = (int)(40 * SimplexNoise.noise((float)(1.0 * i /40*parent.roughness), parent.seed, 0));
                if (i >= parent.size / 2)
                    w = (int)(40 * SimplexNoise.noise((float)(1.0 * (parent.size-i) / 40*parent.roughness), parent.seed, 0));

                heightMap.Add(w+y + 100);
                if (i % (parent.size/2) == 0) d *= -1;
            }
        }
        public void generateWorld(int caves)
        {
            Random rng = new Random(parent.seed);
            for (int i = 0; i < tilemap.GetUpperBound(1); i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                {
                    if (heightMap[i] > j) { continue; }
                    
                    int octaves = 4;
                    int octtotal = 0;

                    tilemap[j, i] = 0;

                    for (int k = 1; k < octaves+1; k++)
                    {
                        tilemap[j, i] += (int)(Math.Abs(SimplexNoise.noise(
                                                (float)(1.0 * i / tilemap.GetUpperBound(1) * 10*Math.Pow(2, k-1)),
                                                (float)(1.0 * j / tilemap.GetUpperBound(0) * 10*Math.Pow(2, k-1)),
                                                parent.seed)) * (256/k));
                        octtotal += 256 / k;
                    }

                    tilemap[j, i] =  (int)(1.0*tilemap[j,i]/octtotal*256);
                    
                    //tilemap[j, i] -= (int)Math.Pow(j/(1.0*tilemap.GetUpperBound(0)/2), 2);
                    
                    //if (heightMap[i] < j-5)
                    //    tilemap[j, i] = rng.Next(1, 256);
                    //else tilemap[j, i] = 256;

                    //if (tilemap[j, i] < caves) tilemap[j, i] = 0;
                    //if (tilemap[j, i] >= caves) tilemap[j, i] = 256;
                    
                }
            }

            for (int k = 0; k < 3; k++)
            {
                int[,] temp = (int[,])tilemap.Clone();
                for (int i = 0; i < tilemap.GetUpperBound(1); i++)
                {
                    for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                    {
                        //automata(temp, i, j, caves);
                    }
                }
                tilemap = temp;

                for (int i = 0; i < tilemap.GetUpperBound(1); i++)
                    for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                        tilemap[j, i] = tilemap[j, i] < caves ? 0 : 256;
            }
        }

        public void automata(int[,] arr, int x, int y, int caves)
        {

            int alive = 0;
            for (int i = -1; i < 2; i++)
            {
                if (x + i < 0 || x + i > arr.GetUpperBound(1)) continue;
                for (int j = -1; j < 2; j++)
                {
                    if (y + j < 0 || y + j > arr.GetUpperBound(0)) continue;
                    if (i == 0 && j == 0) continue;
                    if (tilemap[y + j, x + i] >= caves) alive += 1;

                }
            }

            if (tilemap[y, x] >= caves && tilemap[y, x] < 250)
                switch (alive)
                {
                    case 4:
                    case 5:
                        break;
                    default: arr[y, x] = 0; break;
                }
            if (tilemap[y, x] < caves)
                switch (alive)
                {
                    case 2:
                    case 3:
                        arr[y, x] = 256;
                        break;
                    default: break;
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

            for (int i = 0; i < tilemap.GetUpperBound(1); i++)
            {
                for (int j = 0; j < tilemap.GetUpperBound(0); j++)
                {
                    if (i > tilemap.GetUpperBound(1) || i < 0) continue;
                    if (j > tilemap.GetUpperBound(0) || j < 0) continue;

                    SpriteBatchHelper.DrawRectangle(spriteBatch,
                        new Rectangle(i * zoom, j * zoom, zoom, zoom),
                        new Color(tilemap[(int)(j - pos.Y), (int)(i - pos.X)], tilemap[(int)(j - pos.Y), (int)(i - pos.X)], tilemap[(int)(j - pos.Y), (int)(i - pos.X)]));
                }
            }
        } 
    }
}
