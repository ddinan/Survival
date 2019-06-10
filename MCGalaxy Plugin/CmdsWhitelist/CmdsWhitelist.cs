using System.Collections.Generic;

namespace Survival.CmdsWhitelist
{
    internal class CmdsWhitelist
    {
        public static List<string> Commands = new List<string>();
        private static string _CmdLocation = "./text/survivalPlugin/CmdWhiteList.txt";

        internal static void Add(string Cmd)
        {
            Commands.Add(Cmd);
            Save();
        }

        internal static void Remove(string Cmd)
        {
            int index = Commands.FindIndex(i => i == Cmd);
            if (index == -1) return;

            Commands.RemoveAt(index);
            Save();
        }

        private static void Save()
        {
            System.IO.FileInfo filedir = new System.IO.FileInfo(_CmdLocation);
            filedir.Directory.Create();
            System.IO.File.WriteAllText(_CmdLocation, Helpers.Serialize<List<string>>(Commands));
        }

        internal static void Load()
        {

            if (System.IO.File.Exists(_CmdLocation))
            {
                string json = System.IO.File.ReadAllText(_CmdLocation);
                Commands = Helpers.Deserialize<List<string>>(json);
            }

        }

    }
}
