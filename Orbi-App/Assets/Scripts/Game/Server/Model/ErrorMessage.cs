using System;

namespace ServerModel
{
    [Serializable]
    class ErrorMessage: AbstractModel
    {
        public String message = "Unknown error";
        public long status = 1;


    }
}
