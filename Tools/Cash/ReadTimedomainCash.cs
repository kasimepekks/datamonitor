using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tools.Cash
{
    //泛型静态类
    public static class ReadTimedomainCash<T> where T:class
    {
        private readonly static PropertyInfo[] props=null;
        
        private readonly static bool IsAccorNot=false;
        //泛型静态构造函数，每种类型都只存一个
         static  ReadTimedomainCash()
        {
            if (typeof(T).IsSubclassOf(typeof(TempdataAccBase)))//判断T是否继承自TempdataAccBase父类，如果继承，则可以拿到T的所有属性
            {
                IsAccorNot = true;
            }
            Type ttype = typeof(T);
            
            props = ttype.GetProperties();


          
        }
        public static PropertyInfo[] GetProps()
        {
            return props;
        }
        public static bool ACCorWFT()
        {
            return IsAccorNot;
        }
      
    }
}
