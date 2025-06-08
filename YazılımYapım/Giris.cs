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
    public partial class Giris : Form
    {
        private const string CorrectUsername = "admin";
        private const string CorrectPassword = "12345";

        public Giris()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullanici.Text.Trim();
            string sifre = txtSifre.Text.Trim();
            string connStr = @"Server=EMIR\SQLEXPRESS;Database=Sozluk;Trusted_Connection=True;";
            var provider = new UserProvider(connStr);
            
            Users user = provider.GetUserByUsername(kullaniciAdi);

            if (user == null)
            {
                MessageBox.Show("Böyle bir kullanıcı yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (user.Password != sifre)
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!user.IsValid)
            {
                MessageBox.Show("Kullanıcının yetkisi yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                // Giriş başarılı → Ana ekrana geç
                CurrentSession.CurrentUserId = user.Id;
                AnaEkran anaForm = new AnaEkran();
                anaForm.Show();
                this.Hide();
            }
        }

        private void lblsifreds_Click(object sender, EventArgs e)
        {
            Sifre_Degis sifreForm = new Sifre_Degis();
            sifreForm.Show();
            this.Hide();
        }

        private void lblcl_Click(object sender, EventArgs e)
        {
            txtKullanici.Text = String.Empty;
            txtSifre.Text = String.Empty;
        }

        private void lblKyt_Click(object sender, EventArgs e)
        {
            Kayit kayitForm = new Kayit();
            kayitForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }
}
