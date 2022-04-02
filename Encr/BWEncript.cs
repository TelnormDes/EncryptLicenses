using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Encr
{
    public partial class BWEncript : Form
    {
        public BWEncript()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //Traemos los valores de las fechas a variables y las encriptamos
            string StartDate = txtStartDate.Value.ToString();
            string EndDate = txtEndDate.Value.ToString();
            string encryptdates = StartDate + ";" + EndDate;
            string Encrypt = BWEncript.Encrypt(encryptdates);
            
            //Con el encript lo mostramos en el texbox
            txtEncryptkey.Text = Encrypt;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

            string textvalues = txtEncryptkey.Text;
            Clipboard.Clear();    //Clear if any old value is there in Clipboard        
            Clipboard.SetText(textvalues); //Copy text to Clipboard


        }

        #region Encrypt

        private const string initVector = "TelnormDES#2018V";
        private const string CryptPassword = "3MalduGino42018V";
        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;
        public static string Encrypt(string plainText)
        {
            try
            {
                if (plainText.Equals("") || plainText == null)
                    return "";
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(CryptPassword, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public static string Decrypt(string cipherText)
        {
            try
            {
                if (cipherText.Equals("") || cipherText == null)
                    return "";
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(CryptPassword, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        #endregion

        
    }
}
