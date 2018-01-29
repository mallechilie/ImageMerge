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
        public Bitmap Target;
        private readonly Graphics graphics;


        public Form1()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }
        protected override void OnResize(EventArgs e)
        {
            graphics.DrawImage(Target, 0, 0);
        }
        public override void Refresh()
        {
            graphics.DrawImage(Target, 0, 0);
        }
    }
}
