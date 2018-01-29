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
            //Form1 g = new Form1();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f = new Form1();
            f.Show();
            //new Thread(() => RunForm(f)).Start();
            //RunForm(f);
            RainImage firstImage = ParseLine(Console.ReadLine());
            RainImage secondImage = ParseLine(Console.ReadLine());
            RainImage target = ParseLine(Console.ReadLine());
            ImageMerger merger = new ImageMerger(firstImage, secondImage, target, ImageMerger.CombineMode.Mask);
            f.Target = (Bitmap) merger.Target;
            f.Target.Save(Console.ReadLine());
            RunForm(f);
        }
        static void RunForm(Form1 f)
        {
            Application.Run(f);
        }
        static RainImage ParseLine(string s)
        {
            string[] strings = s.Split(new[] { ' ' }, 5);
            if (strings.Length >= 5)
                return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                    new Int2(int.Parse(strings[2]), int.Parse(strings[3])), strings[4]);
            return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                new Int2(int.Parse(strings[2]), int.Parse(strings[3])), new Color[int.Parse(strings[2]), int.Parse(strings[3])]);
        }
    }
}
