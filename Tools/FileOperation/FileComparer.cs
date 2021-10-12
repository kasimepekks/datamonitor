using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools
{
    public class FileComparer : IComparer
    {
        int IComparer.Compare(object o1, object o2)
        {
            FileInfo fi1 = o1 as FileInfo;
            FileInfo fi2 = o2 as FileInfo;
            return fi1.Name.CompareTo(fi2.Name);
        }
    }
}
