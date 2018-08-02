using NUnit.Framework;
using System.Linq;

namespace Kata.Minesweeper
{
    [TestFixture]
    public class GameShould
    {
        [Test]
        public void Have12Squares()
        {
            var game = new Minesweeper();

            Assert.That(game.Squares.Length, Is.EqualTo(12));
            Assert.That(game.Squares.GetLength(0), Is.EqualTo(4));
            Assert.That(game.Squares.GetLength(1), Is.EqualTo(3));
        }

        [Test]
        public void ContainTwoMines()
        {
            var game = new Minesweeper();

            Assert.That(game.MinesLeft(), Is.EqualTo(2));
        }
    }

    public class Minesweeper
    {
        public string[,] Squares = new string[4,3];

        public Minesweeper()
        {
            Squares[1, 2] = "*";
            Squares[0, 0] = "*";
        }

        public int MinesLeft()
        {
            int minesCount = 0;

            foreach (var square in Squares)
            {
                if (square == "*")
                {
                    minesCount++;
                }
            }

            return minesCount;
        }
    }
}
