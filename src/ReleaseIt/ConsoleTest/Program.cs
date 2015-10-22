using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var run = new Task(() => Run111());
            run.ContinueWith(t => Contiute1());
            run.ContinueWith(t => Contiute2());
            run.Start();
            
            Console.ReadKey();
        }

        static void Run111()
        {
            Thread.Sleep(100);
        }

        static void Contiute1()
        {
            Thread.Sleep(1000);
            Console.WriteLine("run 11");
        }


        static void Contiute2()
        {
            Thread.Sleep(100);
            Console.WriteLine("run 22");
        }
    }
}
