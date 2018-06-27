using System;
using System.Text;

namespace CoreConsoleApp
{
    public class GridItem
    {
        public static GridItem Empty
        {
            get
            {
                return new GridItem('\0', ConsoleColor.Black);
            }
        }
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Character == '\0';
            }
        }

        public bool IsBlocking
        {
            get
            {
                return Character == 'X';
            }
        }

        public GridItem(char character, ConsoleColor color)
        {
            this.Character = character;
            this.Color = color;
        }
    }

    public class Grid
    {
        private GridItem[,] _grid;

        public int Width
        {
            get
            {
                return _grid.GetLength(0);
            }
        }
        public int Height
        {
            get
            {
                return _grid.GetLength(1);
            }
        }

        public GridItem this[int x, int y]
        {
            get
            {
                return _grid[x, y] ?? GridItem.Empty;
            }
            private set
            {
                _grid[x, y] = value;
            }
        }

        public Grid(int width, int height)
        {
            _grid = new GridItem[width, height];

            // Test
            _grid[7, 15] = new GridItem('X', ConsoleColor.Red);
            _grid[7, 14] = new GridItem('X', ConsoleColor.Red);
            _grid[7, 13] = new GridItem('X', ConsoleColor.Red);
            _grid[7, 12] = new GridItem('X', ConsoleColor.Red);
            _grid[7, 11] = new GridItem('X', ConsoleColor.Red);
            _grid[7, 10] = new GridItem('X', ConsoleColor.Red);
            _grid[8, 15] = new GridItem('2', ConsoleColor.Cyan);
            _grid[9, 15] = new GridItem('3', ConsoleColor.Cyan);
            _grid[10, 15] = new GridItem('+', ConsoleColor.Cyan);
            _grid[11, 15] = new GridItem('5', ConsoleColor.Cyan);
            _grid[11, 14] = new GridItem('6', ConsoleColor.Cyan);
            _grid[11, 13] = new GridItem('7', ConsoleColor.Cyan);
            _grid[11, 12] = new GridItem('8', ConsoleColor.Cyan);
        }

        public bool CanMove(Coordinate coordinate, Coordinate direction)
        {
            var nextCoordinate = coordinate + direction;
            nextCoordinate.Wrap(this.Width, this.Height);

            if(this[nextCoordinate.X, nextCoordinate.Y].IsEmpty)
            {
                return true;
            }
            else if(this[nextCoordinate.X, nextCoordinate.Y].IsBlocking)
            {
                return false;
            }
            else
            {
                return CanMove(nextCoordinate, direction);
            }
        }

        private void ExecuteMove(Coordinate coordinate, Coordinate direction)
        {
            var nextCoordinate = coordinate + direction;
            nextCoordinate.Wrap(this.Width, this.Height);

            if (!this[nextCoordinate.X, nextCoordinate.Y].IsEmpty)
            {
                ExecuteMove(nextCoordinate, direction);
            }

            this[nextCoordinate.X, nextCoordinate.Y] = this[coordinate.X, coordinate.Y];
            this[coordinate.X, coordinate.Y] = GridItem.Empty;
        }

        public void Move(Coordinate coordinate, Coordinate direction)
        {
            if(CanMove(coordinate, direction))
            {
                ExecuteMove(coordinate, direction);
            }
        }
    }

    public struct Coordinate
    {
        public static Coordinate Up
        {
            get
            {
                return new Coordinate(0, -1);
            }
        }

        public static Coordinate Down
        {
            get
            {
                return new Coordinate(0, 1);
            }
        }

        public static Coordinate Left
        {
            get
            {
                return new Coordinate(-1, 0);
            }
        }

        public static Coordinate Right
        {
            get
            {
                return new Coordinate(1, 0);
            }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static Coordinate operator +(Coordinate left, Coordinate right)
        {
            return new Coordinate { X = left.X + right.X, Y = left.Y + right.Y };
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Wrap(int width, int height)
        {
            X = X >= width ? 0 : X;
            X = X < 0 ? width - 1 : X;

            Y = Y >= height ? 0 : Y;
            Y = Y < 0 ? height - 1 : Y;
        }
    }
    class Program
    {
        public const int _xOffset = 5;
        public const int _yOffset = 5;
        public const int _width = 20;
        public const int _height = 20;

        static void Main(string[] args)
        {
            Console.SetWindowSize(_width + 2 * _xOffset, _height + 2 * _yOffset);
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;

            var color = ConsoleColor.Cyan;
            var grid = new Grid(_width, _height);


            var position = new Coordinate(0, 0);

            while (true)
            {
                DrawRectangle(_xOffset - 1, _yOffset - 1, _width, _height, color);
                DrawGrid(grid);
                DrawChar(position.X, position.Y, 'ȯ', color);

                var key = Console.ReadKey(true);
                Coordinate? direction = null;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        direction = Coordinate.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Coordinate.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Coordinate.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Coordinate.Right;
                        break;
                }


                if (direction.HasValue && grid.CanMove(position, direction.Value))
                {
                    grid.Move(position, direction.Value);
                    position += direction.Value;
                }

                position.Wrap(_width, _height);
            }
        }

        public static void DrawGrid(Grid grid)
        {
            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    DrawChar(i, j, grid[i, j].Character, grid[i, j].Color);
                }
            }
        }

        public static void DrawChar(int x, int y, char character, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.CursorTop = y + _yOffset;
            Console.CursorLeft = x + _xOffset;

            Console.Write(character);
            Console.ResetColor();
        }
        public static void DrawRectangle(int x, int y, int width, int height, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.CursorTop = y;
            Console.CursorLeft = x;

            string line = "╔";
            for (int i = 0; i < width; i++)
            {
                line += "═";
            }
            line += "╗";

            Console.Write(line);

            for (int i = 0; i < height; i++)
            {
                Console.CursorTop = y + i + 1;
                Console.CursorLeft = x;

                Console.Write("║");
                Console.CursorLeft = x + 1 + width;
                Console.Write("║");
            }
            Console.CursorTop = y + height + 1;
            Console.CursorLeft = x;

            line = "╚";
            for (int i = 0; i < width; i++)
                line += "═";

            line += "╝";
            Console.Write(line);

            Console.ResetColor();
        }

    }
}
