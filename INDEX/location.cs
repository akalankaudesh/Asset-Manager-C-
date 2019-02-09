using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace INDEX
{
    public partial class location : Form
    {
        string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Akalanka Udesh\Documents\TNP.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection con;
        SqlCommand cmd;
        
        public location()
        {
            InitializeComponent();
           
        }

        public void dataset()
        {
            con = new SqlConnection(connect);
            con.Open();
            cmd = new SqlCommand("select * from location", con);
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                cbosub.Items.Add(read.GetValue(1).ToString());
            }

            con.Close();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtlocation.Text == "")
            {
                MessageBox.Show("Please Fill the location", "INFORMATION", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    con = new SqlConnection(connect);
                    con.Open();
                    cmd = new SqlCommand("INSERT INTO location VALUES(@newlocation)", con);
                    if (cbosub.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@newlocation", txtlocation.Text);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@newlocation", ((cbosub.Text) + "-" + (txtlocation.Text)));
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();
                    DialogResult res = MessageBox.Show("Record Saved", "INFORMATION", MessageBoxButtons.OK);
                    if (res == DialogResult.OK)
                    {
                        dataset();
                        this.Close();
                    }
                }
                catch (Exception re)
                {
                    MessageBox.Show(re.ToString());
                }
            }
        }

        private void location_Load(object sender, EventArgs e)
        {
            dataset();
        }

        private void cbosub_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
