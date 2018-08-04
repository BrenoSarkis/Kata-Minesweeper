using System;
using System.Drawing;
using NUnit.Framework;
using System.Linq;

namespace Kata.Minesweeper
{
    [TestFixture]
    public class MinesweeperTests
    {
        private MinePlanterMock minePlanter;
        [SetUp]
        public void SetUp()
        {
            minePlanter = new MinePlanterMock();
        }

        [Test]
        public void FieldIsSized4x3()
        {
            GivenMinesAt(minePlanter, new MineLocation(0, 0), new MineLocation(1, 1));
            var game = new Minesweeper(minePlanter);

            Assert.That(game.Squares.Length, Is.EqualTo(12));
            Assert.That(game.Squares.GetLength(0), Is.EqualTo(4));
            Assert.That(game.Squares.GetLength(1), Is.EqualTo(3));
        }

        [Test]
        public void FieldHasTwoMines()
        {
            GivenMinesAt(minePlanter, new MineLocation(0, 0), new MineLocation(1, 1));
            var game = new Minesweeper(minePlanter);

            Assert.That(game.MinesLeft(), Is.EqualTo(2));
        }

        private void GivenMinesAt(MinePlanterMock minePlanter, params MineLocation[] mines)
        {
            minePlanter.Locations = mines;
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
            return Locations;
        }
    }

    public interface MinePlanter
    {
        MineLocation[] GetLocations();
    }

    public class Minesweeper
    {
        public string[,] Squares = new string[4,3];

        public Minesweeper(MinePlanter minePlanter)
        {
            PlantMines(minePlanter.GetLocations());
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
