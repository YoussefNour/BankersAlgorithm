using System;

namespace ConsoleApp1
{
    class Process
    {
        private int NR;
        public int reqcount;
        public int[] allocated;
        public int[] need;
        public int[] max;
        public int[] request;
        public int[] deallocate;
        public int[] sumreq;
        private static Random rand = new Random();

        public Process(int number_of_resources)
        {
            reqcount = 0;
            NR = number_of_resources;
            allocated = new int[NR];
            need = new int[NR];
            max = new int[NR];
            request = new int[NR];
            sumreq = new int[NR];

            gen_max();
            calc_need();
        }
        public void gen_max()
        {
            for (int i = 0; i < NR; i++)
            {
                max[i] = rand.Next(Program.available[i]+1);
            }
        }
        public void calc_need()
        {
            for (int i = 0; i < NR; i++)
            {
                need[i] = max[i] - allocated[i];
            }
        }
        public void gen_request()
        {
            for (int i = 0; i < NR; i++)
            {
                request[i] = rand.Next(need[i]+1);
            }
            reqcount++;
        }
        public void allocate_request()
        {
            for (int i = 0; i < NR; i++)
            {
                allocated[i] += request[i];
                sumreq[i] += request[i];
                Program.available[i] -= request[i];
            }
            calc_need();
        }
        public void Randomdeallocate()
        {
            int[] dislocate = new int[NR];
            for (int j = 0; j < NR; j++)
            {
                if (request[j] != 1)
                {
                    dislocate[j] = rand.Next((sumreq[j] + 1));
                }
                else
                {
                    dislocate[j] = 1;
                }
                sumreq[j] -= dislocate[j];
                Program.available[j] += dislocate[j];
            }
            deallocate = dislocate;
        }
        public bool noneed()
        {
            for(int i =0;i<NR;i++)
            {
                if(need[i]>0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}