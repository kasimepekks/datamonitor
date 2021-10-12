using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
namespace Tools
{
  public static  class FileOperator
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        public static class FileDelete
        {
            public static void DeleteFile(string file)
            {
                File.Delete(file);
            }

        }
        /// <summary>
        /// 定位哪个文件夹并返回其文件路径
        /// </summary>
        /// <returns></returns>
        ///  确认是哪个文件路径,date是日期
        public static string DatetoName(string date)
        {
           
            string datereplace = date.Replace("-", "_");//把日期格式2019-07-01改为2019_07_01
          
            return datereplace;
        }
        //public static string LastDatetoName(string date, int i)
        //{
        //    string path = @"E:\RealTimeData\";
        //    string absolutepath = @"\\DESKTOP-SHGECD9\input\";
        //    string[] pathgroup = { path, absolutepath };
        //    string datenormal = date;
        //    string datereplace = datenormal.Replace("-", "_");
        //    string datefiledirectorypath = pathgroup[i] + datereplace;

        //    return datefiledirectorypath;
        //}
        //返回目标文件夹下的所有csv文件并按文件名排序,用于历史数据拼接
        public static FileInfo[] Isfileexist(string filePath)
        {
            DirectoryInfo root = new DirectoryInfo(filePath);
            if (!root.Exists)
            {
                root.Create();
            }
            FileInfo[] files = root.GetFiles("*.csv");
            if (files.Length > 0)
            {
                var fc = new FileComparer();
                Array.Sort(files, fc);
            }

            return files;

        }

        //获取最近创建的文件名和创建时间
        //如果没有指定类型的文件，返回null
        public static FileTimeInfo GetLatestFileTimeInfo(string dir, string ext)
        {

            List<FileTimeInfo> list = new List<FileTimeInfo>();
            DirectoryInfo d = new DirectoryInfo(dir);
            if (!d.Exists)
            {
                try
                {
                    d.Create();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
            if (d.GetFiles().Length > 0)
            {
                foreach (FileInfo file in d.GetFiles())
                {
                    if (file.Extension.ToUpper() == ext.ToUpper())
                    {
                        list.Add(new FileTimeInfo()
                        {
                            FullFileName = file.FullName,
                            FileCreateTime = file.CreationTime,
                            FileName = file.Name
                        });
                    }
                }
                var f = from x in list
                        orderby x.FileCreateTime
                        select x;
                return f.LastOrDefault();
            }
            else
            {
                return new FileTimeInfo() {
                    FullFileName = "default",
                    FileCreateTime = DateTime.Today,
                    FileName = "default"
                };
            }
        }

        /// <summary>
        /// 生成ID（数字和字母混和） 
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

    }
}
