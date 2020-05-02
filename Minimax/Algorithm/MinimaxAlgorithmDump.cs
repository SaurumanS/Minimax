using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.Algorithm
{
    class MinimaxAlgorithmDump : IAlgorithm
    {
        public (int, int) Algorithm(string[,] gameField)
        {
            for(int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    if (String.IsNullOrEmpty(gameField[i, j]))
                        return (i, j);
                }
            }
            return (0, 0);
        }

        public (int, int) Algorithm(string[,] gameField, int maxDeph, int playUntil, string computerGameSymbol, string playerGameSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
