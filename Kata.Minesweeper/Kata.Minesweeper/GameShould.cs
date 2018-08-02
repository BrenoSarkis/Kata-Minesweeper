using NUnit.Framework;

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
        }
    }

    public class Minesweeper
    {
        public int[,] Squares = new int[4,3];
    }
}
