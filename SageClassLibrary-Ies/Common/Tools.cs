using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;

namespace SageClassLibrary.Common
{
    public static class Tools
    {

        public static object Copy(object obj, Type type)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            DataContractSerializer res = new DataContractSerializer(type);
            MemoryStream memoryStream = new MemoryStream();
            XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(memoryStream);
            XmlDictionaryReader binaryDictionaryReader = XmlDictionaryReader.CreateBinaryReader(memoryStream, XmlDictionaryReaderQuotas.Max);
            dcSer.WriteObject(binaryDictionaryWriter, obj);
            binaryDictionaryWriter.Flush();
            memoryStream.Position = 0;
            return res.ReadObject(

                binaryDictionaryReader);
        }

        public static object Copy(object obj)
        {
            return Tools.Copy(obj, obj.GetType());
        }
        public static object Copy2(object obj)
        {
            return Copy2(obj, obj.GetType());
        }
        public static object Copy2(object obj, Type targetType)
        {
            object result = Activator.CreateInstance(targetType);

            var fieldmappings = from i in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                                join a in result.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic) on i.Name equals a.Name
                                select new { Original = i, Copy = a };

            var propertymappings = from i in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                                   join a in result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic) on i.Name equals a.Name
                                   where a.CanWrite
                                   select new { Original = i, Copy = a };

            foreach (var mapping in fieldmappings)
                mapping.Copy.SetValue(result, mapping.Original.GetValue(obj));

            foreach (var mapping in propertymappings)
                mapping.Copy.SetValue(result, mapping.Original.GetValue(obj, null), null);

            return result;

        }
        //public static object Copy(object obj,Type type)
        //{
        //    return Tools.Copy(obj, type);
        //}


    }
}

