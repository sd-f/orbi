using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Model
{
    [Serializable]
    class ErrorMessage
    {
        public String message;
        public long status = 1;
    }
}
