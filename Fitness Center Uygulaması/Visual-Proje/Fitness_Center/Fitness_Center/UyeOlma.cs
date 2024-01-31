using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Fitness_Center
{
    public partial class UyeOlma : Form
    {
        public UyeOlma()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\BERNA TAN\\Documents\\VisualProject.accdb\"");


        private void btnUyeOl_Click_1(object sender, EventArgs e)
        {
            if (txtAd.Text == "" ||
                txtSoyad.Text == "" ||
                txtKullanıcıAdı.Text == "" ||
                txtSifre.Text == "" ||
                txtYas.Text == "" ||
                comboBoxCinsiyet.Text == "" ||
                comboBoxUyelikSuresi.Text == "" ||
                comboBoxZamanlama.Text == "")
            {
                MessageBox.Show("Tüm bilgileri doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    baglanti.Open();

                    OleDbCommand kontrol = new OleDbCommand("SELECT COUNT(*) FROM Information WHERE KullanıcıAdı = @p3", baglanti);
                    kontrol.Parameters.AddWithValue("@p3", txtKullanıcıAdı.Text);
                    int kullaniciSayisi = Convert.ToInt32(kontrol.ExecuteScalar());
                    if (kullaniciSayisi > 0)
                    {
                        MessageBox.Show("Bu kullanıcı adı kullanılmaktadır. Lütfen başka bir kullanıcı adı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        baglanti.Close();
                    }
                    else
                    {
                        // OleDbCommand nesnesini oluştururken bağlantıyı atayın
                        OleDbCommand kaydet = new OleDbCommand("INSERT INTO Information (Ad,Soyad,KullanıcıAdı,Sifre,Yas,Cinsiyet,UyelikSuresi,Zamanlama,Rol)" +
                         " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", baglanti);
                        kaydet.Parameters.AddWithValue("@p1", txtAd.Text);
                        kaydet.Parameters.AddWithValue("@p2", txtSoyad.Text);
                        kaydet.Parameters.AddWithValue("@p3", txtKullanıcıAdı.Text);
                        kaydet.Parameters.AddWithValue("@p4", txtSifre.Text);
                        kaydet.Parameters.AddWithValue("@p5", Convert.ToString(txtYas.Text));
                        kaydet.Parameters.AddWithValue("@p6", comboBoxCinsiyet.SelectedItem.ToString());
                        kaydet.Parameters.AddWithValue("@p7", comboBoxUyelikSuresi.SelectedItem.ToString());
                        kaydet.Parameters.AddWithValue("@p8", comboBoxZamanlama.SelectedItem.ToString());
                        kaydet.Parameters.AddWithValue("@p9", "Kullanıcı");
                        kaydet.ExecuteNonQuery();

                        MessageBox.Show("Ödeme sayfasına yönlendiriliyorsunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        baglanti.Close();
                        Odeme frm = new Odeme(txtKullanıcıAdı.Text, txtAd.Text, txtSoyad.Text, comboBoxUyelikSuresi.SelectedItem.ToString());
                        Hesabım hesabım = new Hesabım(txtKullanıcıAdı.Text, txtSifre.Text, txtAd.Text, txtSoyad.Text, Convert.ToInt32(txtYas.Text), comboBoxCinsiyet.SelectedIndex.ToString(), comboBoxUyelikSuresi.SelectedIndex.ToString(), comboBoxZamanlama.SelectedIndex.ToString());
                        frm.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}