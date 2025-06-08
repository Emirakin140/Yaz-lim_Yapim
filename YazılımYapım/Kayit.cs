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
    public partial class Kayit : Form
    {

        private readonly UserProvider _userProvider;
        private string _expectedCode;
        private const string _charSet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789";
        private readonly Random _rnd = new Random();
        public Kayit()
        {
            InitializeComponent();
            _userProvider = new UserProvider();

            
            GenerateAndShowNewCode();
        }
        private void GenerateAndShowNewCode()
        {
            const int codeLength = 5;

            _expectedCode = new string(
                Enumerable.Range(0, codeLength)
                          .Select(_ => _charSet[_rnd.Next(_charSet.Length)])
                          .ToArray()
            );

            lblConfirmcode.Text = _expectedCode;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtConfirmCode.Text = string.Empty;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Kayit_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtUsername.Text.Trim();
            string sifre = txtPassword.Text;
            string sifreOnay = txtConfirmPassword.Text;
            string girilenOnayKodu = txtConfirmCode.Text.Trim();

            if (string.IsNullOrWhiteSpace(kullaniciAdi))
            {
                MessageBox.Show(
                    "Lütfen bir kullanıcı adı girin.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            
            if (string.IsNullOrWhiteSpace(sifre) || string.IsNullOrWhiteSpace(sifreOnay))
            {
                MessageBox.Show(
                    "Şifre ve şifre onay alanlarını boş geçmeyin.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            
            if (sifre != sifreOnay)
            {
                MessageBox.Show(
                    "Şifre ile şifre onay aynı değil. Lütfen tekrar kontrol edin.",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            
            if (string.IsNullOrEmpty(girilenOnayKodu))
            {
                MessageBox.Show(
                    "Lütfen onay kodunu girin.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            
            if (!girilenOnayKodu.Equals(_expectedCode, StringComparison.Ordinal))
            {
                MessageBox.Show(
                    "Onay kodu hatalı. Yeni bir kod üretildi.\n" +
                    "Lütfen aşağıdaki yeni kodu girin.",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                txtConfirmCode.Clear();
                GenerateAndShowNewCode();
                return;
            }

            try
            {
                Users mevcutKullanici = _userProvider.GetUserByUsername(kullaniciAdi);
                if (mevcutKullanici != null)
                {
                    MessageBox.Show(
                        "Bu kullanıcı adı zaten alınmış. Lütfen başka bir ad deneyin.",
                        "Hata",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                bool olusturmaBasarili = _userProvider.CreateUser(kullaniciAdi, sifre, isValid: true);
                if (olusturmaBasarili)
                {
                    MessageBox.Show(
                        "Kayıt başarılı. Giriş yapabilirsiniz.",
                        "Ana sayfaya yönlendiriliyorsunuz.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    Giris girisForm = new Giris();
                    girisForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.",
                        "Hata",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Beklenmeyen bir hata oluştu:\n" + ex.Message,
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Giris girisForm = new Giris();
            girisForm.Show();
            this.Close();
        }
    }
}
