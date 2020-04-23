using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax
{
    public static class ArrayExt
    {
        public static T[,] ToMultidimensional<T>(this T[][] cascade)
        {
            T[,] result = new T[cascade.Length, cascade[0].Length];

            for (int row = 0; row < result.GetLength(0); row++)
            {
                for (int column = 0; column < result.GetLength(1); column++)
                {
                    result[row, column] = cascade[row][column];
                }
            }

            return result;
        }

        public static T[][] ToCascade<T>(this T[,] multiDim)
        {
            T[][] result = new T[multiDim.GetLength(0)][];

            for (int row = 0; row < multiDim.GetLength(0); row++)
            {
                result[row] = new T[multiDim.GetLength(1)];
                for (int column = 0; column < multiDim.GetLength(1); column++)
                {
                    result[row][column] = multiDim[row, column];
                }
            }

            return result;
        }
    }
}
