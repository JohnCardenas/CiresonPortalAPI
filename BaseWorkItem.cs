using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    public abstract class BaseWorkItem
    {
        internal  bool _bObjectInitialized = false;
        protected bool _bDirtyObject = false;

        internal protected bool _bExistingObject;

        protected void SetDirtyBit()
        {
            if (_bObjectInitialized)
                _bDirtyObject = true;
        }
    }
}
