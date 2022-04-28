using System;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);
            player.Initialize(1, 1, board);

            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;

            int last_tick = 0;

            while (true)
            {
                #region 프레임관리
                int currentTick = System.Environment.TickCount;
                if (currentTick - last_tick < WAIT_TICK)
                    continue;
                int deltaTick = currentTick - last_tick;
                last_tick = currentTick;
                #endregion

                // 입력

                // 로직
                player.Update(deltaTick);
                // 렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}