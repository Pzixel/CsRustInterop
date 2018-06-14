using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var result = RustInterop.Println("Hello World!");
            Console.WriteLine($"Result was {result}");
        }
    }

    public class RustInterop
    {

        [DllImport("rust.dll")]
        private static extern int println(byte[] bytes, uint count);

        public static int Println(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return println(bytes, (uint) bytes.Length);
        }
    }
}
