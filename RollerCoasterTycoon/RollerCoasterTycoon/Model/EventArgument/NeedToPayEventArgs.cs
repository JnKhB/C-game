using System;
using System.Collections.Generic;
using System.Text;

namespace RollerCoasterTycoon.Model.EventArgument
{
    public class NeedToPayEventArgs : EventArgs
    {
        public Int32 Fee;
        public NeedToPayEventArgs(Int32 Fee)
        {
            this.Fee = Fee;
        }
    }
}
