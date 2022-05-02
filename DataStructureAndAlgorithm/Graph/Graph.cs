using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        int[,] adj = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 1 },
            { 0, 0, 0, 0, 1, 0 },
        };

        List<int>[] list = new List<int>[]
        {
            new List<int>(){ 1, 3 },
            new List<int>(){ 0, 2, 3 },
            new List<int>(){ 1 },
            new List<int>(){ 0, 1, 4 },
            new List<int>(){ 3, 5 },
            new List<int>(){ 4 },
        };

        bool[] visited = new bool[6];
        public void DFS(int now)
        {
            Console.WriteLine(now);
            // 방문
            visited[now] = true;

            for (int next = 0; next < 6; ++next)
            {
                // 연결되어있지 않으면 건너뛰기
                if (adj[now, next] == 0)
                    continue;

                // 이미 방문한 노드면 건너뛰기
                if (visited[next])
                    continue;

                DFS(next);
            }
        }

        public void DFS2(int now)
        {
            Console.WriteLine(now);
            // 방문
            visited[now] = true;

            foreach (int next in list[now])
            {
                if (visited[next])
                    continue;

                DFS2(next);
            }
        }

        public void SearchAll()
        {
            visited = new bool[6];
            for (int next = 0; next < 6; ++next)
                if (visited[next] == false)
                    DFS2(next);
        }

        public void BFS(int now)
        {
            bool[] visited = new bool[6];
            int[] parent = new int[6];
            int[] distance = new int[6];

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(now);
            visited[now] = true;
            parent[now] = now;
            distance[now] = 0;

            while (queue.Count > 0)
            {
                int cur = queue.Dequeue();
                Console.WriteLine(cur);

                for (int next = 0; next < 6; ++next)
                {
                    if (adj[cur, next] == 0)
                        continue;
                    if (visited[next])
                        continue;
                    queue.Enqueue(next);
                    visited[next] = true;
                    parent[next] = cur;
                    distance[next] = distance[cur] + 1;
                }
            }
        }

        int[,] adj2 = new int[6, 6]
        {
            { -1, 15, -1, 35, -1, -1 },
            { 15, -1, 5, 10, -1, -1 },
            { -1, 5, -1, -1, -1, -1 },
            { 35, 10, -1, -1, 5, -1 },
            { -1, -1, -1, 5, -1, 5 },
            { -1, -1, -1, -1, 5, -1 },
        };

        public void Dijkstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = new int[6];
            int[] parent = new int[6];
            Array.Fill(distance, Int32.MaxValue);

            distance[start] = 0;
            parent[start] = start;

            while (true)
            {
                // 제일 좋은 후보를 찾는다

                // 가장 유력한 후보의 거리와 번호 저장
                int closest = Int32.MaxValue;
                int now = -1;

                for (int i = 0; i < 6; ++i)
                {
                    // 이미 방문한 정점은 스킵
                    if (visited[i])
                        continue;
                    // 아직 발견된 적이 없거나, 기존 후보보다 멀리 있으면 스킵
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                        continue;

                    closest = distance[i];
                    now = i;
                }

                // 다음 후보가 하나도 없다
                if (now == -1)
                    break;

                // 제일 좋은 후보를 찾아서 방문
                visited[now] = true;

                // 방문한 정점을 통해 최단거리 갱신
                for (int next = 0; next < 6; ++next)
                {
                    if (adj2[now, next] == -1)
                        continue;

                    if (visited[next])
                        continue;

                    // 새로 조사된 최단거리 계산
                    int newDist = distance[now] + adj2[now, next];
                    // 새로 조사된 최단거리가 작다면 갱신
                    if (newDist < distance[next])
                    {
                        distance[next] = newDist;
                        parent[next] = now;
                    }
                }
            }
        }
    }
}
