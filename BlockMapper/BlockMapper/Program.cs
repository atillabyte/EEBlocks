using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace BlockMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var blocks = new List<Block>();

            foreach (var image in Directory.GetFiles(@"X:\EEBlocks-master\Bricks"))
            {
                var info = new FileInfo(image);

                if (info.Name.Contains("_"))
                    continue;

                var type = new string(Path.GetFileNameWithoutExtension(image).Where(x => !char.IsLetter(x)).ToArray());
                blocks.Add(new Block() { Type = int.Parse(type), Bitmap = (Bitmap)Bitmap.FromFile(image) });
            }

            var bitmap = new Bitmap(blocks.Count() * 16, 16, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bitmap);

            foreach (var block in blocks)
                g.DrawImage(block.Bitmap, block.Type * 16, 0, 16, 16);

            bitmap.Save(@"X:\output.png", ImageFormat.Png);
            Console.WriteLine("Done.");

            Console.ReadLine();
        }

        class Block
        {
            public int Type { get; set; }
            public Bitmap Bitmap { get; set; }
        }
    }
}
