using System;
using System.Collections;
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
        public string Serialize(object src, string rootname="")
        {

            string result = "";
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sss = json.Serialize(src);


            Type type = src.GetType();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;
            setting.Encoding = Encoding.UTF8;

            //StringBuilder strb = new StringBuilder();
            MemoryStream mm = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(mm, setting);

            xmlWriter.WriteStartDocument(false);
            xmlWriter.WriteStartElement(string.IsNullOrWhiteSpace(rootname) == true?"Anonuymous": rootname);
            Serialize(src, xmlWriter);

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            //result = strb.ToString();
            result = Encoding.UTF8.GetString(mm.ToArray());
            mm.Close();
            mm.Dispose();
            return result;
        }

        void Serialize(object src, XmlWriter xmlw)
        {
            Type src_type = src.GetType();
            var pps = src.GetType().GetProperties();
            if(pps.Length == 0 || Type.GetTypeCode(src_type) == TypeCode.String)
            {
                switch (Type.GetTypeCode(src_type))
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
                        {
                            string vv = Convert.ToString(src);
                            xmlw.WriteElementString(src_type.Name, vv);
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            DateTime vv = (DateTime)src;
                            xmlw.WriteElementString(src_type.Name, vv.ToString("o"));
                        }
                        break;
                    case TypeCode.String:
                        {
                            xmlw.WriteElementString(src_type.Name, src as string);
                        }
                        break;
                }
                return;
            }
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
                        {
                            string vv = Convert.ToString(pp.GetValue(src));
                            xmlw.WriteElementString(pp.Name, vv);
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            DateTime vv = (DateTime)pp.GetValue(src);
                            xmlw.WriteElementString(pp.Name, vv.ToString("o"));
                        }
                        break;
                    case TypeCode.String:
                        {
                            xmlw.WriteElementString(pp.Name, pp.GetValue(src) as string);
                        }
                        break;
                    case TypeCode.Object:
                        {
                            var ll = pp.GetValue(src);
                            if (ll is IEnumerable)
                            {
                                xmlw.WriteStartElement(pp.Name);
                                foreach (var listitem in ll as IEnumerable)
                                {
                                    Type listitemtype = listitem.GetType();
                                    if (listitemtype.GetProperties().Count() > 0)
                                    {
                                        xmlw.WriteStartElement(listitemtype.Name);
                                    }
                                    Serialize(listitem, xmlw);
                                    if (listitemtype.GetProperties().Count() > 0)
                                    {
                                        xmlw.WriteEndElement();
                                    }
                                }
                                xmlw.WriteEndElement();
                            }
                            else
                            {
                                xmlw.WriteStartElement(pp.Name);
                                Serialize(pp.GetValue(src), xmlw);
                                xmlw.WriteEndElement();
                            }
                            
                        }
                        break;
                }

            }
        }
    }
}
