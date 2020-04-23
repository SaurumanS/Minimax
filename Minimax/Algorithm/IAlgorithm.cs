using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minimax.GameLogic;

namespace Minimax.Algorithm
{
    public interface IAlgorithm
    {
        (int, int) Algorithm(string[,] gameField);
    }
}
