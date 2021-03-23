using BattleShipLibrary.GameInit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

// Pattern of unit tests: 
// Arrange, Act, Assert

namespace BattleShip.Tests
{
    public class GameInitTests
    {
        [Fact]
        public void PopulateShotsFired_ShouldReturnGridOfFalseBools()
        {
            // Arrange
            GameInit gameInit = new GameInit();
            bool[,] expected = new bool[10, 10];

            // Act
            bool[,] actual = gameInit.PopulateShotsFired();

            // Assert
            Assert.Equal(expected, actual);

        }
    }


}
