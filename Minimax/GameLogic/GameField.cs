using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.GameLogic
{
    public class GameField
    {
        public readonly string FirstPlayerSymbol;
        public readonly string SecondPlayerSymbol;

        private string[,] gameField { get; set; }

        public GameField(int xProjectionSize,
            int yProjectionSize,
            string firstPlayerSymbol, 
            string secondPlayerSymbol)
        {
            FirstPlayerSymbol = firstPlayerSymbol;
            SecondPlayerSymbol = secondPlayerSymbol;
            gameField = new string[xProjectionSize, yProjectionSize];
        }

        public void FillField(int x, int y, bool isFirstPlayer)
        {
            if (CheckField(x, y))
            {
                throw new ArgumentException("This field is not null, check coordinates");
            }

            if (isFirstPlayer)
            {
                gameField[x, y] = FirstPlayerSymbol;
            }
            else
            {
                gameField[x, y] = SecondPlayerSymbol;
            }
        }

        public bool CheckField(int x, int y) => String.IsNullOrEmpty(gameField[x, y]) ? false : true;

        public string[,] GetGameFied()
        {
            return (string[,]) gameField.Clone();
        }

        public bool IsTheFieldComplete()
        {
            int x = 0;
            bool isComplete = true;
            while (isComplete && x < gameField.GetLength(0))
            {
                int y = 0;
                while (y < gameField.GetLength(1))
                {
                    if (String.IsNullOrEmpty(gameField[x, y]))
                    {
                        isComplete = false;
                        break;
                    }
                    y++;
                }
                x++;
            }

            return isComplete;
        }
    }
}
