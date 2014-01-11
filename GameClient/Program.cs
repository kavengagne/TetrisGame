using System;
using GameClient.Classes;


namespace GameClient 
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // ReSharper disable once UnusedVariable
            var application = Application.Instance;
            application.Run();
        }
    }
#endif
}
