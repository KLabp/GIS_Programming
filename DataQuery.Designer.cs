namespace GIS_Programming
{
    partial class DataQuery
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
            this.label1 = new System.Windows.Forms.Label();
            this.clbFields = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSLayerList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSFRelation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSRelationList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbFRelation = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ckbAll = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbLayer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "空间查询：";
            // 
            // clbFields
            // 
            this.clbFields.CheckOnClick = true;
            this.clbFields.FormattingEnabled = true;
            this.clbFields.Location = new System.Drawing.Point(12, 91);
            this.clbFields.Name = "clbFields";
            this.clbFields.Size = new System.Drawing.Size(358, 84);
            this.clbFields.TabIndex = 1;
            this.clbFields.MouseClick += new System.Windows.Forms.MouseEventHandler(this.clbFields_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "查询字段:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "图层";
            // 
            // cbSLayerList
            // 
            this.cbSLayerList.FormattingEnabled = true;
            this.cbSLayerList.Location = new System.Drawing.Point(103, 245);
            this.cbSLayerList.Name = "cbSLayerList";
            this.cbSLayerList.Size = new System.Drawing.Size(267, 23);
            this.cbSLayerList.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "属性关系";
            // 
            // tbSFRelation
            // 
            this.tbSFRelation.Location = new System.Drawing.Point(103, 280);
            this.tbSFRelation.Name = "tbSFRelation";
            this.tbSFRelation.Size = new System.Drawing.Size(267, 25);
            this.tbSFRelation.TabIndex = 6;
            this.tbSFRelation.Text = "(SQL)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 321);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "空间关系";
            // 
            // cbSRelationList
            // 
            this.cbSRelationList.FormattingEnabled = true;
            this.cbSRelationList.Items.AddRange(new object[] {
            "Intersects",
            "Envelope_Intersects",
            "Index_Intersects",
            "Toucher",
            "Overlaps",
            "Crosses",
            "Within",
            "Contains",
            "Relation"});
            this.cbSRelationList.Location = new System.Drawing.Point(103, 318);
            this.cbSRelationList.Name = "cbSRelationList";
            this.cbSRelationList.Size = new System.Drawing.Size(267, 23);
            this.cbSRelationList.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 368);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "属性查询：";
            // 
            // tbFRelation
            // 
            this.tbFRelation.Location = new System.Drawing.Point(103, 398);
            this.tbFRelation.Name = "tbFRelation";
            this.tbFRelation.Size = new System.Drawing.Size(267, 25);
            this.tbFRelation.TabIndex = 11;
            this.tbFRelation.Text = "(SQL)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 404);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "属性关系";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(188, 452);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(286, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckbAll
            // 
            this.ckbAll.AutoSize = true;
            this.ckbAll.Location = new System.Drawing.Point(15, 181);
            this.ckbAll.Name = "ckbAll";
            this.ckbAll.Size = new System.Drawing.Size(59, 19);
            this.ckbAll.TabIndex = 14;
            this.ckbAll.Text = "全选";
            this.ckbAll.UseVisualStyleBackColor = true;
            this.ckbAll.CheckStateChanged += new System.EventHandler(this.ckbAll_CheckStateChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "查询图层:";
            // 
            // cbLayer
            // 
            this.cbLayer.FormattingEnabled = true;
            this.cbLayer.Location = new System.Drawing.Point(98, 17);
            this.cbLayer.Name = "cbLayer";
            this.cbLayer.Size = new System.Drawing.Size(272, 23);
            this.cbLayer.TabIndex = 16;
            this.cbLayer.SelectedIndexChanged += new System.EventHandler(this.cbLayer_SelectedIndexChanged);
            // 
            // DataQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 503);
            this.Controls.Add(this.cbLayer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ckbAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbFRelation);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbSRelationList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSFRelation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSLayerList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clbFields);
            this.Controls.Add(this.label1);
            this.Name = "DataQuery";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataQuery_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbFields;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSLayerList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSFRelation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbSRelationList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbFRelation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckbAll;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbLayer;
    }
}