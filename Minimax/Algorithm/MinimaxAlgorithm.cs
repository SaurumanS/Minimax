using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.Algorithm
{
    class MinimaxAlgorithm : IAlgorithm
    {

        public (int, int) Algorithm(string[,] gameField, int maxDeph, int playUntil, string computerGameSymbol, string playerGameSymbol)
        {
            this.maxDeph = maxDeph;
            this.computerGameSymbol = computerGameSymbol;
            this.playerGameSymbol = playerGameSymbol;
            this.playUntil = playUntil;
            (int, int) result = (-1,-1);
            int maxScore = int.MinValue;
            List<(int, int, int)> HelpMe = new List<(int, int, int)>();
            for(int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int column = 0; column < gameField.GetLength(1); column++)
                {
                    if (String.IsNullOrEmpty(gameField[row, column]))
                    {
                        int score = Max(gameField, row, column, 1, int.MinValue, int.MaxValue);
                        if (score > maxScore)
                        {
                            maxScore = score;
                            result = (row, column);
                        }
                    }
                }
            }

            return result;
        }
        private int maxDeph { get; set; }
        private string computerGameSymbol { get; set; }
        private string playerGameSymbol { get; set; }
        private int playUntil { get; set; }

        private int Max(string[,] gameField, int row, int column, int deph, int alpha, int beta)
        {
            bool isTerminal = IsTerminal(gameField, row, column, deph);
            if(isTerminal)
                return Heuristic(gameField, row, column);

            if (Heuristic(gameField, row, column) == int.MaxValue)
                return int.MaxValue;
            int score = alpha;

            var childrens = GetChildCoordinates(gameField, row, column);

            foreach(var curr in childrens)
            {
                int tempScore = Min(gameField, curr.Item1, curr.Item2, deph + 1, score, beta);
                if (tempScore > score)
                    score = tempScore;
                if (score >= beta)
                    return score;
            }

            return score;
        }


        private int Min(string[,] gameField, int row, int column, int deph, int alpha, int beta)
        {
            bool isTerminal = IsTerminal(gameField, row, column, deph);
            if (isTerminal)
                return Heuristic(gameField, row, column);

            int score = beta;

            var childrens = GetChildCoordinates(gameField, row, column);

            foreach (var curr in childrens)
            {
                int tempScore = Min(gameField, curr.Item1, curr.Item2, deph + 1, alpha, score);
                if (tempScore < score)
                    score = tempScore;
                if (score <= alpha)
                    return score;
            }

            return score;
        }


        private List<(int, int)> GetChildCoordinates(string[,] gameField, int x, int y)
        {
            List<(int, int)> childrens = new List<(int, int)>();
            int currentX = x - 1;
            int currentY = y - 1;
            if (currentX > 0 && currentY > 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x - 1;
            currentY = y;
            if (currentX > 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x - 1;
            currentY = y + 1;
            if (currentX > 0 && currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x;
            currentY = y + 1;
            if (currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x + 1;
            currentY = y + 1;
            if (currentX < gameField.GetLength(0) && currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x + 1;
            currentY = y;
            if (currentX < gameField.GetLength(0) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x + 1;
            currentY = y - 1;
            if (currentX < gameField.GetLength(0) && currentY > 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            currentX = x;
            currentY = y - 1;
            if (currentY > 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            return childrens;
        }


        /*
         * This is field positions:
         * northWest | north | northEast
         * -----------------------------
         * west      | curr  | east
         * -----------------------------
         * southWest | south | southEast
        */
        private int Heuristic(string[,] gameField, int x, int y)
        {
            string currentSymbol = null;
            int currentX;
            int currentY;
            string symbol;

            int k = 0; //length of the assemble combination (All sides)
            int z = 0; //length of the destroy combination (All sides)

            int tempK;
            int tempZ;
            #region NorthWest->curr->SouthEast


            
            currentSymbol = null;
            currentX = x;
            currentY = y;
            symbol = null;
            tempK = 0;
            tempZ = 0;
            do
            {
                currentX--;
                currentY--;

                if (currentX < 0 ||
                    currentY < 0)
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;

                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if(currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);

            currentSymbol = null;
            currentX = x;
            currentY = y;
            symbol = null;
            do
            {
                currentX++;
                currentY++;

                if (currentX >= gameField.GetLength(0) ||
                    currentY >= gameField.GetLength(1))
                    break;

                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            if (tempK != 0)
                tempK++;
            if (tempK >= playUntil || tempZ >= playUntil-1)
                return int.MaxValue;
            k += tempK;
            z += tempZ;
            #endregion
            #region North->curr->South



            currentSymbol = null;
            currentX = x;
            currentY = y;
            tempK = 0;
            tempZ = 0;
            do
            {

                currentX--;

                if (currentX < 0)
                    break;

                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {

                currentX++;

                if (currentX >= gameField.GetLength(0))
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            if (tempK != 0)
                tempK++;
            if (tempK >= playUntil || tempZ >= playUntil)
                return int.MaxValue;
            k += tempK;
            z += tempZ;
            #endregion
            #region NorthEast->curr->SouthWest

            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {

                currentX--;
                currentY++;

                if (currentX < 0 ||
                    currentY >= gameField.GetLength(1))
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            tempK = 0;
            tempZ = 0;
            do
            {
                currentX++;
                currentY--;

                if (currentX >= gameField.GetLength(0) ||
                    currentY < 0)
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            if (tempK != 0)
                tempK++;
            if (tempK >= playUntil || tempZ >= playUntil)
                return int.MaxValue;
            k += tempK;
            z += tempZ;
            #endregion
            #region East->curr->West



            currentSymbol = null;
            currentX = x;
            currentY = y;
            tempK = 0;
            tempZ = 0;
            do
            {

                currentY++;

                if (currentY >= gameField.GetLength(1))
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {

                currentY--;

                if (currentY < 0)
                    break;
                if (symbol == null)
                    symbol = gameField[currentX, currentY];
                if (symbol == null) break;
                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    tempK++;
                else if (currentSymbol == playerGameSymbol)
                    tempZ++;

            } while (currentSymbol == symbol);
            if (tempK != 0)
                tempK++;
            if (tempK >= playUntil || tempZ >= playUntil)
                return int.MaxValue;
            k += tempK;
            z += tempZ;
            #endregion

            return GetScore(k, z);

        }

        private (int,int) CalculateKZ(string[,] gameField, int x, int y, Func<int,int> xBehaviour, Func<int, int> yBehaviour)
        {
            string currentSymbol = null;
            int currentX = x;
            int currentY = y;
            int k = 0;
            int z = 0;
            string symbol = null;
            do
            {
                currentX = xBehaviour(currentX);
                currentY = yBehaviour(currentY);

                if (currentX < 0 ||
                    currentY < 0)
                    break;
                if (String.IsNullOrEmpty(symbol))
                    symbol = gameField[currentX, currentY];
                if (String.IsNullOrEmpty(symbol)) break;

                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol == computerGameSymbol)
                    k++;
                else if (currentSymbol == playerGameSymbol)
                    z++;

            } while (currentSymbol == symbol);


            if (k != 0)
                k++;
            if (z != 0)
                z++;

            return (k, z);
        }

        private int GetScore(int k, int z)
        {
            int score = Fact(k + 2) + Fact(z + 2);
            return score;
        }

        private int Fact(int x)
        {
            int result = 1;
            for (int i = 2; i <= x; i++)
                result *= i;
            return result;
        }

        private bool IsTerminal(string[,] gameField, int row, int column, int deph)
        {
            if (deph >= maxDeph)
                return true;

            var gameFieldClone =(string[,]) gameField.Clone();
            gameFieldClone[row, column] = computerGameSymbol;
            bool? checkForTerminal = CheckForWin(gameFieldClone, row, column);

            if (checkForTerminal == false)
                return false;
            else
                return true;

        }
        private bool? CheckForWin(string[,] gameField, int lastX, int lastY)
        {
            string currentSymbol = gameField[lastX, lastY];
            string symbol = currentSymbol;
            
            var maxSequence = GetMaxSequenceRelativeMoveMade(gameField, symbol, lastX, lastY);

            bool? result;
            if (maxSequence >= playUntil)
                result = true;
            else
            {
                bool isFieldComplete = IsTheFieldComplete(gameField);
                if (isFieldComplete)
                    result = null;
                else
                    result = false;
            }
            return result;
        }

        /*
         * This is field positions:
         * northWest | north | northEast
         * -----------------------------
         * west      | curr  | east
         * -----------------------------
         * southWest | south | southEast
        */
        private int GetMaxSequenceRelativeMoveMade(string[,] gameField, string symbol, int x, int y)
        {
            string currentSymbol = null;
            int currentX = x;
            int currentY = y;
            #region NorthWest->curr->SouthEast


            int NW_C_SE_maxSequence = 0;

            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                NW_C_SE_maxSequence++;

                currentX--;
                currentY--;

                if (currentX < 0 ||
                    currentY < 0)
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                NW_C_SE_maxSequence++;

                currentX++;
                currentY++;

                if (currentX >= gameField.GetLength(0) ||
                    currentY >= gameField.GetLength(1))
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            NW_C_SE_maxSequence--;
            #endregion
            #region North->curr->South


            int N_C_S_maxSequence = 0;

            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                N_C_S_maxSequence++;

                currentX--;

                if (currentX < 0)
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                N_C_S_maxSequence++;

                currentX++;

                if (currentX >= gameField.GetLength(0))
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            N_C_S_maxSequence--;
            #endregion
            #region NorthEast->curr->SouthWest


            int NE_C_SW_maxSequence = 0;

            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                NE_C_SW_maxSequence++;

                currentX--;
                currentY++;

                if (currentX < 0 ||
                    currentY >= gameField.GetLength(1))
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                NE_C_SW_maxSequence++;

                currentX++;
                currentY--;

                if (currentX >= gameField.GetLength(0) ||
                    currentY < 0)
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            NE_C_SW_maxSequence--;
            #endregion
            #region East->curr->West


            int E_C_W_maxSequence = 0;

            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                E_C_W_maxSequence++;

                currentY++;

                if (currentY >= gameField.GetLength(1))
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            currentSymbol = null;
            currentX = x;
            currentY = y;
            do
            {
                E_C_W_maxSequence++;

                currentY--;

                if (currentY < 0)
                    break;

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);
            E_C_W_maxSequence--;
            #endregion

            //return Math.Max( Math.Max(NW_C_SE_maxSequence, N_C_S_maxSequence), Math.Max(NE_C_SW_maxSequence, E_C_W_maxSequence) );
            return new int[] { NW_C_SE_maxSequence, N_C_S_maxSequence, NE_C_SW_maxSequence, E_C_W_maxSequence }.Max();
        }
        public bool IsTheFieldComplete(string[,] gameField)
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
