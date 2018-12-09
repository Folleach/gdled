using GeometryDashAPI.Memory;
using System;
using System.Diagnostics;
using System.Threading;

namespace gdled
{
    class ProgresReader
    {
        public delegate void Progres(int procentage);
        public event Progres ProgresChanged;

        Thread Checker;

        GameProcess Game = new GameProcess();
        ProcessModule Module;

        public int RefreshDelay { get; }

        public ProgresReader(int delay)
        {
            RefreshDelay = delay;
            Game.Initialize(Access.PROCESS_VM_READ);
            Module = Game.GetModule("GeometryDash.exe");
            Checker = new Thread(Check)
            {
                IsBackground = false
            };
            Checker.Start();
        }
        
        void Check()
        {
            int oldProgres = 0;
            while (true)
            {
                IntPtr ptr = IntPtr.Add(Game.Read<IntPtr>(Module, new[] { 0x003222D0, 0x164, 0x124, 0xB4, 0x3C0 }), 0x12C);
                string readed = Game.ReadString((int)ptr, 4);
                if (!readed.Contains("%"))
                {
                    oldProgres = -1;
                    Thread.Sleep(150);
                    continue;
                }
                int curProgres = int.Parse(readed.Split('%')[0]);
                if (curProgres != oldProgres)
                {
                    ProgresChanged?.Invoke(curProgres);
                    oldProgres = curProgres;
                }
                Thread.Sleep(RefreshDelay);
            }
        }
    }
}
