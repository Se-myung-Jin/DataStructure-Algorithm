﻿using System;

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
    }
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.BFS(0);
        }
    }
}