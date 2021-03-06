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
    public partial class ProductsScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\abusc\OneDrive\Dokumente\Pro-Natur Biomarkt GmbH.mdf;Integrated Security = True; Connect Timeout = 30");
        private int lastSelectedProductKey;

        public ProductsScreen()
        {
            InitializeComponent();
            ShowProducts();

        }
       
        private void btnProductSave_Click(object sender, EventArgs e)
        {
            if(textBoxPrice.Text == ""
                ||textBoxProductName.Text == ""
                ||textBoxBrand.Text == ""
                ||comboBoxCategory.Text == "") {
                MessageBox.Show("Bitte fülle alle Werte aus!");
                return; }
            string productName = textBoxProductName.Text;
            string productBrand = textBoxBrand.Text;
            string productCategory = comboBoxCategory.Text;
            string productPrice = textBoxPrice.Text;

            string query = string.Format("insert into Products values('{0}','{1}','{2}','{3}')", productName, productBrand, productCategory, productPrice);
            ExecuteQuery(query);

            ClearAllFields();
            ShowProducts();
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return;
            }
            string productName = textBoxProductName.Text;
            string productBrand = textBoxBrand.Text;
            string productCategory = comboBoxCategory.Text;
            string productPrice = textBoxPrice.Text;

            string query = string.Format("update Products set Name='{0}', Brand='{1}', Category='{2}', Price='{3}' where Id={4};"
                , productName, productBrand, productCategory, productPrice, lastSelectedProductKey);
            ExecuteQuery(query);

            ClearAllFields();
            ShowProducts();
        }

        private void btnProductClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if(lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return ;
            }
            string query = string.Format("delete from Products where Id={0};", lastSelectedProductKey);
            ExecuteQuery(query);

            ClearAllFields();
            ShowProducts();
        }
        private void ShowProducts()
        {
            //Start
            databaseConnection.Open();
            string query = "select * from Products";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, databaseConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            productsDGV.DataSource = dataSet.Tables[0];
            productsDGV.Columns[0].Visible = false;

            databaseConnection.Close();
        }
        private void ClearAllFields()
        {
            textBoxProductName.Text = "";
            textBoxBrand.Text = "";
            textBoxPrice.Text = "";
            comboBoxCategory.Text = "";
            comboBoxCategory.SelectedItem = null;
        }

        private void textBoxBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void productsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Ich wurde gedrückt!");
            textBoxProductName.Text = productsDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBoxBrand.Text = productsDGV.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxCategory.Text = productsDGV.SelectedRows[0].Cells[3].Value.ToString();
            textBoxPrice.Text = productsDGV.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectedProductKey = (int)productsDGV.SelectedRows[0].Cells[0].Value;
        }
        private void ExecuteQuery(string query)
        {
            databaseConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();

        }

        private void ProductsScreen_Load(object sender, EventArgs e)
        {

        }

        private void ProductsScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainMenuScreen mainMenuScreen = new MainMenuScreen();
            mainMenuScreen.Show();
        }
    }
}
