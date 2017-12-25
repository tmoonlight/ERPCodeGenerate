namespace CY_System.CodeBuilder
{
    partial class SelectForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForms));
            this.lvNotSelectList = new System.Windows.Forms.ListView();
            this.lvSelectList = new System.Windows.Forms.ListView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDataBase = new System.Windows.Forms.ComboBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAngle = new System.Windows.Forms.Button();
            this.btnAllCancel = new System.Windows.Forms.Button();
            this.btnAngleCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkListType = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // lvNotSelectList
            // 
            this.lvNotSelectList.Location = new System.Drawing.Point(11, 78);
            this.lvNotSelectList.Margin = new System.Windows.Forms.Padding(2);
            this.lvNotSelectList.Name = "lvNotSelectList";
            this.lvNotSelectList.Size = new System.Drawing.Size(197, 394);
            this.lvNotSelectList.TabIndex = 0;
            this.lvNotSelectList.UseCompatibleStateImageBehavior = false;
            this.lvNotSelectList.View = System.Windows.Forms.View.Tile;
            // 
            // lvSelectList
            // 
            this.lvSelectList.Location = new System.Drawing.Point(272, 78);
            this.lvSelectList.Margin = new System.Windows.Forms.Padding(2);
            this.lvSelectList.Name = "lvSelectList";
            this.lvSelectList.Size = new System.Drawing.Size(199, 351);
            this.lvSelectList.TabIndex = 0;
            this.lvSelectList.UseCompatibleStateImageBehavior = false;
            this.lvSelectList.View = System.Windows.Forms.View.Tile;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOk.Location = new System.Drawing.Point(275, 444);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 28);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确   定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(380, 444);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关   闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "数据库：";
            // 
            // cboDataBase
            // 
            this.cboDataBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataBase.FormattingEnabled = true;
            this.cboDataBase.Location = new System.Drawing.Point(77, 11);
            this.cboDataBase.Margin = new System.Windows.Forms.Padding(2);
            this.cboDataBase.Name = "cboDataBase";
            this.cboDataBase.Size = new System.Drawing.Size(146, 20);
            this.cboDataBase.TabIndex = 3;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(212, 100);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(56, 27);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = ">>>";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectAngle
            // 
            this.btnSelectAngle.Location = new System.Drawing.Point(212, 132);
            this.btnSelectAngle.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAngle.Name = "btnSelectAngle";
            this.btnSelectAngle.Size = new System.Drawing.Size(56, 27);
            this.btnSelectAngle.TabIndex = 4;
            this.btnSelectAngle.Text = ">";
            this.btnSelectAngle.UseVisualStyleBackColor = true;
            this.btnSelectAngle.Click += new System.EventHandler(this.btnSelectAngle_Click);
            // 
            // btnAllCancel
            // 
            this.btnAllCancel.Location = new System.Drawing.Point(212, 218);
            this.btnAllCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnAllCancel.Name = "btnAllCancel";
            this.btnAllCancel.Size = new System.Drawing.Size(56, 27);
            this.btnAllCancel.TabIndex = 4;
            this.btnAllCancel.Text = "<<<";
            this.btnAllCancel.UseVisualStyleBackColor = true;
            this.btnAllCancel.Click += new System.EventHandler(this.btnAllCancel_Click);
            // 
            // btnAngleCancel
            // 
            this.btnAngleCancel.Location = new System.Drawing.Point(212, 250);
            this.btnAngleCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnAngleCancel.Name = "btnAngleCancel";
            this.btnAngleCancel.Size = new System.Drawing.Size(56, 27);
            this.btnAngleCancel.TabIndex = 4;
            this.btnAngleCancel.Text = "<";
            this.btnAngleCancel.UseVisualStyleBackColor = true;
            this.btnAngleCancel.Click += new System.EventHandler(this.btnAngleCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(37, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "未选的表格类表：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(284, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "已选中的表对象：";
            // 
            // chkListType
            // 
            this.chkListType.BackColor = System.Drawing.SystemColors.Control;
            this.chkListType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListType.CheckOnClick = true;
            this.chkListType.ColumnWidth = 75;
            this.chkListType.FormattingEnabled = true;
            this.chkListType.Items.AddRange(new object[] {
            "数据表",
            "视图",
            "存储过程"});
            this.chkListType.Location = new System.Drawing.Point(239, 14);
            this.chkListType.MultiColumn = true;
            this.chkListType.Name = "chkListType";
            this.chkListType.Size = new System.Drawing.Size(231, 16);
            this.chkListType.TabIndex = 6;
            this.chkListType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListType_ItemCheck);
            // 
            // SelectForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(482, 483);
            this.Controls.Add(this.chkListType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAngleCancel);
            this.Controls.Add(this.btnAllCancel);
            this.Controls.Add(this.btnSelectAngle);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.cboDataBase);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lvSelectList);
            this.Controls.Add(this.lvNotSelectList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectForms";
            this.Text = "表格选择：";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvNotSelectList;
        private System.Windows.Forms.ListView lvSelectList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDataBase;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectAngle;
        private System.Windows.Forms.Button btnAllCancel;
        private System.Windows.Forms.Button btnAngleCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chkListType;
    }
}