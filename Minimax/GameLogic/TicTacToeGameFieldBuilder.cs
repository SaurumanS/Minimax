using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.GameLogic
{
    class TicTacToeGameFieldBuilder
    {
        public static GameField Create(int fieldSize)
        {
            return new GameField(fieldSize,
                fieldSize,
                "X",
                "0");
        }
    }
}
