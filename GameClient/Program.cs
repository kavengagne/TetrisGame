using System;
using System.Diagnostics;
using System.IO;
using GameClient.Classes;


namespace GameClient 
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.GetInstance().Run();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error_log.txt", ex.ToString());
                Process.Start("error_log.txt");
            }
        }
    }
#endif
}
