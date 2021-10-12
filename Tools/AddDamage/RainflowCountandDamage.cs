using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDamage
{
   public class RainflowCountandDamage
    {
        public LinkedList<double> pv_data { get; set; }//用于保存峰谷值点
        public LinkedList<RainflowResult> RFResult { get; set; }//用于保存雨流计数结果类
        public LinkedList<double> Reversal { get; set; }//用于保存发散波的数据
        private static double Tol = 1.0e-6;
        private static double Smax = 5023.77;
        private static double b = -5;
        public double damage { get; set; }


        public void GetPVPoints(List<double> lst)
        {

            int i = 1;
            int count = lst.Count;
            //如果数组的元素的数量大于等于3的时候才能执行下面的比较运算
            while (count > 2)
            {

                if ((lst[i] >= lst[i - 1] && lst[i + 1] >= lst[i]) || (lst[i] <= lst[i - 1] && lst[i + 1] <= lst[i]))
                {
                    lst.RemoveAt(i);//如果不是峰谷值就删除此数据点

                }
                else
                {
                    i++;//如果是峰谷值就继续判断下一组（3个点）是否是峰谷值
                }
                count--;
            }
            LinkedList<double> pv = new LinkedList<double>();

            foreach (double j in lst)
            {
                pv.AddLast(j);
            }
            pv_data = pv;
        }

        public void GetCycle(LinkedList<double> data)
        {
            int count = data.Count;
            LinkedListNode<double> cur = data.First;//获取第一个节点
            LinkedList<RainflowResult> RFResultnew = new LinkedList<RainflowResult>();
            //用4点法计算循环数
            while (count > 3 && cur.Next.Next.Next != null)//4个点以上才能用4点法
            {
                //先判断是否是上升4点
                if (cur.Next.Value > cur.Value)
                {
                    //再根据上升4点法的规则进行判断
                    if (cur.Next.Next.Next.Value >= cur.Next.Value && cur.Next.Next.Value >= cur.Value)
                    {
                        double mean = (cur.Next.Next.Value + cur.Next.Value) / 2;
                        double range = Math.Abs(cur.Next.Next.Value - cur.Next.Value);
                        double rfcount = 1;
                        RainflowResult result = new RainflowResult(mean, range, rfcount);
                        RFResultnew.AddLast(result);
                        //var n2 = cur.Next;
                        //var n3 = cur.Next.Next;

                        data.Remove(cur.Next);
                        data.Remove(cur.Next);
                        count = count - 2;
                        cur = data.First;//有符合条件的4点就从头开始进行判断
                    }
                    //如果不满足4点法则判断下一个4点
                    else
                    {
                        cur = cur.Next;
                        //count--;//这里不能减数量，因为会提前结束4点法判断
                    }
                }
                //如果是下降4点
                else
                {
                    //再根据下降4点法的规则进行判断
                    if (cur.Next.Next.Next.Value <= cur.Next.Value && cur.Next.Next.Value <= cur.Value)
                    {
                        double mean = (cur.Next.Next.Value + cur.Next.Value) / 2;
                        double range = Math.Abs(cur.Next.Next.Value - cur.Next.Value);
                        double rfcount = 1;
                        RainflowResult result = new RainflowResult(mean, range, rfcount);
                        RFResultnew.AddLast(result);
                        //var n2 = cur.Next;
                        //var n3 = cur.Next.Next;

                        data.Remove(cur.Next);
                        data.Remove(cur.Next);
                        count = count - 2;
                        cur = data.First;
                    }
                    //如果不满足4点法则判断下一个4点
                    else
                    {
                        cur = cur.Next;
                        //count--;
                    }
                }

            }


            //计算发散波的循环数，重新计算一下双链的长度，并2个2个点进行计算，count只能算0.5个
            LinkedListNode<double> res = data.First;
            int rcount = data.Count;
            while (rcount > 1)
            {
                double mean = (res.Next.Value + res.Value) / 2;
                double range = Math.Abs(res.Next.Value - res.Value);
                double rfcount = 0.5;
                RainflowResult result = new RainflowResult(mean, range, rfcount);
                RFResultnew.AddLast(result);
                res = res.Next;
                rcount--;
            }
            RFResult = RFResultnew;
            //Reversal = data;
        }

        //相同的range和mean的count相加
        public void GetCombinedCycle()
        {
            LinkedListNode<RainflowResult> cur = RFResult.First;
            LinkedListNode<RainflowResult> cur2 = RFResult.First;

            //第二层循环解决第一个数据和其他所有的数据的比较，第一层循环就是等第一个数据比较完了再把第二个数据当成第一个再循环比较
            if (cur2 != null)
            {
                while (cur2.Next != null)
                {
                    while (cur.Next != null)
                    {

                        if (Math.Abs(cur2.Value.sMean - cur.Next.Value.sMean) <= Tol)
                        {
                            if (Math.Abs(cur2.Value.sRange - cur.Next.Value.sRange) <= Tol)
                            {
                                cur2.Value.sCount = cur2.Value.sCount + cur.Next.Value.sCount;
                                RFResult.Remove(cur.Next);
                            }
                            else
                            {
                                cur = cur.Next;

                            }

                        }
                        else
                        {
                            cur = cur.Next;

                        }

                    }
                    if (cur2.Next != null) 
                    { 
                    cur2 = cur2.Next;
                    cur = cur2;
                    }
                }
            }
        }

        public void GetDamage(LinkedList<RainflowResult> data)
        {
            if (data != null)
            {
                double t = 0;
                foreach (var i in data)
                {
                    t = t + i.sCount / Math.Pow((i.sRange / Smax), b);
                }
                damage = t;
            }
        }


    }
}
