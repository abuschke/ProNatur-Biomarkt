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

namespace ProNatur_Biomarkt_GmbH
{
    public partial class InvoiceScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\abusc\OneDrive\Dokumente\Pro-Natur Biomarkt GmbH.mdf;Integrated Security = True; Connect Timeout = 30");
        private int lastSelectedProductKey;

        public InvoiceScreen()
        {
            InitializeComponent();
            ShowInvoices();
        }

        private void InvoiceScreen_Load(object sender, EventArgs e)
        {
            
        }
        private void ShowInvoices()
        {
            databaseConnection.Open();
            string query = "select * from Invoices";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, databaseConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            invoiceDGV.DataSource = dataSet.Tables[0];
            invoiceDGV.Columns[0].Visible = false;

            databaseConnection.Close();

        }
        private void ClearAllInvoiceFields()
        {
            textInvoiceNumber.Text = "";
            textInvoiceConsignee.Text = "";
            textInvoiceItems.Text = "";
            textInvoiceAmount.Text = "";
        }
        private void ExecuteQuery(string query)
        {
            databaseConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();

        }

        private void invoiceDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textInvoiceNumber.Text = invoiceDGV.SelectedRows[0].Cells[1].Value.ToString();
            textInvoiceConsignee.Text = invoiceDGV.SelectedRows[0].Cells[2].Value.ToString();
            textInvoiceItems.Text = invoiceDGV.SelectedRows[0].Cells[3].Value.ToString();
            textInvoiceAmount.Text = invoiceDGV.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectedProductKey = (int)invoiceDGV.SelectedRows[0].Cells[0].Value;
        }

        private void btnInvoiceSave_Click(object sender, EventArgs e)
        {
            

            string InvoiceNumber = textInvoiceNumber.Text;
            string Consignee = textInvoiceConsignee.Text;
            string InvoiceItems = textInvoiceItems.Text;
            string Amount = textInvoiceAmount.Text;

            if(InvoiceNumber == ""
                ||Consignee == ""
                ||InvoiceItems == ""
                ||Amount == "")
            {
                MessageBox.Show("Bitte fülle alle Werte aus.");
                return;
            }

            string query = string.Format("insert into Invoices values ('{0}','{1}','{2}','{3}')"
                , InvoiceNumber, Consignee, InvoiceItems, Amount);

            ExecuteQuery(query);

            ClearAllInvoiceFields();
            ShowInvoices();
        }

        private void btnInvoiceEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle erst eine Rechnung aus.");
                return;
            }
                string InvoiceNumber = textInvoiceNumber.Text;
                string Consignee = textInvoiceConsignee.Text;
                string Items = textInvoiceItems.Text;
                string Amount = textInvoiceAmount.Text;

                string query = string.Format("update Invoices set InvoiveNumber='{0}', Consignee='{1}', Items='{2}', Amount='{3}' where Id={4};"
                , InvoiceNumber, Consignee, Items, Amount, lastSelectedProductKey);
                ExecuteQuery(query);

                ClearAllInvoiceFields();
                ShowInvoices();
            
        }

        private void btnInvoiceClear_Click(object sender, EventArgs e)
        {
            ClearAllInvoiceFields();
        }

        private void btnInvoiceDelete_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle erst eine Rechnung aus.");
                return;
            }
            string query = string.Format("delete from Invoices where Id={0}",lastSelectedProductKey);
            ExecuteQuery(query);

            ClearAllInvoiceFields();
            ShowInvoices();
        }

        private void InvoiceScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainMenuScreen mainMenuScreen = new MainMenuScreen();
            mainMenuScreen.Show();
        }
    }
}
