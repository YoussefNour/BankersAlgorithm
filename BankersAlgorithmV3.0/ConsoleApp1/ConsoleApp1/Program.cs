using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        #region attributes
        public static int NP, NR;
        public static int[] available;
        public static List<Process> processes = new List<Process>();
        public static Random randgen = new Random();
        #endregion

        static void Main(string[] args)
        {   
            #region reading user defined data
            Console.Out.Write("\nEnter number of Processes: ");
            try
            {
                NP = Convert.ToInt32(Console.In.ReadLine());
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("!!" + e.Message + "!!" + "\n\nplease enter number of processes in right format\n\n");
                NP = Convert.ToInt32(Console.In.ReadLine());
            }
            Console.Out.Write("\nEnter number of Resources: ");
            try
            {
                NR = Convert.ToInt32(Console.In.ReadLine());
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("!!" + e.Message + "!!" + "\n\nplease enter number of Resources in right format\n\n");
                NR = Convert.ToInt32(Console.In.ReadLine());
            }
            available = new int[NR];

            for (int i = 0; i < NR; i++)
            {
                Console.Out.Write("\nenter number of available instances of Resource " + (i + 1) + ":");
                try
                {
                    available[i] = Convert.ToInt32(Console.In.ReadLine());
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message + "\nPlease enter an integer");
                    Console.Out.WriteLine("Enter number of available instances of Resource " + (i + 1));
                    available[i] = Convert.ToInt32(Console.In.ReadLine());
                }
            }
            int[] availablecopy = new int[NR];
            available.CopyTo(availablecopy,0);
            #endregion

            #region initializing processes and showing initial state
            for (int i = 0; i < NP; i++)
            {
                Process p = new Process(NR);
                processes.Add(p);
            }
            print_processes(processes);
            #endregion

            #region processing requests & applying bankers algorithm
            List<Process> unfinished= new List<Process>();
            unfinished.AddRange(processes);
            int releasecounter = randgen.Next(0, 3);

            while (!finishcheck(processes))
            {
                for (int i =0;i<unfinished.Count;i++)
                {
                    int x = processes.IndexOf(unfinished[i]);
                    unfinished[i].gen_request();
                    Console.Write("\nProcess " +(x+1)+" requested"+arraytostring(unfinished[i].request));
                    if(request_Accepted(unfinished[i],x))
                    {
                        releasecounter--;
                        Console.Write(" Request has been accepted\n");
                        unfinished[i].allocate_request();
                        if(AgreaterthanB(available,unfinished[i].request))
                        {
                            if(releasecounter<=0)
                            {
                                releasecounter = randgen.Next(0, 3);
                                unfinished[i].Randomdeallocate();
                                Console.Write("\nProcess " + (x + 1) + " has released " + arraytostring(unfinished[i].deallocate) + "\n");
                                if(unfinished[i].noneed())
                                {
                                    unfinished.Remove(unfinished[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write(" Request has been rejected\n");
                    }
                }
                print_processes(processes);
            }
            available = availablecopy;
            Console.Write("\nSystem done All Processes Needs Fulfilled :D\n");
            print_processes(processes);
            Console.ReadKey();
            #endregion
        }
        #region Methods
        public static void print_processes(List<Process> ps)
        {
            Console.Out.Write("\n\tALLO\t\t");
            Console.Out.Write("MAX.\t\t");
            Console.Out.Write("NEED\t\t");
            Console.Out.Write("REQ. \t\t");
            Console.Out.Write("RQNO\t\t");
            Console.Out.Write("AVAI\n");

            for (int i = 0; i < ps.Count; i++)
            {
                Console.Write("P" + (i + 1) + " :\t");
                for (int j = 0; j < ps[i].allocated.Count(); j++)
                {
                    Console.Write(ps[i].allocated[j] + " ");
                }
                Console.Write("\t");
                for (int j = 0; j < ps[i].max.Count(); j++)
                {
                    Console.Write(ps[i].max[j] + " ");
                }
                Console.Write("\t");
                for (int j = 0; j < ps[i].need.Count(); j++)
                {
                    Console.Write(ps[i].need[j] + " ");
                }
                Console.Out.Write("\t");
                for (int j = 0; j < ps[i].need.Count(); j++)
                {
                    Console.Write(ps[i].request[j] + " ");
                }
                Console.Write("\t");
                Console.Write(ps[i].reqcount);
                Console.Write("\t\t");
                if (i == 0)
                {
                    for (int j = 0; j < available.Length; j++)
                    {
                        Console.Write(available[j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static bool AgreaterthanB(int[] A, int[] B)
        {
            for (int i = 0; i < NR; i++)
            {
                if (B[i] > A[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static void addinA(int[] a, int[] b)
        {
            for (int i = 0; i < NR; i++)
            {
                a[i] = a[i] + b[i];
            }
        }

        public static bool finishcheck(List<Process> ps)
        {
            for (int i = 0; i < ps.Count; i++)
            {
                if (!ps[i].noneed())
                {
                    return false;
                }
            }
            return true;
        }

        public static bool request_Accepted(Process p,int i)
        {
            if (AgreaterthanB(available,p.request))
            {
                return true;
            }
            return false;
        }
        
        public static string arraytostring(int [] x)
        {
            string s="";
            for (int i = 0; i < x.Length; i++)
            {
                s+=(" " + x[i]);
            }
            s += " ";
            return s;
        }

        #endregion
    }
}



