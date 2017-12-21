using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FraudMS
{
    public class HelperMethods
    {
        public static string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();

            //byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //StringBuilder returnValue = new StringBuilder();

            //for (int i = 0; i < hashData.Length; i++)
            //{
            //    returnValue.Append(hashData[i].ToString());
            //}

            return BitConverter.ToString(sha1.ComputeHash(Encoding.Default.GetBytes(data))).Replace("-", ""); ;//returnValue.ToString();
        }
    }
}