using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YazılımYapım.DataAccess;
using YazılımYapım.Enitty;


namespace YazılımYapım
{
    
    public partial class Sinav : Form
    {


        private int _totalQuestions;
        private string _difficulty;

        private WordAttemptHistoryProvider _historyProvider;
        private WordProvider _wordProvider;
        private WordProgressProvider _progressProvider;

        private List<Word> examWords;
        private List<WordProgress> examProgresses;
        private List<int> skippedIndices;

        private int currentQuestionIndex = 0;
        private int correctCount = 0;
        private int wrongCount = 0;

        private int remainingSeconds = 600;
        private DateTime _examStartTime;          
        public Sinav()
        {
            InitializeComponent();   
            _totalQuestions = Settings.SecilenKelimeSayisi;
            _difficulty = Settings.SecilenZorluk;
            _historyProvider = new WordAttemptHistoryProvider();

            SetupAndStart();
        }
        public Sinav(int totalQuestions, string difficulty)
        {
            InitializeComponent();
            _historyProvider = new WordAttemptHistoryProvider();
            _totalQuestions = totalQuestions;
            _difficulty = difficulty;

            SetupAndStart();
        }
        private void SetupAndStart()
        {
            _wordProvider = new WordProvider();
            _progressProvider = new WordProgressProvider();

            skippedIndices = new List<int>();

            _examStartTime = DateTime.Now;
            quizTimer.Interval = 1000;
            quizTimer.Tick += QuizTimer_Tick;

            lblsoru.Text = _totalQuestions.ToString();
            
            remainingSeconds = 600;

            lbltimer.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");
            lblsoru.Text = $"Soru 1/{_totalQuestions}";
            lbldogru.Text = "Doğru 0";
            lblyanlis.Text = "Yanlış 0";

            btn1.Click += OptionButton_Click;
            btn2.Click += OptionButton_Click;
            btn3.Click += OptionButton_Click;
            btn4.Click += OptionButton_Click;
            btn5.Click += btn5_Click;  
            btn6.Click += btn6_Click;  

            PrepareAndStartExam();
        }
        private void PrepareAndStartExam()
        {
            if (quizTimer.Enabled)
                quizTimer.Stop();

            remainingSeconds = 600;
            lbltimer.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");
            _examStartTime = DateTime.Now;
            quizTimer.Start();

            DateTime today = DateTime.Today;
            List<WordProgress> dueProgresses = _progressProvider.GetDueWordProgresses(today);
            List<int> dueIds = dueProgresses.Select(p => p.WordId).ToList();
            List<Word> dueWords = _wordProvider.GetWordsByIds(dueIds);

            List<Word> newWords = new List<Word>();
            if (dueWords.Count < _totalQuestions)
            {
                int need = _totalQuestions - dueWords.Count;
                newWords = _wordProvider.GetNewWords(need, excludeWordIds: dueIds);
            }

            examWords = new List<Word>();
            examWords.AddRange(dueWords);
            examWords.AddRange(newWords);

            examProgresses = new List<WordProgress>();
            foreach (var w in examWords)
                examProgresses.Add(_progressProvider.GetOrCreateProgress(w.ID));

            correctCount = 0;
            wrongCount = 0;
            skippedIndices.Clear();
            currentQuestionIndex = 0;

            lblsoru.Text = $"Soru {currentQuestionIndex + 1}/{_totalQuestions}";
            lbldogru.Text = $"Doğru {correctCount}";
            lblyanlis.Text = $"Yanlış {wrongCount}";

            ShowQuestion(currentQuestionIndex);
        }
        private void ShowQuestion(int index)
        {
            if (index < 0 || index >= examWords.Count) return;

            Word w = examWords[index];
            Sinavlbl.Text = $"\"{w.EngWordName}\"";

            string correctAnswer = w.TurWordName;
            List<string> wrongs = _wordProvider.GetRandomWrongTurkishWords(3, w.ID, null);
            while (wrongs.Count < 3)
                wrongs.Add(" ");

            List<string> options = new List<string> { correctAnswer }
                                        .Concat(wrongs)
                                        .OrderBy(x => Guid.NewGuid())
                                        .ToList();

            btn1.Text = options[0]; btn1.Tag = (options[0] == correctAnswer);
            btn2.Text = options[1]; btn2.Tag = (options[1] == correctAnswer);
            btn3.Text = options[2]; btn3.Tag = (options[2] == correctAnswer);
            btn4.Text = options[3]; btn4.Tag = (options[3] == correctAnswer);

            foreach (var b in new[] { btn1, btn2, btn3, btn4 })
            {
                
                b.Enabled = true;
                b.UseVisualStyleBackColor = true;
                b.ForeColor = Color.Black;
            }

            lbldogru.Text = $"Doğru {correctCount}";
            lblyanlis.Text = $"Yanlış {wrongCount}";
        }
        private void OptionButton_Click(object sender, EventArgs e)
        {
            
            Button clicked = (Button)sender;
            bool isCorrect = (clicked.Tag is bool b && b);

            int userId = CurrentSession.CurrentUserId;
            int wordId = examWords[currentQuestionIndex].ID;
            DateTime answerDate = DateTime.Now;
            _historyProvider.AddAttempt(wordId, userId, isCorrect, answerDate);

            btn1.Enabled = btn2.Enabled = btn3.Enabled = btn4.Enabled = false;

            WordProgress progress = examProgresses[currentQuestionIndex];
            if (isCorrect)
            {
                clicked.BackColor = Color.LightGreen;
                progress.CorrectStreak++;
                progress.LastCorrectDate = DateTime.Today;
                progress.NextDueDate = CalculateNextDueDate(
                    progress.LastCorrectDate.Value,
                    progress.CorrectStreak);
                correctCount++;
            }
            else
            {
                clicked.BackColor = Color.IndianRed;
                progress.CorrectStreak = 0;
                progress.LastCorrectDate = DateTime.Today;
                progress.NextDueDate = DateTime.Today.AddDays(1);
                wrongCount++;
            }

            _progressProvider.UpdateProgress(progress);
            lbldogru.Text = $"Doğru {correctCount}";
            lblyanlis.Text = $"Yanlış {wrongCount}";

            Timer pauseTimer = new Timer();
            pauseTimer.Interval = 500;
            pauseTimer.Tick += (s, args) =>
            {
                pauseTimer.Stop();
                pauseTimer.Dispose();
                NextQuestionOrReturn();
            };
            pauseTimer.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Sinav_Load(object sender, EventArgs e)
        {

        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (!skippedIndices.Contains(currentQuestionIndex))
                skippedIndices.Add(currentQuestionIndex);
            NextQuestionOrReturn();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            FinishExam();
        }
        private void NextQuestionOrReturn()
        {
            currentQuestionIndex++;

            if (currentQuestionIndex >= examWords.Count)
            {
                currentQuestionIndex++;
                if (currentQuestionIndex >= examWords.Count)
                {
                    if (skippedIndices.Count > 0)
                    {
                        int returnIndex = skippedIndices[0];
                        skippedIndices.RemoveAt(0);
                        currentQuestionIndex = returnIndex;
                        ShowQuestion(currentQuestionIndex);
                    }
                    else
                    {
                        FinishExam();
                    }
                }
                else
                {
                    ShowQuestion(currentQuestionIndex);
                }
            }
            else
            {
                ShowQuestion(currentQuestionIndex);
            }
        }
        private void QuizTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            if (remainingSeconds <= 0)
            {
                quizTimer.Stop();
                FinishExam();
                return;
            }
            lbltimer.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");
        }
        private DateTime CalculateNextDueDate(DateTime lastCorrect, int correctStreak)
        {
            switch (correctStreak)
            {
                case 1: return lastCorrect.AddDays(1);
                case 2: return lastCorrect.AddDays(7);
                case 3: return lastCorrect.AddDays(30);
                case 4: return lastCorrect.AddDays(90);
                case 5: return lastCorrect.AddDays(180);
                case 6: return lastCorrect.AddDays(365);
                default: return lastCorrect.AddDays(1);
            }
        }
        private void FinishExam()
        {
            if (quizTimer.Enabled) quizTimer.Stop();

            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = endTime - _examStartTime;
            string elapsedText = elapsed.ToString(@"mm\:ss");

            MessageBox.Show(
                $"Sınav tamamlandı!\nDoğru: {correctCount}\n" +
                $"Yanlış: {wrongCount}\nGeçen süre: {elapsedText}",
                "Sonuç",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            this.Close();
        }
    }
}
