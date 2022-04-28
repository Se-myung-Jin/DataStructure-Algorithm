using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Board
    {
        public TileType[,] Tile { get; set; }
        public int Size { get; private set; }
        const char CIRCLE = '\u25cf';

        public int DesY { get; private set; }
        public int DesX { get; private set; }

        Player _player;

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size, Player player)
        {
            if (size % 2 == 0)
                return;

            Tile = new TileType[size, size];
            Size = size;

            _player = player;

            DesX = size - 2;
            DesY = size - 2;

            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }

        void GenerateBySideWinder()
        {
            // Mazes for Programmers

            // 길을 모두 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        Tile[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tile[y, x] = TileType.Empty;
                    }
                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            // Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (x == Size - 2 && y == Size - 2)
                        continue;

                    // 무조건 아래로 
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 무조건 오른쪽으로
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIdx = rand.Next(0, count);
                        Tile[y + 1, x - randomIdx * 2] = TileType.Empty;
                        count = 1;
                    }
                }
            }
        }

        void GenerateByBinaryTree()
        {
            // Mazes for Programmers

            // 길을 모두 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        Tile[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tile[y, x] = TileType.Empty;
                    }
                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            // Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (x == Size - 2 && y == Size - 2)
                        continue;

                    // 무조건 아래로 
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 무조건 오른쪽으로
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 플레이어 좌표 분기
                    if (y == _player.PosY && x == _player.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (y == DesY && x == DesX)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    }
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Wall:
                    return ConsoleColor.Red;
                case TileType.Empty:
                    return ConsoleColor.Green;
                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
