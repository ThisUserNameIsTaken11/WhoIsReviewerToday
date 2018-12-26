using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace WhoIsReviewerToday.Common
{
    public static class ListExtensions
    {
        private static readonly RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            var n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do
                {
                    _rngCryptoServiceProvider.GetBytes(box);
                } while (!(box[0] < n * (byte.MaxValue / n)));

                var k = box[0] % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}