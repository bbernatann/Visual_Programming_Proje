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

namespace Fitness_Center
{
    public partial class UyelikSuresiGrafiği : Form
    {
        // Access veritabanı bağlantısı için OleDbConnection nesnesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        public UyelikSuresiGrafiği()
        {
            InitializeComponent();
        }

        // Grafik çizimini gerçekleştiren metod
        private void DrawColumnChart()
        {
            // Veritabanı bağlantısı açılıyor
            baglanti.Open();

            // Grafik için bir bitmap oluşturuluyor
            Bitmap bitmap = new Bitmap(1305, 764);

            // Grafik nesnesi oluşturuluyor
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int columnWidth = 50; // Sütun genişliği
                int columnSpacing = 10; // Sütunlar arası boşluk

                // Her bir üyelik süresi için üye sayısı hesaplanıyor
                int birAyValue = HesaplaUyelikSuresi(baglanti, "1 AY (450 TL)", "Kullanıcı");
                int ikiAyValue = HesaplaUyelikSuresi(baglanti, "2 AY (850 TL)", "Kullanıcı");
                int altiAyValue = HesaplaUyelikSuresi(baglanti, "6 AY (2200 TL)", "Kullanıcı");
                int birYilValue = HesaplaUyelikSuresi(baglanti, "1 YIL (4500 TL)", "Kullanıcı");

                // Veritabanı bağlantısı kapatılıyor
                baglanti.Close();

                // Toplam sütun genişliği hesaplanıyor
                int totalWidth = 4 * columnWidth + 3 * columnSpacing;

                // Başlangıç x koordinatı
                int startX = (1350 - totalWidth) / 2;

                // Grafik yüksekliği
                int chartHeight = 200;

                // Toplam üye sayısı hesaplanıyor
                int total = birAyValue + ikiAyValue + altiAyValue + birYilValue;

                // Her bir sütun çiziliyor
                DrawColumn(g, startX, 500 - (chartHeight * birAyValue / total), columnWidth, chartHeight * birAyValue / total, Color.Red, birAyValue);
                startX += columnWidth + columnSpacing;
                DrawColumn(g, startX, 500 - (chartHeight * ikiAyValue / total), columnWidth, chartHeight * ikiAyValue / total, Color.Black, ikiAyValue);
                startX += columnWidth + columnSpacing;
                DrawColumn(g, startX, 500 - (chartHeight * altiAyValue / total), columnWidth, chartHeight * altiAyValue / total, Color.Brown, altiAyValue);
                startX += columnWidth + columnSpacing;
                DrawColumn(g, startX, 500 - (chartHeight * birYilValue / total), columnWidth, chartHeight * birYilValue / total, Color.Blue, birYilValue);
            }

            // Formun arkaplanına oluşturulan bitmap atanıyor
            this.BackgroundImage = bitmap;
        }

        // Belirli bir üyelik süresindeki üye sayısını hesaplayan metod
        static int HesaplaUyelikSuresi(OleDbConnection connection, string uyelikSuresi, string rol)
        {
            int kullaniciCount = 0;

            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM Information WHERE UyelikSuresi = @UyelikSuresi AND Rol=@rol";

            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                // Parametreler atanıyor
                command.Parameters.AddWithValue("@UyelikSuresi", uyelikSuresi);
                command.Parameters.AddWithValue("@rol", rol);

                // Sorgu çalıştırılıyor ve sonuç döndürülüyor
                kullaniciCount = (int)command.ExecuteScalar();
            }

            return kullaniciCount;
        }

        // Sütun çizimini gerçekleştiren metod
        private void DrawColumn(Graphics g, int x, int y, int width, int height, Color color, int count)
        {
            // Fırça oluşturuluyor
            Brush brush = new SolidBrush(color);

            // Sütun çiziliyor
            g.FillRectangle(brush, x, y, width, height);

            // Sütun üzerine sayı yazılıyor
            Font font = new Font("Arial", 10);
            Brush textBrush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            g.DrawString(count.ToString(), font, textBrush, x + width / 2, y - 20, format);
        }

        // "Geri" butonuna tıklandığında çağrılan olay
        private void btnGeri_Click(object sender, EventArgs e)
        {
            // AdminAnaSayfa formunu aç
            AdminAnaSayfa admin = new AdminAnaSayfa();
            admin.Show();
        }

        // Form yüklendiğinde çağrılan olay
        private void UyelikSuresiGrafiği_Load(object sender, EventArgs e)
        {
            // Sütun grafiğini çiz
            DrawColumnChart();
        }
    }
}
