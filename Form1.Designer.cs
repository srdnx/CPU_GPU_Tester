namespace CPU_GPU_Tester
{
    partial class Form1
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
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label1;
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tx1 = new System.Windows.Forms.TextBox();
            this.txn = new System.Windows.Forms.TextBox();
            this.txPar = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(39, 2);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(67, 17);
            label11.TabIndex = 28;
            label11.Text = "Символы";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(39, 100);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(144, 17);
            label12.TabIndex = 29;
            label12.Text = "Путь к изображению";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(39, 155);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(196, 17);
            label1.TabIndex = 31;
            label1.Text = "Потоков вызова CPU и GPU:";
            label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 47);
            this.button1.TabIndex = 4;
            this.button1.Text = "Пуск!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(484, 22);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(840, 477);
            this.textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(42, 120);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(402, 22);
            this.textBox2.TabIndex = 6;
            // 
            // tx1
            // 
            this.tx1.Location = new System.Drawing.Point(42, 206);
            this.tx1.Multiline = true;
            this.tx1.Name = "tx1";
            this.tx1.Size = new System.Drawing.Size(233, 289);
            this.tx1.TabIndex = 17;
            // 
            // txn
            // 
            this.txn.Location = new System.Drawing.Point(42, 22);
            this.txn.Multiline = true;
            this.txn.Name = "txn";
            this.txn.Size = new System.Drawing.Size(402, 72);
            this.txn.TabIndex = 27;
            // 
            // txPar
            // 
            this.txPar.Location = new System.Drawing.Point(42, 175);
            this.txPar.Name = "txPar";
            this.txPar.Size = new System.Drawing.Size(402, 22);
            this.txPar.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1336, 507);
            this.Controls.Add(label1);
            this.Controls.Add(this.txPar);
            this.Controls.Add(label12);
            this.Controls.Add(label11);
            this.Controls.Add(this.txn);
            this.Controls.Add(this.tx1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "CPU/GPU Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox tx1;
        private System.Windows.Forms.TextBox txn;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txPar;
    }
}

