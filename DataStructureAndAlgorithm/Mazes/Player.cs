using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Pos
    {
        public Pos(int y, int x) {  Y= y; X = x; }
        public int Y;
        public int X;
    }
    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        Random rand = new Random();
        Board _board;

        enum Dir
        {
            Up = 0,
            Left,
            Down,
            Right,
        }

        int dir = (int)Dir.Up;
        List<Pos> points = new List<Pos>();

        public void Initialize(int PosX, int PosY, Board board)
        {
            this.PosX = PosX;
            this.PosY = PosY;

            _board = board;

            //RightHand();
            BFS();
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_board.Size, _board.Size];
            // 경로 기록
            Pos[,] parent = new Pos[_board.Size, _board.Size];


            Queue<Pos> queue = new Queue<Pos>();
            queue.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (queue.Count > 0)
            {
                Pos pos = queue.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                for (int i = 0; i < 4; ++i)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    queue.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            int destY = _board.DesY;
            int destX = _board.DesX;
            while (parent[destY, destX].Y != destY || parent[destY, destX].X != destX)
            {
                points.Add(new Pos(destY, destX));
                Pos pos = parent[destY, destX];
                destY = pos.Y;
                destX = pos.X;
            }
            // 시작점 추가
            points.Add(new Pos(destY, destX));
            points.Reverse();
        }

        void RightHand()
        {
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            points.Add(new Pos(PosY, PosX));
            while (PosX != _board.DesX || PosY != _board.DesY)
            {
                // 1. 현재 바라보는 방향을 기준으로 오른쪽 확인
                if (_board.Tile[PosY + rightY[dir], PosX + rightX[dir]] == Board.TileType.Empty)
                {
                    // 오른쪽 회전
                    dir = (dir - 1 + 4) % 4;

                    // 일보 전진
                    PosY = PosY + frontY[dir];
                    PosX = PosX + frontX[dir];
                    points.Add(new Pos(PosY, PosX));
                }
                // 2. 현재 바라보는 방향을 기준으로 전진 확인
                else if (_board.Tile[PosY + frontY[dir], PosX + frontX[dir]] == Board.TileType.Empty)
                {
                    PosY = PosY + frontY[dir];
                    PosX = PosX + frontX[dir];
                    points.Add(new Pos(PosY, PosX));
                }
                // 3. 현재 바라보는 방향을 기준으로 왼쪽 회전
                else
                {
                    // 왼쪽 회전
                    dir = (dir + 1 + 4) % 4;
                }
            }
        }

        const int MOVE_TICK = 100;
        int _sumTick = 0;
        int lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (lastIndex >= points.Count)
                return;

            _sumTick += deltaTick;
            if (_sumTick > MOVE_TICK)
            {
                _sumTick = 0;

                PosY = points[lastIndex].Y;
                PosX = points[lastIndex].X;

                lastIndex++;
            }
        }
    }
}
