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
        }
    }

    public class RustInterop
    {
        [StructLayout(LayoutKind.Sequential)]
        struct StringRef
        {
            public IntPtr Bytes;
            public uint Length;
        }

        [DllImport("rust.dll")]
        private static extern int println(StringRef[] array, uint count);

        public static int Println(params string[] strings)
        {
            var array = strings.Select(str => Encoding.UTF8.GetBytes(str)).Select(bytes =>
            {
                var pointer = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, pointer, bytes.Length);
                return new StringRef
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
    }
}
