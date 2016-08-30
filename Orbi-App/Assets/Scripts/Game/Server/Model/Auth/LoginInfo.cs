using System;

namespace ServerModel
{
    [Serializable]
    class LoginInfo
    {
        public String email;
        public String password;
        public Player player;
    }
}
