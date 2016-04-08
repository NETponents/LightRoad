using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LightRoad.Geometry;

namespace LightRoad
{
    public partial class Visualizer : Form
    {
        Vector2D startDragLocation;
        bool isMouseDown = false;
        Vector2D drawOrigin;
        World simWorld;
        Vector2D capturedPictureBoxSize;
        System.Timers.Timer simulationStepper;
        System.Timers.Timer simulationStopLightTimer;

        public Visualizer()
        {
            InitializeComponent();
            drawOrigin = new Vector2D(0, 0);
            capturedPictureBoxSize = new Vector2D(-1, -1);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            startDragLocation = new Vector2D(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                drawOrigin = drawOrigin + (new Vector2D(e.X, e.Y) - startDragLocation);
                startDragLocation.x = e.X;
                startDragLocation.y = e.Y;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g;
            try
            {
                //g = Graphics.FromImage(pictureBox1.Image);
                g = Graphics.FromImage(bmp);
            }
            catch (Exception ex)
            {
                return;
            }
            // draw black background
            g.Clear(Color.Black);
            if (simWorld != null)
            {
                simWorld.drawWorld(g, drawOrigin);
            }
            pictureBox1.Image = bmp;
            g.Dispose();
            pictureBox1.Invalidate();
        }
        public void publishWorld(ref World w)
        {
            simWorld = w;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simulationStepper = null;
            simulationStopLightTimer = null;
            this.Close();
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simulationStepper.Start();
            simulationStopLightTimer.Start();
        }

        private void SimulationStepper_Tick(object sender, EventArgs e)
        {
            simWorld.step();
            pictureBox1_Paint(sender, null);
        }

        private void SimulationStopLightTimer_Tick(object sender, EventArgs e)
        {
            foreach(Intersection i in simWorld.getIntersections())
            {
                i.pulseStopLights();
            }
            pictureBox1_Paint(sender, null);
        }

        private void Visualizer_Shown(object sender, EventArgs e)
        {
            World world;
            WorldLoader.LoadWorld(out world, "roads.txt", "intersections.txt", "vehicles.txt");
            this.publishWorld(ref world);
            simulationStepper = new System.Timers.Timer();
            simulationStepper.Interval = 17;
            simulationStepper.Elapsed += SimulationStepper_Tick;
            simulationStopLightTimer = new System.Timers.Timer();
            simulationStopLightTimer.Interval = 1000;
            simulationStopLightTimer.Elapsed += SimulationStopLightTimer_Tick;
        }
    }
}
