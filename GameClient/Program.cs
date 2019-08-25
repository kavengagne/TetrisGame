using GameClient.Classes;
using System;

namespace GameClient
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.GetInstance().Run();
        }
    }
#endif
}
