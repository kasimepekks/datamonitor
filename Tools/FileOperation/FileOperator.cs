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
       
            public static void DeleteFile(string file)
            {
                File.Delete(file);
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
        //创建import的flag文件
        public static void CreateDataImportCompleteFlag(string filePath)
        {
           string filename=filePath+ "/DataImportCompleteFlag.txt";
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }
          
        }


       //返回目标文件夹下的所有子文件夹，不包括子文件夹的子文件夹
        public static DirectoryInfo[] GetSubDirectories(string filePath)
        {
            DirectoryInfo root = new DirectoryInfo(filePath);
            return root.GetDirectories();
        }
        //判断是否有这个txt文件，“DataImportCompleteFlag.txt”，如果有则说明这个文件夹已经处理完了返回true，没有返回false
        public static bool DataImportCompleteFlag(DirectoryInfo root)
        {
            string flag = root.FullName + "/DataImportCompleteFlag.txt";
            return File.Exists(flag);

        }
        /// <summary>
        /// 给手动导入用
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool DataImportCompleteFlag(string root)
        {
            string flag = root + "/DataImportCompleteFlag.txt";
            return File.Exists(flag);

        }

        //判断是否有这个txt文件，“Done.txt”，如果有则说明数据已经上传完了返回true，没有返回false
        public static bool DataTransferCompleteFlag(DirectoryInfo root)
        {
            string flag = root.FullName + "/"+root.Name+"_Done.txt";
            return File.Exists(flag);

        }
        //给手动导入用
        public static bool DataTransferCompleteFlag(string root,string datename)
        {
            string flag = root + "/" + datename + "_Done.txt";
            return File.Exists(flag);

        }

        //返回目标文件夹下的所有csv文件，并按文件名排序
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
        /// 判断字符串是否为纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (str == null || str.Length == 0)    //验证这个参数是否为空
                return false;                           //是，就返回False
            ASCIIEncoding ascii = new ASCIIEncoding();//new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里

            foreach (byte c in bytestr)                   //遍历这个数组里的内容
            {
                if ((c < 48 && c!=46 && c != 45) || c > 57)                          //判断是否为数字
                {
                    return false;                              //不是，就返回False
                }
            }
            return true;                                        //是，就返回True
        }


        public static bool FileIsUsed(string fileFullName)
        {
            bool result = false;
            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = false;
            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用
                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                    result = false;
                }
                catch (System.IO.IOException)
                {
                    result = true;
                }
                catch (System.Exception)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
            return result;
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
