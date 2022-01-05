using System.Linq;
using System.Collections.Generic;

namespace MarsRover.Logic.Extensions
{
    public class Utils
    {
        /// <summary>
        /// Chunk By
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
