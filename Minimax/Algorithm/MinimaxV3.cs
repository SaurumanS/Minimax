using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.Algorithm
{
    class MinimaxV3 : IAlgorithm
    {
        private int maxDeph { get; set; }
        private string computerGameSymbol { get; set; }
        private string playerGameSymbol { get; set; }
        private int playUntil { get; set; }
        List<string> afa = new List<string>();

        public (int, int) Algorithm(string[,] gameField, int maxDeph, int playUntil, string computerGameSymbol, string playerGameSymbol)
        {
            this.maxDeph = maxDeph;
            this.computerGameSymbol = computerGameSymbol;
            this.playerGameSymbol = playerGameSymbol;
            this.playUntil = playUntil;
            (int, int) result = (-1, -1);
            double maxScore = int.MinValue;
            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int column = 0; column < gameField.GetLength(1); column++)
                {
                    if (String.IsNullOrEmpty(gameField[row, column]))
                    {
                        double score = Max(gameField, row, column, 1, int.MinValue, int.MaxValue);
                        if (score > maxScore && score !=0)
                        {
                            maxScore = score;
                            result = (row, column);
                        }
                    }
                }
            }

            return result;
        }

        private double Max(string[,] gameField, int row, int column, int deph, double alpha, double beta)
        {
            string[,] gameFieldClone = (string[,])gameField.Clone();
            gameFieldClone[row, column] = computerGameSymbol;

            bool isTerminal = IsTerminal(gameFieldClone, row, column, deph);
            if (isTerminal)
                return FieldHeuristic(gameFieldClone, true);

            double score = alpha;

            var childrens = GetChildCoordinates(gameFieldClone, row, column);


            foreach (var curr in childrens)
            {
                double tempScore = Min(gameFieldClone, curr.Item1, curr.Item2, deph + 1, score, beta);
                if (tempScore > score)
                    score = tempScore;
                if (beta <= score)
                    return score;
            }

            return score;
        }


        private double Min(string[,] gameField, int row, int column, int deph, double alpha, double beta)
        {
            string[,] gameFieldClone = (string[,])gameField.Clone();
            gameFieldClone[row, column] = playerGameSymbol;

            bool isTerminal = IsTerminal(gameFieldClone, row, column, deph);
            if (isTerminal)
                return FieldHeuristic(gameFieldClone, false);

            double score = beta;

            var childrens = GetChildCoordinates(gameFieldClone, row, column);

            foreach (var curr in childrens)
            {
                double tempScore = Max(gameFieldClone, curr.Item1, curr.Item2, deph + 1, alpha, score);
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

        private double FieldHeuristic(string[,] gameField, bool isMaxPlayer)
        {
            bool[,] whoIsCheck = new bool[gameField.GetLength(0), gameField.GetLength(1)];
            double computerScore = 0;
            double playerScore = 0;
            for (int row = 0; row < gameField.GetLength(0); row++)
            {
                for (int column = 0; column < gameField.GetLength(1); column++)
                {
                    if (!whoIsCheck[row, column])
                    {
                        if(gameField[row,column] == computerGameSymbol)
                            computerScore += Heuristic(gameField, row, column, whoIsCheck);
                        else if(gameField[row, column] == playerGameSymbol)
                            playerScore += Heuristic(gameField, row, column, whoIsCheck);
                    }
                }
            }

            double finalScore = 0;

            playerScore *= -1;
            finalScore = computerScore + playerScore;
            afa.Add($" {finalScore}");
            //if (isMaxPlayer)
            //    finalScore = computerScore;
            //else
            //    finalScore = computerScore;


            //if (isMaxPlayer)
            //{
            //    if (playerScore >= 10 * (int)Math.Pow(100, playUntil - 3))
            //        finalScore = -playerScore;
            //    else
            //        finalScore = computerScore - playerScore;
            //}
            //else
            //{
            //    if (computerScore >= 10 * (int)Math.Pow(100, playUntil - 3))
            //        finalScore = computerScore;
            //    else
            //        finalScore = computerScore - playerScore;
            //}

            return finalScore;
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
        private int Heuristic(string[,] gameField, int x, int y, bool[,] whoIsCheck)
        {
            whoIsCheck[x, y] = true;
            if (String.IsNullOrEmpty(gameField[x, y]))
            {
                return 0;
            }

            Func<int, int> xBehaviour1;
            Func<int, int> yBehaviour1;

            Func<int, int> xBehaviour2;
            Func<int, int> yBehaviour2;

            Func<Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, int> getHeuristicScore = (xBehav1, yBehav1, xBehav2, yBehav2) => GetHeuristicScore(gameField, x, y, whoIsCheck, xBehav1, yBehav1, xBehav2, yBehav2);

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
            bool[,] whoIsCheck,
            Func<int, int> xBehaviour1,
            Func<int, int> yBehaviour1,
            Func<int, int> xBehaviour2,
            Func<int, int> yBehaviour2)
        {

            string symbol = gameField[x,y];
            string currentSymbol = symbol;
            int currentX = x;
            int currentY = y;
            bool leftLocked = false;
            bool rightLocked = false;

            int k = 1;
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

                

                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol != symbol)
                {
                    if (!String.IsNullOrEmpty(currentSymbol))
                        leftLocked = true;
                    break;
                }
                else
                    whoIsCheck[currentX, currentY] = true;

                k++;

            } while (currentSymbol == symbol);

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


                currentSymbol = gameField[currentX, currentY];
                if (currentSymbol != symbol)
                {
                    if (!String.IsNullOrEmpty(currentSymbol))
                        rightLocked = true;
                    break;
                }
                else
                    whoIsCheck[currentX, currentY] = true;

                k++;

            } while (currentSymbol == symbol);

            

            return GetScore(k, leftLocked, rightLocked);
        }

        //See LogicInfo\ScoreInfo.txt for score logic
        private int GetScore(int k, bool leftLocked, bool rightLocked)
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
                else if((rightLocked && !leftLocked) || (!rightLocked && leftLocked))
                    score += (int)Math.Pow(100, k - 1);
                else if (!rightLocked && !leftLocked)
                    score += 10 * (int)Math.Pow(100, k - 1);
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
