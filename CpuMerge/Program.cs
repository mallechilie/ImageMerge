using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuMerge
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Form1 f = new Form1();
            new Thread(() => RunForm(f)).Start();
            RainImage firstImage = ParseLine(Console.ReadLine());
            RainImage secondImage = ParseLine(Console.ReadLine());
            RainImage target = ParseLine(Console.ReadLine());
            ImageMerger merger = new ImageMerger(firstImage, secondImage, target, ImageMerger.CombineMode.Mean);
            f.target = (Bitmap)merger.target;
        }
        static void RunForm(Form1 f)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(f);
        }
        static RainImage ParseLine(string s)
        {
            string[] strings = s.Split();
            if (strings.Length >= 5)
                return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                    new Int2(int.Parse(strings[2]), int.Parse(strings[3])), strings[4]);
            return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                new Int2(int.Parse(strings[2]), int.Parse(strings[3])), new Color[int.Parse(strings[2]), int.Parse(strings[3])]);
        }
    }
}
