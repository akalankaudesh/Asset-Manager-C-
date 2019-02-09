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
    public partial class partner : Form
    {
        string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Akalanka Udesh\Documents\TNP.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection con;
        SqlCommand cmd;

        public void set()
        {
            con = new SqlConnection(connect);
            con.Open();
            SqlDataAdapter ak = new SqlDataAdapter("select isnull(max(cast(Parner_ID as int)),0)+1 from partner", con);
            DataTable dt = new DataTable();
            ak.Fill(dt);
            txtpid.Text = (dt.Rows[0][0]).ToString();
            con.Close();
        }
        public partner()
        {
            InitializeComponent();
            
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcompany.Clear();
            txtemail.Clear();
            txttelephone.Clear();
           
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(connect);
                con.Open();
                cmd = new SqlCommand("INSERT INTO partner VALUES(@Parner_ID,@company,@telephone,@email)", con);
                cmd.Parameters.AddWithValue("@parner_ID",int.Parse(txtpid.Text));
                cmd.Parameters.AddWithValue("@company", txtcompany.Text);
                cmd.Parameters.AddWithValue("@telephone", txttelephone.Text);
                cmd.Parameters.AddWithValue("@email", txtemail.Text);
                cmd.ExecuteNonQuery();
                DialogResult res = MessageBox.Show("Record Saved, Do you want to Add More!","Information", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if(res==DialogResult.Yes)
                {
                    txtcompany.Clear();
                    txtemail.Clear();
                    txttelephone.Clear();
                    set();
                }
                else
                {
                    this.Close();
                }
                con.Close();
             

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtpid_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void partner_Load(object sender, EventArgs e)
        {
            set();
        }
    }
}
