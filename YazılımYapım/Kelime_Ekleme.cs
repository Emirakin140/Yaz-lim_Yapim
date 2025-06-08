using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YazılımYapım.DataAccess;
using YazılımYapım.Enitty;

namespace YazılımYapım
{
    public partial class Kelime_Ekleme : Form
    {
        private readonly WordProvider _wordProvider;

        public Kelime_Ekleme()
        {
            InitializeComponent();
            _wordProvider = new WordProvider(); 
            comboBox1.Items.AddRange(new string[] { "Kolay", "Orta", "Zor" });
            comboBox1.SelectedIndex = -1;
            dgvWords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWords.MultiSelect = false;
            dgvWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWords.ReadOnly = true;
            LoadAllWordsToGrid();
            txtarama.TextChanged += txtSearch_TextChanged;
            dgvWords.SelectionChanged += dgvWords_SelectionChanged;
        }
        private void LoadAllWordsToGrid()
        {
            List<Word> words = _wordProvider.GetAllWords();
            dgvWords.DataSource = words;


            if (dgvWords.Columns.Contains("ID"))
                dgvWords.Columns["ID"].HeaderText = "ID";

            if (dgvWords.Columns.Contains("EngWordName"))
                dgvWords.Columns["EngWordName"].HeaderText = "İngilizce";

            if (dgvWords.Columns.Contains("TurWordName"))
                dgvWords.Columns["TurWordName"].HeaderText = "Türkçe";

            if (dgvWords.Columns.Contains("Picture"))
                dgvWords.Columns["Picture"].HeaderText = "Resim Yolu";

            if (dgvWords.Columns.Contains("Zorluk"))
                dgvWords.Columns["Zorluk"].HeaderText = "Zorluk";

            if (dgvWords.Columns.Contains("IsValid"))
                dgvWords.Columns["IsValid"].Visible = false;

            if (dgvWords.Columns.Contains("Progress"))
                dgvWords.Columns["Progress"].Visible = false;

            if (dgvWords.Columns.Contains("WordProgress"))
                dgvWords.Columns["WordProgress"].Visible = false;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtarama.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadAllWordsToGrid();
            }
            else
            {
                List<Word> filtered = _wordProvider.SearchWords(keyword);
                dgvWords.DataSource = filtered;
            }
        }
        private void dgvWords_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWords.SelectedRows == null || dgvWords.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = dgvWords.SelectedRows[0];
            if (row == null)
                return;

            if (dgvWords.Columns.Contains("EngWordName"))
            {
                object engValue = row.Cells["EngWordName"].Value;
                txting.Text = engValue != null ? engValue.ToString() : String.Empty;
            }
            else
            {
                txting.Text = String.Empty;
            }

            if (dgvWords.Columns.Contains("TurWordName"))
            {
                object turValue = row.Cells["TurWordName"].Value;
                txtturkce.Text = turValue != null ? turValue.ToString() : String.Empty;
            }
            else
            {
                txtturkce.Text = String.Empty;
            }

            if (dgvWords.Columns.Contains("Zorluk"))
            {
                object zorlukValue = row.Cells["Zorluk"].Value;
                if (zorlukValue == null || zorlukValue == DBNull.Value)
                {
                    comboBox1.SelectedIndex = -1;
                }
                else
                {
                    string z = zorlukValue.ToString();
                    if (comboBox1.Items.Contains(z))
                        comboBox1.SelectedItem = z;
                    else
                        comboBox1.SelectedIndex = -1;
                }
            }
            else
            {
                comboBox1.SelectedIndex = -1;
            }

            string picturePath = String.Empty;
            if (dgvWords.Columns.Contains("Picture"))
            {
                object picValue = row.Cells["Picture"].Value;
                picturePath = (picValue == null || picValue == DBNull.Value)
                               ? String.Empty
                               : picValue.ToString();
            }
            txtpictures.Text = picturePath;

            if (!String.IsNullOrEmpty(picturePath) && File.Exists(picturePath))
            {
                try
                {
                    using (var fs = new FileStream(picturePath, FileMode.Open, FileAccess.Read))
                    {
                        pcbox1.Image = Image.FromStream(fs);
                    }
                }
                catch
                {
                    pcbox1.Image = null;
                }
            }
            else
            {
                pcbox1.Image = null;
            }
        }


        private void Kelime_Ekleme_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)  
        {
            string ingilizce = txting.Text.Trim();
            string turkce = txtturkce.Text.Trim();
            string pictures = txtpictures.Text.Trim();
            string zorluk = comboBox1.SelectedItem == null
                                 ? ""
                                 : comboBox1.SelectedItem.ToString();

            if (string.IsNullOrEmpty(ingilizce) || string.IsNullOrEmpty(turkce))
            {
                MessageBox.Show("İngilizce ve Türkçe alanları boş bırakılamaz.",
                                "Eksik Bilgi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            Word yeniKelime = new Word
            {
                EngWordName = ingilizce,
                TurWordName = turkce,
                Picture = string.IsNullOrEmpty(pictures) ? null : pictures,
                Zorluk = string.IsNullOrEmpty(zorluk) ? null : zorluk,
                IsValid = true 
            };

            bool ok = _wordProvider.AddWord(yeniKelime);
            if (ok)
            {
                MessageBox.Show("Kelime başarıyla eklendi.",
                                "Başarılı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                LoadAllWordsToGrid();
                ClearFormFields();
            }
            else
            {
                MessageBox.Show("Kelime ekleme sırasında hata oluştu.",
                                "Hata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvWords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek kelimeyi seçin.",
                                "Uyarı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int selectedId = Convert.ToInt32(
                                dgvWords.SelectedRows[0].Cells["Id"].Value);

            var result = MessageBox.Show(
                             "Seçili kelimeyi silmek istediğinize emin misiniz?",
                             "Onay",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool ok = _wordProvider.DeleteWord(selectedId);

                if (ok)
                {
                    MessageBox.Show("Kelime başarıyla silindi.",
                                    "Başarılı",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    LoadAllWordsToGrid();
                    ClearFormFields();
                }
                else
                {
                    MessageBox.Show("Kelime silme sırasında hata oluştu.",
                                    "Hata",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnaEkran ana = new AnaEkran(); 
            ana.Show();
            this.Close();
        }
        private void ClearFormFields()
        {
            txtarama.Clear();
            txting.Clear();
            txtturkce.Clear();
            txtpictures.Clear();
            comboBox1.SelectedIndex = -1;
            pcbox1.Image = null;
            dgvWords.ClearSelection();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dgvWords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellenecek kelimeyi seçin.",
                                "Uyarı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int selectedId = Convert.ToInt32(dgvWords.SelectedRows[0].Cells["Id"].Value);

            string ingilizce = txting.Text.Trim();
            string turkce = txtturkce.Text.Trim();
            string pictures = txtpictures.Text.Trim();
            string zorluk = comboBox1.SelectedItem == null
                                 ? ""
                                 : comboBox1.SelectedItem.ToString();

            if (string.IsNullOrEmpty(ingilizce) || string.IsNullOrEmpty(turkce))
            {
                MessageBox.Show("İngilizce ve Türkçe alanları boş bırakılamaz.",
                                "Eksik Bilgi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            Word guncellenecekKelime = new Word
            {
                ID = selectedId,
                EngWordName = ingilizce,
                TurWordName = turkce,
                Picture = string.IsNullOrEmpty(pictures) ? null : pictures,
                Zorluk = string.IsNullOrEmpty(zorluk) ? null : zorluk,
                IsValid = true 
            };

            bool ok = _wordProvider.UpdateWord(guncellenecekKelime);
            if (ok)
            {
                MessageBox.Show("Kelime başarıyla güncellendi.",
                                "Başarılı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                LoadAllWordsToGrid();
                ClearFormFields();
            }
            else
            {
                MessageBox.Show("Kelime güncelleme sırasında hata oluştu.",
                                "Hata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            // Projenizin kökündeki Pictures klasörü
            openFileDialog1.InitialDirectory =
                @"C:\Users\emira\source\repos\YazılımYapım\YazılımYapım\Pictures";

            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp|All files|*.*";

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string srcPath = openFileDialog1.FileName;
            string destDir = openFileDialog1.InitialDirectory;
            string destPath = Path.Combine(destDir, Path.GetFileName(srcPath));

            File.Copy(srcPath, destPath, overwrite: true);

            txtpictures.Text = destPath;
            try
            {
                using (var fs = new FileStream(destPath, FileMode.Open, FileAccess.Read))
                    pcbox1.Image = Image.FromStream(fs);
            }
            catch
            {
                pcbox1.Image = null;
            }
        }
    }
}

