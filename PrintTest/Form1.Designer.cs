namespace PrintTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            dummyPanel = new Panel();
            printPreviewControl1 = new PrintPreviewControl();
            button2 = new Button();
            textBox1 = new TextBox();
            btn_TestPrint = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(129, 23);
            button1.TabIndex = 0;
            button1.Text = "Preview Dialog";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // dummyPanel
            // 
            dummyPanel.Location = new Point(12, 412);
            dummyPanel.Name = "dummyPanel";
            dummyPanel.Size = new Size(17, 26);
            dummyPanel.TabIndex = 1;
            // 
            // printPreviewControl1
            // 
            printPreviewControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            printPreviewControl1.Location = new Point(239, 2);
            printPreviewControl1.Name = "printPreviewControl1";
            printPreviewControl1.Size = new Size(558, 451);
            printPreviewControl1.TabIndex = 2;
            // 
            // button2
            // 
            button2.Location = new Point(12, 41);
            button2.Name = "button2";
            button2.Size = new Size(129, 23);
            button2.TabIndex = 0;
            button2.Text = "Preview Control";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(147, 41);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(86, 23);
            textBox1.TabIndex = 3;
            textBox1.Text = "15";
            // 
            // btn_TestPrint
            // 
            btn_TestPrint.Location = new Point(12, 83);
            btn_TestPrint.Name = "btn_TestPrint";
            btn_TestPrint.Size = new Size(129, 23);
            btn_TestPrint.TabIndex = 0;
            btn_TestPrint.Text = "Test Print";
            btn_TestPrint.UseVisualStyleBackColor = true;
            btn_TestPrint.Click += btn_TestPrint_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 126);
            button3.Name = "button3";
            button3.Size = new Size(129, 23);
            button3.TabIndex = 4;
            button3.Text = "Text Rotation";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(textBox1);
            Controls.Add(printPreviewControl1);
            Controls.Add(dummyPanel);
            Controls.Add(btn_TestPrint);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Button button1;
        private Panel dummyPanel;
        private PrintPreviewControl printPreviewControl1;
        private Button button2;
        private TextBox textBox1;
        private Button btn_TestPrint;
        private Button button3;
    }
}