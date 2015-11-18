using System;
using System.Collections.Generic;
using System.IO;
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
            Class1 d = new Class1(Path.GetFullPath(System.Environment.CurrentDirectory + "/../"));
            Console.Read();
            var Start = new Task(() => start());
            Start.ContinueWith(t => Contiute1_1())
                .ContinueWith(t => Contiute1_2());


            var task2_1 = Start.ContinueWith(t => Contiute2_1());
            var task2_2 = task2_1.ContinueWith(t => Contiute2_2());


            Start.RunSynchronously();
            Console.WriteLine("end");
            Console.ReadKey();
        }

        static void start()
        {
            Thread.Sleep(100);
            Console.WriteLine("run");

        }

        static void Contiute1_1()
        {
            Thread.Sleep(200);
            Console.WriteLine("run 1_1");
           
        }


        static void Contiute1_2()
        {
            Console.WriteLine("run 1_2");
            Thread.Sleep(100);
        }


        static void Contiute2_1()
        {

            Console.WriteLine("run 2_1");
            Thread.Sleep(100);
        }


        static void Contiute2_2()
        {

            Console.WriteLine("run 2_2");
            Thread.Sleep(100);
        }
    }
}
