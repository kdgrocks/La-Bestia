using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomAllotter
{
    class dorm : optimization
    {
        public List<String> dorms = new List<String> { "Zeus", "Athena", "Hercules", "Bacchus", "Pluto" };
        public List<Tuple<String, String, String>> pref = new List<Tuple<string, string, string>>();
        public List<Tuple<int, int>> Domain = new List<Tuple<int, int>>();
        public dorm()
        {
            pref.Add(Tuple.Create("Toby", "Bacchus", "Hercules"));
            pref.Add(Tuple.Create("Steve", "Zeus", "Pluto"));
            pref.Add(Tuple.Create("Andrea", "Athena", "Zeus"));
            pref.Add(Tuple.Create("Sarah", "Zeus", "Pluto"));
            pref.Add(Tuple.Create("Dave", "Athena", "Bacchus"));
            pref.Add(Tuple.Create("Jeff", "Hercules", "Pluto"));
            pref.Add(Tuple.Create("Fred", "Pluto", "Athena"));
            pref.Add(Tuple.Create("Suzie", "Bacchus", "Hercules"));
            pref.Add(Tuple.Create("Laura", "Bacchus", "Hercules"));
            pref.Add(Tuple.Create("Neil", "Hercules", "Athena"));

            for (int i = 0; i < pref.Count; i++)
            {
                Domain.Add(Tuple.Create(0, pref.Count - 1 - i));
            }

        }

        public void printSolution(List<int> vec)
        {
            List<int> slot = new List<int>();
            int j = 0;
            String dorme = null;
            for (int i = 0; i < Domain.Count; i++)
            {
                slot.Add(i);
                slot.Add(i);
            }

            foreach (var room in vec)
            {
                dorme = dorms[slot[room]];
                Console.WriteLine(pref[j].Item1 + " : " + dorme);
                slot.Remove(slot[room]);
                j++;
            }

        }
        public double costfunction(List<int> vec)
        {
            List<int> slot = new List<int>();
            double cost = 0;
            int j = 0;
            String dorme = null;
            for (int i = 0; i < Domain.Count; i++)
            {
                slot.Add(i);
                slot.Add(i);
            }

            foreach (var room in vec)
            {
                dorme = dorms[slot[room]];
                if (pref[j].Item2 == dorme)
                    cost += 0;
                else if (pref[j].Item3 == dorme)
                    cost += 1;
                else
                    cost += 3;
                //  Console.WriteLine(pref[j].Item1 + " : " + dorme);
                slot.Remove(slot[room]);
                j++;
            }
            return cost;
        }
    }
}
