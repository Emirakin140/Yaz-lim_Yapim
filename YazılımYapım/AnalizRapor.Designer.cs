namespace YazılımYapım
{
    partial class AnalizRapor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewRapor = new System.Windows.Forms.DataGridView();
            this.btnKapat = new System.Windows.Forms.Button();
            this.chartDogruYanlis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerBaslangic = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBitis = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxZorluk = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRaporOlustur = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRapor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDogruYanlis)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRapor
            // 
            this.dataGridViewRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRapor.Location = new System.Drawing.Point(12, 349);
            this.dataGridViewRapor.Name = "dataGridViewRapor";
            this.dataGridViewRapor.Size = new System.Drawing.Size(676, 150);
            this.dataGridViewRapor.TabIndex = 0;
            this.dataGridViewRapor.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRapor_CellContentClick);
            // 
            // btnKapat
            // 
            this.btnKapat.BackColor = System.Drawing.Color.Black;
            this.btnKapat.Font = new System.Drawing.Font("Franklin Gothic Medium", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKapat.ForeColor = System.Drawing.Color.White;
            this.btnKapat.Location = new System.Drawing.Point(247, 505);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.Size = new System.Drawing.Size(175, 50);
            this.btnKapat.TabIndex = 5;
            this.btnKapat.Text = "Çıkış";
            this.btnKapat.UseVisualStyleBackColor = false;
            this.btnKapat.Click += new System.EventHandler(this.btnKapat_Click);
            // 
            // chartDogruYanlis
            // 
            chartArea1.Name = "ChartArea1";
            this.chartDogruYanlis.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDogruYanlis.Legends.Add(legend1);
            this.chartDogruYanlis.Location = new System.Drawing.Point(12, 163);
            this.chartDogruYanlis.Name = "chartDogruYanlis";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartDogruYanlis.Series.Add(series1);
            this.chartDogruYanlis.Size = new System.Drawing.Size(676, 180);
            this.chartDogruYanlis.TabIndex = 6;
            this.chartDogruYanlis.Text = "Rapor Oluştur";
            this.chartDogruYanlis.Click += new System.EventHandler(this.chartDogruYanlis_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Medium", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(240, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 37);
            this.label1.TabIndex = 7;
            this.label1.Text = "ANALİZ RAPOR";
            // 
            // dateTimePickerBaslangic
            // 
            this.dateTimePickerBaslangic.Location = new System.Drawing.Point(175, 68);
            this.dateTimePickerBaslangic.Name = "dateTimePickerBaslangic";
            this.dateTimePickerBaslangic.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerBaslangic.TabIndex = 8;
            // 
            // dateTimePickerBitis
            // 
            this.dateTimePickerBitis.Location = new System.Drawing.Point(175, 104);
            this.dateTimePickerBitis.Name = "dateTimePickerBitis";
            this.dateTimePickerBitis.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerBitis.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(26, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Başlangıç Tarihi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(26, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Bitiş Tarihi";
            // 
            // comboBoxZorluk
            // 
            this.comboBoxZorluk.FormattingEnabled = true;
            this.comboBoxZorluk.Location = new System.Drawing.Point(523, 65);
            this.comboBoxZorluk.Name = "comboBoxZorluk";
            this.comboBoxZorluk.Size = new System.Drawing.Size(121, 21);
            this.comboBoxZorluk.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(381, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Başlangıç Tarihi";
            // 
            // btnRaporOlustur
            // 
            this.btnRaporOlustur.BackColor = System.Drawing.Color.Black;
            this.btnRaporOlustur.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnRaporOlustur.ForeColor = System.Drawing.Color.White;
            this.btnRaporOlustur.Location = new System.Drawing.Point(415, 98);
            this.btnRaporOlustur.Name = "btnRaporOlustur";
            this.btnRaporOlustur.Size = new System.Drawing.Size(203, 27);
            this.btnRaporOlustur.TabIndex = 14;
            this.btnRaporOlustur.Text = "oluştur";
            this.btnRaporOlustur.UseVisualStyleBackColor = false;
            this.btnRaporOlustur.Click += new System.EventHandler(this.btnRaporOlustur_Click);
            // 
            // AnalizRapor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 567);
            this.ControlBox = false;
            this.Controls.Add(this.btnRaporOlustur);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxZorluk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerBitis);
            this.Controls.Add(this.dateTimePickerBaslangic);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartDogruYanlis);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.dataGridViewRapor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AnalizRapor";
            this.Text = "AnalizRapor";
            this.Load += new System.EventHandler(this.AnalizRapor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRapor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDogruYanlis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRapor;
        private System.Windows.Forms.Button btnKapat;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDogruYanlis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerBaslangic;
        private System.Windows.Forms.DateTimePicker dateTimePickerBitis;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxZorluk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRaporOlustur;
    }
}