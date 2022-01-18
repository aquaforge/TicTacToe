﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Tests
{
    [TestClass()]
    public class PlayerMinMaxTests
    {
        [TestMethod()]
        public void GetNextMovementTest()
        {
            Board board = Board.CreateExactPosition(new string[] {
                " oooo",
                new string(' ', 5),
                 }, new Point(1, 1));

            IPlayerAI playerMinMax = new PlayerMinMax();

            PlayerTypes player = PlayerTypes.O;
            Point p = playerMinMax.GetNextMovement(board, player);
            Assert.AreEqual<Point>(p, new Point(0, 0));
        }
    }
}