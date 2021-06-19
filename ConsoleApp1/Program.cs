using QSoft.XmlserializerEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            XmlserializerEx xmlex = new XmlserializerEx();
            string str = xmlex.Serialize(new
            {
                Mode = Modes.Manual,
                Array = new int[] { 1,2,3,4,5,6},
                List = new List<string> { "AA", "BB", "CC" },
                Account ="AA",
                Password="BB",
                Age=18,
                Time= DateTime.Now,
                Configs = new List<Config>()
                {
                    new Config()
                    {
                        Number = 1,
                        Max =10,
                        Min=-100
                    },
                    new Config()
                    {
                        Number = 2,
                        Max =20,
                        Min=-200
                    },
                    new Config()
                    {
                        Number = 3,
                        Max =30,
                        Min=-300
                    },
                },
                Config = new
                {
                    Number = 100,
                    Max =10,
                    Min=-10
                }
            }, "AA");
            Console.WriteLine(str);

            XmlSerializer xml = new XmlSerializer(typeof(AA));
            using (MemoryStream mm = new MemoryStream())
            {
                xml.Serialize(mm, new AA());
                Console.WriteLine(Encoding.UTF8.GetString(mm.ToArray()));

            }

            using (MemoryStream mm = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                object oo = xml.Deserialize(mm);

            }

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

    public class AA
    {
        //public Array Array { set; get; } = new int[] { 1, 2, 3, 4, 5, 6 };
        public Modes Mode { set; get; } = Modes.Manual;
        public List<string> List { set; get; } = new List<string> { "AA", "BB", "CC" };
        public string Account { set; get; } = "AA";
        public string Password { set; get; } = "BB";
        public int Age { set; get; } = 18;
        public DateTime Time { set; get; } = DateTime.Now;
        public Config Config { set; get; }
        public List<Config> Configs { set; get; }
    }

    public enum Modes
    {
        Auto,
        Manual,
        Test
    }

    public class Config
    {
        [XmlAttribute("Number")]
        public int Number { set; get; } = 100;
        public int Max { set; get; } = 10;
        public int Min { set; get; } = -10;
    }
}
