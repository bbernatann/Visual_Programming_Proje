using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fitness_Center
{
    public partial class Hesabım : Form
    {
        // Kullanıcı bilgilerini tutan private alanlar
        private String KullanıcıAdı, Sifre, Ad, Soyad, Cinsiyet, UyelikSuresi, Zamanlama;

        // Hesabım formu yüklendiğinde çalışacak olay
        private void Hesabım_Load(object sender, EventArgs e)
        {
            // Veritabanından kullanıcı bilgilerini çekmek için SQL sorgusu
            OleDbCommand bilgiCommand = new OleDbCommand("SELECT * FROM Information WHERE KullanıcıAdı=@p1", baglanti);
            bilgiCommand.Parameters.AddWithValue("@p1", KullanıcıAdı);

            // Veritabanı bağlantısını aç
            baglanti.Open();

            // SQL sorgusunu çalıştır ve verileri oku
            OleDbDataReader reader = bilgiCommand.ExecuteReader();

            // Kullanıcı bilgilerini Hesabım formundaki kontrol elemanlarına yerleştir
            if (reader.Read())
            {
                txtKullanıcıAdı.Text = reader["KullanıcıAdı"].ToString();
                txtSifre.Text = reader["Sifre"].ToString();
                txtAd.Text = reader["Ad"].ToString();
                txtSoyad.Text = reader["Soyad"].ToString();
                txtYas.Text = reader["Yas"].ToString();
                txtCinsiyet.Text = reader["Cinsiyet"].ToString();
                txtUyelikSuresi.Text = reader["UyelikSuresi"].ToString();
                comboBoxZamanlama.Text = reader["Zamanlama"].ToString();

                // Bitiş Tarihi değerini al ve txtUyelikBitis TextBox'ına yaz
                if (DateTime.TryParse(reader["BitisTarihi"].ToString(), out DateTime bitisTarihi))
                {
                    txtUyelikBitis.Text = bitisTarihi.ToShortDateString();
                }
            }

            // Veritabanı bağlantısını kapat
            baglanti.Close();
        }

        // Çıkış butonuna tıklandığında çalışacak olay
        private void btnCikis_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Oturumunuz kapatılmıştır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // Yapıcı metod ile formun başlangıç değerlerini alan ve form yüklendiğinde çalıştırılan olay
        private int Yas;
        public Hesabım(String KullanıcıAdı, String Sifre, String Ad, String Soyad, int Yas, String Cinsiyet, String UyelikSuresi, String Zamanlama)
        {
            InitializeComponent();

            // Constructor üzerinden gelen değerleri private alanlara atama
            this.KullanıcıAdı = KullanıcıAdı;
            this.Sifre = Sifre;
            this.Ad = Ad;
            this.Soyad = Soyad;
            this.Yas = Yas;
            this.Cinsiyet = Cinsiyet;
            this.UyelikSuresi = UyelikSuresi;
            this.Zamanlama = Zamanlama;

            // Load olayına text kutularını doldurmak için bağlama
            this.Load += Hesabım_Load;
        }

        // Veritabanı bağlantısı için kullanılan nesne
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        // Güncelleme butonuna tıklandığında çalışacak olay
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcının düzenlediği değerlerle özellikleri güncelle
                Sifre = txtSifre.Text;
                Ad = txtAd.Text;
                Soyad = txtSoyad.Text;
                Yas = int.Parse(txtYas.Text);
                Cinsiyet = txtCinsiyet.Text;
                UyelikSuresi = txtUyelikSuresi.Text;

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Güncelleme SQL sorgusunu hazırla ve çalıştır
                OleDbCommand guncelle = new OleDbCommand("UPDATE Information SET Sifre=@p1, Ad=@p2, Soyad=@p3, Yas=@p4, Cinsiyet=@p5, UyelikSuresi=@p6, Zamanlama=@p7 WHERE KullanıcıAdı=@p8", baglanti);
                guncelle.Parameters.AddWithValue("@p1", Sifre);
                guncelle.Parameters.AddWithValue("@p2", Ad);
                guncelle.Parameters.AddWithValue("@p3", Soyad);
                guncelle.Parameters.AddWithValue("@p4", Yas);
                guncelle.Parameters.AddWithValue("@p5", Cinsiyet);
                guncelle.Parameters.AddWithValue("@p6", UyelikSuresi);
                guncelle.Parameters.AddWithValue("@p7", comboBoxZamanlama.Text);
                guncelle.Parameters.AddWithValue("@p8", KullanıcıAdı);
                guncelle.ExecuteNonQuery();

                // Bilgi mesajı göster
                MessageBox.Show("Kayıt güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                // Format hatası durumunda hata mesajı göster
                MessageBox.Show("Lütfen doğru formatta değerler girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OleDbException ex)
            {
                // Veritabanı hatası durumunda hata mesajı göster
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                baglanti.Close();
            }
        }
    }
}
