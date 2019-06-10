using MCGalaxy;
using System.Collections.Generic;

namespace Survival
{
    public class CmdsPermissions
    {
        private const int _permsNb = 6;
        private static List<LevelPermission> _perms = new List<LevelPermission>();
        private static string _permissionsLocation = "./text/survivalPlugin/perms.txt";

        public static void set(int i, int perm) {
            _perms[i] = (LevelPermission)perm;
            Save();
        }

        public static LevelPermission get(int i){
            return _perms[i];
        }

        private static void Save()
        {
            System.IO.FileInfo filedir = new System.IO.FileInfo(_permissionsLocation);
            filedir.Directory.Create();
            System.IO.File.WriteAllText(_permissionsLocation, Helpers.Serialize<List<LevelPermission>>(_perms));
        }

        internal static void Load()
        {

            if (System.IO.File.Exists(_permissionsLocation))
            {
                string json = System.IO.File.ReadAllText(_permissionsLocation);
                _perms = Helpers.Deserialize<List<LevelPermission>>(json);
            }else
            {
                for (int i = 0; i < _permsNb; i++) {
                    _perms.Add(LevelPermission.Operator);
                }
                Save();
            }

        }
    }
}
