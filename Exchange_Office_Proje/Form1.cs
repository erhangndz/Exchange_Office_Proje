using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Exchange_Office_Proje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        SqlConnection baglanti = new SqlConnection("Data Source=ERHAN;Initial Catalog=DbDovizOfisi;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugun);
            string dolaralis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            LblDolarAlis.Text = dolaralis;
            string dolarsatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            LblDolarSatis.Text = dolarsatis;
            string euroalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            LblEuroAlis.Text = euroalis;
            string eurosatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            LblEuroSatis.Text = eurosatis;

           
        }

        private void BtnDolarAl_Click(object sender, EventArgs e)
        {
            txtKur.Text = LblDolarAlis.Text;
        }

        private void BtnSatisYap_Click(object sender, EventArgs e)
        {
            double kur, doviz, TL;
            kur = double.Parse(txtKur.Text);
            doviz = double.Parse(txtDoviz.Text);
            TL = kur * doviz;
            txtTL.Text = TL.ToString();
            txtKalan.Text = "";
            
                baglanti.Open();
                SqlCommand TLhesapla = new SqlCommand("select * from TblKasa", baglanti);
            SqlDataReader dr = TLhesapla.ExecuteReader();
            while (dr.Read())
            {
                
                 int dolar = int.Parse(dr[1].ToString());
                double turklirasi = double.Parse(dr[3].ToString());
                int Euro = int.Parse(dr[2].ToString());

                double kasatl = turklirasi - double.Parse(txtTL.Text);
                LblKasaTL.Text = kasatl.ToString();
                int kasadolar = dolar + int.Parse(txtDoviz.Text);
                LblKasaDolar.Text = kasadolar.ToString();
               int kasaEuro = Euro + int.Parse(txtDoviz.Text);
                LblKasaEuro.Text = kasaEuro.ToString();
            }
            baglanti.Close();
            if (radioButton1.Checked == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update Tblkasa set dolar=@p1 ,tl=@p2  where ID=1", baglanti);
                komut.Parameters.AddWithValue("@p1", int.Parse(LblKasaDolar.Text));
                komut.Parameters.AddWithValue("@p2", double.Parse(LblKasaTL.Text));
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Dolar alışı gerçekleşti ve kasa bilgileri güncellendi.");

            }
             else if (radioButton2.Checked == true)
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update tblkasa set euro=@p1 ,tl=@p2 where ID=1", baglanti);
                komut2.Parameters.AddWithValue("@p1", int.Parse(LblKasaEuro.Text));
                komut2.Parameters.AddWithValue("@p2", double.Parse(LblKasaTL.Text));
                komut2.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Euro alışı gerçekleşti ve kasa bilgileri güncellendi.");
            }

            else
            {
                txtKalan.Text = "";
            }





        }

        private void BtnDolarSat_Click(object sender, EventArgs e)
        {
            txtKur.Text = LblDolarSatis.Text;
        }

        private void BtnEuroAl_Click(object sender, EventArgs e)
        {
            txtKur.Text = LblEuroAlis.Text;
        }

        private void BtnEuroSat_Click(object sender, EventArgs e)
        {
            txtKur.Text = LblEuroSatis.Text;
        }

        private void txtKur_TextChanged(object sender, EventArgs e)
        {
            txtKur.Text = txtKur.Text.Replace(".", ",");
        }

        private void BtnTutarHesapla_Click(object sender, EventArgs e)
        {
            double kur;
            int doviz, TL;

            kur = double.Parse(txtKur.Text);
            TL = int.Parse(txtTL.Text);
            doviz = (int)(TL / kur);
            txtDoviz.Text = doviz.ToString();
            double kalan;
            kalan = TL - (doviz * kur);
            txtKalan.Text = kalan.ToString();

            baglanti.Open();
            SqlCommand TLhesapla = new SqlCommand("select * from TblKasa", baglanti);
            SqlDataReader dr = TLhesapla.ExecuteReader();
            while (dr.Read())
            {

                int dolar = int.Parse(dr[1].ToString());
                double turklirasi = double.Parse(dr[3].ToString());
                int Euro = int.Parse(dr[2].ToString());

                double kasatl = turklirasi + (double.Parse(txtTL.Text) - double.Parse(txtKalan.Text));
                LblKasaTL.Text = kasatl.ToString();
                int kasadolar = dolar - int.Parse(txtDoviz.Text);
                LblKasaDolar.Text = kasadolar.ToString();
                int kasaEuro = Euro - int.Parse(txtDoviz.Text);
                LblKasaEuro.Text = kasaEuro.ToString();
            }
            baglanti.Close();
            if (radioButton1.Checked == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update Tblkasa set dolar=@p1 ,tl=@p2  where ID=1", baglanti);
                komut.Parameters.AddWithValue("@p1", int.Parse(LblKasaDolar.Text));
                komut.Parameters.AddWithValue("@p2", double.Parse(LblKasaTL.Text));
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Dolar satışı gerçekleşti ve kasa bilgileri güncellendi.");

            }
            else if (radioButton2.Checked == true)
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update tblkasa set euro=@p1 ,tl=@p2 where ID=1", baglanti);
                komut2.Parameters.AddWithValue("@p1", int.Parse(LblKasaEuro.Text));
                komut2.Parameters.AddWithValue("@p2", double.Parse(LblKasaTL.Text));
                komut2.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Euro satışı gerçekleşti ve kasa bilgileri güncellendi.");
            }

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("select * from tblkasa", baglanti);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                int dolar = int.Parse(dr3[1].ToString());
                int Euro = int.Parse(dr3[2].ToString());
                double turklirasi = double.Parse(dr3[3].ToString());

                MessageBox.Show(" Dolar Tutarı: " + dolar.ToString() + " $  " +
                    "            Euro Tutarı: " + Euro.ToString() + " €  " +
                    "            Türk Lirası Tutarı: " + turklirasi.ToString() + " ₺   ","KASA BİLGİLERİ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
            baglanti.Close();
        }
    }
}
