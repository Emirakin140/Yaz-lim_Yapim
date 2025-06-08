using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public static class CurrentSession
    {
        public static int CurrentUserId { get; set; }
        public static bool IsLoggedIn => CurrentUserId > 0;
    }
}
