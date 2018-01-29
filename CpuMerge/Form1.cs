using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuMerge
{
    public partial class Form1 : Form
    {
        private Bitmap target;
        public Bitmap Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                Size = new Size(target.Width, target.Height);
            }
        }
        private Graphics graphics;


        public Form1()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }
        protected override void OnResize(EventArgs e)
        {
            graphics = CreateGraphics();
            graphics.DrawImage(Target, 0, 0);
        }
        public override void Refresh()
        {
            graphics.DrawImage(Target, 0, 0);
        }
    }
}
