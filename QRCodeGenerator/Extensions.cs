using System;
using System.Collections.Generic;

namespace QRCodeGenerator
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action, Action empty)
        {
            var e = src.GetEnumerator();
            if (e.MoveNext())
            {
                action(e.Current);
                while (e.MoveNext()) action(e.Current);
            }
            else
            {
                empty();
            }
        }
    }
}
