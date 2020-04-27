using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.GameLogic
{
    class TicTacToeGameBuilder
    {
        public static TicTacToeGame Create(int size, bool DidPlayerChoseX, int playUntil, int level, MainWindow thisForm)
        {
            bool firstPlayerIsComputer = !DidPlayerChoseX;
            Minimax.Algorithm.IAlgorithm algorithm = new Algorithm.MinimaxAlgorithmDump();
            TicTacToeGame TicTacToe = new TicTacToeGame(size, algorithm, playUntil, level, firstPlayerIsComputer);
            GameFieldObserver gameFieldObserver = new GameFieldObserver(thisForm);
            TicTacToe.AddObserver(gameFieldObserver);
            return TicTacToe;
        }
    }
}
