using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QSoft.XmlserializerEx
{
    public class XmlserializerEx
    {
        public string Serialize(object src)
        {

            string result = "";
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sss = json.Serialize(src);


            Type type = src.GetType();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;
            

            StringBuilder strb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(strb, setting);

            xmlWriter.WriteStartDocument(false);
            xmlWriter.WriteStartElement("Anonuymous");
            Serialize(src, xmlWriter);

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            string xml = strb.ToString();
            Console.WriteLine(xml);
            return result;
        }

        void Serialize(object src, XmlWriter xmlw)
        {
            var pps = src.GetType().GetProperties();
            foreach (var pp in pps)
            {
                var code = Type.GetTypeCode(pp.PropertyType);
                switch (Type.GetTypeCode(pp.PropertyType))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Char:
                    case TypeCode.Single:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.String:
                        {
                            string vv = Convert.ToString(pp.GetValue(src));
                            xmlw.WriteElementString(pp.Name, vv);
                        }
                        break;
                    case TypeCode.Object:
                        {
                            xmlw.WriteStartElement(pp.Name);
                            Serialize(pp.GetValue(src), xmlw);
                            xmlw.WriteEndElement();
                        }
                        break;
                }

            }
        }
    }
}
