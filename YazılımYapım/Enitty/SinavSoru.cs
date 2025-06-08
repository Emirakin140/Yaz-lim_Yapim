using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.Enitty
{
    public class SinavSoru
    {
        public int ExamQuestionID { get; set; }
        public int ExamID { get; set; }
        public int WordID { get; set; }

        public Sinav Sinav { get; set; }
        public Word Word { get; set; }
        public ICollection<WordSample> ExamQuestions { get; set; }

    }
}
