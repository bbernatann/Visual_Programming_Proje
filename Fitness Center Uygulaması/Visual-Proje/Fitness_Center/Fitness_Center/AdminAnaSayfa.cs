using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Fitness_Center
{
    public partial class AdminAnaSayfa : Form
    {
        // Veritabanına bağlantı nesnesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        // Formun yapıcı metodu
        public AdminAnaSayfa()
        {
            InitializeComponent();
        }

        // Kullanıcı Bilgileri butonuna tıklandığında çalışacak olay
        private void btnKullanıcıBilgileri_Click(object sender, EventArgs e)
        {
            KullanıcıBilgileri frm = new KullanıcıBilgileri();
            frm.Show();
        }

        // Oturumu Kapat butonuna tıklandığında çalışacak olay
        private void btnOturumKapatma_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            MessageBox.Show("Oturumunuz sonlandırılmıştır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            login.Show();
        }

        // Form yüklendiğinde çalışacak olay
        private void AdminAnaSayfa_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde oturumu kapat butonunun konumunu ayarlama
            baglanti.Open();
            btnOturumKapatma.Location = new Point(this.ClientSize.Width - btnOturumKapatma.Width, 0);

            // Erkek ve kadın üye sayılarını hesapla
            int erkekSayisi = HesaplaCinsiyetSayisi(baglanti, "Kullanıcı", "Erkek");
            int kadinSayisi = HesaplaCinsiyetSayisi(baglanti, "Kullanıcı", "Kadın");
            int toplamUye = erkekSayisi + kadinSayisi;

            // Uyelik sürelerine göre toplam kazancı hesapla
            int r1 = HesaplaUyelikSuresi(baglanti, "1 AY (450 TL)", "Kullanıcı") * 450;
            int r2 = HesaplaUyelikSuresi(baglanti, "2 AY (850 TL)", "Kullanıcı") * 850;
            int r3 = HesaplaUyelikSuresi(baglanti, "6 AY (2200 TL)", "Kullanıcı") * 2200;
            int r4 = HesaplaUyelikSuresi(baglanti, "1 YIL (4500 TL)", "Kullanıcı") * 4500;
            int toplamKazanc = r1 + r2 + r3 + r4;

            // Form üzerindeki metin kutularına değerleri yerleştirme
            txtKadinUye.Text = kadinSayisi.ToString();
            txtErkekUye.Text = erkekSayisi.ToString();
            txtToplamKazanc.Text = toplamKazanc.ToString();
            txtÜyeSayısı.Text = Convert.ToString(toplamUye);

            // Veritabanı bağlantısını kapat
            baglanti.Close();
        }

        // Belirtilen cinsiyet ve roldeki üye sayısını hesapla
        static int HesaplaCinsiyetSayisi(OleDbConnection connection, string rol, string cinsiyet)
        {
            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM Information WHERE Rol = @Rol AND Cinsiyet = @Cinsiyet";

            // Parametre ekleyerek SQL komutunu oluştur
            using (OleDbCommand command = new OleDbCommand(query, connection))//using bloğu, OleDbCommand nesnesinin kullanıldıktan sonra otomatik olarak temizlenmesini sağlar.
            {
                command.Parameters.AddWithValue("@Rol", rol);
                command.Parameters.AddWithValue("@Cinsiyet", cinsiyet);

                // Sayıyı getir
                int sayi = Convert.ToInt32(command.ExecuteScalar());

                return sayi;
            }
        }

        // Belirtilen üyelik süresi ve roldeki üye sayısını hesapla
        static int HesaplaUyelikSuresi(OleDbConnection connection, string uyelikSuresi, string rol)
        {
            int kullaniciCount = 0;
            string query = "SELECT COUNT(*) FROM Information WHERE UyelikSuresi = @UyelikSuresi AND Rol=@rol";

            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UyelikSuresi", uyelikSuresi);
                command.Parameters.AddWithValue("@rol", rol);

                kullaniciCount = (int)command.ExecuteScalar();
            }

            return kullaniciCount;
        }

        // Üyelik Zamanlama Grafiği butonuna tıklandığında çalışacak olay
        private void btnVeriGoster_Click(object sender, EventArgs e)
        {
            UyelikZamanlamaGrafigi frm = new UyelikZamanlamaGrafigi();
            frm.Show();
        }

        // Üyelik Süresi Grafiği butonuna tıklandığında çalışacak olay
        private void btnUyelikSuresi_Click(object sender, EventArgs e)
        {
            UyelikSuresiGrafiği frm = new UyelikSuresiGrafiği();
            frm.Show();
        }
    }
}
