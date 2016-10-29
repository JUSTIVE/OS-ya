namespace Scheduling_Jh
{
    partial class box
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ghattbox = new System.Windows.Forms.Panel();
            this.Ghannt_base = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.chart = new Scheduling_Jh.DoubleBufferedPanel();
            this.usageText = new System.Windows.Forms.Label();
            this.artText = new System.Windows.Forms.Label();
            this.artLabel = new System.Windows.Forms.Label();
            this.attText = new System.Windows.Forms.Label();
            this.attLabel = new System.Windows.Forms.Label();
            this.awtText = new System.Windows.Forms.Label();
            this.awtLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ghattbox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.chart.SuspendLayout();
            this.SuspendLayout();
            // 
            // ghattbox
            // 
            this.ghattbox.BackColor = System.Drawing.Color.White;
            this.ghattbox.Controls.Add(this.Ghannt_base);
            this.ghattbox.Location = new System.Drawing.Point(12, 101);
            this.ghattbox.Name = "ghattbox";
            this.ghattbox.Size = new System.Drawing.Size(676, 92);
            this.ghattbox.TabIndex = 0;
            // 
            // Ghannt_base
            // 
            this.Ghannt_base.Location = new System.Drawing.Point(5, 8);
            this.Ghannt_base.Margin = new System.Windows.Forms.Padding(0);
            this.Ghannt_base.Name = "Ghannt_base";
            this.Ghannt_base.Size = new System.Drawing.Size(664, 51);
            this.Ghannt_base.TabIndex = 0;
            this.Ghannt_base.Paint += new System.Windows.Forms.PaintEventHandler(this.Ghannt_base_Paint);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(144, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 53);
            this.button1.TabIndex = 1;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.Location = new System.Drawing.Point(200, 445);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(307, 53);
            this.button2.TabIndex = 2;
            this.button2.Text = "자동 출력";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(508, 445);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(55, 53);
            this.button3.TabIndex = 3;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 73);
            this.panel2.TabIndex = 4;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // button5
            // 
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Gulim", 27F);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(16, 25);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(47, 33);
            this.button5.TabIndex = 7;
            this.button5.Text = "≡\r\n";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(66, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 33);
            this.label4.TabIndex = 6;
            this.label4.Text = "Output";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            this.panel3.Controls.Add(this.button4);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(700, 18);
            this.panel3.TabIndex = 5;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseDown);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseMove);
            this.panel3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseUp);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(644, 0);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(56, 17);
            this.button4.TabIndex = 6;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(5, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(176, 202);
            this.listBox1.TabIndex = 5;            
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.listBox1);
            this.panel4.Location = new System.Drawing.Point(12, 198);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(676, 238);
            this.panel4.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.chart);
            this.panel6.Controls.Add(this.artText);
            this.panel6.Controls.Add(this.artLabel);
            this.panel6.Controls.Add(this.attText);
            this.panel6.Controls.Add(this.attLabel);
            this.panel6.Controls.Add(this.awtText);
            this.panel6.Controls.Add(this.awtLabel);
            this.panel6.Controls.Add(this.label5);
            this.panel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(156)))), ((int)(((byte)(138)))));
            this.panel6.Location = new System.Drawing.Point(188, 27);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(483, 202);
            this.panel6.TabIndex = 6;            
            // 
            // chart
            // 
            this.chart.Controls.Add(this.usageText);
            this.chart.Location = new System.Drawing.Point(15, 34);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(160, 160);
            this.chart.TabIndex = 1;
            // 
            // usageText
            // 
            this.usageText.AutoSize = true;
            this.usageText.Font = new System.Drawing.Font("Sony Sketch EF", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usageText.Location = new System.Drawing.Point(56, 68);
            this.usageText.Name = "usageText";
            this.usageText.Size = new System.Drawing.Size(53, 16);
            this.usageText.TabIndex = 0;
            this.usageText.Text = "label3";
            // 
            // artText
            // 
            this.artText.AutoSize = true;
            this.artText.Font = new System.Drawing.Font("Sony Sketch EF", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artText.Location = new System.Drawing.Point(204, 38);
            this.artText.Name = "artText";
            this.artText.Size = new System.Drawing.Size(80, 27);
            this.artText.TabIndex = 7;
            this.artText.Text = "label6";            
            // 
            // artLabel
            // 
            this.artLabel.AutoSize = true;
            this.artLabel.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.artLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.artLabel.Location = new System.Drawing.Point(206, 20);
            this.artLabel.Name = "artLabel";
            this.artLabel.Size = new System.Drawing.Size(225, 18);
            this.artLabel.TabIndex = 6;
            this.artLabel.Text = "평균 실행 시간(Average Running Time)";            
            // 
            // attText
            // 
            this.attText.AutoSize = true;
            this.attText.Font = new System.Drawing.Font("Sony Sketch EF", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attText.Location = new System.Drawing.Point(204, 149);
            this.attText.Name = "attText";
            this.attText.Size = new System.Drawing.Size(80, 27);
            this.attText.TabIndex = 5;
            this.attText.Text = "label8";
            // 
            // attLabel
            // 
            this.attLabel.AutoSize = true;
            this.attLabel.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.attLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.attLabel.Location = new System.Drawing.Point(206, 131);
            this.attLabel.Name = "attLabel";
            this.attLabel.Size = new System.Drawing.Size(250, 18);
            this.attLabel.TabIndex = 4;
            this.attLabel.Text = "평균 반환 시간(Average Turn-around Time)";
            // 
            // awtText
            // 
            this.awtText.AutoSize = true;
            this.awtText.Font = new System.Drawing.Font("Sony Sketch EF", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.awtText.Location = new System.Drawing.Point(204, 93);
            this.awtText.Name = "awtText";
            this.awtText.Size = new System.Drawing.Size(80, 27);
            this.awtText.TabIndex = 3;
            this.awtText.Text = "label6";
            // 
            // awtLabel
            // 
            this.awtLabel.AutoSize = true;
            this.awtLabel.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.awtLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.awtLabel.Location = new System.Drawing.Point(206, 75);
            this.awtLabel.Name = "awtLabel";
            this.awtLabel.Size = new System.Drawing.Size(220, 18);
            this.awtLabel.TabIndex = 2;
            this.awtLabel.Text = "평균 대기 시간(Average Waiting Time)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Noto Sans CJK KR Bold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.label5.Location = new System.Drawing.Point(11, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "CPU 사용률";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "LOG";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.label2.Location = new System.Drawing.Point(13, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ghantt- Chart";
            // 
            // box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(695, 507);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ghattbox);
            this.Controls.Add(this.panel4);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "box";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "box";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.box_FormClosing);
            this.ghattbox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.chart.ResumeLayout(false);
            this.chart.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel ghattbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel Ghannt_base;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label attLabel;
        private System.Windows.Forms.Label awtText;
        private System.Windows.Forms.Label awtLabel;
        private System.Windows.Forms.Label attText;
        private System.Windows.Forms.Label usageText;
        private System.Windows.Forms.Label artText;
        private System.Windows.Forms.Label artLabel;
        private DoubleBufferedPanel chart;
        private System.Windows.Forms.Button button5;
    }
}