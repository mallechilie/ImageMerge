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
        private static OpenFileDialog openFileDialog1;
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
            RainImage firstImage = ParseLine(Console.ReadLine(), OpenFileDialog());
            RainImage secondImage = ParseLine(Console.ReadLine(), OpenFileDialog());
            RainImage target = ParseLine(Console.ReadLine());
            ImageMerger merger = new ImageMerger(firstImage, secondImage, target, ImageMerger.CombineMode.Mask);
            f.Target = (Bitmap) merger.Target;
            f.Target.Save(SaveFileDialog());
            RunForm(f);
        }
        static void RunForm(Form1 f)
        {
            Application.Run(f);
        }
        static string OpenFileDialog()
        {
            if (openFileDialog1 == null)
            {
                openFileDialog1 = new OpenFileDialog();
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.InitialDirectory = @"C:\Users\Reus\Pictures\Wallpapers";
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return  openFileDialog1.FileName;
            }
            return null;
        }
        static string SaveFileDialog()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            return null;
        }
        static RainImage ParseLine(string s, string path = null)
        {
            string[] strings = s.Split(new[] { ' ' }, 5);
            if (path != null)
            {
                if (strings.Length == 1)
                    return new RainImage(ParseRatio(strings[0]), path);
                return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                                     new Int2(int.Parse(strings[2]), int.Parse(strings[3])), path);
            }
            return new RainImage(new Int2(int.Parse(strings[0]), int.Parse(strings[1])),
                new Int2(int.Parse(strings[2]), int.Parse(strings[3])), new Color[int.Parse(strings[2]), int.Parse(strings[3])]);
        }
        private static RainImage.PreserveAspectRatio ParseRatio(string ratio)
        {
            switch (ratio)
            {
                case "exact":
                {
                    return RainImage.PreserveAspectRatio.Exact;
                }
                case "fill":
                {
                    return RainImage.PreserveAspectRatio.Fill;
                }
                case "preserve":
                {
                    return RainImage.PreserveAspectRatio.Preserve;
                }
                default:
                    return RainImage.PreserveAspectRatio.Coordinates;
            }
        }
    }
}
