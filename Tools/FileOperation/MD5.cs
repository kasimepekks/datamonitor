using System;
using System.Collections.Generic;
using System.Text;

namespace Tools
{
    public class MD5
    {

        public static string Sign(string prestr, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
    }
}
