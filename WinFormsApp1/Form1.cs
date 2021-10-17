using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using ClosedXML.Excel;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        int index;
        Bitmap bitmap;
        private const string mySecurityKey = "L4S$aX1B8m%!!";
        public Form1()
        {
            InitializeComponent();
            rdMale.Checked = true;
            rdVip.Checked = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        public static string Encrypt(string encrypttext)
        {
            byte[] MyEncryptedArray = UTF8Encoding.UTF8.GetBytes(encrypttext);

            MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(mySecurityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService.CreateEncryptor();

            byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyEncryptedArray, 0,MyEncryptedArray.Length);

            MyTripleDESCryptoService.Clear();

            return Convert.ToBase64String(MyresultArray, 0,MyresultArray.Length);
        }

        public static string Decrypt(string decrypttext)
        {
            byte[] MyDecryptArray = Convert.FromBase64String(decrypttext);

            MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(mySecurityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService.CreateDecryptor();

            byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyDecryptArray, 0,MyDecryptArray.Length);

            MyTripleDESCryptoService.Clear();

            return UTF8Encoding.UTF8.GetString(MyresultArray);
        }
        private void dtgUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                index = e.RowIndex;
                DataGridViewRow selectedRow = dtgUser.Rows[index];
                txtName.Text = selectedRow.Cells[0].Value.ToString();
                txtPhone.Text = selectedRow.Cells[1].Value.ToString();
                string gender = selectedRow.Cells[2].Value.ToString();
                chkGenderText(gender);
                rtAddress.Text = selectedRow.Cells[3].Value.ToString();
                dtpDate.Text = selectedRow.Cells[4].Value.ToString();
                string type = selectedRow.Cells[5].Value.ToString();
                chkTypeText(type);
            }
            catch (Exception)
            {
                
            }       
        }
        string chkGender()
        {
            string gender = "Male";
            if (rdMale.Checked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            return gender;
        }
        void chkGenderText(string gender)
        {
            if (gender == "Male")
            {
                rdMale.Checked = true;
            }
            else
            {
                rdFemale.Checked = true;
            }
        }
        string chkType()
        {
            string type = "VIP";
            if (rdVip.Checked == true)
            {
                type = "VIP";
            }
            else
            {
                type = "Normal";
            }
            return type;
        }
        void chkTypeText(string type)
        {
            if (type == "VIP")
            {
                rdVip.Checked = true;
            }
            else
            {
                rdNormal.Checked = true;
            }
        }
        void InputDataGrid(int i)
        {
            try
            {
                if (i <= 10000)
                {
                    string Name = txtName.Text.ToString();
                    string Address = rtAddress.Text.ToString();
                    string Phone = txtPhone.Text.ToString();
                    string Birthday = dtpDate.Text.ToString();
                    string Gender = chkGender();
                    string Type = chkType();

                    if (Name!="")
                    {
                        i = dtgUser.Rows.Add();
                        dtgUser.Rows[i].Cells[0].Value = Name;
                        int number = -1;
                        if (!int.TryParse(Phone, out number))
                        {
                            label7.Text = "Should input a phone number";
                        }
                        else
                        {
                            dtgUser.Rows[i].Cells[1].Value = Phone;

                        }
                        dtgUser.Rows[i].Cells[2].Value = Gender;
                        dtgUser.Rows[i].Cells[3].Value = Address;
                        dtgUser.Rows[i].Cells[4].Value = Birthday;
                        dtgUser.Rows[i].Cells[5].Value = Type;
                        label7.Text = "";
                    }
                    else
                    {
                        label7.Text = "You need to input Name";
                    }
                }
                txtName.Clear();
                rtAddress.Clear();
                txtPhone.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Error while input user!!!", "Error");
            }          
        }
        void Delete()
        {
            try
            {
                dtgUser.Rows.RemoveAt(index);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while delete user!!!", "Error");
            }              
                      
        }
        void Save()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (txtName.Text != "" && txtPhone.Text != "")
                    {
                        TextWriter writer = new StreamWriter(saveFileDialog1.FileName);
                        for (int i = 0; i < dtgUser.Rows.Count - 1; i++) // rows
                        {
                            for (int j = 0; j < dtgUser.Columns.Count; j++) // columns
                            {
                                if (j == dtgUser.Columns.Count - 1) // if last column
                                {
                                    writer.Write(Encrypt(dtgUser.Rows[i].Cells[j].Value.ToString()));
                                }
                                else
                                    writer.Write(Encrypt(dtgUser.Rows[i].Cells[j].Value.ToString()) + "|");
                            }
                            writer.WriteLine("");
                        }
                        writer.Close();
                        MessageBox.Show("Data Exported","Export Successful");
                    }
                    else
                    {
                        MessageBox.Show("Missing Username or Phone","Error");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error");
                }
            }
        }

        void Update()
        {
            string Name = txtName.Text.ToString();
            string Address = rtAddress.Text.ToString();
            string Phone = txtPhone.Text.ToString();
            string Birthday = dtpDate.Text.ToString();
            string Gender = chkGender();
            string Type = chkType();

            DataGridViewRow newData = dtgUser.Rows[index];
            newData.Cells[0].Value = Name;
            newData.Cells[1].Value = Phone;
            newData.Cells[2].Value = Gender;
            newData.Cells[3].Value = Address;
            newData.Cells[4].Value = Birthday;
            newData.Cells[5].Value = Type;

            label6.Text = "Update Successfully";
            
            //--------Timer----------//
            var t = new Timer();
            t.Interval = 2000;
            t.Tick += (s, e) =>
            {
                label6.Hide();
            };
            t.Start();
        }

        void LoadFile()
        {
            dtgUser.Rows.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                string[] values;

                for (int i = 0; i < lines.Length; i++)
                {
                    values = lines[i].ToString().Split('|');
                    string[] store = new string[values.Length];
                    for (int x = 0; x < values.Length; x++)
                    {
                        string decrypt = Decrypt(values[x].ToString());
                        store[x] = decrypt;
                    }
                    
                    string[] row = new string[store.Length];

                    for (int j = 0; j < store.Length; j++)
                    {
                        
                        row[j] = store[j].Trim();
                    }
                    dtgUser.Rows.Add(row);
                }
                MessageBox.Show("Load Successfully!!!","Load");
            }          
        }
        private void btnInput_Click(object sender, EventArgs e)
        {
            int i = 1;
            InputDataGrid(i);
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {           
            Delete();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            switch (MessageBox.Show("You need to save the data. Click Yes to Exit", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }               
        }

        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int height = dtgUser.Height;
            dtgUser.Height = dtgUser.RowCount * dtgUser.RowTemplate.Height * 2;
            bitmap = new Bitmap(dtgUser.Width, dtgUser.Height);
            dtgUser.DrawToBitmap(bitmap, new Rectangle(0, 0, dtgUser.Width, dtgUser.Height));
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
            dtgUser.Height = height;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
                dtgUser.SelectAll();
                DataObject data = dtgUser.GetClipboardContent();
                if (data!=null)
                {
                    Clipboard.SetDataObject(data);
                }
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook;
                Microsoft.Office.Interop.Excel.Worksheet xlsheet;
                object miseddata = System.Reflection.Missing.Value;
                xlWbook = xlapp.Workbooks.Add(miseddata);

                xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
                xlr.Select();

                xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
