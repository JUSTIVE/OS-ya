using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Scheduling_Jh{
    public partial class box : Form{
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
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;
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
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;// drag the form
        }
        /////////////////////////////////////////////////////////////////여기까지 폼에 그림자를 형성하는 부분
        public sidebar sidebar;
        Color[] myPalette=new Color[20];
        main main;
        private Rectangle[] graphs;
        bool is_down;
        Point position;
        private int currentPosition;        //현재 진행중인 stamp 번호
        private List<Stamp> stmp_list;      //스탬프 리스트
        private List<Process> pro_list;     //프로세스 리스트
        bool draw = false;
        int targetPosition;
        bool is_run = false;
        double usage;//사용시간 백분율
        double awt, att,art;
        bool isopen;
        int drawgap=500;
        
        public box(List<Process> list,main main){
            InitializeComponent();
            this.main = main;
            this.pro_list = list;            
            this.stmp_list = new List<Stamp>();
            position = new Point();
            currentPosition = 0;
            is_down = false;
            usage = 0;
            isopen = false;
            drawgap = 50;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            usageText.Text = "";
            awtText.Text = "";
            attText.Text = "";
            artText.Text = "";
            myPalette[0] = System.Drawing.ColorTranslator.FromHtml("#FF565A");
            myPalette[1] = System.Drawing.ColorTranslator.FromHtml("#FF9379");
            myPalette[2] = System.Drawing.ColorTranslator.FromHtml("#FFDD9E");
            myPalette[3] = System.Drawing.ColorTranslator.FromHtml("#C2D5B0");
            myPalette[4] = System.Drawing.ColorTranslator.FromHtml("#84CDC2");
            myPalette[5] = System.Drawing.ColorTranslator.FromHtml("#18709C");
            myPalette[6] = System.Drawing.ColorTranslator.FromHtml("#7E346B");
            myPalette[7] = System.Drawing.ColorTranslator.FromHtml("#EF4089");
            Graphics gc = chart.CreateGraphics();
            Pen background = new Pen((new SolidBrush(Color.FromArgb((byte)0xFF, 66, 66, 66))), 20);
            Rectangle range = new Rectangle(new Point(20, 20), new Size(chart.Size.Width - 40, chart.Size.Height - 40));
            gc.DrawArc(background, range, 0, 360);
            button1.BackColor = Color.Gray;
            sidebar = new sidebar(list);
            sidebar.Visible = true;
        }
        public void setStamp(List<Stamp> list){stmp_list = list;graphs = new Rectangle[list.Count];}
        //그릴 함수
        public void setAwtATT(double AWT, double ATT) {
            double ART=0;
            for (int i = 0; i < pro_list.Count; i++)ART += pro_list[i].getBurstTime();
            ART /= pro_list.Count;
            this.awt = AWT;
            this.att = ATT;
            this.art = ART;
        }
        private void box_FormClosing(object sender, FormClosingEventArgs e){this.Dispose();}
        private void button4_Click(object sender, EventArgs e){sidebar.Close();Close();}
        private void panel2_MouseDown(object sender, MouseEventArgs e){position.X = e.X;position.Y = e.Y;is_down = true;}
        private void panel3_MouseDown(object sender, MouseEventArgs e){position.X = e.X;position.Y = e.Y;is_down = true;}
        private void panel2_MouseUp(object sender, MouseEventArgs e){is_down = false;}
        private void panel3_MouseUp(object sender, MouseEventArgs e){is_down = false;}
        private void panel3_MouseMove(object sender, MouseEventArgs e){if (is_down){Point p = PointToScreen(e.Location);Location = new Point(p.X - position.X, p.Y - position.Y);sidebar.Location = new Point(p.X - position.X - 284, p.Y - position.Y);}}
        private void panel2_MouseMove(object sender, MouseEventArgs e){if (is_down){Point p = PointToScreen(e.Location);Location = new Point(p.X - position.X, p.Y - position.Y);sidebar.Location = new Point(p.X - position.X - 284, p.Y - position.Y);}}
        private bool isExist(List<int>list,int target){
            for (int i = 0; i < list.Count; i++)
                if (list[i] == target)
                    return true;
            return false;
        }
        private void Ghannt_base_Paint(object sender, PaintEventArgs e){
            if (!is_run){
                if (draw){
                    Application.DoEvents();
                    artText.Text = Math.Round(art,4) + "";attText.Text = Math.Round(att, 4) + "";awtText.Text = Math.Round(awt, 4) + "";

                    Graphics gc = chart.CreateGraphics();
                    gc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    int timesum = 0;
                    usage = 0;
                    is_run = true;
                    List<int> log=new List<int>();
                    Graphics fg= ghattbox.CreateGraphics();
                    Graphics g = e.Graphics;
                    Random r = new Random();
                    Brush[] brush = new SolidBrush[pro_list.Count];
                    Pen p;

                    Pen background = new Pen((new SolidBrush(Color.FromArgb((byte)0xFF, 66, 66, 66))), 20);
                    Rectangle range = new Rectangle(new Point(20, 20), new Size(chart.Size.Width - 40, chart.Size.Height - 40));
                    gc.DrawArc(background, range, 0, 360);
                    Point startPoint = new Point();
                    int max_end = 0;
                    for (int i = 0; i < pro_list.Count; i++){
                        //최대 프로세스 종료 시간을 구한다.
                        max_end = (max_end > pro_list[i].getEndTime() ? max_end : pro_list[i].getEndTime());
                        //같은 반복문 내에서 브러시를 만든다                        
                        if (i < 8)brush[i] = new SolidBrush(myPalette[i]);
                        else {
                            byte red = (byte)r.Next(100, 200), green = (byte)r.Next(100, 200), blue = (byte)r.Next(100, 200);
                            brush[i] = new SolidBrush(Color.FromArgb((byte)0xFF,red, green, blue));
                        }
                    }
                    startPoint.X = (0);
                    startPoint.Y = (0);
                    fg.DrawString(0 + "", new Font("Microsoft Sans Serif", 8), Brushes.Black, new Point(startPoint.X, startPoint.Y + 58));
                    log.Add(0);
                    if (targetPosition == -1){//자동진행                     
                        for (; currentPosition < stmp_list.Count; currentPosition++){
                            listBox1.Items.Insert(0,"<Process Inserted>");
                            listBox1.Items.Insert(0, " : Name :" + stmp_list[currentPosition].getName() + "  At >"+stmp_list[currentPosition].getStartTime());
                            listBox1.Refresh();
                            usageText.Text = Math.Round(usage * 100, 2) + "%";
                            int index = 0;
                            for (int j = 0; j < pro_list.Count; j++){
                                if ((String.Compare(pro_list[j].getName(), stmp_list[currentPosition].getName()) == 0)){
                                    index = j;
                                    break;
                                }
                            }
                            startPoint.X = (((stmp_list[currentPosition].getStartTime()) * Ghannt_base.Size.Width) / max_end);                                       
                            //get width in pixel
                            double width = Convert.ToInt32(Math.Round((double)((stmp_list[currentPosition].getTimeGap() * Ghannt_base.Size.Width) / max_end)));
                            graphs[currentPosition] = new Rectangle(new Point(startPoint.X, startPoint.Y), new Size((int)width, Ghannt_base.Size.Height));
                            
                            p = new Pen(brush[index], 5);
                            g.FillRectangle(brush[index],graphs[currentPosition]);
                            
                            g.DrawString(stmp_list[currentPosition].getName(), new Font("Microsoft Sans Serif", 12), Brushes.White, graphs[currentPosition]);
                            if (!isExist(log, stmp_list[currentPosition].getStartTime())){
                                fg.DrawString(stmp_list[currentPosition].getStartTime() + "", 
                                    new Font("Microsoft Sans Serif", 8), Brushes.Black, new Point(startPoint.X, startPoint.Y + 58));
                                log.Add(stmp_list[currentPosition].getStartTime());
                            }
                            if (!isExist(log, stmp_list[currentPosition].getEndTime())){
                                fg.DrawString(stmp_list[currentPosition].getEndTime() + "", 
                                    new Font("Microsoft Sans Serif", 8), Brushes.Black, new Point(startPoint.X + (int)width, startPoint.Y + 58));
                                log.Add(stmp_list[currentPosition].getEndTime());
                            }
                            timesum += stmp_list[currentPosition].getTimeGap();
                            usage=(double)timesum/(double)stmp_list[currentPosition].getEndTime();

                            listBox1.Items.Insert(0, "<Process Ended>");
                            listBox1.Items.Insert(0, " : Name :" + stmp_list[currentPosition].getName() + "  At >" + stmp_list[currentPosition].getStartTime());
                            listBox1.Refresh();attText.Refresh();artText.Refresh();awtText.Refresh();
                            Thread.Sleep(drawgap);
                        }
                        targetPosition = stmp_list.Count;                 
                    }
                    else{//단계진행
                        for (; currentPosition < targetPosition; currentPosition++){                            
                            int index = 0;
                            for (int j = 0; j < targetPosition; j++){
                                if ((String.Compare(pro_list[j].getName(), stmp_list[currentPosition].getName()) == 0)){
                                    index = j;
                                    break;
                                }
                            }
                            startPoint.X = (((stmp_list[currentPosition].getStartTime()) * Ghannt_base.Size.Width) / max_end);
                            //get width in pixel
                            double width = Convert.ToInt32(Math.Round((double)((stmp_list[currentPosition].getTimeGap() * Ghannt_base.Size.Width) / max_end)));
                            graphs[currentPosition] = new Rectangle(new Point(startPoint.X, startPoint.Y), new Size((int)width, Ghannt_base.Size.Height));

                            
                            p = new Pen(brush[index], 5);
                            g.FillRectangle(brush[index], graphs[currentPosition]);
                            
                            g.DrawString(stmp_list[currentPosition].getName(), new Font("Microsoft Sans Serif", 12), Brushes.White, graphs[currentPosition]);
                            if (!isExist(log, stmp_list[currentPosition].getStartTime())){
                                fg.DrawString(stmp_list[currentPosition].getStartTime() + "", new Font("Microsoft Sans Serif", 8), Brushes.Black, new Point(startPoint.X, startPoint.Y + 58));
                                log.Add(stmp_list[currentPosition].getStartTime());
                            }
                            if (!isExist(log, stmp_list[currentPosition].getEndTime())){
                                fg.DrawString(stmp_list[currentPosition].getEndTime() + "", new Font("Microsoft Sans Serif", 8), Brushes.Black, new Point(startPoint.X + (int)width, startPoint.Y + 58));
                                log.Add(stmp_list[currentPosition].getEndTime());
                            }
                            timesum += stmp_list[currentPosition].getTimeGap();
                            usage = (double)timesum / (double)stmp_list[currentPosition].getEndTime();                                                                                    
                        }                        
                    }
                    if (currentPosition > 0){
                        gc.DrawArc(background, range, 0, 360);
                        g.DrawString("" + max_end, new Font("Microsoft Sans Serif", 12), Brushes.Black, new Point(4, 53));
                        Thread weedlink = new Thread(new ThreadStart(weeded));
                        weedlink.Start();
                        weedlink.Join();
                        draw = false;
                    }
                    else{
                        usageText.Text = "0%";
                        usageText.Refresh();
                    }                            
                }
                is_run = false;
            }
        }
        //그래프를 그리는 스레드
        public void weeded(){
            Pen background = new Pen((new SolidBrush(Color.FromArgb((byte)0xFF, 66, 66, 66))), 20);
            Graphics gc = chart.CreateGraphics();            
            gc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen foreground = new Pen((new SolidBrush(Color.FromArgb((byte)0xFF, 255, 193, 7))), 20);
            Rectangle range = new Rectangle(new Point(20, 20), new Size(chart.Size.Width - 40, chart.Size.Height - 40));
            gc.DrawArc(background, range, 0, 360);
            float limit = (float)(usage * 360);            
            float bottom=0;
            int time=Convert.ToInt32(limit/360*20);//고정시간: 선형 애니메이션
            for (bottom=0; bottom < limit+1; bottom += limit / 50){
                usageText.Text = Math.Round(bottom /360*100, 2) + "%";
                usageText.Refresh();                
                gc.DrawArc(foreground, range, 0, bottom);                
                Thread.Sleep(time);
            }           
            return;            
        }             
        private void button2_Click(object sender, EventArgs e){             
            targetPosition = -1;
            currentPosition = 0;
            draw = true;
            Ghannt_base.Paint += new PaintEventHandler(Ghannt_base_Paint);
            Ghannt_base.Refresh();
            
            button1.BackColor = Color.FromArgb(255, 255, 193, 7);
            button3.BackColor = Color.Gray;
        }
        private void button3_Click(object sender, EventArgs e){
            button1.BackColor = Color.FromArgb(255, 255, 193, 7);
            button3.BackColor = Color.FromArgb(255, 255, 193, 7);
            if (targetPosition < stmp_list.Count) {
                currentPosition = 0;
                targetPosition++;
                draw = true;
                Ghannt_base.Paint += new PaintEventHandler(Ghannt_base_Paint);
                Ghannt_base.Refresh();
            }
            else this.main.listBox1.Items.Insert(0, "프로세스의 끝입니다");
            if (targetPosition == stmp_list.Count)button3.BackColor= Color.Gray;
            else button3.BackColor = Color.FromArgb(255, 255, 193, 7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(255, 255, 193, 7);
            button1.BackColor = Color.FromArgb(255, 255, 193, 7);
            if (targetPosition > 0){
                currentPosition = 0;
                targetPosition--;
                draw = true;
                if(!is_run) Ghannt_base.Paint += new PaintEventHandler(Ghannt_base_Paint);
                Ghannt_base.Refresh();
            }
            else
                this.main.listBox1.Items.Insert(0, "프로세스의 처음입니다");            
            if (targetPosition == 0) button1.BackColor = Color.Gray;
            else button1.BackColor = Color.FromArgb(255, 255, 193, 7);
        }       
        private void button5_Click(object sender, EventArgs e){
            if (!isopen){
                isopen = true;
                sidebar.Visible = true;
            }
            else{
                sidebar.Visible = false;
                Ghannt_base.SuspendLayout();
                isopen = false;                
                Ghannt_base.ResumeLayout();                
            }
            drawgap = 0;
            drawgap = 50;
            this.Focus();
        }
    }
}
