using Minimax.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax.GameLogic
{
    public class TicTacToeGame: IObservable
    {
        private IAlgorithm Algorithm;
        private GameField GameField;

        private readonly int fieldSize;

        //The necessary sequence for victory
        private readonly int playUntil;

        //The First player makes the move first
        private const byte FIRST_PLAYER_ID = 1;
        private const byte SECOND_PLAYER_ID = 2;

        //Which player should make a move now
        private byte currentPlayer;
        private byte CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                CurrentPlayerPropertyChanged();
            }
        }

        public bool? IsWinnerHere { get; set; } = false;

        private readonly int level;

        private void CurrentPlayerPropertyChanged()
        {
            if (firstPlayerIsAComputer && currentPlayer == FIRST_PLAYER_ID)
                ComputerMove();
            else if(!firstPlayerIsAComputer && currentPlayer == SECOND_PLAYER_ID)
                ComputerMove();
        }

        //This is a question. This game knows that one of these players is a computer
        private readonly bool firstPlayerIsAComputer;

        public TicTacToeGame(int fieldSize, IAlgorithm algorithm, int playUntil, int level,  bool firstPlayerIsAComputer)
        {
            Algorithm = algorithm;
            this.firstPlayerIsAComputer = firstPlayerIsAComputer;
            GameField = TicTacToeGameFieldBuilder.Create(fieldSize);
            this.fieldSize = fieldSize;
            this.playUntil = playUntil;
            this.level = level;
            Observers = new List<IObserver>();

        }

        bool gameHasBegun = false;
        public void StartGame()
        {
            if (gameHasBegun)
                return;
            CurrentPlayer = FIRST_PLAYER_ID;
        }

        public string[,] GetGameField => GameField.GetGameFied();

        //This method only for real player moves
        public void MakeMove(int x, int y)
        {
            if (IsWinnerHere == true || IsWinnerHere == null)
                return;

            if (CurrentPlayer == FIRST_PLAYER_ID)
                GameField.FillField(x, y, true);
            else
                GameField.FillField(x, y, false);


            IsWinnerHere = CheckForWin(x, y);
            if (IsWinnerHere == false)
                ChangeCurrentPlayer();

            Notify();
        }

        private void ComputerMove()
        {
            string computerSymbol;
            string playerSymbol;
            if (firstPlayerIsAComputer)
            {
                computerSymbol = GameField.FirstPlayerSymbol;
                playerSymbol = GameField.SecondPlayerSymbol;
            }
            else
            {
                computerSymbol = GameField.SecondPlayerSymbol;
                playerSymbol = GameField.FirstPlayerSymbol;
            }

            var moveCoordinate = Algorithm.Algorithm(GetGameField, level, playUntil, computerSymbol,playerSymbol);
            MakeMove(moveCoordinate.Item1, moveCoordinate.Item2);
        }
        private void ChangeCurrentPlayer()
        {
            if (CurrentPlayer == FIRST_PLAYER_ID)
            {
                CurrentPlayer = SECOND_PLAYER_ID;
            }
            else
            {
                CurrentPlayer = FIRST_PLAYER_ID;
            }
        }

        //This method must be used after the move, but before the current player changes
        //true - current player win
        //false - current player didn't win
        //null - draw
        private bool? CheckForWin(int lastX, int lastY)
        {
            var gameField = GetGameField;
            string currentSymbol = gameField[lastX, lastY];
            string symbol;
            if (CurrentPlayer == FIRST_PLAYER_ID)
                symbol = GameField.FirstPlayerSymbol;
            else
                symbol = GameField.SecondPlayerSymbol;
            var maxSequence = GetMaxSequenceRelativeMoveMade(gameField, symbol, lastX, lastY);

            bool? result;
            if (maxSequence >= playUntil)
                result = true;
            else
            {
                bool isFieldComplete = GameField.IsTheFieldComplete();
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


        private GameField CreateNewGameField()
        {
            throw new NotImplementedException();
        }

        List<IObserver> Observers;
        public void AddObserver(IObserver o)
        {
            Observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            Observers.Remove(o);
        }

        public void Notify()
        {
            foreach (var curr in Observers)
                curr.Update();
        }
        public string InfoAboutWinner()
        {
            string result;
            if (IsWinnerHere == false)
                result = null;
            else if (IsWinnerHere == null)
                result = "Draw";
            else
            {
                if (firstPlayerIsAComputer && currentPlayer == FIRST_PLAYER_ID)
                    result = "You lose";
                else if (!firstPlayerIsAComputer && currentPlayer == SECOND_PLAYER_ID)
                    result = "You lose";
                else
                    result = "You win. Congratulations";
            }
            return result;
        }
    }
}
