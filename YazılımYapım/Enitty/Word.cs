using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public class Word
    {
        
        public int ID { get; set; } 
        public string EngWordName { get; set; }
        public string TurWordName { get; set; }
        public string Picture { get; set; }
        public string Zorluk {  get; set; }
        public bool IsValid { get; set; }

        public WordProgress Progress { get; set; }
    }
}
