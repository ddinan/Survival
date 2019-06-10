namespace Survival.ActionBlocks
{
    public static class Door
    {
        //{} -> b_n, b_e, b_s, b_w, t_n, t_e, t_s, t_w
        public static void Builder(ushort[] b) {

            for (int i = 0; i < b.Length; i++) {
                ActionsBlocks.Init(b[i]);

                string condConstructor = "";
                for (int c = 0; c < b.Length; c++)
                {
                    condConstructor += Helpers.IDConvert2(b[c]);
                    if (c != b.Length - 1)
                    {
                        condConstructor += ",";
                    }
                }

                //Trigger 0------
                ActionsBlocks.AddTrigger(b[i],1); 

                int k = i % 2 == 0 ? 1 : -1;
                ActionsBlocks.AddAction(b[i], 0, 1, b[i + k] + " 0 0 0" ); //-> Action 0

                int j = i/4 <= 0.75 ? 4 : -4;
                int l = j == 4 ? 1 : -1;
                ActionsBlocks.AddAction(b[i], 0, 1, b[i + k + j] + " 0 " + l + " 0 0," + condConstructor); //-> Action 1
                //-----------------

                
                //Trigger 1------
                ActionsBlocks.AddTrigger(b[i], 3);

                int p = i / 4 <= 0.75 ? 1 : -1;
                ActionsBlocks.AddAction(b[i], 1, 1, "0 0 " + p + " 0 " + condConstructor); //-> Action 1
                //-----------------
            }
        }
    }
}
