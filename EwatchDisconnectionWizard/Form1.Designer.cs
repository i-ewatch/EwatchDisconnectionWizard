
namespace EwatchDisconnectionWizard
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MySqlTimelabel = new System.Windows.Forms.Label();
            this.MsSqlTimelabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 20F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "MySqL系統最後輪巡時間 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 20F);
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(318, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "MsSqL系統最後輪巡時間 :";
            // 
            // MySqlTimelabel
            // 
            this.MySqlTimelabel.AutoSize = true;
            this.MySqlTimelabel.Font = new System.Drawing.Font("新細明體", 20F);
            this.MySqlTimelabel.Location = new System.Drawing.Point(339, 9);
            this.MySqlTimelabel.Name = "MySqlTimelabel";
            this.MySqlTimelabel.Size = new System.Drawing.Size(264, 27);
            this.MySqlTimelabel.TabIndex = 2;
            this.MySqlTimelabel.Text = "1999/1/1 上午 00:00:00";
            // 
            // MsSqlTimelabel
            // 
            this.MsSqlTimelabel.AutoSize = true;
            this.MsSqlTimelabel.Font = new System.Drawing.Font("新細明體", 20F);
            this.MsSqlTimelabel.Location = new System.Drawing.Point(339, 62);
            this.MsSqlTimelabel.Name = "MsSqlTimelabel";
            this.MsSqlTimelabel.Size = new System.Drawing.Size(264, 27);
            this.MsSqlTimelabel.TabIndex = 3;
            this.MsSqlTimelabel.Text = "1999/1/1 上午 00:00:00";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 106);
            this.Controls.Add(this.MsSqlTimelabel);
            this.Controls.Add(this.MySqlTimelabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "平台斷線精靈";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label MySqlTimelabel;
        private System.Windows.Forms.Label MsSqlTimelabel;
        private System.Windows.Forms.Timer timer1;
    }
}

