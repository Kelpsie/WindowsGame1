using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Planet
    {
        public Map map;
        public int size;
        public Double roughness;
        public int seed;
        public List<int> circlePointsX;
        public List<int> circlePointsY;

        public Planet(int size_, int seed_, Double roughness_)
        {
            seed = seed_;
            size = size_;
            roughness = roughness_;
            map = new Map(this);
            map.generateHeights();
            generateCirclePoints();
            //if map already exists;
            //map = new Map(); 
            //Use Map constructor overload to load map, rather than create
        }
        public void generateCirclePoints()
        {
            int cx;
            int cy;
            circlePointsX = new List<int>();
            circlePointsY = new List<int>();
            for (int i = 0; i < map.heightMap.Count(); i++)
            {

                cx = (int)(Math.Cos((1.0 * i / 1536) * Math.PI * 2) * (map.heightMap[i]+256) / 768 / 2 * size);
                cy = (int)(Math.Sin((1.0 * i / 1536) * Math.PI * 2) * (map.heightMap[i]+256) / 768 / 2 * size);
                circlePointsX.Add(cx);
                circlePointsY.Add(cy);

            }
        }
        public void drawCircle(SpriteBatch spriteBatch, Rectangle tracedSize)
        {



            for (int i = 0; i < map.heightMap.Count(); i++)
            {
                //SpriteBatchHelper.DrawRectangle(spriteBatch, new Rectangle(i, planet.map.heightMap[i], 1, 1), Color.White);
                SpriteBatchHelper.DrawRectangle(spriteBatch,
                    new Rectangle(circlePointsX[i] + (tracedSize.Width / 2),
                        circlePointsY[i] + (tracedSize.Height / 4)*3, 1, 1),
                    Color.White);
            }
        }
    }
}
