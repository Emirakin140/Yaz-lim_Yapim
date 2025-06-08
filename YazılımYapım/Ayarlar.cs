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
    public partial class Ayarlar : Form
    {
        private const int DefaultKelimeSayisi = 10;
        private const string DefaultZorluk = "Kolay";
        public int SecilenKelimeSayisi { get; private set; }
        public string SecilenZorluk { get; private set; }



        public Ayarlar()
        {
            InitializeComponent();

            txtWord.Text = Settings.DefaultKelimeSayisi.ToString();

            if (cmbzorluk.Items.Count == 0)
                cmbzorluk.Items.AddRange(new object[] { "Kolay", "Orta", "Zor" });

            cmbzorluk.SelectedItem = Settings.DefaultZorluk;

            button1.Click += button1_Click;   
            ckbtn.Click += ckbtn_Click;     
            txtWord.KeyPress += txtWord_KeyPress;
        }
        public Ayarlar(int mevcutKelimeSayisi, string mevcutZorluk)
        {
            InitializeComponent();

            txtWord.Text = mevcutKelimeSayisi.ToString();

            if (cmbzorluk.Items.Count == 0)
                cmbzorluk.Items.AddRange(new object[] { "Kolay", "Orta", "Zor" });

            cmbzorluk.SelectedItem = mevcutZorluk;

            button1.Click += button1_Click;
            ckbtn.Click += ckbtn_Click;
            txtWord.KeyPress += txtWord_KeyPress;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtWord.Text.Trim(), out int sayi) || sayi <= 0)
            {
                MessageBox.Show(
                    "Lütfen geçerli bir pozitif tam sayı girin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtWord.Focus();
                return;
            }

            if (cmbzorluk.SelectedItem == null)
            {
                MessageBox.Show(
                    "Lütfen bir zorluk seviyesi seçin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmbzorluk.DroppedDown = true;
                return;
            }

            SecilenKelimeSayisi = sayi;
            SecilenZorluk = cmbzorluk.SelectedItem.ToString();

            Settings.SecilenKelimeSayisi = SecilenKelimeSayisi;
            Settings.SecilenZorluk = SecilenZorluk;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void ckbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtWord_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void Ayarlar_Load(object sender, EventArgs e)
        {

        }
    }
}
