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
            //BFS();
            AStar();
        }

        class PriorityQueueNode : IComparable<PriorityQueueNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PriorityQueueNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;
            }
        }
        public void AStar()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1  };
            int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            // 점수 매기기
            // F (최종 점수) = G(시작점에서 해당 좌표까지 드는 비용) + H(목적지에서 얼마나 가까운지)

            // (y, x) 이미 방문했는지 여부(방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size];

            // (y, x) 가는 길을 한 번이라도 발견했는지
            // 발견X => MaxValue , 발견 F = G + H 
            int[,] open = new int[_board.Size, _board.Size];
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            // 오픈 리스트에 있는 노드 중 가장 좋은 후보를 뽑기 위한 도구
            PriorityQueue<PriorityQueueNode> pq = new PriorityQueue<PriorityQueueNode>();
            
            // 시작점 발견
            open[PosY, PosX] = 10 * (Math.Abs(_board.DesY - PosY) + Math.Abs(_board.DesX - PosX));
            pq.Push(new PriorityQueueNode() { F = 10 * (Math.Abs(_board.DesY - PosY) + Math.Abs(_board.DesX - PosX)), G = 0, Y = PosY, X = PosX});
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count() > 0)
            {
                // 제일 좋은 후보를 찾는다
                PriorityQueueNode pqNode = pq.Pop();
                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해 이미 방문(closed)된 경우 스킵
                if (closed[pqNode.Y, pqNode.X])
                    continue;

                // 방문한다
                closed[pqNode.Y, pqNode.X] = true;
                // 목적지 도착하면 종료
                if (pqNode.Y == _board.DesY && pqNode.X == _board.DesX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인 후 예약(open)한다
                for (int i = 0; i < deltaY.Length; ++i)
                {
                    int nextY = pqNode.Y + deltaY[i];
                    int nextX = pqNode.X + deltaX[i];

                    // 유효 범위 조건 검사
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // 벽 조건 검사
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 방문했는지 검사
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = pqNode.G + cost[i];
                    int h = 10 * (Math.Abs(_board.DesY - nextY) + Math.Abs(_board.DesX - nextX));
                    // 다른 경로에서 더 빠른 길 이미 찾았는지 검사
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PriorityQueueNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(pqNode.Y, pqNode.X);
                }
            }

            CalcPathFromParent(parent);
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

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {
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
            {
                lastIndex = 0;
                points.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);
            }

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
