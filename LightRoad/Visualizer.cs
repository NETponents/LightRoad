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

        public Visualizer()
        {
            InitializeComponent();
            drawOrigin = new Vector2D(0, 0);
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
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width,
                        pictureBox1.Height);
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
            this.Close();
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            World world = new World();
            world.addVehicle(new Vehicles.Vehicle());
            this.publishWorld(ref world);
        }
    }
}
