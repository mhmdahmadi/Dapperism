using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Dapperism.Utilities
{
 public static   class ObjectExt
    {
        public static string GetObjectSizeAsByte(this object currentObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, currentObject);
            Array = ms.ToArray();
            return Array.Length.ToString();
        }

        public static string GetObjectSizeAsKB(this object currentObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, currentObject);
            Array = ms.ToArray();
            return (Array.Length / 1024).ToString();
        }

        public static string GetObjectSizeAsMB(this object currentObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, currentObject);
            Array = ms.ToArray();
            return (Array.Length / 1024 / 1024).ToString();
        }
    }
}
