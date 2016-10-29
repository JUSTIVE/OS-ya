using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling_Jh
{
    public class Process
    {
        private bool isEnd;
        private String name;
        private int arrival_time;
        private int burst_time;
        private int end_time;   //완전 프로세스 끝나는 시간
        private int priority;
        public Process(String name, int arrival_time, int burst_time)
        {
            isEnd = false;
            this.name = name;
            this.arrival_time = arrival_time;
            this.burst_time = burst_time;
            this.priority = -1;
        }
        public Process(String name, int arrival_time, int burst_time, int priority)
        {
            isEnd = false;
            this.name = name;
            this.arrival_time = arrival_time;
            this.burst_time = burst_time;
            this.priority = priority;
        }
        public String getName() { return name; }
        public bool getIsEnd() { return isEnd; }
        public int getArrivalTime() { return arrival_time; }
        public int getBurstTime() { return burst_time; }
        public int getPriority() { return priority; }
        public int getEndTime() { return end_time; }
        public void setBurstTime(int time) { burst_time = time; }
        public void setEndTime(int time) { end_time = time; }
    }

    public class Process_HRN :Process
    {
        private double Priority;

        public Process_HRN(String name, int arrival_time, int burst_time)
            :base(name, arrival_time, burst_time)
        { }

        public void setPriority(double p) { Priority = p; }
        public double getPriority() { return Priority; }
    }
}
