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
        public Bitmap target;
        private Graphics graphics;


        public Form1()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }
        
        public override void Refresh()
        {
            graphics.DrawImage(target, 0, 0);
        }
    }
}
