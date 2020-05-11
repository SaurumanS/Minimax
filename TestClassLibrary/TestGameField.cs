using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Minimax.GameLogic;

namespace TestClassLibrary
{
    [TestFixture]
    class TestGameField
    {
        [Test]
        public void TestCheckOnComplete()
        {
            var gameField = TicTacToeGameFieldBuilder.Create(3);
            gameField.FillField(0, 0, true);
            gameField.FillField(0, 1, true);
            gameField.FillField(0, 2, true);
            gameField.FillField(1, 0, true);
            gameField.FillField(1, 1, true);
            gameField.FillField(1, 2, true);
            gameField.FillField(2, 0, true);
            gameField.FillField(2, 1, true);
            gameField.FillField(2, 2, true);

            bool expect = true;
            bool actual = gameField.IsTheFieldComplete();

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void TestCheckNullField()
        {
            var gameField = TicTacToeGameFieldBuilder.Create(3);
            var actual = gameField.CheckField(0, 0);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestCheckNotNullField()
        {
            var gameField = TicTacToeGameFieldBuilder.Create(3);
            gameField.FillField(0, 0, true);
            var actual = gameField.CheckField(0, 0);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }
    }
}
