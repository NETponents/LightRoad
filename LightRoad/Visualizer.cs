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
        Timer simulationStepper;

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
            Vector2D pbSize = new Vector2D(pictureBox1.Width, pictureBox1.Height);
            if (pbSize != capturedPictureBoxSize)
            {
                capturedPictureBoxSize = pbSize;
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            }
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            // draw black background
            g.Clear(Color.Black);
            if (simWorld != null)
            {
                simWorld.drawWorld(ref g, drawOrigin);
            }
            g.Dispose();
            pictureBox1.Invalidate();
        }
        public void publishWorld(ref World w)
        {
            simWorld = w;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simulationStepper.Stop();
            this.Close();
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            simulationStepper.Start();
        }

        private void SimulationStepper_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("5");
            simWorld.step();
            pictureBox1_Paint(sender, null);
        }

        private void Visualizer_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("1");
            World world = new World();
            Console.WriteLine("2");
            world.addVehicle(new Vehicles.Vehicle());
            world.addRoad(new Road());
            Console.WriteLine("3");
            this.publishWorld(ref world);
            Console.WriteLine("4");
            simulationStepper = new Timer();
            simulationStepper.Interval = 1000;
            simulationStepper.Tick += SimulationStepper_Tick;
        }
    }
}
