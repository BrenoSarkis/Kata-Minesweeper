using System;
using System.Drawing;
using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Kata.Minesweeper
{
    [TestFixture]
    public class MinesweeperTests
    {
        [Test]
        public void FieldIsSized4x3()
        {
            var game = GivenGameWithMinesAt();

            game.Start();

            Assert.That(game.Squares.Length, Is.EqualTo(12));
            Assert.That(game.Squares.GetLength(0), Is.EqualTo(4));
            Assert.That(game.Squares.GetLength(1), Is.EqualTo(3));
        }

        [Test]
        public void FieldHasTwoMines()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0), new MineLocation(1, 1));

            game.Start();

            Assert.That(game.MinesLeft(), Is.EqualTo(2));
            Assert.That(game.Squares[0, 0], Is.EqualTo("*"));
            Assert.That(game.Squares[1, 1], Is.EqualTo("*"));
        }

        [Test]
        public void SquareIdentifiesLeftAdjacentMine()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0));

            game.Start();

            Assert.That(game.Squares[0, 1], Is.EqualTo("1"));
        }

        private Minesweeper GivenGameWithMinesAt(params MineLocation[] minesLocation)
        {
            return new Minesweeper(GivenMinesAt(minesLocation));
        }

        private MinePlanterMock GivenMinesAt(params MineLocation[] mines)
        {
            return new MinePlanterMock {Locations = mines};
        }
    }

    public class MineLocation
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MineLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class MinePlanterMock : MinePlanter
    {
        public MineLocation[] Locations { get; set; }

        public MineLocation[] GetLocations()
        {
            return Locations ?? new MineLocation[0];
        }
    }

    public interface MinePlanter
    {
        MineLocation[] GetLocations();
    }

    public class Minesweeper
    {
        public string[,] Squares = new string[4,3];
        private MinePlanter minePlanter;

        public Minesweeper(MinePlanter minePlanter)
        {
            this.minePlanter = minePlanter;
        }

        public void Start()
        {
            PlantMines(minePlanter.GetLocations());

            Squares[0, 1] = "1";
        }

        private void PlantMines(MineLocation[] locations)
        {
            foreach (var mineLocation in locations)
            {
                Squares[mineLocation.X, mineLocation.Y] = "*";
            }
        }

        public int MinesLeft()
        {
            return Squares.Cast<string>().Count(square => square == "*");
        }
    }
}
