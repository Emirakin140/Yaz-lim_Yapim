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
    public partial class Sifre_Degis : Form
    {

        private readonly UserProvider _userProvider;
        private void Sifre_Degis_Load(object sender, EventArgs e)
        {

        }
        public Sifre_Degis()
        {
            InitializeComponent();
            _userProvider = new UserProvider();
        }

        
        


        private void Sifre_Degistirme_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Giris girisForm = new Giris();
            girisForm.Show();
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string kullaniciAdi = txtUsername.Text.Trim();
            string yeniSifre = txtNewPassword.Text;
            string sifreOnay = txtConfirmPassword.Text;
            if (string.IsNullOrWhiteSpace(kullaniciAdi))
            {
                MessageBox.Show("Lütfen kullanıcı adını girin.",
                                "Eksik Bilgi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(yeniSifre) || string.IsNullOrWhiteSpace(sifreOnay))
            {
                MessageBox.Show("Yeni şifre ve şifre onay alanları boş olamaz.",
                                "Eksik Bilgi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            if (yeniSifre != sifreOnay)
            {
                MessageBox.Show("Şifre ile şifre onay aynı değil. Lütfen tekrar kontrol edin.",
                                "Hata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                Users mevcutKullanici = _userProvider.GetUserByUsername(kullaniciAdi);
                if (mevcutKullanici == null)
                {
                    MessageBox.Show("Böyle bir kullanıcı bulunamadı.",
                                    "Hata",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                bool guncellemeBasarili = _userProvider.UpdatePassword(kullaniciAdi, yeniSifre);
                if (guncellemeBasarili)
                {
                    MessageBox.Show("Şifreniz başarıyla güncellendi.",
                                    "Başarılı",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    Giris girisForm = new Giris();
                    girisForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Şifre güncelleme sırasında bir hata oluştu.",
                                    "Hata",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu:\n" + ex.Message,
                                "Hata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
