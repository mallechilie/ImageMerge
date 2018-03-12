using System;
using System.Drawing;
using System.Windows.Forms;

namespace CpuMerge
{
    static class Program
    {
        private static OpenFileDialog openFileDialog1;
        private static bool easier = true;
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f = new Form1();
            f.Show();

            RainImage firstImage;
            RainImage secondImage;
            RainImage target;
            if (easier)
            {
                firstImage = new RainImage(new Int2(30, 30), new Int2(940, 940), OpenFileDialog(), RainImage.PreserveAspectRatio.Preserve);
                secondImage = new RainImage(new Int2(0, 0), new Int2(1000, 1000), @"C:\Users\Reus\Documents\Rainmeter\Skins\Custom Slideshow Frame\@Resources\circle.png", RainImage.PreserveAspectRatio.Fill);
                target = ParseLine("30 30 940 940");
            }
            else
            {
                firstImage = ParseLine(Console.ReadLine(), OpenFileDialog());
                secondImage = ParseLine(Console.ReadLine(), OpenFileDialog());
                target = ParseLine(Console.ReadLine());
            }
            ImageMerger merger = new ImageMerger(firstImage, secondImage, target);
            f.Target = (Bitmap)merger.Target;
            string filename = SaveFileDialog();
            if (filename != null)
                f.Target.Save(filename);
            Application.Run(f);
        }
        
        static string OpenFileDialog()
        {
            if (openFileDialog1 == null)
            {
                openFileDialog1 = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    InitialDirectory = @"C:\Users\Reus\Pictures\Wallpapers"
                };
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
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
