//Source: https://github.com/Diyari-Kurdi
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Kurdistan.Testing
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection();
        MySqlCommand command = new MySqlCommand();
        DataTable dt = new DataTable();
        //ConnectionString
        private readonly string conString = "server=localhost;Initial Catalog='Kurdistan_DB';user=root;port=3306;password=;";
        int totalRecords = 0;
        double isDouble = 0;
        int lastPageRecords = 0;
        int totalPagesINT = 0;
        bool isLPage = true;
        bool isRPage = false;
        int currentPage = 0;
        string Page;
        //Nawi DB w Table Bnusa
        string DBname = "Kurdistan_DB";
        string TableName = "table_Test";
        private int DecimalToString(int TotalRecords)
        {
            decimal dblNumber = Convert.ToDecimal(TotalRecords) / 10;
            string dblNumberAsString = dblNumber.ToString();
            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
            if (regex.IsMatch(dblNumberAsString))
            {
                return Convert.ToInt32(regex.Match(dblNumberAsString).Value);
            }
            else
            {
                return 0;
            }
        }

        public Form1()
        {
            InitializeComponent();
            totalRecords = CountingRecords();
            isDouble = Convert.ToDouble(totalRecords) / 10;
            if (isDouble.ToString().Contains("."))
            {
                currentPage = totalRecords / 10 + 1;
                totalPagesINT = totalRecords / 10 + 1;
            }
            else
            {
                currentPage = totalRecords / 10;
                totalPagesINT = totalRecords / 10;
            }

            lastPageRecords = DecimalToString(totalRecords);
        }

        private void SetText()
        {
            if (totalPagesINT < 10 && currentPage < 10)
                label1.Text = currentPage + " : " + totalPagesINT;
            else if (totalPagesINT > 10 && currentPage < 10)
                label1.Text = currentPage + " : " + totalPagesINT;
            else if (totalPagesINT < 10 && currentPage > 10)
                label1.Text = currentPage + " : " + totalPagesINT;
            else if (totalPagesINT > 10 && currentPage > 10)
                label1.Text = currentPage + " : " + totalPagesINT;
        }

        internal DataTable CustomSelect(string columnName, int L = 0, int R = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Clear();
                connection.ConnectionString = conString;
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT " + columnName + " FROM " + DBname + "." + TableName + " order by id desc limit " + L + "," + R + ";";

                MySqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        internal int CountingRecords()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Clear();
                connection.ConnectionString = conString;
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(id) AS id FROM " + DBname + "." + TableName + ";";

                MySqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            if (isDouble.ToString().Contains("."))
            {
                dgv.DataSource = CustomSelect("*", 0, lastPageRecords);
            }
            else
            {
                dgv.DataSource = CustomSelect("*", 0, 10);
            }
            SetText();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPage == totalPagesINT || currentPage == totalPagesINT - 1)
            {
                isRPage = true;
            }
            if (currentPage != totalPagesINT)
            {
                if (isDouble.ToString().Contains("."))
                {

                    if (isRPage == true)
                    {
                        dgv.DataSource = CustomSelect("*", 0, lastPageRecords);
                        isRPage = false;
                    }
                    else
                    {
                        if (currentPage == 1)
                        {
                            Page = lastPageRecords.ToString();
                        }
                        else
                        {
                            Page = "1" + lastPageRecords.ToString();
                        }
                        dgv.DataSource = CustomSelect("*", Convert.ToInt32(Page), 10);
                    }
                    currentPage++;
                }
                else
                {
                    if (currentPage == 1)
                    {
                        Page = lastPageRecords.ToString();
                    }
                    else
                    {
                        Page = "1" + lastPageRecords.ToString();
                    }
                    dgv.DataSource = CustomSelect("*", Convert.ToInt32(Page), 10);
                    currentPage++;
                }
                SetText();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentPage == totalPagesINT)
            {
                isLPage = true;
            }
            if (currentPage != 1)
            {
                if (isDouble.ToString().Contains("."))
                {
                    currentPage--;
                    if (isLPage == true)
                    {
                        dgv.DataSource = CustomSelect("*", lastPageRecords, 10);
                        isLPage = false;
                    }
                    else
                    {
                        string Page = currentPage + lastPageRecords.ToString();
                        dgv.DataSource = CustomSelect("*", Convert.ToInt32(Page), 10);
                    }
                }
                else
                {
                    currentPage--;
                    string Page = currentPage + lastPageRecords.ToString();
                    dgv.DataSource = CustomSelect("*", Convert.ToInt32(Page), 10);
                }
                SetText();
            }
        }
    }
}
