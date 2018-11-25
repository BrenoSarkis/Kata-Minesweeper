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
        public void FieldIsSized4X3()
        {
            var game = GivenGameWithMinesAt();

            game.Start();

            Assert.That(game.Squares.Length, Is.EqualTo(16));
            Assert.That(game.Squares.GetLength(0), Is.EqualTo(4));
            Assert.That(game.Squares.GetLength(1), Is.EqualTo(4));
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
        public void WhenNoMineIsNearSquareIndicatesZero()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0));

            game.Start();

            Assert.That(game.Squares[0, 2], Is.EqualTo("0"));
        }

        [Test]
        public void WhenTheresAnAdjacentMineSquareIndicatesOne()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0));

            game.Start();

            Assert.That(game.Squares[0, 1], Is.EqualTo("1"));
        }

        [Test]
        public void WhenTheresTwoAdjacentMineSquareIndicatesTwo()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0), new MineLocation(1, 0));

            game.Start();

            Assert.That(game.Squares[0, 1], Is.EqualTo("2"));
        }

        [Test]
        public void CalculatesFullField()
        {
            var game = GivenGameWithMinesAt(new MineLocation(0, 0), new MineLocation(2, 1));

            game.Start();

            StringAssert.AreEqualIgnoringCase("* 1 0 0 \r\n" +
                                              "2 2 1 0 \r\n" +
                                              "1 * 1 0 \r\n" +
                                              "1 1 1 0 \r\n", game.PrintField());
        }

        private Minesweeper GivenGameWithMinesAt(params MineLocation[] minesLocation)
        {
            return new Minesweeper(GivenMinesAt(minesLocation));
        }

        private MinePlanterMock GivenMinesAt(params MineLocation[] mines)
        {
            return new MinePlanterMock { Locations = mines };
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
        public string[,] Squares = new string[4, 4];
        private readonly MinePlanter minePlanter;

        public Minesweeper(MinePlanter minePlanter)
        {
            this.minePlanter = minePlanter;
        }

        public void Start()
        {
            InitializeFieldWithZeroes();

            var mineLocations = minePlanter.GetLocations();
            PlantMines(mineLocations);

            CalculateAdjacentMines(mineLocations);
        }

        private void CalculateAdjacentMines(MineLocation[] locations)
        {
            foreach (var mineLocation in locations)
            {
                for (int x = mineLocation.X - 1; x <= mineLocation.X + 1; x++)
                {
                    for (int y = mineLocation.Y - 1; y <= mineLocation.Y + 1; y++)
                    {
                        if (x >= 0 && y >= 0)
                        {
                            if (Squares[x, y] == "*") continue;

                            IncreaseNearbyMineCount(x, y);
                        }
                    }
                }
            }
        }

        private void IncreaseNearbyMineCount(int x, int y)
        {
            Squares[x, y] = (int.Parse(Squares[x, y]) + 1).ToString();
        }

        private void InitializeFieldWithZeroes()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    Squares[i, j] = "0";
                }
            }
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

        public string PrintField()
        {
            string field = "";

            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    field += string.Format("{0} ", Squares[i, j]);
                }

                field += "\r\n";
            }
            return field;
        }
    }
}
