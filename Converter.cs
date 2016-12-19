using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace InsightWebApi.Utilities
{
    /// <summary>
    /// This class is used to Convert JSON/XML into C# object.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// This Method is used to Convert JSON/XML into C# object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T TranslateObject<T>(string input) where T : class
        {
            try
            {
                T obj = null;
                input = input.ToString();

                bool isXML = input.StartsWith("<");
                if (isXML)
                {
                    obj = TranslateXmlToObject<T>(input);
                }
                else if (input.StartsWith("{") || input.StartsWith("["))
                {
                    obj = TranslateJsonToObject<T>(input);
                }
                return obj;
            }
            catch (Exception ex)
            {
                //Recording and Notifying the Exception Details
                //GlobalFunctions.HandleLogError(ex.Message, ex.InnerException);

                throw ex;
            }

            finally
            {

            }
        }


        /// <summary>
        /// This method is used to translate XML into C# object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T TranslateXmlToObject<T>(string input) where T : class
        {
            try
            {
                T obj = null;
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (TextReader tr = new StringReader(input))
                {

                    obj = (T)ser.Deserialize(tr);
                }
                return obj;
            }
            catch (Exception ex)
            {
                //Recording and Notifying the Exception Details
                // GlobalFunctions.HandleLogError(ex.Message, ex.InnerException);

                throw ex;
            }
            finally
            {

            }

        }

        /// <summary>
        /// This method is used to translate JSON into C# object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T TranslateJsonToObject<T>(string input) where T : class
        {
            try
            {
                T obj = JsonConvert.DeserializeObject(input, typeof(T)) as T;
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {

            }
        }


        public static string UnescapeCodes(string src)
        {
            var rx = new Regex("\\\\([0-9A-Fa-f]+)");
            var res = new StringBuilder();
            var pos = 0;
            foreach (Match m in rx.Matches(src))
            {
                res.Append(src.Substring(pos, m.Index));
                pos = m.Index + m.Length;
                res.Append((char)Convert.ToInt32(m.Groups[1].ToString(), 16));
            }
            res.Append(src.Substring(pos));
            return res.ToString();
        }




        /// <summary>
        /// This Method is used to Convert JSON/XML into C# object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TranslateObject<T>(T t, bool isXML = false) where T : class
        {
            try
            {
                //Check for XML.
                if (!isXML)
                {
                    //Translate the XML into object and return
                    return TranslateObjectToJson<T>(t);
                }
                else
                {
                    //Translate the JSON into object and return
                    return TranslateObjectToXml<T>(t);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to Convert an object into json.
        /// </summary>
        public static string TranslateObjectToJson<T>(T t)
        {
            try
            {
                return JsonConvert.SerializeObject(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to convert an object into Xml.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToConvert"></param>
        /// <returns></returns>
        public static string TranslateObjectToXml<T>(T t)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(t.GetType());
                System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(ms, null);

                xs.Serialize(xtw, t);
                ms = (System.IO.MemoryStream)xtw.BaseStream;
                string asdf = ms.ToString();

                return new UTF8Encoding().GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            try
            {
                return JsonConvert.SerializeObject(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }





        public static string ConvertObjectToXmlString<T>(T objectToConvert)
        {

            System.IO.MemoryStream objMemoryStream;
            System.Xml.Serialization.XmlSerializer objXmlSerializer;
            System.Xml.XmlTextWriter objXmlTextWriter;
            try
            {
                objMemoryStream = new System.IO.MemoryStream();
                objXmlSerializer = new System.Xml.Serialization.XmlSerializer(objectToConvert.GetType());
                objXmlTextWriter = new System.Xml.XmlTextWriter(objMemoryStream, null);

                objXmlSerializer.Serialize(objXmlTextWriter, objectToConvert);
                objMemoryStream = (System.IO.MemoryStream)objXmlTextWriter.BaseStream;
                return new UTF8Encoding().GetString(objMemoryStream.ToArray());

            }
            catch (Exception ex)
            {
                //Recording and Notifying the Exception Details
                //GlobalFunctions.HandleLogError(ex.Message, ex.InnerException);

                throw ex;
            }

            finally
            {
                objMemoryStream = null;
                objXmlSerializer = null;
                objXmlTextWriter = null;
            }

        }

    }
}