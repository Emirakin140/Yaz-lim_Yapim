using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using YazılımYapım.DataAccess;
using YazılımYapım.Enitty;

namespace YazılımYapım
{
    public partial class AnalizRapor : Form
    {
        private WordAttemptHistoryProvider _historyProvider;

        public AnalizRapor()
        {
            InitializeComponent();

            _historyProvider = new WordAttemptHistoryProvider();

            dateTimePickerBaslangic.Value = DateTime.Today.AddDays(-7);
            dateTimePickerBitis.Value = DateTime.Today;

            comboBoxZorluk.Items.AddRange(new object[] { "Tümü", "Kolay", "Orta", "Zor" });
            comboBoxZorluk.SelectedIndex = 0;

            btnRaporOlustur.Click += btnRaporOlustur_Click;
            btnKapat.Click += (s, e) => this.Close();

            InitializeChart();
        }

        private void AnalizRapor_Load(object sender, EventArgs e)
        {
        }

        private void InitializeChart()
        {
            chartDogruYanlis.Series.Clear();
            chartDogruYanlis.ChartAreas.Clear();
            chartDogruYanlis.Titles.Clear();

            chartDogruYanlis.Titles.Add("Sınav Başarı Oranları");
            chartDogruYanlis.ChartAreas.Add("Default");

            
            var seriesCorrect = new Series("Doğru")
            {
                ChartType = SeriesChartType.StackedColumn100
            };
            var seriesWrong = new Series("Yanlış")
            {
                ChartType = SeriesChartType.StackedColumn100
            };

            chartDogruYanlis.Series.Add(seriesCorrect);
            chartDogruYanlis.Series.Add(seriesWrong);

            var area = chartDogruYanlis.ChartAreas["Default"];
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            area.AxisY.LabelStyle.Format = "{0}%";
            area.AxisY.Maximum = 100;
        }

        private void btnRaporOlustur_Click(object sender, EventArgs e)
        {
            if (!CurrentSession.IsLoggedIn)
            {
                MessageBox.Show("Önce giriş yapmalısınız.",
                                "Uyarı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DateTime start = dateTimePickerBaslangic.Value.Date;
            DateTime end = dateTimePickerBitis.Value.Date;
            if (end < start)
            {
                MessageBox.Show("Bitiş tarihi, başlangıç tarihinden küçük olamaz.",
                                "Uyarı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int userId = CurrentSession.CurrentUserId;
            string difficulty = comboBoxZorluk.SelectedItem.ToString();

            DataTable dtReport = _historyProvider.GetReportData(
                userId, start, end, difficulty, int.MaxValue);

            if (!dtReport.Columns.Contains("Last6Dates"))
                dtReport.Columns.Add("Last6Dates", typeof(string));

            foreach (DataRow row in dtReport.Rows)
            {
                int wordId = Convert.ToInt32(row["WordId"]);
                List<DateTime> last6 = _historyProvider.GetLastSixDates(
                    userId, wordId, start, end);

                string datesStr = string.Join(", ",
                    last6.Select(d => d.ToString("dd.MM.yyyy")));

                row["Last6Dates"] = datesStr;
            }

            dataGridViewRapor.DataSource = dtReport;

            UpdateChart(userId, start, end, difficulty);
        }

        private void UpdateChart(int userId, DateTime start, DateTime end, string difficulty)
        {
            var sessions = _historyProvider
                .GetCorrectWrongBySession(userId, start, end, difficulty);

            var sCorrect = chartDogruYanlis.Series["Doğru"];
            var sWrong = chartDogruYanlis.Series["Yanlış"];

            sCorrect.Points.Clear();
            sWrong.Points.Clear();

            foreach (var sess in sessions)
            {
                // sess.SessionLabel = "08.06", sess.CorrectCount = 7, sess.WrongCount = 3 vb.
                sCorrect.Points.AddXY(sess.SessionLabel, sess.CorrectCount);
                sWrong.Points.AddXY(sess.SessionLabel, sess.WrongCount);
            }

            // (Renkleri ve eksen ayarlarını daha önce InitializeChart içinde yapmıştık)
        }

        private void dataGridViewRapor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            AnaEkran ana = new AnaEkran();
            ana.Show();
            this.Close();
        }

        private void chartDogruYanlis_Click(object sender, EventArgs e)
        {

        }
    }
}
