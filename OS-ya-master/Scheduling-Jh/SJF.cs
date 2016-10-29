using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling_Jh
{
    class SJF : Scheduler
    {
        List<Process> Ready;   //레디큐
        List<Process>[] temp;
        public SJF(List<Process> list)
            : base(list)
        {
            Ready = new List<Process>();
            currentTime = 0;
        }
        public void sjf_run()
        {
            temp = new List<Process>[20];
            for (int i = 0; i < 20; i++){
                temp[i] = new List<Process>();
            }
            //들어온순-우선순위으로 정렬(기수정렬을 응용)
            inputData.Sort(new Comparer(0));//들어온 순으로 정렬
            for (int i = 0; i < inputData.Count; i++){
                temp[inputData[i].getArrivalTime()].Add(inputData[i]);//도착시간의 범위가 0-9 이므로 이렇게 했습니다
            }
            for (int i = 0; i < 20; i++){
                temp[i].Sort(new Comparer(1));
            }
            inputData = new List<Process>();
            for (int i = 0; i < 20; i++){
                for (int j = 0; j < temp[i].Count; j++){
                    inputData.Insert(0, temp[i][j]);//저게 큐가 아니니까 이렇게 처리해야 순서대로 들어갑니다
                }
            }

            List<int> blacklist = new List<int>();
            inputData.Sort(new Comparer(0));

            for (int i = 0; i < inputData.Count; i++){
                Console.WriteLine(inputData[i].getName());
            }
            //airgap
            blacklist.Add(0);
            Ready.Insert(0, inputData[0]);
            for (int i = 0; i < inputData.Count; i++){
                Ready.Sort(new Comparer(1));
                for (int j = 0; j < Ready.Count; j++){
                    Console.WriteLine("Ready[" + j + "]" + Ready[j].getName());
                }
                Console.WriteLine(" ");
                addStamp(new Stamp(Ready[0].getName(), currentTime, (currentTime += Ready[0].getBurstTime())));

                for (int j = 0; j < inputData.Count; j++){
                    if (inputData[j].getName().CompareTo(Ready[0].getName()) == 0)
                        inputData[j].setEndTime(currentTime);
                }
                Ready.RemoveAt(0);

                //검색
                for (int j = 0; j < inputData.Count; j++){
                    if (inputData[j].getArrivalTime() < currentTime){
                        bool flag = true;
                        for (int l = 0; l < blacklist.Count; l++){
                            if (j == blacklist[l]){
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            Ready.Add(inputData[j]);
                            blacklist.Insert(0, j);
                        }
                    }
                }
            }
        }
    }
}
