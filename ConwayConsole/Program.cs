using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;

namespace ConwayConsole
{
    public class Program
    {
        private const int BoardWidth = 100;
        private const int BoardHeight = 50;
        private const int WindowWidth = BoardWidth;
        private const int WindowHeight = BoardHeight + 2;
        private const int StepIntervalMilliseconds = 50;
        private const char CellDisplay = '█';

        private static readonly ImmutableList<(int X, int Y)> InitialCells = new List<(int X, int Y)>
        {
            { (49, 25) },
            { (50, 25) },
            { (51, 25) },
            { (50, 24) },
            { (50, 26) },
            { (53, 25) },
            { (54, 25) },
            { (55, 27) },
            { (54, 24) },
            { (54, 26) }
        }.ToImmutableList();

        private static bool[,] Board = new bool[BoardHeight, BoardWidth];

        public static void Main()
        {
            InitWindow();
            InitBoard();
            while (true)
            {
                DrawBoard();
                Thread.Sleep(StepIntervalMilliseconds);
                Next();
            }
        }

        private static void Next()
        {
            var newBoard = new bool[BoardHeight, BoardWidth];
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    var neighbours = CountNeighbours(j, i);
                    newBoard[i, j] = neighbours == 3 || (Board[i, j] && neighbours == 2);
                }
            }
            Board = newBoard;
        }

        private static int CountNeighbours(int x, int y)
        {
            return
                IsInBoardAndAlive(x - 1, y - 1) +
                IsInBoardAndAlive(x - 1, y) +
                IsInBoardAndAlive(x - 1, y + 1) +
                IsInBoardAndAlive(x, y - 1) +
                IsInBoardAndAlive(x, y + 1) +
                IsInBoardAndAlive(x + 1, y - 1) +
                IsInBoardAndAlive(x + 1, y) +
                IsInBoardAndAlive(x + 1, y + 1);
        }

        private static int IsInBoardAndAlive(int x, int y)
        {
            return x >= 0 && x < BoardWidth && y >= 0 && y < BoardHeight && Board[y, x]
                ? 1
                : 0;
        }

        private static void InitWindow()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;
        }

        private static void InitBoard()
        {
            foreach (var (x, y) in InitialCells)
            {
                Board[y, x] = true;
            }
        }

        private static void DrawBoard()
        {
            var toDisplay = new StringBuilder();
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    toDisplay.Append(Board[i, j] ? CellDisplay : ' ');
                }
                toDisplay.AppendLine();
            }
            Console.Clear();
            Console.WriteLine(toDisplay.ToString());
        }
    }
}
