namespace AILinkFactoryAuto.GenJts.SmartBraceletJts
{
    partial class CreatJtsForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.cbSkg = new System.Windows.Forms.CheckBox();
            this.cbSm005 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 258);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "生成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbSkg
            // 
            this.cbSkg.AutoSize = true;
            this.cbSkg.Location = new System.Drawing.Point(29, 41);
            this.cbSkg.Name = "cbSkg";
            this.cbSkg.Size = new System.Drawing.Size(53, 19);
            this.cbSkg.TabIndex = 1;
            this.cbSkg.Text = "SKG";
            this.cbSkg.UseVisualStyleBackColor = true;
            // 
            // cbSm005
            // 
            this.cbSm005.AutoSize = true;
            this.cbSm005.Location = new System.Drawing.Point(29, 88);
            this.cbSm005.Name = "cbSm005";
            this.cbSm005.Size = new System.Drawing.Size(69, 19);
            this.cbSm005.TabIndex = 2;
            this.cbSm005.Text = "SM005";
            this.cbSm005.UseVisualStyleBackColor = true;
            // 
            // CreatJtsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 326);
            this.Controls.Add(this.cbSm005);
            this.Controls.Add(this.cbSkg);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CreatJtsForm";
            this.Text = "CreatJtsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbSkg;
        private System.Windows.Forms.CheckBox cbSm005;
    }
}