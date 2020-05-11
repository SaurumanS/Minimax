using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.Algorithm
{
    public class MinimaxVersion2 : IAlgorithm
    {
        private int maxDeph { get; set; }
        private string computerGameSymbol { get; set; }
        private string playerGameSymbol { get; set; }
        private int playUntil { get; set; }

        public (int, int) Algorithm(string[,] gameField, int maxDeph, int playUntil, string computerGameSymbol, string playerGameSymbol)
        {
            this.maxDeph = maxDeph;
            this.computerGameSymbol = computerGameSymbol;
            this.playerGameSymbol = playerGameSymbol;
            this.playUntil = playUntil;
            (int, int) result = (-1, -1);
            int maxScore = int.MinValue;
            for (int row = 0; row < gameField.GetLength(0); row++)
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

        private int Max(string[,] gameField, int row, int column, int deph, int alpha, int beta)
        {
            string[,] gameFieldClone = (string[,])gameField.Clone();
            gameFieldClone[row, column] = computerGameSymbol;

            bool isTerminal = IsTerminal(gameFieldClone, row, column, deph);
            if (isTerminal)
                return Heuristic(gameField, row, column);

            int score = alpha;

            var childrens = GetChildCoordinates(gameFieldClone, row, column);


            foreach (var curr in childrens)
            {
                int tempScore = Min(gameFieldClone, curr.Item1, curr.Item2, deph + 1, score, beta);
                if (tempScore > score)
                    score = tempScore;
                if (beta <= score)
                    return score;
            }

            return score;
        }


        private int Min(string[,] gameField, int row, int column, int deph, int alpha, int beta)
        {
            string[,] gameFieldClone = (string[,])gameField.Clone();
            gameFieldClone[row, column] = playerGameSymbol;

            bool isTerminal = IsTerminal(gameFieldClone, row, column, deph);
            if (isTerminal)
                return -Heuristic(gameField, row, column);

            int score = beta;

            var childrens = GetChildCoordinates(gameFieldClone, row, column);

            foreach (var curr in childrens)
            {
                int tempScore = Max(gameFieldClone, curr.Item1, curr.Item2, deph + 1, alpha, score);
                if (tempScore < score)
                    score = tempScore;
                if (score <= alpha)
                    return score;
            }

            return score;
        }

        private List<(int, int)> GetChildCoordinates(string[,] gameField, int x, int y)
        {
            //North West
            List<(int, int)> childrens = new List<(int, int)>();
            int currentX = x - 1;
            int currentY = y - 1;
            if (currentX >= 0 && currentY >= 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //North
            currentX = x - 1;
            currentY = y;
            if (currentX >= 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //North East
            currentX = x - 1;
            currentY = y + 1;
            if (currentX >= 0 && currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //East
            currentX = x;
            currentY = y + 1;
            if (currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //South East
            currentX = x + 1;
            currentY = y + 1;
            if (currentX < gameField.GetLength(0) && currentY < gameField.GetLength(1) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //South
            currentX = x + 1;
            currentY = y;
            if (currentX < gameField.GetLength(0) && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //South West
            currentX = x + 1;
            currentY = y - 1;
            if (currentX < gameField.GetLength(0) && currentY >= 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            //West
            currentX = x;
            currentY = y - 1;
            if (currentY >= 0 && String.IsNullOrEmpty(gameField[currentX, currentY]))
                childrens.Add((currentX, currentY));

            return childrens;
        }


        #region HeuristicRegion
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

            Func<int, int> xBehaviour1;
            Func<int, int> yBehaviour1;

            Func<int, int> xBehaviour2;
            Func<int, int> yBehaviour2;

            Func<Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, int> getHeuristicScore = (xBehav1, yBehav1, xBehav2, yBehav2) => GetHeuristicScore(gameField, x, y, xBehav1, yBehav1, xBehav2, yBehav2);

            int score = 0;
            #region NorthWest->curr->SouthEast

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index - 1;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index + 1;

            score += getHeuristicScore(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2);

            #endregion
            #region North->curr->South

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index;

            score += getHeuristicScore(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2);

            #endregion
            #region NorthEast->curr->SouthWest

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index + 1;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index - 1;

            score += getHeuristicScore(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2);


            #endregion
            #region West->curr->East

            xBehaviour1 = (index) => index;
            yBehaviour1 = (index) => index - 1;

            xBehaviour2 = (index) => index;
            yBehaviour2 = (index) => index + 1;

            score += getHeuristicScore(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2);
            #endregion
            
            return score;
        }

        private int GetHeuristicScore(string[,] gameField,
            int x,
            int y,
            Func<int, int> xBehaviour1,
            Func<int, int> yBehaviour1,
            Func<int, int> xBehaviour2,
            Func<int, int> yBehaviour2)
        {

            string symbol = null;
            string currentSymbol = symbol;
            int currentX = x;
            int currentY = y;
            bool leftLocked = false;
            bool rightLocked = false;
            bool wasLeftK = false;

            int k = 1;
            int z = 1;
            do
            {
                currentX = xBehaviour1(currentX);
                currentY = yBehaviour1(currentY);

                if (currentX < 0 ||
                    currentY < 0 ||
                    currentX > gameField.GetLength(0) - 1 ||
                    currentY > gameField.GetLength(1) - 1)
                {
                    leftLocked = true;
                    break;
                }
                
                if(String.IsNullOrEmpty(symbol))
                    symbol = gameField[currentX, currentY];
                if (String.IsNullOrEmpty(symbol))
                    break;

                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol != symbol)
                {
                    if (!String.IsNullOrEmpty(currentSymbol))
                        leftLocked = true;
                    break;
                }

                if (currentSymbol == computerGameSymbol)
                {
                    wasLeftK = true;
                    k++;
                }
                else if (currentSymbol == playerGameSymbol)
                    z++;

            } while (currentSymbol == symbol);

            symbol = null;
            currentX = x;
            currentY = y;
            do
            {
                currentX = xBehaviour2(currentX);
                currentY = yBehaviour2(currentY);

                if (currentX < 0 ||
                    currentY < 0 ||
                    currentX > gameField.GetLength(0) - 1 ||
                    currentY > gameField.GetLength(1) - 1)
                {
                    rightLocked = true;
                    break;
                }

                if (String.IsNullOrEmpty(symbol))
                    symbol = gameField[currentX, currentY];
                if (String.IsNullOrEmpty(symbol))
                    break;

                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol != symbol)
                {
                    if (!String.IsNullOrEmpty(currentSymbol))
                        rightLocked = true;
                    break;
                }

                if (currentSymbol == computerGameSymbol)
                    k++;
                else if (currentSymbol == playerGameSymbol)
                    z++;

            } while (currentSymbol == symbol);

            //In this case, k (x,y coordinate) was counted twice
            if (k == 1 && z == 0)
                return 0;

            return GetScore(k, z, wasLeftK, leftLocked, rightLocked);
        }

        //See LogicInfo\ScoreInfo.txt for score logic
        private int GetScore(int k, int z, bool wasLeftK,  bool leftLocked, bool rightLocked)
        {
            int score = 0;

            if (k != 0)
            {
                if (k >= playUntil)
                {
                    score += (int)Math.Pow(100, k - 1);
                }
                else if (rightLocked && leftLocked)
                    score = 0;
                else if (wasLeftK)
                {
                    if (leftLocked)
                    {
                        if (z == 0)
                            score += (int)Math.Pow(100, k - 1);
                    }
                    else
                    {
                        if (z != 0)
                            score += (int)Math.Pow(100, k - 1);
                        else
                            score += 10 * (int)Math.Pow(100, k - 1);
                    }
                }
                else
                {
                    if (rightLocked)
                    {
                        if (z == 0)
                            score += (int)Math.Pow(100, k - 1);
                    }
                    else
                    {
                        if (z != 0)
                            score += (int)Math.Pow(100, k - 1);
                        else
                            score += 10 * (int)Math.Pow(100, k - 1);
                    }
                }
            }

            if (z != 0)
            {
                if (z >= playUntil)
                {
                    score += (int)Math.Pow(100, z - 1);
                }
                else if (rightLocked && leftLocked)
                    score = 0;
                else if (wasLeftK)
                {
                    if (rightLocked)
                    {
                        if (k == 0)
                            score += (int)Math.Pow(100, z - 1);
                    }
                    else
                    {
                        if (k == 0)
                            score += 10 *(int)Math.Pow(100, z - 1);
                        else
                            score += (int)Math.Pow(100, z - 1);
                    }
                }
                else
                {
                    if (leftLocked)
                    {
                        if (k == 0)
                            score += (int)Math.Pow(100, z - 1);
                    }
                    else
                    {
                        if (k != 0)
                            score += (int)Math.Pow(100, z - 1);
                        else
                            score += 10 * (int)Math.Pow(100, z - 1);
                    }
                }
            }

            return score;
        }
        #endregion 

        #region CheckingRegion
        private bool IsTerminal(string[,] gameField, int row, int column, int deph)
        {
            if (deph >= maxDeph)
                return true;


            bool? checkForTerminal = CheckForWin(gameField, row, column);

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
        private int GetMaxSequenceRelativeMoveMade(string[,] gameField,
            string symbol,
            int x,
            int y)
        {
            Func<int, int> xBehaviour1;
            Func<int, int> yBehaviour1;

            Func<int, int> xBehaviour2;
            Func<int, int> yBehaviour2;

            Func<Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, int> getSequence = (xBehav1, yBehav1, xBehav2, yBehav2) => GetSequence(gameField, symbol, x, y, xBehav1, yBehav1, xBehav2, yBehav2);

            List<int> sequences = new List<int>();
            #region NorthWest->curr->SouthEast

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index - 1;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index + 1;

            sequences.Add(getSequence(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2));

            #endregion
            #region North->curr->South

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index;

            sequences.Add(getSequence(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2));

            #endregion
            #region NorthEast->curr->SouthWest

            xBehaviour1 = (index) => index - 1;
            yBehaviour1 = (index) => index + 1;

            xBehaviour2 = (index) => index + 1;
            yBehaviour2 = (index) => index - 1;

            sequences.Add(getSequence(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2));


            #endregion
            #region West->curr->East

            xBehaviour1 = (index) => index;
            yBehaviour1 = (index) => index - 1;

            xBehaviour2 = (index) => index;
            yBehaviour2 = (index) => index + 1;

            sequences.Add(getSequence(xBehaviour1, yBehaviour1, xBehaviour2, yBehaviour2));
            #endregion

            return sequences.Max();
        }
        private int GetSequence(string[,] gameField, 
            string symbol, 
            int x, 
            int y,
            Func<int, int> xBehaviour1,
            Func<int, int> yBehaviour1,
            Func<int, int> xBehaviour2,
            Func<int, int> yBehaviour2)
        {
            string currentSymbol = symbol;
            int currentX = x;
            int currentY = y;
            int sequence = 0;

            do
            {
                sequence++;

                currentX = xBehaviour1(currentX);
                currentY = yBehaviour1(currentY);

                if (currentX < 0 ||
                    currentY < 0 ||
                    currentX > gameField.GetLength(0) - 1 ||
                    currentY > gameField.GetLength(1) - 1)
                {
                    break;
                }

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);

            currentX = x;
            currentY = y;
            do
            {
                sequence++;

                currentX = xBehaviour2(currentX);
                currentY = yBehaviour2(currentY);

                if (currentX < 0 ||
                    currentY < 0 ||
                    currentX > gameField.GetLength(0) - 1 ||
                    currentY > gameField.GetLength(1) - 1)
                {
                    break;
                }

                currentSymbol = gameField[currentX, currentY];

            } while (currentSymbol == symbol);

            return sequence;
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
        #endregion
    }
}
