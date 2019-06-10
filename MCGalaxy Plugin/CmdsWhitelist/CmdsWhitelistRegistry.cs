namespace Survival
{
    internal class CmdsWhitelistRegistry
    {
        public static void Add(string cmd)
        {

            CmdsWhitelist.CmdsWhitelist.Add(cmd);
        }

        public static void Remove(string cmd)
        {
            CmdsWhitelist.CmdsWhitelist.Remove(cmd);
        }

        public static string getListString()
        {
            string list = "%f";
            foreach (string cmd in CmdsWhitelist.CmdsWhitelist.Commands)
            {
                list += cmd + "\n%f";
            }
            return list;
        }

        internal static void Load()
        {
            CmdsWhitelist.CmdsWhitelist.Load();
        }
    }
}

