using System.Windows.Forms;
using System;

namespace ClientForm
{
    partial class ClientForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        /*private void Form_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; // 开启键盘事件监听
            this.KeyPress += new KeyPressEventHandler(Form_KeyPress); // 订阅键盘输入事件
        }

        private void Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 检查按键是否为 Enter 键
            if (e.KeyChar == (char)13)
            {
                // 触发按钮的 Click 事件
                this.btnSendMsg.PerformClick();
              
            }
        }*/
        private void Form1_Load()
        {
            this.AcceptButton = btnSendMsg; // 将btnSendMsg设为默认按钮
        }


        private void InitializeComponent()
        {   
            this.btnShock = new System.Windows.Forms.Button();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnShock
            // 
            this.btnShock.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShock.Location = new System.Drawing.Point(526, 511);
            this.btnShock.Name = "btnShock";
            this.btnShock.Size = new System.Drawing.Size(111, 27);
            this.btnShock.TabIndex = 13;
            this.btnShock.Text = "戳一戳";
            this.btnShock.UseVisualStyleBackColor = true;
            this.btnShock.Click += new System.EventHandler(this.btnShock_Click);
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendMsg.Location = new System.Drawing.Point(392, 512);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(112, 27);
            this.btnSendMsg.TabIndex = 12;
            this.btnSendMsg.Text = "发送消息";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMsg.Location = new System.Drawing.Point(23, 512);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(343, 28);
            this.txtMsg.TabIndex = 11;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(23, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(614, 479);
            this.txtLog.TabIndex = 10;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 551);
            this.Controls.Add(this.btnShock);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ClientForm";
            this.Text = "聊天室-客户端";

            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            /*//
            //btnChose
            //
            this.btnChose.Location = new System.Drawing.Point(425, 255);
            this.btnChose.Name = "btnChose";
            this.btnChose.Size = new System.Drawing.Size(96, 23);
            this.btnChose.TabIndex = 8;
            this.btnChose.Text = "选择文件路径";
            this.btnChose.UseVisualStyleBackColor = true;
            this.btnChose.Click += new System.EventHandler(this.btnChose_Click);
            //
            //btnSendFile
            //
            this.btnSendFile.Location = new System.Drawing.Point(541, 255);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(124, 23);
            this.btnSendFile.TabIndex = 9;
            this.btnSendFile.Text = "向客户端发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            //
            //txtFilePath
            //
            this.txtFilePath.Location = new System.Drawing.Point(20, 257);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(399, 21);
            this.txtFilePath.TabIndex = 6;*/
        }

        #endregion

        private System.Windows.Forms.Button btnShock;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtLog;
    }
}

