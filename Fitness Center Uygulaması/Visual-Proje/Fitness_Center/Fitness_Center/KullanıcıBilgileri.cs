using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fitness_Center
{
    public partial class KullanıcıBilgileri : Form
    {
        // Veritabanı bağlantısını temsil eden OleDbConnection nesnesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        public KullanıcıBilgileri()
        {
            InitializeComponent();
        }

        // Kullanıcı bilgilerini listeleme işlemini gerçekleştiren metod
        void Listele()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Geçerlilik süresi biten kullanıcıları silme işlemi
                OleDbCommand deleteExpiredUsersCommand = new OleDbCommand("DELETE FROM Information WHERE BitisTarihi < @p1", baglanti);
                deleteExpiredUsersCommand.Parameters.AddWithValue("@p1", OleDbType.DBDate).Value = DateTime.Now.Date;
                deleteExpiredUsersCommand.ExecuteNonQuery();

                // Veritabanından "Rol"ü 'Kullanıcı' olan tüm kayıtları çekme
                DataTable dt = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Information WHERE Rol = 'Kullanıcı'", baglanti);
                da.Fill(dt);

                // DataGridView'e çekilen verileri bind etme
                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            }
            catch (OleDbException ex)
            {
                // Hata durumunda kullanıcıya bilgi verme
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantı durumunu kontrol et ve kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        // Form yüklenirken çalışacak olan metod
        private void KullanıcıBilgileri_Load(object sender, EventArgs e)
        {
            // Kullanıcı bilgilerini listeleyen metodun çağrılması
            Listele();
        }

        // Geri butonuna tıklandığında çalışacak olan metod
        private void btnGeri_Click(object sender, EventArgs e)
        {
            // AdminAnaSayfa formunu göster
            AdminAnaSayfa admin = new AdminAnaSayfa();
            admin.Show();
        }

        // DataGridView hücresine tıklandığında çalışacak olan metod
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Satıra tıklanan hücrenin indeksi 0'dan büyükse
            if (e.RowIndex >= 0)
            {
                // Seçilen satırdaki hücrelerden bilgileri TextBox ve diğer kontrollere yerleştirme
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtKullanıcıAdı.Text = selectedRow.Cells["KullanıcıAdı"].Value.ToString();
                txtSifre.Text = selectedRow.Cells["Sifre"].Value.ToString();
                txtAd.Text = selectedRow.Cells["Ad"].Value.ToString();
                txtSoyad.Text = selectedRow.Cells["Soyad"].Value.ToString();

                // Yaş değeri sayıya dönüştürülebiliyorsa TextBox'e yerleştirme
                if (int.TryParse(selectedRow.Cells["Yas"].Value.ToString(), out int yas))
                {
                    txtYas.Text = yas.ToString();
                }

                comboBoxCinsiyet.Text = selectedRow.Cells["Cinsiyet"].Value.ToString();
                comboBoxUyelikSuresi.Text = selectedRow.Cells["UyelikSuresi"].Value.ToString();
                comboBoxZamanlama.Text = selectedRow.Cells["Zamanlama"].Value.ToString();

                // Başlangıç ve bitiş tarihlerini DateTimePicker'lara yerleştirme
                if (DateTime.TryParse(selectedRow.Cells["BaslangıcTarihi"].Value.ToString(), out DateTime baslangıcTarihi))
                {
                    dateTimePickerOT.Value = baslangıcTarihi;
                }
                if (DateTime.TryParse(selectedRow.Cells["BitisTarihi"].Value.ToString(), out DateTime bitisTarihi))
                {
                    dateTimePickerBT.Value = bitisTarihi;
                }
            }
        }

        // Güncelle butonuna tıklandığında çalışacak olan metod
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\""))
                {
                    // Bağlantıyı aç
                    baglanti.Open();

                    // Güncellenecek kaydın başlangıç ve bitiş tarihlerini belirleme
                    DateTime baslangicTarihi = dateTimePickerOT.Value.Date;
                    DateTime bitisTarihi;

                    switch (comboBoxUyelikSuresi.SelectedItem.ToString())
                    {
                        case "1 AY (450 TL)":
                            bitisTarihi = baslangicTarihi.AddMonths(1);
                            break;
                        case "2 AY (850 TL)":
                            bitisTarihi = baslangicTarihi.AddMonths(2);
                            break;
                        case "6 AY (2200 TL)":
                            bitisTarihi = baslangicTarihi.AddMonths(6);
                            break;
                        case "1 YIL (4500 TL)":
                            bitisTarihi = baslangicTarihi.AddYears(1);
                            break;
                        default:
                            throw new ArgumentException("Geçersiz üyelik süresi");
                    }

                    // Güncelleme SQL komutu
                    using (OleDbCommand guncelle = new OleDbCommand("UPDATE Information SET Sifre=@p1, Ad=@p2, Soyad=@p3, Yas=@p4, Cinsiyet=@p5, UyelikSuresi=@p6, Zamanlama=@p7, BaslangıcTarihi=@p8, BitisTarihi=@p9 WHERE KullanıcıAdı=@p10", baglanti))
                    {
                        // Parametreleri belirleme
                        guncelle.Parameters.AddWithValue("@p1", txtSifre.Text);
                        guncelle.Parameters.AddWithValue("@p2", txtAd.Text);
                        guncelle.Parameters.AddWithValue("@p3", txtSoyad.Text);
                        guncelle.Parameters.AddWithValue("@p4", txtYas.Text);
                        guncelle.Parameters.AddWithValue("@p5", comboBoxCinsiyet.Text);
                        guncelle.Parameters.AddWithValue("@p6", comboBoxUyelikSuresi.Text);
                        guncelle.Parameters.AddWithValue("@p7", comboBoxZamanlama.Text);
                        guncelle.Parameters.AddWithValue("@p8", baslangicTarihi);
                        guncelle.Parameters.AddWithValue("@p9", bitisTarihi);
                        guncelle.Parameters.AddWithValue("@p10", txtKullanıcıAdı.Text);

                        // Güncelleme işlemini gerçekleştirme
                        guncelle.ExecuteNonQuery();

                        // Kullanıcıya bilgi verme
                        MessageBox.Show("Kullanıcının bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Kullanıcı bilgilerini tekrar listeleme ve TextBox'ları temizleme
                        Listele();
                        txtKullanıcıAdı.Text = ""; txtSifre.Text = ""; txtAd.Text = ""; txtSoyad.Text = ""; txtYas.Text = "";
                        comboBoxCinsiyet.Text = ""; comboBoxUyelikSuresi.Text = ""; comboBoxZamanlama.Text = ""; dateTimePickerOT.Value = DateTime.Now; dateTimePickerBT.Value = DateTime.Now;
                    }
                }
            }
            catch (FormatException)
            {
                // Hatalı format durumunda kullanıcıya bilgi verme
                MessageBox.Show("Lütfen doğru formatta değerler girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OleDbException ex)
            {
                // Veritabanı hatası durumunda kullanıcıya bilgi verme
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sil butonuna tıklandığında çalışacak olan metod
        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection localBaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\""))
                {
                    // Bağlantıyı aç
                    localBaglanti.Open();

                    // Kullanıcı silme SQL komutu
                    OleDbCommand komut = new OleDbCommand("DELETE FROM Information WHERE KullanıcıAdı=@p1", localBaglanti);
                    komut.Parameters.AddWithValue("@p1", txtKullanıcıAdı.Text);
                    komut.ExecuteNonQuery();

                    // Kullanıcıya bilgi verme
                    MessageBox.Show("Üye silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Kullanıcı bilgilerini tekrar listeleme ve TextBox'ları temizleme
                    Listele();
                    txtKullanıcıAdı.Text = ""; txtSifre.Text = ""; txtAd.Text = ""; txtSoyad.Text = ""; txtYas.Text = "";
                    comboBoxCinsiyet.Text = ""; comboBoxUyelikSuresi.Text = ""; comboBoxZamanlama.Text = ""; dateTimePickerOT.Value = DateTime.Now; dateTimePickerBT.Value = DateTime.Now;
                }
            }
            catch (OleDbException ex)
            {
                // Veritabanı hatası durumunda kullanıcıya bilgi verme
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKullanıcıEkle_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan bilgilerin eksiksiz olduğunu kontrol et
            if (txtAd.Text == "" ||
                txtSoyad.Text == "" ||
                txtKullanıcıAdı.Text == "" ||
                txtSifre.Text == "" ||
                txtYas.Text == "" ||
                comboBoxCinsiyet.Text == "" ||
                comboBoxUyelikSuresi.Text == "" ||
                comboBoxZamanlama.Text == "")
            {
                // Eksik bilgi varsa uyarı mesajı göster
                MessageBox.Show("Tüm bilgileri doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    // Access veritabanına bağlantı aç
                    using (OleDbConnection localBaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\""))
                    {
                        localBaglanti.Open();

                        // Kullanıcı adının veritabanında daha önce kullanılıp kullanılmadığını kontrol et
                        OleDbCommand checkUsernameCommand = new OleDbCommand("SELECT COUNT(*) FROM Information WHERE KullanıcıAdı = @p1", localBaglanti);
                        checkUsernameCommand.Parameters.AddWithValue("@p1", txtKullanıcıAdı.Text);
                        //Kullanıcı adının kaç kere kullanıldığını bulur.
                        int existingUserCount = Convert.ToInt32(checkUsernameCommand.ExecuteScalar());
                        //Eğer kullanıcı adı kullanılmışsa hata verir.
                        if (existingUserCount > 0)
                        {
                            // Kullanıcı adı kullanılmışsa hata mesajı göster
                            MessageBox.Show("Bu kullanıcı adı alınmıştır. Başka kullanıcı adı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            // Üyelik süresine göre bitiş tarihini hesapla
                            DateTime baslangicTarihi = dateTimePickerOT.Value;
                            DateTime bitisTarihi;

                            switch (comboBoxUyelikSuresi.SelectedItem.ToString())
                            {
                                case "1 AY (450 TL)":
                                    bitisTarihi = baslangicTarihi.AddMonths(1);
                                    break;
                                case "2 AY (850 TL)":
                                    bitisTarihi = baslangicTarihi.AddMonths(2);
                                    break;
                                case "6 AY (2200 TL)":
                                    bitisTarihi = baslangicTarihi.AddMonths(6);
                                    break;
                                case "1 YIL (4500 TL)":
                                    bitisTarihi = baslangicTarihi.AddYears(1);
                                    break;
                                default:
                                    throw new ArgumentException("Geçersiz üyelik süresi");
                            }

                            // Kullanıcı bilgilerini veritabanına ekle
                            using (OleDbCommand kaydet = new OleDbCommand("INSERT INTO Information (Ad, Soyad, KullanıcıAdı, Sifre, Yas, Cinsiyet, UyelikSuresi, Zamanlama, Rol, BaslangıcTarihi, BitisTarihi)" +
                                " VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)", localBaglanti))
                            {
                                kaydet.Parameters.AddWithValue("@p1", txtAd.Text);
                                kaydet.Parameters.AddWithValue("@p2", txtSoyad.Text);
                                kaydet.Parameters.AddWithValue("@p3", txtKullanıcıAdı.Text);
                                kaydet.Parameters.AddWithValue("@p4", txtSifre.Text);
                                kaydet.Parameters.AddWithValue("@p5", int.Parse(txtYas.Text));//Yaş verisini tam sayıya dönüştürür.
                                kaydet.Parameters.AddWithValue("@p6", comboBoxCinsiyet.SelectedItem.ToString());
                                kaydet.Parameters.AddWithValue("@p7", comboBoxUyelikSuresi.SelectedItem.ToString());
                                kaydet.Parameters.AddWithValue("@p8", comboBoxZamanlama.SelectedItem.ToString());
                                kaydet.Parameters.AddWithValue("@p9", "Kullanıcı");
                                kaydet.Parameters.AddWithValue("@p10", baslangicTarihi);
                                kaydet.Parameters.AddWithValue("@p11", bitisTarihi);

                                kaydet.ExecuteNonQuery();

                                // Başarılı ekleme mesajı göster
                                MessageBox.Show("Üyelik işlemleri başarıyla gerçekleştirildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Kullanıcı arayüzünü temizle
                                Listele();
                                txtKullanıcıAdı.Text = ""; txtSifre.Text = ""; txtAd.Text = ""; txtSoyad.Text = ""; txtYas.Text = "";
                                comboBoxCinsiyet.Text = ""; comboBoxUyelikSuresi.Text = ""; comboBoxZamanlama.Text = ""; dateTimePickerOT.Value = DateTime.Now; dateTimePickerBT.Value = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda hata mesajını göster
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
