using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosGraphs.CosmosGraphs
{
    public class Graph : Bitmap
    {

        // sorry for bad code tho i am too tired to fix it rn

        public int MaxPoints { get; private set; } = 60;
        /// <summary>
        /// the distance between each point
        /// </summary>
        public int PointDistance = 6;
        public float HeightMultiplayer = 10;
        public float Zoom = 1;
        public int GridCellWidth = 40;
        public int GridCellheight = 40;
        public bool DrawGrid = true;
        public Color LineColor = Color.Blue;
        public Color FillColor = Color.DodgerBlue;
        public Color BackgroundColor = Color.White;
        public Color GridColor = Color.Silver;

        public List<int> Points { get; private set; } = new List<int>();

        public Graph(uint width, uint height, ColorDepth colorDepth) : base(width, height, colorDepth)
        {

            PointDistance = (int)(width / MaxPoints - 1);

        }
        
        public void SetMaxPointsCount(int MaxPoints)
        {
            this.MaxPoints = MaxPoints - 1;
            PointDistance = (int)(Width / this.MaxPoints);
        }

        public void AddPoint(int value)
        {

            if (Points.Count > MaxPoints)
            {
                Points.RemoveAt(0);
            }
            if (value * HeightMultiplayer > Height)
            {
                HeightMultiplayer = (int)Height / value;
            }
            Points.Add(value);

        }

        public void Update()
        {

            Clear();

            if (DrawGrid)
            {
                drawgrid(GridColor, 0, 0, (int)Width, (int)Height, GridCellWidth, GridCellheight);
            }

            for (int p = 1; p < Points.Count; p++)
            {

                int prevpoint = Points[Math.Max(p - 1, 0)];
                var x1 = PointDistance * Math.Max(p - 1, 0) * Zoom;
                var y1 = Height - (prevpoint * HeightMultiplayer * Zoom);
                var x2 = PointDistance * p * Zoom;
                var y2 = Height - (Points[p] * HeightMultiplayer * Zoom);
                DrawLine(LineColor, (int)x1, (int)y1, (int)x2, (int)y2);

            }

        }

        // Private Drawing functions
        void DrawPixel(Color color, int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                RawData[x + y * Width] = color.ToArgb();
        }

        void Clear()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    DrawPixel(BackgroundColor,x,y);
                }
            }
        }
        void drawcollumn(Color col, int x, int y,int count)
        {

            for (int Y = 0; Y < count; Y++)
            {
                try
                {
                    DrawPixel(col, x, Y+y);
                }
                catch
                {

                }
                
            }

        }

        void drawraw(Color col, int x, int y, int count)
        {

            for (int X = 0; X < count; X++)
            {
                try
                {
                    DrawPixel(col, X+x, y);
                }
                catch
                {

                }

            }

        }

        void drawgrid(Color col,int x,int y,int w,int h,int cols,int rows)
        {

            int gridstepc = w / (int)(cols * Zoom);
            int gridstepr = h / (int)(rows * Zoom);

            for (int X = 0; X < gridstepc; X++)
            {
                drawcollumn(col,(int)(cols * X * Zoom),y,h);
            }
            for (int Y = 0; Y < gridstepr; Y++)
            {
                drawraw(col,x,(int)(rows * Y * Zoom),w);
            }

        }

        // from cosmos CGS
        void DrawLine(Color color, int x1, int y1, int x2, int y2,bool drawbackground = true)
        {
            // Bresenham's line algorithm
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {

                if (drawbackground && y1 >1)
                {
                    drawcollumn(FillColor, x1,y1,(int)Height-y1);
                }
                
                DrawPixel(color, x1, y1);

                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

    }
}
