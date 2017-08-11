namespace Factory
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bodyCreatTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.motorCreatTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.accessoriesSupplyTimeText = new System.Windows.Forms.TextBox();
            this.motorSupplyTimeText = new System.Windows.Forms.TextBox();
            this.bodySupplyTimeText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.carInProgressText = new System.Windows.Forms.TextBox();
            this.totalCarsText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.currentMotorCountText = new System.Windows.Forms.TextBox();
            this.totalMotorCountText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.currentBodyCountText = new System.Windows.Forms.TextBox();
            this.totalBodyCountText = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.carsInStorageText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bodyCreatTimeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorCreatTimeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // bodyCreatTimeTrackBar
            // 
            this.bodyCreatTimeTrackBar.Location = new System.Drawing.Point(29, 44);
            this.bodyCreatTimeTrackBar.Minimum = 1;
            this.bodyCreatTimeTrackBar.Name = "bodyCreatTimeTrackBar";
            this.bodyCreatTimeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.bodyCreatTimeTrackBar.Size = new System.Drawing.Size(45, 176);
            this.bodyCreatTimeTrackBar.TabIndex = 0;
            this.bodyCreatTimeTrackBar.Value = 1;
            this.bodyCreatTimeTrackBar.ValueChanged += new System.EventHandler(this.bodyCreatTimeTrackBar_ValueChanged);
            // 
            // motorCreatTimeTrackBar
            // 
            this.motorCreatTimeTrackBar.Location = new System.Drawing.Point(122, 44);
            this.motorCreatTimeTrackBar.Minimum = 1;
            this.motorCreatTimeTrackBar.Name = "motorCreatTimeTrackBar";
            this.motorCreatTimeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.motorCreatTimeTrackBar.Size = new System.Drawing.Size(45, 176);
            this.motorCreatTimeTrackBar.TabIndex = 0;
            this.motorCreatTimeTrackBar.Value = 1;
            this.motorCreatTimeTrackBar.ValueChanged += new System.EventHandler(this.motorCreatTimeTrackBar_ValueChanged);
            // 
            // trackBar4
            // 
            this.trackBar4.Location = new System.Drawing.Point(216, 44);
            this.trackBar4.Minimum = 1;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar4.Size = new System.Drawing.Size(45, 176);
            this.trackBar4.TabIndex = 0;
            this.trackBar4.Value = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(76, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Поставщики:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(17, 102);
            this.label2.TabIndex = 1;
            this.label2.Text = "К\r\nу\r\nз\r\nо\r\nв\r\nа";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.accessoriesSupplyTimeText);
            this.panel2.Controls.Add(this.motorSupplyTimeText);
            this.panel2.Controls.Add(this.bodySupplyTimeText);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.trackBar4);
            this.panel2.Controls.Add(this.motorCreatTimeTrackBar);
            this.panel2.Controls.Add(this.bodyCreatTimeTrackBar);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 252);
            this.panel2.TabIndex = 0;
            // 
            // accessoriesSupplyTimeText
            // 
            this.accessoriesSupplyTimeText.Enabled = false;
            this.accessoriesSupplyTimeText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.accessoriesSupplyTimeText.Location = new System.Drawing.Point(216, 223);
            this.accessoriesSupplyTimeText.Name = "accessoriesSupplyTimeText";
            this.accessoriesSupplyTimeText.ReadOnly = true;
            this.accessoriesSupplyTimeText.Size = new System.Drawing.Size(40, 20);
            this.accessoriesSupplyTimeText.TabIndex = 2;
            this.accessoriesSupplyTimeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // motorSupplyTimeText
            // 
            this.motorSupplyTimeText.Enabled = false;
            this.motorSupplyTimeText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.motorSupplyTimeText.Location = new System.Drawing.Point(122, 224);
            this.motorSupplyTimeText.Name = "motorSupplyTimeText";
            this.motorSupplyTimeText.ReadOnly = true;
            this.motorSupplyTimeText.Size = new System.Drawing.Size(40, 20);
            this.motorSupplyTimeText.TabIndex = 2;
            this.motorSupplyTimeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bodySupplyTimeText
            // 
            this.bodySupplyTimeText.Enabled = false;
            this.bodySupplyTimeText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bodySupplyTimeText.Location = new System.Drawing.Point(29, 224);
            this.bodySupplyTimeText.Name = "bodySupplyTimeText";
            this.bodySupplyTimeText.ReadOnly = true;
            this.bodySupplyTimeText.Size = new System.Drawing.Size(40, 20);
            this.bodySupplyTimeText.TabIndex = 2;
            this.bodySupplyTimeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(193, 50);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(18, 170);
            this.label4.TabIndex = 1;
            this.label4.Text = "А\r\nк\r\nс\r\nе\r\nс\r\nс\r\nу\r\nа\r\nр\r\nы";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(86, 59);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(19, 153);
            this.label3.TabIndex = 1;
            this.label3.Text = "Д\r\nв\r\nи\r\nг\r\nа\r\nт\r\nе\r\nл\r\nи";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Машин в процессе создания:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 318);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Всего созданно машин:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(170, 399);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // carInProgressText
            // 
            this.carInProgressText.Enabled = false;
            this.carInProgressText.Location = new System.Drawing.Point(186, 290);
            this.carInProgressText.Name = "carInProgressText";
            this.carInProgressText.Size = new System.Drawing.Size(100, 20);
            this.carInProgressText.TabIndex = 3;
            this.carInProgressText.Text = "0";
            // 
            // totalCarsText
            // 
            this.totalCarsText.Enabled = false;
            this.totalCarsText.Location = new System.Drawing.Point(186, 316);
            this.totalCarsText.Name = "totalCarsText";
            this.totalCarsText.Size = new System.Drawing.Size(100, 20);
            this.totalCarsText.TabIndex = 3;
            this.totalCarsText.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.currentMotorCountText);
            this.panel1.Controls.Add(this.totalMotorCountText);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(609, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 4;
            // 
            // currentMotorCountText
            // 
            this.currentMotorCountText.Location = new System.Drawing.Point(101, 62);
            this.currentMotorCountText.Name = "currentMotorCountText";
            this.currentMotorCountText.Size = new System.Drawing.Size(84, 20);
            this.currentMotorCountText.TabIndex = 2;
            // 
            // totalMotorCountText
            // 
            this.totalMotorCountText.Location = new System.Drawing.Point(101, 36);
            this.totalMotorCountText.Name = "totalMotorCountText";
            this.totalMotorCountText.Size = new System.Drawing.Size(84, 20);
            this.totalMotorCountText.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(65, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Моторы";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Всего созданно:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "На складе:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.currentBodyCountText);
            this.panel3.Controls.Add(this.totalBodyCountText);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Location = new System.Drawing.Point(609, 128);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 100);
            this.panel3.TabIndex = 4;
            // 
            // currentBodyCountText
            // 
            this.currentBodyCountText.Location = new System.Drawing.Point(101, 62);
            this.currentBodyCountText.Name = "currentBodyCountText";
            this.currentBodyCountText.Size = new System.Drawing.Size(84, 20);
            this.currentBodyCountText.TabIndex = 2;
            // 
            // totalBodyCountText
            // 
            this.totalBodyCountText.Location = new System.Drawing.Point(101, 36);
            this.totalBodyCountText.Name = "totalBodyCountText";
            this.totalBodyCountText.Size = new System.Drawing.Size(84, 20);
            this.totalBodyCountText.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(65, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 17);
            this.label10.TabIndex = 1;
            this.label10.Text = "Кузова";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 39);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Всего созданно:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(31, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "На складе:";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox3);
            this.panel4.Controls.Add(this.textBox4);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Location = new System.Drawing.Point(609, 236);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 100);
            this.panel4.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(101, 62);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(84, 20);
            this.textBox3.TabIndex = 2;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(101, 36);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(84, 20);
            this.textBox4.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(65, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "Аксессуары";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 39);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Всего созданно:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(31, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "На складе:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(23, 342);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(99, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Машин на складе:";
            // 
            // carsInStorageText
            // 
            this.carsInStorageText.Enabled = false;
            this.carsInStorageText.Location = new System.Drawing.Point(186, 342);
            this.carsInStorageText.Name = "carsInStorageText";
            this.carsInStorageText.Size = new System.Drawing.Size(100, 20);
            this.carsInStorageText.TabIndex = 3;
            this.carsInStorageText.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 471);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.totalCarsText);
            this.Controls.Add(this.carsInStorageText);
            this.Controls.Add(this.carInProgressText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "F.T.G.O.S. Factory";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.bodyCreatTimeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorCreatTimeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar bodyCreatTimeTrackBar;
        private System.Windows.Forms.TrackBar motorCreatTimeTrackBar;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox carInProgressText;
        private System.Windows.Forms.TextBox totalCarsText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox currentMotorCountText;
        private System.Windows.Forms.TextBox totalMotorCountText;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox currentBodyCountText;
        private System.Windows.Forms.TextBox totalBodyCountText;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox bodySupplyTimeText;
        private System.Windows.Forms.TextBox accessoriesSupplyTimeText;
        private System.Windows.Forms.TextBox motorSupplyTimeText;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox carsInStorageText;
    }
}

