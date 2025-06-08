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

namespace YazılımYapım
{
    public partial class Puzzle : Form
    {
        private readonly WordProvider _wordProvider;
        private readonly WordProgressProvider _progressProvider;
        private string targetWord;
        private const string DefaultWord = "BLACK";
        private int currentRow = 0;
        public Puzzle()
        {
            InitializeComponent();

            _wordProvider = new WordProvider();
            _progressProvider = new WordProgressProvider();

            ChooseTargetWord();

            txtGuess.MaxLength = 5;
            txtGuess.CharacterCasing = CharacterCasing.Upper;
            txtGuess.KeyPress += TxtGuess_KeyPress;

            btnonay.Click += btnonay_Click;
            btnExit.Click += (s, e) => this.Close();

            ClearAllGridLabels();

            this.Load += (s, e) => txtGuess.Focus();
            this.AcceptButton = btnonay;
            this.CancelButton = btnExit;
        }
        private void ClearAllGridLabels()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Control ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
                    if (ctl is Label lbl)
                    {
                        lbl.Text = "";
                        lbl.BackColor = Color.White;
                        lbl.ForeColor = Color.Black;
                        lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.BorderStyle = BorderStyle.FixedSingle;
                        lbl.Dock = DockStyle.Fill;
                    }
                }
            }
        }
        private void ChooseTargetWord()
        {
            var learned = _progressProvider.GetLearnedWordProgresses()
                             .Select(p => p.WordId)
                             .ToList();

            var words = _wordProvider.GetWordsByIds(learned)
                         .Where(w => w.EngWordName.Length == 5)
                         .Select(w => w.EngWordName.ToUpper())
                         .ToList();

            if (words.Count > 0)
            {
                var rnd = new Random();
                targetWord = words[rnd.Next(words.Count)];
            }
            else
            {
                targetWord = DefaultWord;
            }
        }
        private void TxtGuess_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ipucubtn_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ckbtn_Click(object sender, EventArgs e)
        {
            AnaEkran ana = new AnaEkran();
            ana.Show();
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Puzzle_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void btnonay_Click(object sender, EventArgs e)
        {
            
            string raw = txtGuess.Text.Trim().ToUpper();

            if (raw.Length != 5)
            {
                return;
            }

            if (currentRow >= 5)
            {
                MessageBox.Show(
                    "Tahmin hakkınız doldu. Oyunu yeniden başlatın.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            string target = targetWord.ToUpper();
            var letterCounts = new Dictionary<char, int>();
            foreach (char c in target)
            {
                if (!letterCounts.ContainsKey(c))
                    letterCounts[c] = 0;
                letterCounts[c]++;
            }

            Color[] resultColors = new Color[5];
            bool[] exactMatches = new bool[5];

            for (int i = 0; i < 5; i++)
            {
                if (raw[i] == target[i])
                {
                    resultColors[i] = Color.LightGreen;
                    exactMatches[i] = true;
                    letterCounts[raw[i]]--;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (exactMatches[i])
                    continue;

                char g = raw[i];
                if (letterCounts.ContainsKey(g) && letterCounts[g] > 0)
                {
                    resultColors[i] = Color.Gold;
                    letterCounts[g]--;
                }
                else
                {
                    resultColors[i] = Color.LightGray;
                }
            }

            for (int col = 0; col < 5; col++)
            {
                Control ctl = tableLayoutPanel1.GetControlFromPosition(col, currentRow);
                if (ctl is Label lbl)
                {
                    lbl.Text = raw[col].ToString();
                    lbl.BackColor = resultColors[col];
                    lbl.ForeColor = Color.Black;
                }
            }

            bool allGreen = resultColors.All(c => c == Color.LightGreen);
            if (allGreen)
            {
                MessageBox.Show($"Tebrikler! Doğru kelime: {targetWord}",
                                "Kazandınız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            currentRow++;
            txtGuess.Clear();
            txtGuess.Focus();

            if (currentRow >= 5)
            {
                MessageBox.Show($"Tahmin hakkınız doldu.\nDoğru kelime: {targetWord}",
                                "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void txtGuess_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl4_Click(object sender, EventArgs e)
        {

        }

        private void lbl2_Click(object sender, EventArgs e)
        {

        }

        private void lbl3_Click(object sender, EventArgs e)
        {

        }

        private void lbl5_Click(object sender, EventArgs e)
        {

        }

        private void lbl10_Click(object sender, EventArgs e)
        {

        }

        private void lbl8_Click(object sender, EventArgs e)
        {

        }

        private void lbl13_Click(object sender, EventArgs e)
        {

        }

        private void lbl14_Click(object sender, EventArgs e)
        {

        }

        private void lbl15_Click(object sender, EventArgs e)
        {

        }

        private void lbl20_Click(object sender, EventArgs e)
        {

        }

        private void lbl19_Click(object sender, EventArgs e)
        {

        }

        private void lbl23_Click(object sender, EventArgs e)
        {

        }

        private void lbl24_Click(object sender, EventArgs e)
        {

        }

        private void lbl25_Click(object sender, EventArgs e)
        {

        }
    }
}
