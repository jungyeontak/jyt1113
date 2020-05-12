namespace KaKaoRestApiCallSample
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRestApi = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.txtParam = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRestApi
            // 
            this.btnRestApi.Location = new System.Drawing.Point(179, 259);
            this.btnRestApi.Name = "btnRestApi";
            this.btnRestApi.Size = new System.Drawing.Size(96, 23);
            this.btnRestApi.TabIndex = 0;
            this.btnRestApi.Text = "CallRestApi";
            this.btnRestApi.UseVisualStyleBackColor = true;
            this.btnRestApi.Click += new System.EventHandler(this.btnRestApi_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(11, 87);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(33, 12);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "MSG";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(11, 115);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(39, 12);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "CODE";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(11, 143);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(52, 12);
            this.lblResult.TabIndex = 3;
            this.lblResult.Text = "RESULT";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(69, 12);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(349, 21);
            this.txtURL.TabIndex = 4;
            // 
            // txtParam
            // 
            this.txtParam.Location = new System.Drawing.Point(69, 39);
            this.txtParam.Name = "txtParam";
            this.txtParam.Size = new System.Drawing.Size(349, 21);
            this.txtParam.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "파라미터";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 294);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtParam);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnRestApi);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRestApi;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.TextBox txtParam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

