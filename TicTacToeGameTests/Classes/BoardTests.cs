using TicTacToeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToeGame.Tests
{
    [TestClass()]
    public class BoardTests
    {
        [TestMethod()]
        public void CreateExactPositionWin()
        {
            Board board = Board.CreateExactPosition(new string[] {
                new string(' ', 5),
                " ooo ",
                " ooo ",
                " ooo ",
                " ooo ",
                new string(' ', 5)
                 }, new Point(1, 1));
            board[0, 0] = PlayerTypes.O;
            board[4, 4] = PlayerTypes.O;
            Assert.IsTrue(board.IsWon);
            Assert.IsFalse(board.IsDraw);
        }
    }
}