using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20200617_LinqToSql_Giris
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NorthwindDataContext ctx = new NorthwindDataContext();

        private void Form1_Load(object sender, EventArgs e)
        {

            var sonuc = from urun in ctx.Urunlers
                        join kategori in ctx.Kategorilers
                        on urun.KategoriID equals kategori.KategoriID
                        select new
                        {
                            ÜrünId = urun.UrunID,
                            ÜrünAdı = urun.UrunAdi,
                            Fiyat = urun.Fiyat,
                            Stok = urun.Stok,
                            KategoriAdı = kategori.KategoriAdi
                        };
            //Kolanların ismini değiştirebildik.
            //Kategori Adını gösterebildik.
            //İstediğimiz kolonları gösterebildik.
            dataGridView1.DataSource = sonuc;
            cbxKategori.DisplayMember = "KategoriAdi";
            cbxKategori.ValueMember = "KategoriID";
            cbxTedarikci.DisplayMember = "SirketAdi";
            cbxTedarikci.ValueMember = "TedarikciID";
            cbxKategori.DataSource = ctx.Kategorilers;
            cbxTedarikci.DataSource = ctx.Tedarikcilers;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urunler urun = new Urunler();
            urun.UrunAdi = txtUrunAdi.Text;
            urun.Fiyat = nudFiyat.Value;
            urun.Stok = Convert.ToInt16(nudStok.Value);
            urun.KategoriID = (int)cbxKategori.SelectedValue;
            urun.TedarikciID = (int)cbxTedarikci.SelectedValue;

            ctx.Urunlers.InsertOnSubmit(urun);
            ctx.SubmitChanges();
            dataGridView1.DataSource = ctx.Urunlers;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            int urunId = (int)dataGridView1.CurrentRow.Cells["UrunID"].Value;
            
            Urunler p = ctx.Urunlers.SingleOrDefault(urun=>urun.UrunID == urunId);
            ctx.Urunlers.DeleteOnSubmit(p);
            ctx.SubmitChanges();
            dataGridView1.DataSource = ctx.Urunlers;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            txtUrunAdi.Text = row.Cells["UrunAdi"].Value.ToString();
            nudFiyat.Value = Convert.ToDecimal(row.Cells["Fiyat"].Value);
            nudStok.Value = Convert.ToDecimal(row.Cells["Stok"].Value);
            cbxKategori.SelectedValue = row.Cells["KategoriID"].Value;
            cbxTedarikci.SelectedValue = row.Cells["TedarikciID"].Value;
            txtUrunAdi.Tag = row.Cells["UrunID"].Value;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = (int)txtUrunAdi.Tag;
            Urunler p = ctx.Urunlers.SingleOrDefault(x => x.UrunID == id);
            p.UrunAdi = txtUrunAdi.Text;
            p.Fiyat = nudFiyat.Value;
            p.Stok = Convert.ToInt16(nudStok.Value);
            p.KategoriID = (int)cbxKategori.SelectedValue;
            p.TedarikciID = (int)cbxTedarikci.SelectedValue;
            ctx.SubmitChanges();
            dataGridView1.DataSource = ctx.Urunlers;
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            //Ürünlerdeki ürün adı txtara'daki bilgiyi içeriyorsa dgw'nin datasource'da göster.
            dataGridView1.DataSource = ctx.Urunlers.Where(x => x.UrunAdi.Contains(txtAra.Text));
        }
    }
}
