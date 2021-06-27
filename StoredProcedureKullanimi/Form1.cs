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

namespace StoredProcedureKullanimi
{
    public partial class Form1 : Form
    {
        SqlConnection con; SqlCommand cmd;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=NORTHWND;Integrated Security = true");
            cmd = con.CreateCommand();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BilgileriDoldur();
        }

        

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.CommandText = "usp_YoksaKategoriEkle";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@isim", tb_isim.Text);
                cmd.Parameters.AddWithValue("@aciklama", tb_aciklama.Text);
                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                con.Open();
                cmd.ExecuteNonQuery();
                int result = Convert.ToInt32(returnParameter.Value);
                if (result == -1)
                {
                    MessageBox.Show("Kategori Mevcut", "Eklenmedi");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");
            }
            finally
            {
                con.Close();
            }
            BilgileriDoldur();
        }

        private void BilgileriDoldur()
        {
            try
            {
                cmd.CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");
            }
            finally
            {
                con.Close();
            }
        }

    }
}
