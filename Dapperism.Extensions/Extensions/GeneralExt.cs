using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Dapperism.Extensions.Extensions
{
    public static class GeneralExt
    {
        public static bool IsGuid(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            var format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            var match = format.Match(s);

            return match.Success;
        }
        public static bool IsNullable(this Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static T? ToNullable<T>(this T value) where T : struct
        {
            return (value.Equals(default(T)) ? null : (T?)value);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static void ForEachReverse<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source.Reverse())
                action(item);
        }


        public static IQueryable<T> PagedList<T, TResult>(this IQueryable<T> source,
            int pageIndex, int pageSize, Expression<Func<T, TResult>> orderByProperty
            , bool isAscendingOrder, out int rowsCount)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException("pageIndex must be greater than zero");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize must be greater than zero");
            }

            var src = source;

            rowsCount = source.Count();

            src = isAscendingOrder ? src.OrderBy(orderByProperty) : src.OrderByDescending(orderByProperty);

            var result = src.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return result;
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            var streamLength = Convert.ToInt32(stream.Length);
            var data = new byte[streamLength + 1];
            stream.Read(data, 0, streamLength);
            stream.Close();
            return data;
        }

        public static MemoryStream ToMemoryStream(this Byte[] buffer)
        {
            var ms = new MemoryStream(buffer);
            ms.Position = 0;
            return ms;
        }

        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            var ret = new MemoryStream();
            var buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                ret.Write(buffer, 0, bytesRead);
            ret.Position = 0;
            return ret;
        }

        public static byte[] ConvertToByteArray(this Image imageIn, ImageFormat imgFormat)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, imgFormat);
            return ms.ToArray();
        }

        public static Image ConvertToImage(this byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static string ConvertImageToBase64(this Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image ConvertBase64ToImage(this string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);
            return image;
        }

        public static IEnumerable<T> GetPage<T>(this IList<T> source, int pageIndex, int pageSize)
        {
            for (var i = pageIndex;
                i < pageIndex + pageSize && i < source.Count;
                i++)
            {
                yield return source[i];
            }
        }

        public static bool IsOfType<T>(this object obj)
        {
            return (obj.GetType() == typeof(T));
        }

        public static T CastTo<T>(this object o)
        {
            if (o == null)
                return default(T);
            return (T)Convert.ChangeType(o, typeof(T));
        }


    }
}
