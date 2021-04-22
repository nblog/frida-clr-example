
namespace frida_clr_example
{
    partial class FrmFrida
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
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.lstProcesses = new System.Windows.Forms.ListView();
            this.txtScript = new System.Windows.Forms.TextBox();
            this.labInfo = new System.Windows.Forms.Label();
            this.btnInject = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(402, 627);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ReadOnly = true;
            this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDebug.Size = new System.Drawing.Size(592, 82);
            this.txtDebug.TabIndex = 8;
            // 
            // lstProcesses
            // 
            this.lstProcesses.FullRowSelect = true;
            this.lstProcesses.GridLines = true;
            this.lstProcesses.HideSelection = false;
            this.lstProcesses.Location = new System.Drawing.Point(12, 12);
            this.lstProcesses.Name = "lstProcesses";
            this.lstProcesses.Size = new System.Drawing.Size(381, 697);
            this.lstProcesses.TabIndex = 7;
            this.lstProcesses.UseCompatibleStateImageBehavior = false;
            this.lstProcesses.View = System.Windows.Forms.View.Details;
            // 
            // txtScript
            // 
            this.txtScript.Enabled = false;
            this.txtScript.Location = new System.Drawing.Point(402, 95);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtScript.Size = new System.Drawing.Size(592, 526);
            this.txtScript.TabIndex = 6;
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.Location = new System.Drawing.Point(408, 48);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(47, 15);
            this.labInfo.TabIndex = 10;
            this.labInfo.Text = "Name:";
            // 
            // btnInject
            // 
            this.btnInject.Enabled = false;
            this.btnInject.Location = new System.Drawing.Point(910, 35);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(84, 41);
            this.btnInject.TabIndex = 9;
            this.btnInject.Text = "Test";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // FrmFrida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.lstProcesses);
            this.Controls.Add(this.txtScript);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmFrida";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmFrida_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.ListView lstProcesses;
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.Label labInfo;
        private System.Windows.Forms.Button btnInject;
    }
}