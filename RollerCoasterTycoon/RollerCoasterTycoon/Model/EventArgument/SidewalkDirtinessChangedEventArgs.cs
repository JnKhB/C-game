using System;
using System.Collections.Generic;
using System.Text;

namespace RollerCoasterTycoon.Model.EventArgument {
    public class SidewalkDirtinessChangedEventArgs: EventArgs {

        public AmusementPark.Position Position { get; private set; }
        public bool IsDirty { get; private set; }
        public SidewalkDirtinessChangedEventArgs(AmusementPark.Position pos, bool isDirty) {
            Position = pos;
            IsDirty = isDirty;
        }
    }
}
