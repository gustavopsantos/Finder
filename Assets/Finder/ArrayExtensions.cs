using System;
using System.Collections.Generic;
using System.Linq;

namespace Finder
{
    internal static class ArrayExtensions
    {
        internal static List<T[]> Chunk<T>(this T[] collection, int size)
        {
            var chunkCount = (int) Math.Ceiling((double) collection.Length / size);
            var chunks = new List<T[]>();

            for (int i = 0; i < chunkCount; i++)
            {
                chunks.Add(collection.Skip(i * size).Take(size).ToArray());
            }

            return chunks;
        }
    }
}