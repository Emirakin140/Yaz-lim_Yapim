using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YazılımYapım.Enitty;

namespace YazılımYapım
{
    public partial class AnaEkran : Form
    {
        public AnaEkran()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Puzzle puzzleForm = new Puzzle();
            puzzleForm.Show();
            this.Hide();
        }

        private void AnaEkran_Load(object sender, EventArgs e)
        {

        }

        private void Başla_Click(object sender, EventArgs e)
        {
            int soruSayisi = Settings.SecilenKelimeSayisi;
            string zorluk = Settings.SecilenZorluk;

            var sinavForm = new Sinav(soruSayisi, zorluk);
            sinavForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kelime_Ekleme Kelime_EklemeForm = new Kelime_Ekleme();
            Kelime_EklemeForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AnalizRapor RaporForm = new AnalizRapor();
            RaporForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (var ayarlar = new Ayarlar(Settings.SecilenKelimeSayisi, Settings.SecilenZorluk))
            {
                if (ayarlar.ShowDialog() == DialogResult.OK)
                {
                   
                    
                }
                
            }
        }
    }
}
