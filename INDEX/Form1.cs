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
using System.IO;
using Microsoft.PointOfService;




namespace INDEX
{
    public partial class Form1 : Form
    {
        string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TNP.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection con;
        SqlCommand cmd;
        SqlCommand shit = new SqlCommand();
        DataTable qwe;
        DataTable service;
        DataTable op;

        private PosExplorer explorer;
        private Scanner scanner;

        void explorer_DeviceAddedEvent(object sender, DeviceChangedEventArgs e)


        {


            if (e.Device.Type == "Scanner")


            {
                scanner = (Scanner)explorer.CreateInstance(e.Device);


                scanner.Open();


                scanner.Claim(1000);


                scanner.DeviceEnabled = true;


                scanner.DataEvent += new DataEventHandler(scanner_DataEvent);


                scanner.DataEventEnabled = true;


                scanner.DecodeData = true;
            }
        }

        void scanner_DataEvent(object sender, DataEventArgs e)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            scanner.DataEventEnabled = true;
            txtsearch.Text = encoding.GetString(scanner.ScanDataLabel);
            txtserial.Text= encoding.GetString(scanner.ScanDataLabel);
            txtservice_serial.Text= encoding.GetString(scanner.ScanDataLabel);
        }

        public void Barcod()
        {

        }
        public void DataCon()
        {
           con= new SqlConnection(connection);
           con.Open();
        }

        public void conclose()
        {
            con.Close();
        }

        public void autoservice()
        {
            DataCon();
            SqlDataAdapter rrr = new SqlDataAdapter("select isnull(max(cast(service_No as int)),0)+1 from Services", con);
            DataTable d = new DataTable();
            rrr.Fill(d);
            txtse1.Text = d.Rows[0][0].ToString();
            conclose();
        }

        public Form1()
        {
            InitializeComponent();


            autoservice();
        }

        public void front()
        {
            DataCon();
            cmd = new SqlCommand("select Serial,Location,Request,Model,Warant_Start,Warant_End,Price from New_Asset", con);
            SqlDataAdapter a = new SqlDataAdapter(cmd);
            qwe = new DataTable();
            a.Fill(qwe);
            BindingSource bs = new BindingSource();
            bs.DataSource = qwe;
            dataGridView1.DataSource = qwe;
            a.Update(qwe);
            conclose();
        }


        public void ser()
        {
            DataCon();
            cmd = new SqlCommand("select service_No,Serial_No,Service_Partner,Service_In_Date,Service_Out_Date,Cost from Services", con);
            SqlDataAdapter w = new SqlDataAdapter(cmd);
            service = new DataTable();
            w.Fill(service);
            BindingSource s = new BindingSource();
            s.DataSource = service;
            dataGridView2.DataSource = service;
            w.Update(service);
            conclose();
        }

        public void serive_report()
        {
            DataCon();
            cmd = new SqlCommand("SELECT count(*) as total_records, Serial_No,sum(Cost) as tot FROM Services group by Serial_No;", con);
            SqlDataAdapter ww = new SqlDataAdapter(cmd);
            op = new DataTable();
            ww.Fill(op);
            BindingSource h = new BindingSource();
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.ColumnCount = 3;
            dataGridView3.Columns[0].HeaderText = "Serials";
            dataGridView3.Columns[0].DataPropertyName = "Serial_No";
            dataGridView3.Columns[1].HeaderText = "Total Count";
            dataGridView3.Columns[1].DataPropertyName = "total_records";
            dataGridView3.Columns[2].HeaderText = "Total Cost";
            dataGridView3.Columns[2].DataPropertyName = "tot";

            dataGridView3.DataSource = op;
            ww.Update(op);
            conclose();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            explorer = new PosExplorer(this);
            explorer.DeviceAddedEvent += new DeviceChangedEventHandler(explorer_DeviceAddedEvent);

            front();

            ser();

            serive_report();

            DataCon();
            cmd = new SqlCommand("select * from location", con);
            SqlDataReader sd = cmd.ExecuteReader();
            while (sd.Read())
            {
                cbolocation.Items.Add(sd.GetValue(1).ToString());
             
            }
            con.Close();
            DataCon();
            cmd = new SqlCommand("select * from partner", con);
            SqlDataReader ak = cmd.ExecuteReader();
            while(ak.Read())
            {
                cbopartners.Items.Add(ak.GetValue(1).ToString());
                cbovendor.Items.Add(ak.GetValue(1).ToString());
            }
            con.Close();


        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void chkreplace_CheckedChanged(object sender, EventArgs e)
        {
            if(chkreplace.Checked==true)
            {
                txtreplace.Visible = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            
            ImageConverter attached = new ImageConverter();
            byte[] attach = (byte[])attached.ConvertTo(picattachment.Image, Type.GetType("System.Byte[]")); 

            try { 
            DataCon();
            
        
                cmd = new SqlCommand("INSERT INTO New_Asset VALUES (@Serial,@Description,@Replace,@Location,@Model,@Warant_Start,@Warant_End,@Price,@Attachment,@Vendor,@Request)", con);
                cmd.Parameters.AddWithValue("@Serial", txtserial.Text);
                cmd.Parameters.AddWithValue("@Description", txtdescription.Text);
                cmd.Parameters.AddWithValue("@Replace", txtreplace.Text);
                cmd.Parameters.AddWithValue("@Location", cbolocation.Text);
                cmd.Parameters.AddWithValue("@Model", txtmodel.Text);
                cmd.Parameters.AddWithValue("@Warant_Start", dtp_start.Value.ToString());
                cmd.Parameters.AddWithValue("@Warant_End", dtp_end.Value.ToString());
                cmd.Parameters.AddWithValue("@Price", txtprice.Text);
                cmd.Parameters.AddWithValue("@Attachment", attach);
                cmd.Parameters.AddWithValue("@Vendor",cbovendor.Text);
                cmd.Parameters.AddWithValue("@Request", txtrequest.Text);
                cmd.ExecuteNonQuery();
                conclose();
               DialogResult d= MessageBox.Show("Record Successfully Saved","INFORMATION",MessageBoxButtons.OK);
                front();
                if (d == DialogResult.OK)
                {
                    txtdescription.Clear();
                    txtserial.Clear();
                    cbolocation.SelectedIndex = -1;
                    txtmodel.Clear();
                    txtreplace.Clear();
                    txtprice.Clear();
                    txtrequest.Clear();
                    picattachment.Image = null;
                    cbovendor.SelectedIndex = -1;
                    front();
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog pic = new OpenFileDialog() { Filter = "JPEG|*.jpg", Multiselect = false })
            {
                if(pic.ShowDialog()==DialogResult.OK)
                {
                    picattachment.Image = Image.FromFile(pic.FileName);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            picattachment.Image = null;
        }

        private void btnclean_Click(object sender, EventArgs e)
        {
            txtdescription.Clear();
            txtserial.Clear();
            cbolocation.SelectedIndex = -1;
            txtmodel.Clear();
            txtreplace.Clear();
            txtprice.Clear();
            txtrequest.Clear();
            picattachment.Image = null;
            cbovendor.SelectedIndex = -1;
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcost.Clear();
            cbopartners.SelectedIndex = -1;
            txtservice_serial.Clear();
            
            
        }

        private void picadd_Click(object sender, EventArgs e)
        {
           
            partner asd = new partner();
            asd.ShowDialog();
        }
        
        private void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                
                DataCon();
                
                cmd = new SqlCommand("INSERT INTO Services VALUES(@Serial_No,@Service_Partner,@Service_In_Date,@Service_Out_Date,@Cost)", con);
                cmd.Parameters.AddWithValue("@Serial_No",txtservice_serial.Text);
                cmd.Parameters.AddWithValue("@Service_Partner", cbopartners.Text);
                cmd.Parameters.AddWithValue("@Service_In_Date",DBNull.Value);
                cmd.Parameters.AddWithValue("@Service_Out_Date", dtpservice.Value.ToString());
                cmd.Parameters.AddWithValue("@Cost",DBNull.Value);
                cmd.ExecuteNonQuery();
                con.Close();
              DialogResult x= MessageBox.Show("Record Added","INFORMATION",MessageBoxButtons.OK);
                ser();
                if(x==DialogResult.OK)
                {
                    txtcost.Clear();
                    cbopartners.SelectedIndex = -1;
                    txtservice_serial.Clear();
                    autoservice();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            partner asd = new partner();
            asd.ShowDialog();
        }

        private void piclocation_Click(object sender, EventArgs e)
        {
            location loc = new location();
            loc.ShowDialog();
        }

        private void cbodate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbp3_Click(object sender, EventArgs e)
        {

        }

        private void tbp2_Click(object sender, EventArgs e)
        {

        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(qwe);
            dv.RowFilter = string.Format("Serial like '%{0}%' or Location like '%{0}%' or Request like '%{0}%'", txtsearch.Text);
            dataGridView1.DataSource = dv;
        }

        private void picsearch_Click(object sender, EventArgs e)
        {   
           if(dataGridView1.RowCount==0)
            {
                MessageBox.Show("Cannot finf entry");
            }
        }

        private void txtservice_serial_TextChanged(object sender, EventArgs e)
        {

            DataView dv = new DataView(service);
            dv.RowFilter = string.Format("Serial_No like '%{0}%'",txtservice_serial.Text);
            dataGridView2.DataSource = dv;
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount==1)
            {
                MessageBox.Show("Cannot find entry");
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataCon();
                string qu = "UPDATE Services SET Service_Partner='" + cbopartners.Text + "',Service_In_Date='" + dtpindate.Value.ToString() + "',Service_Out_Date='" + txtout.Text + "',Cost='"+ double.Parse(txtcost.Text) + "' WHERE service_No = '" + txtse1.Text+"' ";
                SqlDataAdapter af = new SqlDataAdapter(qu, con);
                af.SelectCommand.ExecuteNonQuery();
                DialogResult s = MessageBox.Show("Record updated", "Information", MessageBoxButtons.OK);
                con.Close();
               if(s==DialogResult.OK)
                {
                    txtcost.Clear();
                    cbopartners.SelectedIndex = -1;
                    txtservice_serial.Clear();
                    autoservice();
                    ser();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Refresh();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtse1.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            cbopartners.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            txtcost.Text= dataGridView2.SelectedRows[0].Cells[5].Value.ToString();
            txtin.Text= dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
            txtout.Text= dataGridView2.SelectedRows[0].Cells[4].Value.ToString();
            txtservice_serial.Text= dataGridView2.SelectedRows[0].Cells[1].Value.ToString();

        }

        private void txtserial_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(op);
            dv.RowFilter = string.Format("Serial_No like '%{0}%'", txtselect.Text);
            dataGridView3.DataSource = dv;
            


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            
        }

        private void txtse1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
