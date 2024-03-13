using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CosmosGraphs.CosmosGraphs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sys = Cosmos.System;

namespace CosmosGraphs
{
    public class Kernel : Sys.Kernel
    {

        public static int FPS = 0;

        public static int LastS = -1;
        public static int Ticken = 0;
        Canvas canvas;
        Graph graph;
        protected override void BeforeRun()
        {

            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(800,600,ColorDepth.ColorDepth32));
            canvas.Clear(Color.Green);

            graph = new Graph(400,400,ColorDepth.ColorDepth32);
            graph.SetMaxPointsCount(20);

            graph.Update();

            canvas.DrawImage(graph,10,10);

            canvas.Display();

        }

        protected override void Run()
        {

            if (LastS == -1)
            {
                LastS = System.DateTime.UtcNow.Second;
            }
            if (System.DateTime.UtcNow.Second - LastS != 0)
            {
                if (System.DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken / (System.DateTime.UtcNow.Second - LastS);
                }
                LastS = System.DateTime.UtcNow.Second;
                Ticken = 0;

                graph.AddPoint(FPS);
                graph.Update();
                canvas.DrawImage(graph,10,10);

            }
            Ticken++;

            canvas.DrawString(FPS.ToString(),PCScreenFont.Default,Color.Black,0,0);

            canvas.Display();
            Heap.Collect();

        }
    }
}
