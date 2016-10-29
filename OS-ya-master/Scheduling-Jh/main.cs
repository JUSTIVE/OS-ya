using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Scheduling_Jh{
    public partial class main : Form{
        /////////////////////////////////////////////////////////////////폼에 그림자를 형성하는 부분    
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS{                           // struct for box shadow        
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams{
            get{
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled) cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        private bool CheckAeroEnabled(){
            if (Environment.OSVersion.Version.Major >= 6){
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m){
            switch (m.Msg){
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled){
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS(){
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default:break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;// drag the form
                
        }
        /////////////////////////////////////////////////////////////////여기까지 폼에 그림자를 형성하는 부분
        //클래스 내부 변수
        private bool is_down;
        private Point position;
        private box chlid=null;
        private List<Process> processListval;
        private List<Stamp> timestamp;
        int target_scheduler;
        private bool avail;
       
        public main(){InitializeComponent();}

        private void Form1_Load(object sender, EventArgs e){//폼이 로드되었을때 초기화
            processListval=new List<Process>(); //프로세스 목록
            timeSlice.Hide();                   //폼 생성 초기에는 숨김
            timeSliceText.Hide();               //폼 생성 초기에는 숨김
            timesliceUnderline.Hide();          //폼 생성 초기에는 숨김
            priority.Hide();                    //폼 생성 초기에는 숨김
            priorityText.Hide();                //폼 생성 초기에는 숨김
            priorityUnderline.Hide();           //폼 생성 초기에는 숨김
            is_down = false;                    //마우스가 눌린 상태를 검사합니다
            target_scheduler = 0;               //어떤 스케줄러를 선택하였습니까?
            running_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;   //버튼 배경 초기화
            addProcess.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;//버튼 배경 초기화
            button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;   //버튼 배경 초기화
            button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;   //버튼 배경 초기화
            listBox1.Items.Add("이 곳은 상태 메시지가 출력되는 곳입니다");                    //메시지 초기화
        }       
        private List<Process> getData() {return processListval;}//datalist내의 프로세스 리스트를 반환하는 함수
        private void button1_Click(object sender, EventArgs e){ //실행 버튼을 눌렀을 때의 이벤트 처리 함수
            if (processListval.Count > 0) {                     //추가된 프로세스가 1개 이상일 때
                switch (target_scheduler){                      //선택된 스케줄러 종류에 따른 분기문
                    case 0://선입선출 알고리즘
                        ////////////////////자식 폼을 생성합니다
                        chlid = new box(processListval,this);
                        chlid.SetBounds(Location.X+665,Location.Y,700,507);                
                        chlid.Show();
                        ////////////////////여기까지 자식 폼을 생성합니다
                        FCFS fcfs=new FCFS(getData());                        
                        fcfs.fcfs_run();
                        chlid.setStamp(fcfs.getTimestamp());
                        chlid.setAwtATT(System.Convert.ToDouble(fcfs.getAWT()),System.Convert.ToDouble(fcfs.getATT()));
                        break;
                    case 1://비선점 우선순위 알고리즘
                        /////////////////////우선순위가 기입되지 않은 프로세스를 검사합니다
                        avail = true;
                        for (int i = 0; i < processListval.Count; i++)
                            if (processListval[i].getPriority() == -1)
                                avail = false;
                        /////////////////////여기까지 우선순위가 기입되지 않은 프로세스를 검사합니다
                        if (avail){         //모든 프로세스가 우선순위를 가지고 있는 경우
                            ////////////////////자식 폼을 생성합니다
                            chlid = new box(processListval, this);
                            chlid.SetBounds(Location.X + 665, Location.Y, 700, 507);
                            chlid.Show();
                            ////////////////////여기까지 자식 폼을 생성합니다
                            Priority p1 = new Priority(getData());
                            p1.pri_run(false);
                            chlid.setStamp(p1.getTimestamp());
                            chlid.setAwtATT(System.Convert.ToDouble(p1.getAWT()), System.Convert.ToDouble(p1.getATT()));
                        }
                        else listBox1.Items.Insert(0,"우선순위가 없는 프로세스가 존재합니다");                        
                        break;
                    case 2://선점 우선순위 알고리즘
                        /////////////////////우선순위가 기입되지 않은 프로세스를 검사합니다
                        avail = true;
                        for (int i = 0; i < processListval.Count; i++)
                            if (processListval[i].getPriority() == -1)
                                avail = false;
                        /////////////////////여기까지 우선순위가 기입되지 않은 프로세스를 검사합니다
                        if (avail){
                            ////////////////////자식 폼을 생성합니다
                            chlid = new box(processListval, this);
                            chlid.SetBounds(Location.X + 665, Location.Y, 700, 507);
                            chlid.Show();
                            ////////////////////여기까지 자식 폼을 생성합니다
                            Priority p1 = new Priority(getData());
                            p1.pri_run(true);
                            timestamp = p1.getTimestamp();
                            timestamp.Sort(stmp_compare);
                            chlid.setStamp(timestamp);
                            chlid.setAwtATT(System.Convert.ToDouble(p1.getAWT()), System.Convert.ToDouble(p1.getATT()));
                        }
                        else listBox1.Items.Insert(0,"우선순위가 없는 프로세스가 존재합니다");
                        break;
                    case 3://숏잡먼저
                        ////////////////////자식 폼을 생성합니다
                        chlid = new box(processListval,this);
                        chlid.SetBounds(Location.X+665,Location.Y,700,507);                
                        chlid.Show();
                        ////////////////////여기까지 자식 폼을 생성합니다
                        SJF sjf = new SJF(processListval);
                        sjf.sjf_run();
                        chlid.setStamp(sjf.getTimestamp());
                        chlid.setAwtATT(System.Convert.ToDouble(sjf.getAWT()), System.Convert.ToDouble(sjf.getATT()));
                        break;                    
                    case 4://스르트
                        ////////////////////자식 폼을 생성합니다
                        chlid = new box(processListval,this);
                        chlid.SetBounds(Location.X+665,Location.Y,700,507);                
                        chlid.Show();
                        ////////////////////여기까지 자식 폼을 생성합니다
                        SRT srt = new SRT(getData());
                        srt.srt_run();
                        timestamp = srt.getTimestamp();
                        timestamp.Sort(stmp_compare);
                        chlid.setStamp(timestamp);
                        chlid.setAwtATT(System.Convert.ToDouble(srt.getAWT()), System.Convert.ToDouble(srt.getATT()));
                        break;
                    case 5://랄랄
                        ////////////////////시간할당량이 기입되었는지 확인합니다
                        if(timeSlice.Text==""){
                            timeSliceText.ForeColor=Color.Red;
                            listBox1.Items.Insert(0, "시간 할당량이 설정되지 않았습니다");
                            return;
                        }
                        ////////////////////여기까지 시간할당량이 기입되었는지 확인합니다
                        else{
                            ////////////////////자식 폼을 생성합니다
                            chlid = new box(processListval, this);
                            chlid.SetBounds(Location.X + 665, Location.Y, 700, 507);
                            chlid.Show();
                            ////////////////////여기까지 자식 폼을 생성합니다
                            timeSliceText.ForeColor = Color.Black;
                            int quant;
                            Int32.TryParse(timeSlice.Text, out quant);
                            RR rr=new RR(getData(), quant);
                            rr.rr_alg();
                            chlid.setStamp(rr.getTimestamp());
                            chlid.setAwtATT(System.Convert.ToDouble(rr.getAWT()), System.Convert.ToDouble(rr.getATT()));
                        }
                        break;
                    case 6:
                        ////////////////////자식 폼을 생성합니다
                        chlid = new box(processListval, this);
                        chlid.SetBounds(Location.X + 665, Location.Y, 700, 507);
                        chlid.Show();
                        ////////////////////여기까지 자식 폼을 생성합니다
                        HRN hrn = new HRN(getData());
                        hrn.hrn_alg();
                        chlid.setStamp(hrn.getTimestamp());
                        chlid.setAwtATT(System.Convert.ToDouble(hrn.getAWT()), System.Convert.ToDouble(hrn.getATT()));
                        break;                        
                }
                this.Focus();//버튼의 잔여 포커스를 제거하기 위해 사용
                chlid.sidebar.Location = new Point(chlid.Location.X-284,chlid.Location.Y);//사이드바의 위치를 여기서 미리 지정
                chlid.sidebar.Visible = false;//사이드바를 숨김
            }
            else listBox1.Items.Insert(0, "프로세스를 추가해주세요");//프로세스가 존재하지 않는 경우 에러 메시지 출력
        }

        ////////////////////////////라디오 버튼 누를때의 이벤트 처리
        private void radioButton7_CheckedChanged(object sender, EventArgs e){if (radioButton7.Checked)target_scheduler = 6;}                
        private void radioButton4_CheckedChanged(object sender, EventArgs e){if (radioButton4.Checked){target_scheduler = 5;timeSlice.Show();timeSliceText.Show();timesliceUnderline.Show();}
            else{timeSlice.Hide(); timeSliceText.Hide(); timesliceUnderline.Hide();}}
        private void radioButton5_CheckedChanged(object sender, EventArgs e) { if (radioButton5.Checked)target_scheduler = 4; }
        private void radioButton6_CheckedChanged(object sender, EventArgs e) { if (radioButton6.Checked)target_scheduler = 3; }
        private void radioButton3_CheckedChanged(object sender, EventArgs e){if (radioButton3.Checked){target_scheduler = 2;priority.Show();priorityText.Show();priorityUnderline.Show();}
            else{priority.Hide();priorityText.Hide();priorityUnderline.Hide();}}
        private void radioButton2_CheckedChanged(object sender, EventArgs e){if (radioButton2.Checked){target_scheduler = 1;priority.Show();priorityText.Show();priorityUnderline.Show();}
            else{priority.Hide();priorityText.Hide();priorityUnderline.Hide();}}
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { if (radioButton1.Checked)target_scheduler = 0; }
        ////////////////////////////여기까지 라디오 버튼 누를때의 이벤트 처리
        public int stmp_compare(Stamp a, Stamp b) { if (a.getStartTime() > b.getStartTime()) return 1; else if (a.getStartTime() == b.getStartTime())return 0; else return -1; }//스탬프의 정렬
                
        private void main_FormClosing(object sender, FormClosingEventArgs e){Application.Exit();}//메인 폼이 닫힐 때의 처리
        private void arrivalTime_KeyPress(object sender, KeyPressEventArgs e){
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;            
        }
        private void burstTime_KeyPress(object sender, KeyPressEventArgs e){
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;            
            if (Convert.ToInt32(e.KeyChar) == 13){
                arrivalTime.Focus();
                if (arrivalTime.Text == ""){
                    listBox1.Items.Insert(0, "도착시간을 기입해주세요");
                    arrivalTimeText.ForeColor = Color.Red;
                }
                else{
                    arrivalTimeText.ForeColor = Color.Black;
                    if (burstTime.Text == ""){
                        listBox1.Items.Insert(0, "실행시간을 기입해주세요");
                        burstTimeText.ForeColor = Color.Red;
                    }
                    else{
                        burstTimeText.ForeColor = Color.Black;
                        if (radioButton2.Checked || radioButton3.Checked){
                            if (priority.Text == ""){
                                listBox1.Items.Insert(0, "우선순위를 기입해주세요");
                                priorityText.ForeColor = Color.Red;
                            }
                            else{
                                priorityText.ForeColor = Color.Black;
                                addProcessToList(true);
                            }
                        }
                        else
                            addProcessToList(false);
                    }
                }
            }
        }
        private void priority_KeyPress(object sender, KeyPressEventArgs e){
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }
        private void timeSlice_KeyPress(object sender, KeyPressEventArgs e){
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46)//8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void addProcessToList(bool flag){//프로세스를 추가하는 버튼
            int temp1,temp2,temp3;
            Process newProcess;
            String name;
            if (processName.Text == "") name = ("p"+(processListval.Count + 1));
            else name = processName.Text;
            Int32.TryParse(arrivalTime.Text,out temp1);
            Int32.TryParse(burstTime.Text,out temp2);
            if (flag){
                newProcess = new Process(name, temp1,temp2);
                processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), 0 + "");
            }
            else{
                Int32.TryParse(priority.Text, out temp3);
                newProcess = new Process(name, temp1, temp2,temp3);
                processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), priority.Text);
                priority.Text = "";
            }
            processListval.Add(newProcess);
            processName.Text = "";
            arrivalTime.Text = "";            
            burstTime.Text = "";
        }
        private void button2_Click(object sender, EventArgs e){
            if(arrivalTime.Text==""){
                listBox1.Items.Insert(0,"도착시간을 기입해주세요");
                arrivalTimeText.ForeColor = Color.Red;
            }
            else{
                arrivalTimeText.ForeColor = Color.Black;
                if(burstTime.Text==""||burstTime.Text=="0"){
                    if (burstTime.Text == "") listBox1.Items.Insert(0, "실행시간을 기입해주세요");
                    else listBox1.Items.Insert(0, "프로세스의 실행시간은 0 이상의 정수입니다.");
                    burstTimeText.ForeColor = Color.Red;
                }
                else{
                    burstTimeText.ForeColor = Color.Black;
                    if (radioButton2.Checked || radioButton3.Checked){
                        if (priority.Text == ""){
                            listBox1.Items.Insert(0, "우선순위를 기입해주세요");
                            priorityText.ForeColor = Color.Red;
                        }
                        else{
                            priorityText.ForeColor = Color.Black;
                            addProcessToList(true);
                        }
                    }
                    else addProcessToList(false);
                }
            }
        }        
        private void button3_Click(object sender, EventArgs e){//랜덤추가 버튼
            Random r=new Random();
            Process newProcess;
            arrivalTime.Text = "";
            burstTime.Text = "";
            priority.Text = "";
            processName.Text = "";
            int temp1=r.Next(0,10);
            int temp2=r.Next(1,10);
            int temp3=r.Next(0,10);

            if (radioButton2.Checked || radioButton3.Checked){
                newProcess= new Process("P"+(processListval.Count+1)+"",temp1,temp2,temp3);
                processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), temp3+"");
            }
            else{
                newProcess = new Process("P"+(processListval.Count + 1), temp1, temp2);
                processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime());
            }
            processListval.Add(newProcess);
            int icon = r.Next(0,5);
            this.button3.Image = null;
            switch (icon) { 
                case 0:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice1_icon));break;
                case 1:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice2_icon));break;
                case 2:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice3_icon));break;
                case 3:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice4_icon));break;
                case 4:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice5_icon));break;
                case 5:this.button3.Image = ((System.Drawing.Image)(Properties.Resources.dice6_icon));break;
            }
        }
        private void button2_Click_1(object sender, EventArgs e) { if (chlid != null) chlid.Close(); processListval = null; timestamp = null; Application.Exit(); }

        private void panel1_MouseDown(object sender, MouseEventArgs e){is_down = true; position.X = e.X; position.Y = e.Y; }
        private void panel5_MouseDown(object sender, MouseEventArgs e){is_down = true; position.X = e.X; position.Y = e.Y; }
        private void panel1_MouseMove(object sender, MouseEventArgs e){if (is_down){Point p = PointToScreen(e.Location);Location = new Point(p.X - position.X, p.Y - position.Y); } }
        private void panel5_MouseMove(object sender, MouseEventArgs e){if (is_down){Point p = PointToScreen(e.Location);Location = new Point(p.X - position.X, p.Y - position.Y);}}
        
        private void button4_Click(object sender, EventArgs e){if (processListval.Count > 0) { int index=processListval.Count - 1;processListval.RemoveAt(index);processList.Rows.RemoveAt(index);}}
        ////////////////////////////////////////////예제버튼
        private void button5_Click(object sender, EventArgs e){
            Process newProcess;
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 8);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(),"");
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 1, 4);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), "");
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 2, 9);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), "");
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 3, 5);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), "");
            processListval.Add(newProcess);
        }
        private void DEV2_Click(object sender, EventArgs e){
            Process newProcess;
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 10,3);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);            
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 1,1);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 2,3);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0,1, 4);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 5, 2);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
        }
        private void button5_Click_1(object sender, EventArgs e){
            Process newProcess;
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 10, 3);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 1, 1, 1);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 2,2, 3);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 3, 1, 4);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 4, 5, 2);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
        }
        private void button5_Click_2(object sender, EventArgs e){
            Process newProcess;
            newProcess = new Process("p" + (processListval.Count + 1) + "", 0, 7, 3);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 2, 4, 2);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 5, 2, 4);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 6, 5, 1);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            newProcess = new Process("p" + (processListval.Count + 1) + "", 10, 1, 2);
            processList.Rows.Add(newProcess.getName(), newProcess.getArrivalTime(), newProcess.getBurstTime(), newProcess.getPriority());
            processListval.Add(newProcess);
            timeSlice.Text = "2";
        }
        ////////////////////////////////////////////여기까지 예제버튼
        private void panel5_MouseUp(object sender, MouseEventArgs e) {is_down = false;}
        private void panel1_MouseUp(object sender, MouseEventArgs e) {is_down = false;}

        private void button1_MouseLeave(object sender, EventArgs e) { this.running_button.BackgroundImage = null; }
        private void addProcess_MouseLeave(object sender, EventArgs e) { this.addProcess.BackgroundImage = null; }
        private void button4_MouseLeave(object sender, EventArgs e) { this.button4.BackgroundImage = null; }
        private void button3_MouseLeave(object sender, EventArgs e) { this.button3.BackgroundImage = null; }

        private void button1_MouseHover(object sender, EventArgs e){this.running_button.BackColor = System.Drawing.Color.Transparent;}
        private void button3_MouseHover(object sender, EventArgs e) { this.button3.BackColor = System.Drawing.Color.Transparent; }
        private void button4_MouseHover(object sender, EventArgs e) { this.button4.BackColor = System.Drawing.Color.Transparent; }
        private void addProcess_MouseHover(object sender, EventArgs e) { this.addProcess.BackColor = System.Drawing.Color.Transparent; }

        private void button1_MouseEnter(object sender, EventArgs e){this.running_button.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void addProcess_MouseEnter(object sender, EventArgs e){this.addProcess.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void button4_MouseEnter(object sender, EventArgs e){this.button4.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void button3_MouseEnter(object sender, EventArgs e){this.button3.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}

        private void button1_MouseDown(object sender, MouseEventArgs e){this.running_button.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void button4_MouseDown(object sender, MouseEventArgs e){this.button4.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void addProcess_MouseDown(object sender, MouseEventArgs e){this.addProcess.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}
        private void button3_MouseDown(object sender, MouseEventArgs e){this.button3.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonbackground));}

        private void nameUnderline_Paint(object sender, PaintEventArgs e)
        {

        }       
    }
}
