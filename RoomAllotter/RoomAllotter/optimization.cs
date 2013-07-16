using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomAllotter
{
    class optimization
    {
        public List<int> RandomFinder(List<Tuple<int, int>> Domain, Func<List<int>, double> caluc, int times = 1000)
        {
            Random myRand = new Random();
            List<int> best = new List<int>();
            double best_val = 100000;
            double curr_val = 0;
            int count = 0;
            List<int> curr = new List<int>();
            while (times-- > 0)
            {
                curr.Clear();
                for (int j = 0; j < Domain.Count; j++)
                    curr.Add(myRand.Next(Domain[j].Item1, Domain[j].Item2));

                curr_val = caluc(curr);
                if (best_val > curr_val)
                {
                    best = curr;
                    best_val = curr_val;
                    count++;
                }

            }

            Console.WriteLine(best_val);
            return best;
        }

        public List<int> HillClimbing(List<Tuple<int, int>> Domain, Func<List<int>, double> caluc, int times = 1000)
        {
            int curr_inx_val = 0;
            double curr_val = 0, best_val = 100000, last_val = 0, whole_best = 10000;
            List<int> best_sol = new List<int>();
            List<int> curr_sol = new List<int>();
            List<List<int>> neighbours = new List<List<int>>();
            Random myRand = new Random();

            while (times-- > 0)
            {
                curr_sol.Clear();
                for (int i = 0; i < Domain.Count; i++)
                    curr_sol.Add(myRand.Next(Domain[i].Item1, Domain[i].Item2));
                while (true)
                {
                    neighbours.Clear();

                    for (int i = 0; i < Domain.Count; i++)
                    {
                        curr_inx_val = curr_sol[i];
                        List<int> new_list1 = new List<int>(curr_sol);
                        if (curr_sol[i] > Domain[i].Item1)
                        {
                            new_list1[i] = curr_inx_val - 1;
                            neighbours.Add(new_list1);
                        }
                        List<int> new_list2 = new List<int>(curr_sol);

                        if (curr_sol[i] < Domain[i].Item2)
                        {
                            new_list2[i] = curr_inx_val + 1;
                            neighbours.Add(new_list2);
                        }
                    }
                    //   Console.WriteLine(neighbours.Count);
                    foreach (var neighbour in neighbours)
                    {

                        curr_val = caluc(neighbour);
                        //  Console.WriteLine(curr_val);
                        if (best_val > curr_val)
                        {
                            best_sol = neighbour;
                            best_val = curr_val;
                        }

                    }
                    if (best_val == last_val)
                    {
                        break;
                    }
                    last_val = best_val;
                }
                if (whole_best > best_val)
                    whole_best = best_val;

            }
            Console.WriteLine("-->" + whole_best);
            return best_sol;
        }

        public List<int> Annealing(List<Tuple<int, int>> Domain, Func<List<int>, double> caluc, double temperature = 10000.0, double cool = 0.95, int shift = 1)
        {
            List<int> curr_sol = new List<int>();
            int falter = 0, index = -1;
            Random myRand = new Random();
            double curr_cost = 0, updated_cost = 0, percentage = 0;

            for (int i = 0; i < Domain.Count; i++)
                curr_sol.Add(myRand.Next(Domain[i].Item1, Domain[i].Item2));

            while (temperature > 0.1)
            {

                falter = myRand.Next(-shift, shift);
                index = myRand.Next(0, Domain.Count);
                List<int> updated_sol = new List<int>(curr_sol);
                updated_sol[index] += falter;
                if (updated_sol[index] < Domain[index].Item1)
                    updated_sol[index] = Domain[index].Item1;

                if (updated_sol[index] > Domain[index].Item2)
                    updated_sol[index] = Domain[index].Item2;

                curr_cost = caluc(curr_sol);
                updated_cost = caluc(updated_sol);
                percentage = Math.Pow(Math.E, (-curr_cost - updated_cost) / temperature);
                if (updated_cost < curr_cost || percentage < myRand.NextDouble())
                    curr_sol = updated_sol;
                Console.WriteLine(curr_cost);
                temperature = temperature * cool;
            }
            return curr_sol;
        }

        public List<int> Crossover(List<Tuple<int, int>> Domain, List<int> c1, List<int> c2)
        {
            Random MyRandom = new Random();
            int index = MyRandom.Next(0, Domain.Count);
            List<int> curr = new List<int>(c1.Take(index));
            curr.AddRange(c2.Skip(index));
            return curr;
        }

        public List<int> Mutation(List<Tuple<int, int>> Domain, List<int> c1, int step = 1)
        {
            Random MyRandom = new Random();
            int index = MyRandom.Next(0, Domain.Count);
            List<int> curr = new List<int>(c1);

            if (MyRandom.NextDouble() < 0.5 && curr[index] > Domain[index].Item1)
                curr[index] -= step;
            else if (curr[index] < Domain[index].Item2)
                curr[index] += step;

            return curr;
        }

        public List<int> geneticAlgo(List<Tuple<int, int>> Domain, Func<List<int>, double> caluc, int pop_size = 500, double mutation_prob = 0.2, double elite = 0.2, int maxiterations = 100, int step = 1)
        {
            Dictionary<List<int>, double> Score = new Dictionary<List<int>, double>();
            Random MyRand = new Random();
            int top_elite = (int)(elite * pop_size);
            int index1, index2;
            List<List<int>> population = new List<List<int>>();

            for (int i = 0; i < pop_size; i++)
            {
                List<int> curr_sol = new List<int>();
                for (int j = 0; j < Domain.Count; j++)
                    curr_sol.Add(MyRand.Next(Domain[j].Item1, Domain[j].Item2));
                population.Add(curr_sol);
            }

            while (maxiterations-- > 0)
            {
                var hello = from s in population
                            orderby caluc(s)
                            select s;
                // Console.WriteLine(hello.Count());
                population = hello.ToList();
                //Console.WriteLine(population.Count);
                //Console.WriteLine(top_elite);
                population = population.Take(top_elite).ToList();


                while (population.Count < pop_size)
                {
                    if (MyRand.NextDouble() < mutation_prob)
                    {

                        index1 = MyRand.Next(0, top_elite);
                        population.Add(Mutation(Domain, population[index1]));
                    }
                    else
                    {
                        index1 = MyRand.Next(0, top_elite);
                        index2 = MyRand.Next(0, top_elite);
                        population.Add(Crossover(Domain, population[index1], population[index2]));

                    }
                }
               // Console.WriteLine(maxiterations + ": " + caluc(population[0]));
            }
            Console.WriteLine(caluc(population[0]));
            return (population[0]);
        }

    }
}
