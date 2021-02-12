namespace AILinkFactoryAuto.Task.Common
{
    partial class TipAndCheckUartForm
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
            this.txtTips = new System.Windows.Forms.TextBox();
            this.lblCountDown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTips
            // 
            this.txtTips.Font = new System.Drawing.Font("微软雅黑", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTips.Location = new System.Drawing.Point(33, 32);
            this.txtTips.Multiline = true;
            this.txtTips.Name = "txtTips";
            this.txtTips.ReadOnly = true;
            this.txtTips.Size = new System.Drawing.Size(556, 186);
            this.txtTips.TabIndex = 1;
            this.txtTips.Text = "请按音量+";
            // 
            // lblCountDown
            // 
            this.lblCountDown.AutoSize = true;
            this.lblCountDown.Font = new System.Drawing.Font("微软雅黑", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCountDown.ForeColor = System.Drawing.Color.Black;
            this.lblCountDown.Location = new System.Drawing.Point(32, 221);
            this.lblCountDown.Name = "lblCountDown";
            this.lblCountDown.Size = new System.Drawing.Size(333, 78);
            this.lblCountDown.TabIndex = 4;
            this.lblCountDown.Text = "倒计时开始";
            // 
            // TipAndCheckUartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 323);
            this.Controls.Add(this.lblCountDown);
            this.Controls.Add(this.txtTips);
            this.Name = "TipAndCheckUartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TipAndCheckUartForm";
            this.Shown += new System.EventHandler(this.TipAndCheckUartForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTips;
        private System.Windows.Forms.Label lblCountDown;
    }
}