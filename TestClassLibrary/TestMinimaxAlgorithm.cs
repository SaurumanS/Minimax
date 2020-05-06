using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Minimax.Algorithm;
using System.Collections;

namespace TestClassLibrary
{
    [TestFixture]
    public class TestMinimaxAlgorithm
    {
        static string computerGameSymbol = "X";
        static string playerGameSymbol = "0";
        [Test]
        [TestCaseSource("CheckForBreakingTheOpponentSequenceCollection")]
        public (int,int) CheckForBreakingTheOpponentSequence(IAlgorithm algorithm, string[,] gameField, int maxDeph, int playUntil)
        {
            return algorithm.Algorithm(gameField, maxDeph, playUntil, computerGameSymbol, playerGameSymbol);
        }
        public static IEnumerable CheckForBreakingTheOpponentSequenceCollection()
        {
            int playUntil = 3;
            int maxDeph = 1;
            IAlgorithm algorithm = new MinimaxAlgorithm();
            string[,] gameField = new string[,]
                {
                    {playerGameSymbol,null,playerGameSymbol },
                    {null,null,null },
                    {null,null,null }
                };
            yield return new TestCaseData(algorithm, gameField, playUntil, maxDeph).Returns((0,1));

            gameField = new string[,]
                {
                    {playerGameSymbol,null, null},
                    {null,null,null },
                    {null,null,playerGameSymbol }
                };
            yield return new TestCaseData(algorithm, gameField, playUntil, maxDeph).Returns((1, 1));
        }
    }
}
