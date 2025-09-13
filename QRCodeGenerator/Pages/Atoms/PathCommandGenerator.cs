namespace QRCodeGenerator.Pages.Atoms;

internal static class PathCommandGenerator
{
    public static IEnumerable<PathCommand> Generate(this Func<int, int, bool> m, int w, int h)
    {
        var plz = new List<_>();

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (m(x, y))
                {
                    if (x == 0 || !m(x - 1, y))
                    {
                        plz.Add(new _(x, y, x, y + 1, w + 1, 0));
                    }

                    if (y == 0 || !m(x, y - 1))
                    {
                        plz.Add(new _(x + 1, y, x, y, w + 1, 3));
                    }

                    if (x == w - 1 || !m(x + 1, y))
                    {
                        plz.Add(new _(x + 1, y + 1, x + 1, y, w + 1, 2));
                    }

                    if (y == h - 1 || !m(x, y + 1))
                    {
                        plz.Add(new _(x, y + 1, x + 1, y + 1, w + 1, 1));
                    }
                }
            }
        }

        var pl = plz.ToArray();
        int current = 0;
        while (current < pl.Length)
        {
            for (; current < pl.Length; current++)
            {
                if (!pl[current].IsUsed) break;
            }
            if (current >= pl.Length) break;
            var target = pl[current];
            var prev = target;

            yield return new PathCommand(PathMethod.M, prev.SX, prev.SY);

            while (true)
            {
                int fc = 0;
                int dd = 100;
                int fi = 0;
                for (int i = current + 1; i < pl.Length; i++)
                {
                    if (pl[i].SP == prev.EP && !pl[i].IsUsed)
                    {
                        var dt = (pl[i].Direction + 4 - prev.Direction) % 4;
                        if (dd > dt)
                        {
                            dd = dt;
                            fi = i;
                        }

                        fc += 1;
                        if (fc == 2) break;
                    }
                }
                var next = pl[fi];
                pl[fi].IsUsed = true;

                if (prev.Direction != next.Direction)
                {
                    if (prev.Direction % 2 == 0)
                    {
                        yield return new PathCommand(PathMethod.V, next.SY);
                    }
                    else
                    {
                        yield return new PathCommand(PathMethod.H, next.SX);
                    }
                }

                if (next.EP == target.SP) break;
                prev = next;
            }

            yield return new PathCommand(PathMethod.z);
            current += 1;
        }
    }

    private struct _
    {
        public byte SX;
        public byte SY;
        public byte Direction;
        public int SP;
        public int EP;
        public bool IsUsed;

        public _(int sx, int sy, int ex, int ey, int w, int direction)
        {
            SX = (byte)sx;
            SY = (byte)sy;
            SP = sx + sy * w;
            EP = ex + ey * w;
            Direction = (byte)direction;
            IsUsed = false;
        }

        public override string ToString()
        {
            return $"{SX},{SY} -> {Direction} {IsUsed}";
        }
    }
}

/// <summary>
/// WPFやSVGで使用できるパス命令です。
/// </summary>
public struct PathCommand
{
    /// <summary>パス命令のメソッドです。</summary>
    public PathMethod Method;
    /// <summary>このコマンドの1つ目のパラメータです。</summary>
    public byte Param1;
    /// <summary>このコマンドの2つ目のパラメータです。</summary>
    public byte Param2;

    public PathCommand(PathMethod method, byte param1 = 0, byte param2 = 0)
    {
        Method = method;
        Param1 = param1;
        Param2 = param2;
    }

    public override string? ToString()
    {
        return ToString(0, 0);
    }

    /// <summary>
    /// パス命令を指定したオフセットずらし、文字列に変換します。
    /// </summary>
    public string? ToString(int offsetX, int offsetY)
    {
        return Method switch
        {
            PathMethod.M => "M" + (Param1 + offsetX) + " " + (Param2 + offsetY),
            PathMethod.V => "V" + (Param1 + offsetX),
            PathMethod.H => "H" + (Param1 + offsetY),
            PathMethod.z => "z",
            _ => null,
        };
    }
}

/// <summary>
/// パス命令のメソッドです。
/// </summary>
public enum PathMethod : byte
{
    /// <summary>ペンを、線の描画をせずに指定した絶対座標に移動します。</summary>
    M,
    /// <summary>指定した絶対Y座標へ、線を描画しながら垂直移動します。</summary>
    V,
    /// <summary>指定した絶対X座標へ、線を描画しながら水平移動します。</summary>
    H,
    /// <summary>線を閉じます。</summary>
    z,
}
