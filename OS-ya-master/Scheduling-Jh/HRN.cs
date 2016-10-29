using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling_Jh
{


    class HRN : Scheduler   //HRN 알고리즘 
    {

        public class Process_HRN : Process   //HRN에서 쓰는 프로세스! Process클래스 상속받음
        {
            private double Priority;    //hrn에서 우선순위는 double형입니다

            public Process_HRN(String name, int arrival_time, int burst_time)
                : base(name, arrival_time, burst_time)
            { }

            //getter & setter
            public void setPriority(double p) { Priority = p; }
            public double getPriority() { return Priority; }
        }

        class Comparer_HRN : IComparer<Process_HRN> //HRN에서 우선순위대로 Sorting시 사용되는 클래스
        {
            public int Compare(Process_HRN x, Process_HRN y)
            {
                return -x.getPriority().CompareTo(y.getPriority()); //x, y의 우선순위 비교
            }
        }

        Queue<Process_HRN> Ready;   //큐
        List<Process_HRN> Arrived;  //레디큐

        public HRN(List<Process> list)
            : base(list)
        {
            currentTime = 0;    //현재 시간 0부터 시작
            Ready = new Queue<Process_HRN>();
            Arrived = new List<Process_HRN>();
            for (int i = 0; i < inputData.Count; i++) //원본 프로세스들 리스트로부터 레디큐에 차례로 삽입
            {
                //HRN에서 사용되는 Process_HRN 클래스
                Process_HRN p = new Process_HRN(inputData[i].getName(), inputData[i].getArrivalTime(), inputData[i].getBurstTime());
                Ready.Enqueue(p);
            }
        }

        public void hrn_alg()   //HRN 알고리즘
        {
            int start = 0, end = 0;

            do
            {
                Process_HRN process;

                while (Ready.Count > 0 && Ready.Peek().getArrivalTime() <= currentTime) //도착한 프로세스를 Arrived 리스트로 옮김
                {
                    Arrived.Add(Ready.Dequeue());
                }

                if (Arrived.Count <= 0) //현재 시간에 도착 프로세스가 없는 경우
                {
                    currentTime++;  //현재 시간 1 증가
                    break;
                }

                for (int j = 0; j < Arrived.Count; j++)
                {
                    double p = (currentTime - Arrived[j].getArrivalTime() + Arrived[j].getBurstTime()) / (double)Arrived[j].getBurstTime();
                    Arrived[j].setPriority(p);  //우선순위 계산
                }
                Arrived.Sort(new Comparer_HRN());   //우선순위별로 정렬

                start = currentTime;    //
                process = Arrived.First();
                currentTime += process.getBurstTime();  //Burst Time만큼 현재 시간 증가
                end = currentTime;

                for (int j = 0; j < inputData.Count; j++)
                {
                    if (inputData[j].getName().Equals(process.getName()))
                    {
                        inputData[j].setEndTime(end);   //프로세스 종료 시간 설정
                    }
                }

                Stamp s = new Stamp(process.getName(), start, end);
                addStamp(s);    //스탬프 찍어줌

                Arrived.RemoveAt(0);    //제일 처음의 프로세스를 삭제

            } while (Ready.Count > 0 || Arrived.Count > 0);
        }
    }
}