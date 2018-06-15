using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var result = RustInterop.Println("Hello", "World");
            Console.WriteLine($"Result was {result}");

            var range = RustInterop.GetRange(10);
            Console.WriteLine(string.Join("\n", range));
        }
    }

    public class RustInterop
    {
        [StructLayout(LayoutKind.Sequential)]
        struct ArrayRef
        {
            public IntPtr Bytes;
            public uint Length;
        }

        private delegate void GetRangeCallback(ArrayRef array);

        [DllImport("rust.dll")]
        private static extern int println(ArrayRef[] array, uint count);

        [DllImport("rust.dll")]
        private static extern int get_range(uint count, GetRangeCallback callback);

        public static int Println(params string[] strings)
        {
            var array = strings.Select(str => Encoding.UTF8.GetBytes(str)).Select(bytes =>
            {
                var pointer = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, pointer, bytes.Length);
                return new ArrayRef
                {
                    Bytes = pointer,
                    Length = (uint) bytes.Length
                };
            }).ToArray();
            int i = println(array, (uint) array.Length);
            foreach (var stringRef in array)
            {
                Marshal.FreeHGlobal(stringRef.Bytes);
            }
            return i;
        }

        public static int[] GetRange(uint count)
        {
            int[] result = null;
            get_range(count, arrayRef =>
            {
                var res = new int[arrayRef.Length];
                Marshal.Copy(arrayRef.Bytes, res, 0, res.Length);
                result = res;
            });
            return result;
        }
    }
}
