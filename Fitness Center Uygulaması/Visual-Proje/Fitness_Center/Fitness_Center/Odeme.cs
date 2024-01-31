using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fitness_Center
{
    public partial class Odeme : Form
    {
        private String Ad, Soyad, KullanıcıAdı, UyelikSuresi;

        OleDbConnection baglanti;

        // Constructor - Form'un başlatılması
        public Odeme(String KullanıcıAdı, String Ad, String Soyad, String UyelikSuresi)
        {
            InitializeComponent();
            this.Ad = Ad;
            this.Soyad = Soyad;
            this.KullanıcıAdı = KullanıcıAdı;
            this.UyelikSuresi = UyelikSuresi;

            // Veritabanı bağlantısı oluşturulması
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");
        }

        // Form yüklendiğinde çalışan olay
        private void Odeme_Load(object sender, EventArgs e)
        {
            // Kullanıcı bilgilerini arayüzde gösterme
            txtKullanıcıAdı.Text = KullanıcıAdı;
            txtAd.Text = Ad;
            txtSoyad.Text = Soyad;

            // Seçilen üyelik süresine göre ücreti ayarlama
            switch (UyelikSuresi)
            {
                case "1 AY (450 TL)":
                    txtTutar.Text = "450";
                    break;
                case "2 AY (850 TL)":
                    txtTutar.Text = "850";
                    break;
                case "6 AY (2200 TL)":
                    txtTutar.Text = "2200";
                    break;
                case "1 YIL (4500 TL)":
                    txtTutar.Text = "4500";
                    break;
                default:
                    txtTutar.Text = "";
                    break;
            }
        }

        // Ödeme yap butonuna tıklandığında çalışan olay
        private void btnOde_Click(object sender, EventArgs e)
        {
            // Gerekli alanların eksiksiz doldurulup doldurulmadığını kontrol etme
            if (dateTimePickerOT.Text == "" || txtAd.Text == "" || txtSoyad.Text == "")
            {
                MessageBox.Show("Ödeme bilgilerini eksiksiz giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    baglanti.Open();

                    // Veritabanından mevcut üyelik bilgilerini alma
                    OleDbCommand Command = new OleDbCommand("SELECT BaslangıcTarihi FROM Information WHERE KullanıcıAdı = @p1", baglanti);
                    Command.Parameters.AddWithValue("@p1", KullanıcıAdı);

                    OleDbDataReader reader = Command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Eğer kullanıcı bilgileri bulunduysa
                        DateTime baslangicTarihi = reader["BaslangıcTarihi"] == DBNull.Value //Database de başlangıç tarihinin null olup olmadığını kontrol eder
                            ? DateTime.MinValue//Eğer veritabanınndan gelen değer null ise en küçük tarih değerini atar.
                            : Convert.ToDateTime(reader["BaslangıcTarihi"]);//Veriyi date time a dönüştürür

                        // DateTimePicker'dan seçilen tarihi al
                        DateTime odemeTarihi = dateTimePickerOT.Value.Date;

                        // Üyelik süresine göre bitiş tarihi hesaplama
                        DateTime bitisTarihi;

                        switch (UyelikSuresi)
                        {
                            case "1 AY (450 TL)":
                                bitisTarihi = odemeTarihi.AddMonths(1);
                                break;
                            case "2 AY (850 TL)":
                                bitisTarihi = odemeTarihi.AddMonths(2);
                                break;
                            case "6 AY (2200 TL)":
                                bitisTarihi = odemeTarihi.AddMonths(6);
                                break;
                            case "1 YIL (4500 TL)":
                                bitisTarihi = odemeTarihi.AddYears(1);
                                break;
                            default:
                                throw new ArgumentException("Geçersiz üyelik süresi");
                        }

                        // Veritabanını yeni bitiş tarihi ile güncelleme
                        OleDbCommand guncelle = new OleDbCommand("UPDATE Information SET BaslangıcTarihi=@p1, BitisTarihi=@p2 WHERE KullanıcıAdı=@p3", baglanti);
                        guncelle.Parameters.Add("@p1", OleDbType.Date).Value = odemeTarihi;
                        guncelle.Parameters.Add("@p2", OleDbType.Date).Value = bitisTarihi;
                        guncelle.Parameters.AddWithValue("@p3", KullanıcıAdı);
                        guncelle.ExecuteNonQuery();

                        MessageBox.Show("Ödeme ve üyelik işlemleri başarıyla gerçekleştirildi. Giriş sayfasına yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        // Kullanıcı bilgileri bulunamadıysa hata mesajı gösterme
                        MessageBox.Show("Kullanıcı bilgileri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda hata mesajı gösterme
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Bağlantıyı kapatma
                    baglanti.Close();
                }
            }
        }
    }
}
