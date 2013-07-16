using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomAllotter
{
    class Program
    {
        static void Main(string[] args)
        {
            dorm alloter = new dorm();
            var s = alloter.geneticAlgo(alloter.Domain, alloter.costfunction);
            alloter.printSolution(s);

            Console.Read();

        }
    }
}
