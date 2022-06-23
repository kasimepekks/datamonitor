using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools.FileOperation
{
    public static class FileWatcher
    {
        public static string filepath;
        public static void WatcherStrat(string path, string filter)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;

            watcher.Created += new FileSystemEventHandler(OnProcess);
          
            watcher.EnableRaisingEvents = true;
        }



        private static void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
          

        }



        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            filepath = e.FullPath;
            FileInfo fi = new FileInfo(filepath);
            if (!fi.Exists)
            {
                Console.WriteLine("file not exits!!");

            }
            else
            {
                Console.WriteLine("文件新建事件处理逻辑 {0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
            }
          

        }
       
    }
}
