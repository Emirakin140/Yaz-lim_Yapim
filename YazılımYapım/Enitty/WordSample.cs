using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public class WordSample
    {
        public int WordSamplesID { get; set; } 
        public int WordID { get; set; }        
        public string Samples { get; set; }

        public Word Word { get; set; }
    }
}
