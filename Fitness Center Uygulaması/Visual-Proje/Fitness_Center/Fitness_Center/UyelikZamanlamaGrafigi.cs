using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Fitness_Center
{
    public partial class UyelikZamanlamaGrafigi : Form
    {
        // Access veritabanı bağlantısı için OleDbConnection nesnesi
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");

        public UyelikZamanlamaGrafigi()
        {
            InitializeComponent();
        }

        // Zamanlama grafiğini çizmek için kullanılan metod
        private void DrawZamanlamaPieChart(float pr1, float pr2, float pr3, float pr4)
        {
            // Grafik için bir bitmap oluşturuluyor
            Bitmap bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);

            // Grafik nesnesi oluşturuluyor
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Grafik çizgilerinin özellikleri belirleniyor
                Pen p = new Pen(AdminAnaSayfa.DefaultBackColor);
                p.Color = AdminAnaSayfa.DefaultBackColor;
                p.Width = 4;

                // Grafik alanının genişliği ve yüksekliği
                int chartWidth = 250;
                int chartHeight = 250;

                // Formun genişliği ve yüksekliği
                int formWidth = ClientSize.Width;
                int formHeight = ClientSize.Height;

                // Grafik alanının ortasına yerleştirilmesi
                int chartX = formWidth - chartWidth - 650;
                int chartY = (formHeight - chartHeight) / 2;

                // Grafik alanının sınırlarını belirleyen dikdörtgen
                Rectangle rec = new Rectangle(chartX, chartY, chartWidth, chartHeight);

                // Fırçalar oluşturuluyor
                Brush b1 = new SolidBrush(Color.Red);
                Brush b2 = new SolidBrush(Color.Purple);
                Brush b3 = new SolidBrush(Color.Tan);
                Brush b4 = new SolidBrush(Color.OliveDrab);

                // Grafik arkaplanı temizleniyor
                g.Clear(AdminAnaSayfa.DefaultBackColor);

                // Sütunlar çiziliyor ve renklendiriliyor
                g.DrawPie(p, rec, 0, pr1);
                g.FillPie(b1, rec, 0, pr1);
                g.DrawPie(p, rec, pr1, pr2);
                g.FillPie(b2, rec, pr1, pr2);
                g.DrawPie(p, rec, pr1 + pr2, pr3);
                g.FillPie(b3, rec, pr1 + pr2, pr3);
                g.DrawPie(p, rec, pr1 + pr2 + pr3, pr4);
                g.FillPie(b4, rec, pr1 + pr2 + pr3, pr4);
            }

            // Formun arkaplanına oluşturulan bitmap atanıyor
            BackgroundImage = bitmap;
        }

        // Belirli bir zaman dilimindeki üyelik sayısını hesaplayan metod
        static int HesaplaZamanlamaSayisi(OleDbConnection connection, string zamanlama, string rol)
        {
            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM Information WHERE Zamanlama = @Zamanlama AND Rol = @Rol ";

            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                // Parametreler atanıyor
                command.Parameters.AddWithValue("@Zamanlama", zamanlama);
                command.Parameters.AddWithValue("@Rol", rol);

                // Sorgu çalıştırılıyor ve sonuç döndürülüyor
                int sayi = Convert.ToInt32(command.ExecuteScalar());
                return sayi;
            }
        }

        // Zamanlama grafiğini çizen metodun çağrıldığı metod
        private void PieChartıciz()
        {
            // Veritabanı bağlantısı açılıyor
            baglanti.Open();

            float r1, r2, r3, r4, total;
            // Her bir zaman dilimi için üyelik sayısı hesaplanıyor
            r1 = HesaplaZamanlamaSayisi(baglanti, "08.00-10.00", "Kullanıcı");
            r2 = HesaplaZamanlamaSayisi(baglanti, "10.00-12.00", "Kullanıcı");
            r3 = HesaplaZamanlamaSayisi(baglanti, "17.00-19.00", "Kullanıcı");
            r4 = HesaplaZamanlamaSayisi(baglanti, "19.00-21.00", "Kullanıcı");

            // Veritabanı bağlantısı kapatılıyor
            baglanti.Close();

            // Toplam üyelik sayısı hesaplanıyor
            total = r1 + r2 + r3 + r4;

            // Her bir zaman dilimi için yüzde hesaplanıyor
            float pr1, pr2, pr3, pr4;
            pr1 = (r1 / total) * 360;
            pr2 = (r2 / total) * 360;
            pr3 = (r3 / total) * 360;
            pr4 = (r4 / total) * 360;

            // Zamanlama grafiği çiziliyor
            DrawZamanlamaPieChart(pr1, pr2, pr3, pr4);
        }

        // Form yüklendiğinde çağrılan olay
        private void UyelikZamanlamaGrafigi_Load(object sender, EventArgs e)
        {
            // Zamanlama grafiğini çiz
            PieChartıciz();
        }

        // "Geri" butonuna tıklandığında çağrılan olay
        private void btnGeri_Click(object sender, EventArgs e)
        {
            // AdminAnaSayfa formunu aç
            AdminAnaSayfa admin = new AdminAnaSayfa();
            admin.Show();
        }
    }
}
