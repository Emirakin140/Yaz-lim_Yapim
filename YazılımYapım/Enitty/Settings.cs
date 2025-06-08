using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public static class Settings
    {
        
        public const int DefaultKelimeSayisi = 10;
        public const string DefaultZorluk = "Kolay";

        
        public static int SecilenKelimeSayisi { get; set; } = DefaultKelimeSayisi;
        public static string SecilenZorluk { get; set; } = DefaultZorluk;
    }
}
