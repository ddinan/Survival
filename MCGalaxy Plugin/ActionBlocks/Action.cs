namespace Survival.ActionBlocks
{
    public class Action
    {
        public byte ActionTypeID { get; set; }
        public string args { get; set; }

        public Action(byte AT_ID,string A_args)
        {
            ActionTypeID = AT_ID;
            args = A_args;
        }

        public Action()
        {
            ActionTypeID = 0;
            args = "";
        }
    }
}
