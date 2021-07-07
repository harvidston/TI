using System;
using System.IO;
using System.Windows.Forms;
using System.Numerics;
using System.Linq;

namespace TI_3task
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Encrypt_Btn_Click(object sender, EventArgs e)
        {
            if (!IsFieldsFilled())
            {
                MessageBox.Show("Please, fill all input fields", "Warning", MessageBoxButtons.OKCancel, 
                                MessageBoxIcon.Warning);
                return;
            }

            if (!IsEncryptFieldsFilledCorrectly())
            {
                return;
            }

            EncryptFile();
        }

        private void EncryptFile()
        {
            byte[] message = File.ReadAllBytes(openFileDialog.FileName);
            BigInteger p = BigInteger.Parse(p_textBox.Text);
            BigInteger q = BigInteger.Parse(q_textBox.Text);
            BigInteger closedKey = BigInteger.Parse(closedKey_textBox.Text);
            BigInteger eulerFunc = Helper.CalcEulerFunc(p, q);
            BigInteger key = Helper.CalcMultiplicativeInverseKey(eulerFunc, closedKey);
            ushort[] result = new ushort[message.Length];
            try
            {
                result = RSAEncryptor.Encrypt(message, key, p * q);
            }
            catch(ArithmeticException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string resultText = "";
            foreach (var num in result)
            {
                resultText += num.ToString() + " ";
            }

            result_textBox.Text = resultText;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, Helper.ToByteArray(result));
            }
        }

        private bool IsEncryptFieldsFilledCorrectly()
        {
            BigInteger p = BigInteger.Parse(p_textBox.Text);
            BigInteger q = BigInteger.Parse(q_textBox.Text);
            if (!Helper.MillerRabinTest(p, 10) || !Helper.MillerRabinTest(q, 10))
            {
                MessageBox.Show("p and q parameters must be PRIME", "Error", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return false;
            }

            if (!IsCorrectCLosedKey(p, q))
            {
                return false;
            }

            if (p * q < 256)
            {
                MessageBox.Show("p * q must be higher than 255, because 255 symbols can be ciphered", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool IsCorrectCLosedKey(BigInteger p, BigInteger q)
        {
            BigInteger eulerFunction = Helper.CalcEulerFunc(p, q);
            BigInteger closedKey = BigInteger.Parse(closedKey_textBox.Text);
            if ((closedKey <= 1 || closedKey > eulerFunction) || Helper.GCD(closedKey, eulerFunction) != 1)
            {
                MessageBox.Show("Closed key must be:\n1 < closed key < euler function\n" +
                                "GCD(closed key, euler function) = 1");
                return false;
            }

            return true;
        }

        private bool IsFieldsFilled()
        {
            if (p_textBox.Text == "" || q_textBox.Text == "" || closedKey_textBox.Text == "" || 
                file_textBox.Text == "")
            {
                return false;
            }

            return true;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Decrypt_Btn_Click(object sender, EventArgs e)
        {
            if (r_textBox.Text == "" || closedKey_textBox.Text == "" || file_textBox.Text == "")
            {
                MessageBox.Show("Please, fill all input fields for decrypt", "Warning", MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Warning);
                return;
            }

            if (!IsOutputFieldsFilledCorrectly())
            {
                return;
            }

            DecryptFile();
        }

        private bool IsOutputFieldsFilledCorrectly()
        {
            BigInteger r = BigInteger.Parse(r_textBox.Text);
            if (!IsRHasTwoPrimeDividers(r))
            {
                MessageBox.Show("Parameter r must be the multiplication of two primes", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            BigInteger[] dividers = GetRDividers(r);
            if (!IsCorrectCLosedKey(dividers[0], dividers[1]))
            {
                return false;
            }

            if (r < 256)
            {
                MessageBox.Show("r parameter must be higher than 255, because 255 symbols can be ciphered", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private BigInteger[] GetRDividers(BigInteger r)
        {
            int index = 0;
            BigInteger[] dividers = new BigInteger[2];
            for (BigInteger i = 2; i < r / 2; i++)
            {
                if (r % i == 0)
                {
                    dividers[index] = i;
                    index++;
                }
            }

            return dividers;
        }

        private bool IsRHasTwoPrimeDividers(BigInteger r)
        {
            int counter = 0;
            for(BigInteger i = 2; i < r / 2; i++)
            {
                if (r % i == 0)
                {
                    counter++;
                    if(!Helper.MillerRabinTest(i, 10))
                    {
                        return false;
                    }
                }
            }

            if (counter == 2)
            {
                return true;
            }

            return false;
        }

        private void DecryptFile()
        {
            byte[] message = File.ReadAllBytes(file_textBox.Text);
            BigInteger key = BigInteger.Parse(closedKey_textBox.Text);
            BigInteger r = BigInteger.Parse(r_textBox.Text);
            byte[] result = new byte[message.Length / 2];
            try
            {
                result = RSAEncryptor.Decrypt(message, key, r);
            }
            catch (ArithmeticException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string resultText = "";
            foreach (var num in result)
            {
                resultText += num.ToString() + " ";
            }

            result_textBox.Text = resultText;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, result);
            }
        }

        private void ParamTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = new string
                (
                    textBox.Text
                           .Where
                            (ch =>
                                ch == '0' || ch == '1' || ch == '2' || ch == '3' ||
                                ch == '4' || ch == '5' || ch == '6' || ch == '7' ||
                                ch == '8' || ch == '9'
                            )
                           .ToArray()
                );
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file_textBox.Text = openFileDialog.FileName;
            }
        }

        private void result_Lbl_Click(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }
    }
}
