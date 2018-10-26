using ShComp;
using System;
using System.IO;
using System.Linq;

namespace QRCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            args.Where(File.Exists).ForEach(arg =>
            {
                var data = File.ReadAllText(arg);

                var ecl = QRCoder.QRCodeGenerator.ECCLevel.L;
                var dr = false;

                var name = Path.GetFileNameWithoutExtension(arg);
                var d = name.LastIndexOf("-");
                if (d != -1 && d + 1 < name.Length)
                {
                    foreach (var item in name.Substring(d + 1).ToUpper())
                    {
                        switch (item)
                        {
                            case 'M': ecl = QRCoder.QRCodeGenerator.ECCLevel.M; break;
                            case 'Q': ecl = QRCoder.QRCodeGenerator.ECCLevel.Q; break;
                            case 'H': ecl = QRCoder.QRCodeGenerator.ECCLevel.H; break;
                            case 'L': ecl = QRCoder.QRCodeGenerator.ECCLevel.L; break;
                            case 'R': dr = true; break;
                        }
                    }
                }

                var qr = new QRCoder.QRCodeGenerator();
                var qrcd = qr.CreateQrCode(data, ecl);
                var matrix = qrcd.ModuleMatrix;
                var width = matrix[0].Count - 8;
                var height = matrix.Count - 8;
                var commands = PathCommandGenerator.Generate((x, y) => matrix[y + 4].Get(x + 4), width, height);

                var path = Path.GetExtension(arg).Equals(".svg", StringComparison.CurrentCultureIgnoreCase) ? arg + ".svg" : Path.ChangeExtension(arg, ".svg");
                using (var writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                    writer.WriteLine($@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 {width + 8} {height + 8}"">");
                    writer.Write(@"<path d=""");

                    foreach (var command in commands)
                    {
                        writer.Write(command.ToString(4, 4));
                    }

                    writer.WriteLine(@"""/>");
                    if (dr) writer.WriteLine($@"<rect x=""0"" y=""0"" width=""{width + 8}"" height=""{height + 8}"" stroke=""black"" stroke-width=""0.1"" fill=""none"" />");
                    writer.WriteLine("</svg>");
                }
            }, () =>
            {
                Console.WriteLine("ファイルをドロップしてね。");
                Console.WriteLine("-LMQH R");
                Console.ReadLine();
            });
        }
    }
}
