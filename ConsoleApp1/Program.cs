using QSoft.XmlserializerEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlserializerEx xmlex = new XmlserializerEx();
            string str = xmlex.Serialize(new
            {
                Account="AA",
                Password="BB",
                Age=18,
                Config = new
                {
                    Number = 100,
                    Max =10,
                    Min=-10
                }
            });
            Console.WriteLine(str);
            int sum = 0;
            for(int i=1; i<=3; i++)
            {
                sum = sum + i;
            }
            Console.WriteLine(sum);
            sum = Sum(3);
            sum = Sum(1, 3);
            sum = Sum(1, 5, (x)=>x+2);
            var vv = Test();
        }

        static (string, int)Test()
        {
            return ("11", 22);
        }

        static int Sum(int begin, int end, Func<int,int> func)
        {
            if (end < begin)
            {
                return 0;
            }
            return begin + Sum(func(begin), end, func);
        }

        static int Sum(int begin, int end)
        {
            if (end < begin)
            {
                return 0;
            }
            return begin + Sum(begin + 1, end);
        }

        static int Sum(int data)
        {
            if(data <=0)
            {
                return 0;
            }
            return data+ Sum(data-1);
        }
        
    }
}
