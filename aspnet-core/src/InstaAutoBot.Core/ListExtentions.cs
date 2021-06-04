using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAutoBot
{
    public static class ListExtensions
    {
        public static IEnumerable<List<T>> SplitList<T>(this List<T> list, int nSize = 30)
        {
            for (int i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
    }
}
