namespace RemoteServer
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
            this.btTest = new System.Windows.Forms.Button();
            this.btSpace = new System.Windows.Forms.Button();
            this.btServer = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btCapture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(12, 12);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 0;
            this.btTest.Text = "Test move";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btSpace
            // 
            this.btSpace.Location = new System.Drawing.Point(108, 11);
            this.btSpace.Name = "btSpace";
            this.btSpace.Size = new System.Drawing.Size(75, 23);
            this.btSpace.TabIndex = 1;
            this.btSpace.Text = "Space";
            this.btSpace.UseVisualStyleBackColor = true;
            this.btSpace.Click += new System.EventHandler(this.btSpace_Click);
            // 
            // btServer
            // 
            this.btServer.Location = new System.Drawing.Point(210, 12);
            this.btServer.Name = "btServer";
            this.btServer.Size = new System.Drawing.Size(75, 23);
            this.btServer.TabIndex = 2;
            this.btServer.Text = "Server";
            this.btServer.UseVisualStyleBackColor = true;
            this.btServer.Click += new System.EventHandler(this.btServer_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Move";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btCapture
            // 
            this.btCapture.Location = new System.Drawing.Point(119, 56);
            this.btCapture.Name = "btCapture";
            this.btCapture.Size = new System.Drawing.Size(75, 23);
            this.btCapture.TabIndex = 4;
            this.btCapture.Text = "Capture";
            this.btCapture.UseVisualStyleBackColor = true;
            this.btCapture.Click += new System.EventHandler(this.btCapture_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btCapture);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btServer);
            this.Controls.Add(this.btSpace);
            this.Controls.Add(this.btTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Button btSpace;
        private System.Windows.Forms.Button btServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btCapture;
    }
}

