using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_Center
{
    public partial class Login : Form
    {
        // Form üzerinde kullanılacak değişkenler tanımlanıyor
        private String KullanıcıAdı, Sifre, Ad, Soyad, Cinsiyet, UyelikSuresi, Zamanlama;
        private int Yas;

        // Access veritabanı bağlantısı için OleDbConnection nesnesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        // Form yüklendiğinde çağrılan olay
        private void Login_Load(object sender, EventArgs e)
        {
            // "Çıkış" butonu konumlandırılıyor
            btnCikis.Location = new Point(this.ClientSize.Width - btnCikis.Width, 0);
            // Textbox'ların içeriği temizleniyor
            txtKullanıcıAdı.Text = "";
            txtSifre.Text = "";
            // Checkbox durumu değiştiğinde gerçekleşecek olaya metot atanıyor
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // Şifre textbox'ının karakterleri gizli tutuluyor
            txtSifre.PasswordChar = '*';
        }

        // Checkbox durumu değiştiğinde gerçekleşecek olay
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Checkbox işaretli ise şifre karakterleri gösterilir, değilse gizlenir
            txtSifre.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        // "Çıkış" butonuna tıklandığında gerçekleşecek olay
        private void btnCikis_Click(object sender, EventArgs e)
        {
            // Uygulamadan çıkış yapılır
            Application.Exit();
        }

        // Form oluşturulurken çağrılan constructor metodu
        public Login()
        {
            InitializeComponent();
            // Kullanıcı adı ve şifre değişkenleri textbox'lara atanır
            txtKullanıcıAdı.Text = KullanıcıAdı;
            txtSifre.Text = Sifre;
        }

        // "Kaydol" butonuna tıklandığında gerçekleşecek olay
        private void btnKaydol_Click(object sender, EventArgs e)
        {
            // UyeOlma formu oluşturularak gösterilir
            UyeOlma frm = new UyeOlma();
            frm.Show();
        }

        // "Giriş" butonuna tıklandığında gerçekleşecek olay
        private void btnGiris_Click(object sender, EventArgs e)
        {
            // Admin veya Kullanıcı seçeneği işaretlenmediyse uyarı verilir
            if (!radioBtnAdmin.Checked && !radioBtnKullanıcı.Checked)
            {
                MessageBox.Show("Lütfen 'Admin' veya 'Kullanıcı' seçeneğini işaretleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcı adı ve şifre textbox'lardan alınır
            string kullaniciAdi = txtKullanıcıAdı.Text;
            string sifre = txtSifre.Text;

            // SQL sorgusu
            string query = "SELECT * FROM Information WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre";
            OleDbCommand command = new OleDbCommand(query, baglanti);
            command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
            command.Parameters.AddWithValue("@sifre", sifre);

            try
            {
                // Veritabanı bağlantısı açılır
                baglanti.Open();
                // Sorgu çalıştırılır ve sonuç okuyucu (reader) nesnesine atanır
                OleDbDataReader reader = command.ExecuteReader();

                // Eğer kullanıcı bilgileri bulunursa
                if (reader.Read())
                {
                    // Kullanıcının rolü alınır
                    string isAdmin = reader["Rol"].ToString();

                    // Eğer "Admin" radio butonu seçiliyse ve kullanıcı admin ise
                    if (radioBtnAdmin.Checked && isAdmin == "Admin")
                    {
                        // AdminAnaSayfa formu açılır
                        MessageBox.Show("Admin girişi başarılı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AdminAnaSayfa adminForm = new AdminAnaSayfa();
                        adminForm.Show();
                        return; // Eğer kullanıcı admin ise, aşağıdaki kodu çalıştırmaya gerek yok
                    }
                    // Eğer "Kullanıcı" radio butonu seçiliyse ve kullanıcı kullanıcı ise
                    else if (radioBtnKullanıcı.Checked && isAdmin == "Kullanıcı")
                    {
                        // Kullanıcının bilgileri ile Hesabım formu açılır
                        MessageBox.Show("Kullanıcı girişi başarılı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hesabım hesabım = new Hesabım(txtKullanıcıAdı.Text, txtSifre.Text, Ad, Soyad, Convert.ToInt32(reader["Yas"]), Cinsiyet, UyelikSuresi, Zamanlama);
                        hesabım.Show();
                        return;
                    }
                }

                // Eğer buraya kadar gelindi ise, giriş bilgileri doğru, ancak seçilen rol gerçek rol ile uyuşmuyor
                MessageBox.Show("Kullanıcı adı veya şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajı gösterilir
                MessageBox.Show("Hata oluştu: " + ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılır
                baglanti.Close();
            }
        }
    }
}
