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

                var qr = new QRCoder.QRCodeGenerator();
                var qrcd = qr.CreateQrCode(data, QRCoder.QRCodeGenerator.ECCLevel.L);

                //var ecl = ErrorCorrectionLevel.L;
                //var dr = false;

                //var name = Path.GetFileNameWithoutExtension(arg);
                //var d = name.LastIndexOf("-");
                //if (d != -1 && d + 1 < name.Length)
                //{
                //    foreach (var item in name.Substring(d + 1).ToUpper())
                //    {
                //        switch (item)
                //        {
                //            case 'M': ecl = ErrorCorrectionLevel.M; break;
                //            case 'Q': ecl = ErrorCorrectionLevel.Q; break;
                //            case 'H': ecl = ErrorCorrectionLevel.H; break;
                //            case 'L': ecl = ErrorCorrectionLevel.L; break;
                //            case 'R': dr = true; break;
                //        }
                //    }
                //}

                //var qre = new QrEncoder(ecl);
                //var qrc = qre.Encode(data);

                //var path = Path.GetExtension(arg).Equals(".svg", StringComparison.CurrentCultureIgnoreCase) ? arg + ".svg" : Path.ChangeExtension(arg, ".svg");
                //using (var writer = new StreamWriter(path, false))
                //{
                //    writer.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                //    writer.WriteLine($@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 {qrc.Matrix.Width + 8} {qrc.Matrix.Height + 8}"">");
                //    writer.Write(@"<path d=""");

                //    foreach (var command in qrc.ConvertPathCommand())
                //    {
                //        writer.Write(command.ToString(4, 4));
                //    }

                //    writer.WriteLine(@"""/>");
                //    if (dr) writer.WriteLine($@"<rect x=""0"" y=""0"" width=""{qrc.Matrix.Width + 8}"" height=""{qrc.Matrix.Height + 8}"" stroke=""black"" stroke-width=""0.1"" fill=""none"" />");
                //    writer.WriteLine("</svg>");
                //}
            }, () =>
            {
                Console.WriteLine("ファイルをドロップしてね。");
                Console.WriteLine("-LMQH R");
                Console.ReadLine();
            });
        }
    }
}
