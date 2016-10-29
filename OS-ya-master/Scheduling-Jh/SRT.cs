using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling_Jh
{
    class SRT : Scheduler    //선점방식 사용
    {
        List<Process> Ready;
        List<Process> Copy;
        public SRT(List<Process> list)
            : base(list)
        {
            currentTime = 0;
            Ready = new List<Process>();
            Copy = list;
        }
        public void srt_run()
        {
            int remain = 0, smallest;
            int time;
            int[] remain_times = new int[20];

            for (int i = 0; i < Copy.Count; i++)
                remain_times[i] = Copy[i].getBurstTime();
            remain_times[19] = 999;

            for (time = 0; remain != Copy.Count; time++)
            {
                Console.WriteLine("time = " + time);
                smallest = 19;
                for (int i = 0; i < Copy.Count; i++)
                    if ((Copy[i].getArrivalTime() <= time) && (remain_times[i] < remain_times[smallest]) && (remain_times[i] > 0))
                        smallest = i;

                timestamp.Add(new Stamp(inputData[smallest].getName(), time, time + 1));
                remain_times[smallest]--;
                Console.WriteLine("remain:" + remain_times[smallest]);
                if (remain_times[smallest] == 0)
                {
                    remain++;
                    inputData[smallest].setEndTime(time + 1);
                    Console.WriteLine(inputData[smallest].getName() + " " + inputData[smallest].getEndTime());
                }
            }
            //스탬프 합치기
            for (int i = 0; i < timestamp.Count - 1; i++)
                if (timestamp[i].getName().CompareTo(timestamp[i + 1].getName()) == 0)
                {
                    Stamp newStamp = new Stamp(timestamp[i].getName(), timestamp[i].getStartTime(), timestamp[i + 1].getEndTime());
                    timestamp.RemoveAt(i);
                    timestamp.RemoveAt(i);
                    timestamp.Insert(i, newStamp);
                    i--;
                }
        }
//      .d8888. db    db  .o88b.  .o88b. d88888b .d8888. .d8888. 
//      88'  YP 88    88 d8P  Y8 d8P  Y8 88'     88'  YP 88'  YP 
//      `8bo.   88    88 8P      8P      88ooooo `8bo.   `8bo.   
//        `Y8b. 88    88 8b      8b      88~~~~~   `Y8b.   `Y8b. 
//      db   8D 88b  d88 Y8b  d8 Y8b  d8 88.     db   8D db   8D 
//      `8888Y' ~Y8888P'  `Y88P'  `Y88P' Y88888P `8888Y' `8888Y'

        int stmp_compare(Stamp a, Stamp b)    //스탬프의 정렬
        {
            if (a.getStartTime() > b.getStartTime())
                return 1;
            else if (a.getStartTime() == b.getStartTime())
                return 0;
            else
                return -1;
        }
    }

}

