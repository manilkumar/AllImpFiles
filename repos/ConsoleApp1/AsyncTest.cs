using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class AsyncTest
    {
        public async Task<string> TestAsync1()
        {
            Thread.Sleep(2000);
            Console.WriteLine("reached 1");
            return string.Empty;
        }

        public async Task<string> TestAsync2()
        {
            Thread.Sleep(5000);
            Console.WriteLine("reached 2");
            return string.Empty;
        }
    }
}
