using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling_Jh
{
    class Priority : Scheduler
    {
        List<Process>[] temp;
        List<Process> Ready;
        List<Process> Copy = new List<Process>();
        public Priority(List<Process> list)
            : base(list)
        {
            Copy = list;
            currentTime = 0;
            Ready = new List<Process>();
        }
        private int isNowInStamp(Process p, List<Stamp> list)//currentTime이 스탬프에 찍혀있는 범위 내의 값인지 확인
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (p.getArrivalTime() < list[i].getStartTime() && p.getArrivalTime() + p.getBurstTime() > timestamp[i].getStartTime())    //스탬프의 앞쪽이 겹칠 때
                    return 0;
                if (p.getArrivalTime() >= list[i].getStartTime() && p.getArrivalTime() < list[i].getEndTime())   //스탬프랑 겹칠때 (FCFS 쓸 때)
                    return 1;
            }
            return 2;   //아무 경우도 X
        }

        private void StampProcess(Process p)    //선점 priority에서 프로세스 스탬프 찍어주는 함수
        {
            int type = isNowInStamp(p, timestamp);  //스탬프 어떻게 찍힐지 정함
            Stamp s;
            int startpoint = 0, endpoint = 0;
            Process next;
            switch (type)
            {
                case 0: //프로세스가 중간에 끊기는 경우
                    for (int i = 0; i < timestamp.Count; i++)
                    {
                        if (p.getArrivalTime() < timestamp[i].getStartTime() && p.getArrivalTime() + p.getBurstTime() > timestamp[i].getStartTime())
                        {
                            startpoint = timestamp[i].getStartTime();
                            endpoint = timestamp[i].getEndTime();
                            break;
                        }
                    }
                    int n_burst = p.getBurstTime() - (startpoint - p.getArrivalTime()); //업데이트될 Burst Time

                    next = new Process(p.getName(), endpoint, n_burst); //다음에 스탬프로 찍어줄 업데이트된 프로세스
                    s = new Stamp(p.getName(), p.getArrivalTime(), startpoint); //스탬프 쾅쾅

                    addStamp(s);
                    StampProcess(next); //순환 호출
                    break;
                case 1:     //프로세스가 이미 스탬프가 찍혀있는 범위에 있는 경우
                    for (int i = 0; i < timestamp.Count; i++)
                    {
                        //가장 먼저 만나는 스탬프의 endpoint를 시작시간으로 설정
                        if (p.getArrivalTime() >= timestamp[i].getStartTime() && p.getArrivalTime() < timestamp[i].getEndTime())
                        {
                            startpoint = timestamp[i].getStartTime();
                            endpoint = timestamp[i].getEndTime();
                            break;
                        }
                    }
                    next = new Process(p.getName(), endpoint, p.getBurstTime()); //다음에 스탬프로 찍어줄 업데이트된 프로세스
                    StampProcess(next); //순환 호출
                    break;
                case 2:     //그냥 찍으면 됨
                    int end = p.getArrivalTime() + p.getBurstTime();
                    s = new Stamp(p.getName(), p.getArrivalTime(), end);
                    addStamp(s);    //스탬프 찍고 순환호출 안하고 끝내면 됩니다
                    for (int i = 0; i < inputData.Count; i++)
                    {
                        if (inputData[i].getName().Equals(s.getName()))
                        {
                            inputData[i].setEndTime(end);   //끝나는 시간을 설정해줌
                        }
                    }
                    break;
            }
        }

        public void pri_run(bool preemptive)
        {
            if (preemptive)//선점 - 지희
            {
                //우선순위- 들어온순으로 정렬
                temp = new List<Process>[20];
                for (int i = 0; i < 20; i++)
                {
                    temp[i] = new List<Process>();
                }
                //들어온순-우선순위으로 정렬(기수정렬을 응용)
                inputData.Sort(pri_compare);//우선순위
                for (int i = 0; i < inputData.Count; i++)
                {
                    temp[inputData[i].getPriority()].Add(inputData[i]);//우선순위의 범위가 0-9 이므로 이렇게 했습니다
                }

                for (int i = 0; i < 20; i++)
                {
                    temp[i].Sort(new Comparer(0));  //들어온순 정렬
                }
                inputData = new List<Process>();
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < temp[i].Count; j++)
                    {
                        inputData.Insert(0, temp[i][j]);//저게 큐가 아니니까 이렇게 처리해야 순서대로 들어갑니다
                        Ready.Insert(0, temp[i][j]);    //레디 리스트에도 삽입
                    }
                }

                do
                {
                    Process p = Ready.First();
                    timestamp.Sort(stamp_compare);  //스탬프 리스트를 정렬
                    StampProcess(p);    //스탬프를 계속 만들어줍니당
                    Ready.RemoveAt(0);
                } while (Ready.Count > 0);  //레디 리스트가 빌 때까지 반복
            }


            else   //비선점 - 민상
            {
                temp = new List<Process>[20];
                for (int i = 0; i < 20; i++)
                    temp[i] = new List<Process>();
                //들어온순-우선순위으로 정렬(기수정렬을 응용)
                Copy.Sort(new Comparer(0));//들어온 순으로 정렬
                for (int i = 0; i < inputData.Count; i++)
                    temp[Copy[i].getArrivalTime()].Add(Copy[i]);//도착시간의 범위가 0-9 이므로 이렇게 했습니다
                for (int i = 0; i < 20; i++)
                    temp[i].Sort(pri_compare);
                Copy = new List<Process>();
                for (int i = 0; i < 20; i++)
                    for (int j = 0; j < temp[i].Count; j++)
                        Copy.Add(temp[i][j]);//copy에 삽입
                //정렬은 끝났고 아래부분에서는 처리해줍니다                
                for (int i = 0; i < inputData.Count; i++)//즉 시간순으로 처리됩니다
                {
                    Ready.Add(Copy[0]);//리스트에 들어온 순서대로 데이터를 넣었습니다 이후 모든 처리는 큐를 기준으로 돌아갑니다
                    Copy.RemoveAt(0);
                    Copy.Sort(pri_compare);//큐가 아닌 리스트이므로 리스트를 큐처럼 만들기 위해 정렬을 합니다.
                    if (currentTime > Ready[0].getArrivalTime())//현재 시간이 실행할 놈 도착시간보다 빠르면(겹치면)
                        addStamp(new Stamp(Ready[0].getName(), currentTime, (currentTime += Ready[0].getBurstTime()))); //스탬프 쾅쾅
                    else
                    {
                        currentTime = Ready[0].getArrivalTime();
                        addStamp(new Stamp(Ready[0].getName(), currentTime, (currentTime += Ready[0].getBurstTime()))); //스탬프 쾅쾅
                    }
                    //this is the way how to get the tombstone point
                    int target = 0;
                    for (int j = 0; j < inputData.Count; j++)
                        if (Ready[0].getName().CompareTo(inputData[j].getName()) == 0)
                            target = j;
                    inputData[target].setEndTime(currentTime);//프로세스의 묘비명을 적어줍니다
                    Ready.RemoveAt(0);//실행끝난 프로세스는 죽입니다
                }
            }
        }

        public int pri_compare(Process a, Process b)    //정렬 Priority 기준으로 할라고 만듬
        {
            if (a.getPriority() > b.getPriority())
                return 1;
            else if (a.getArrivalTime() == b.getArrivalTime())
                return 0;
            else
                return -1;
        }

        private int stamp_compare(Stamp A, Stamp B) //스탬프 정렬하려고 만듬
        {
            return A.getStartTime().CompareTo(B.getStartTime());    //starttime 순으로 정렬
        }
    }
}