using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public class WordProgress
    {
        public int WordProgressId { get; set; }
        public int WordId { get; set; }
        public DateTime? LastCorrectDate { get; set; }
        public int CorrectStreak { get; set; }    
        public DateTime? NextDueDate { get; set; }
    }
}
