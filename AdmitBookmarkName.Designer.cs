namespace GIS_Programming
{
    partial class AdmitBookmarkName
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
            this.tbBookmarkName = new System.Windows.Forms.TextBox();
            this.btnName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbBookmarkName
            // 
            this.tbBookmarkName.Location = new System.Drawing.Point(45, 63);
            this.tbBookmarkName.Name = "tbBookmarkName";
            this.tbBookmarkName.Size = new System.Drawing.Size(176, 25);
            this.tbBookmarkName.TabIndex = 0;
            // 
            // btnName
            // 
            this.btnName.Location = new System.Drawing.Point(252, 63);
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(75, 25);
            this.btnName.TabIndex = 1;
            this.btnName.Text = "确认";
            this.btnName.UseVisualStyleBackColor = true;
            this.btnName.Click += new System.EventHandler(this.btnName_Click);
            // 
            // AdmitBookmarkName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 153);
            this.Controls.Add(this.btnName);
            this.Controls.Add(this.tbBookmarkName);
            this.Name = "AdmitBookmarkName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "书签名称设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBookmarkName;
        private System.Windows.Forms.Button btnName;
    }
}