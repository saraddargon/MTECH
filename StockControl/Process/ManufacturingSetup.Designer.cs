namespace StockControl
{
    partial class ManufacturingSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManufacturingSetup));
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnSave = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarGroup3 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnRefresh = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarGroup5 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.radRibbonBarButtonGroup3 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            this.btnFilter1 = new Telerik.WinControls.UI.RadButtonElement();
            this.btnUnfilter1 = new Telerik.WinControls.UI.RadButtonElement();
            this.ribbonBarGroupSeparator1 = new Telerik.WinControls.UI.RibbonBarGroupSeparator();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.radMenuItem2 = new Telerik.WinControls.UI.RadMenuItem();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbbShowCapa = new Telerik.WinControls.UI.RadDropDownList();
            this.txtEndingTime = new Telerik.WinControls.UI.RadTimePicker();
            this.txtStartingTime = new Telerik.WinControls.UI.RadTimePicker();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            this.radRibbonBarButtonGroup2 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            this.btnFilter = new Telerik.WinControls.UI.RadButtonElement();
            this.Unfilter = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarButtonGroup5 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbShowCapa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.radRibbonBar1.ExitButton.Text = "Exit";
            this.radRibbonBar1.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radRibbonBar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.OptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radRibbonBar1.OptionsButton.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // 
            // 
            this.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBar1.Size = new System.Drawing.Size(636, 160);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.StartMenuItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1,
            this.radMenuSeparatorItem1,
            this.radMenuItem2});
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "Shift (กะทำงาน)";
            this.radRibbonBar1.ThemeName = "Office2010Blue";
            this.radRibbonBar1.Click += new System.EventHandler(this.radRibbonBar1_Click);
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1,
            this.radRibbonBarGroup3,
            this.radRibbonBarGroup5});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Action";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnSave});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "New List";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "บันทึกรายการ";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnRefresh});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = "Page";
            // 
            // btnRefresh
            // 
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(197)))));
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Text = "รีเฟรช";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRefresh.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // radRibbonBarGroup5
            // 
            this.radRibbonBarGroup5.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBarGroup5.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
            this.radRibbonBarGroup5.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarButtonGroup3});
            this.radRibbonBarGroup5.Name = "radRibbonBarGroup5";
            this.radRibbonBarGroup5.Text = "Filter";
            this.radRibbonBarGroup5.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radRibbonBarButtonGroup3
            // 
            this.radRibbonBarButtonGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnFilter1,
            this.btnUnfilter1,
            this.ribbonBarGroupSeparator1});
            this.radRibbonBarButtonGroup3.Name = "radRibbonBarButtonGroup3";
            this.radRibbonBarButtonGroup3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.radRibbonBarButtonGroup3.Padding = new System.Windows.Forms.Padding(1);
            this.radRibbonBarButtonGroup3.ShowBackColor = false;
            this.radRibbonBarButtonGroup3.ShowBorder = false;
            this.radRibbonBarButtonGroup3.Text = "radRibbonBarButtonGroup3";
            // 
            // btnFilter1
            // 
            this.btnFilter1.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter1.Image")));
            this.btnFilter1.Name = "btnFilter1";
            this.btnFilter1.ShowBorder = false;
            this.btnFilter1.Text = "กรอง";
            this.btnFilter1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFilter1.Click += new System.EventHandler(this.btnFilter1_Click);
            // 
            // btnUnfilter1
            // 
            this.btnUnfilter1.Image = ((System.Drawing.Image)(resources.GetObject("btnUnfilter1.Image")));
            this.btnUnfilter1.Name = "btnUnfilter1";
            this.btnUnfilter1.ShowBorder = false;
            this.btnUnfilter1.Text = "ยกเลิก";
            this.btnUnfilter1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnfilter1.Click += new System.EventHandler(this.btnUnfilter1_Click);
            // 
            // ribbonBarGroupSeparator1
            // 
            this.ribbonBarGroupSeparator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ribbonBarGroupSeparator1.Name = "ribbonBarGroupSeparator1";
            this.ribbonBarGroupSeparator1.Text = "ribbonBarGroupSeparator1";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("radMenuItem1.Image")));
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "Exit";
            this.radMenuItem1.Click += new System.EventHandler(this.radMenuItem1_Click);
            // 
            // radMenuSeparatorItem1
            // 
            this.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radMenuItem2
            // 
            this.radMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("radMenuItem2.Image")));
            this.radMenuItem2.Name = "radMenuItem2";
            this.radMenuItem2.Text = "History View";
            this.radMenuItem2.Click += new System.EventHandler(this.radMenuItem2_Click);
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radLabelElement1});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 491);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(636, 26);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "Status : รายละเอียดกะทำงาน";
            this.radLabelElement1.TextWrap = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cbbShowCapa);
            this.panel1.Controls.Add(this.txtEndingTime);
            this.panel1.Controls.Add(this.txtStartingTime);
            this.panel1.Controls.Add(this.radLabel3);
            this.panel1.Controls.Add(this.radLabel2);
            this.panel1.Controls.Add(this.radLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 160);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 331);
            this.panel1.TabIndex = 2;
            // 
            // cbbShowCapa
            // 
            this.cbbShowCapa.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.cbbShowCapa.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cbbShowCapa.Location = new System.Drawing.Point(171, 129);
            this.cbbShowCapa.Name = "cbbShowCapa";
            this.cbbShowCapa.Size = new System.Drawing.Size(100, 20);
            this.cbbShowCapa.TabIndex = 2;
            this.cbbShowCapa.ThemeName = "Office2010Blue";
            // 
            // txtEndingTime
            // 
            this.txtEndingTime.Culture = new System.Globalization.CultureInfo("en-GB");
            this.txtEndingTime.Location = new System.Drawing.Point(171, 88);
            this.txtEndingTime.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 0);
            this.txtEndingTime.MinValue = new System.DateTime(((long)(0)));
            this.txtEndingTime.Name = "txtEndingTime";
            this.txtEndingTime.Size = new System.Drawing.Size(100, 20);
            this.txtEndingTime.TabIndex = 1;
            this.txtEndingTime.TabStop = false;
            this.txtEndingTime.Value = new System.DateTime(2018, 9, 4, 22, 38, 20, 655);
            // 
            // txtStartingTime
            // 
            this.txtStartingTime.Culture = new System.Globalization.CultureInfo("en-GB");
            this.txtStartingTime.Location = new System.Drawing.Point(171, 48);
            this.txtStartingTime.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 0);
            this.txtStartingTime.MinValue = new System.DateTime(((long)(0)));
            this.txtStartingTime.Name = "txtStartingTime";
            this.txtStartingTime.Size = new System.Drawing.Size(100, 20);
            this.txtStartingTime.TabIndex = 1;
            this.txtStartingTime.TabStop = false;
            this.txtStartingTime.Value = new System.DateTime(2018, 9, 4, 22, 38, 20, 655);
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(51, 129);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(109, 18);
            this.radLabel3.TabIndex = 0;
            this.radLabel3.Text = "Show Capacity UOM";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(92, 89);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(68, 18);
            this.radLabel2.TabIndex = 0;
            this.radLabel2.Text = "Ending Time";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(92, 49);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(73, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Starting Time";
            // 
            // radRibbonBarButtonGroup2
            // 
            this.radRibbonBarButtonGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnFilter,
            this.Unfilter});
            this.radRibbonBarButtonGroup2.Name = "radRibbonBarButtonGroup2";
            this.radRibbonBarButtonGroup2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.radRibbonBarButtonGroup2.Padding = new System.Windows.Forms.Padding(1);
            this.radRibbonBarButtonGroup2.ShowBackColor = false;
            this.radRibbonBarButtonGroup2.Text = "radRibbonBarButtonGroup2";
            // 
            // btnFilter
            // 
            this.btnFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter.Image")));
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Text = "Filter";
            this.btnFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // Unfilter
            // 
            this.Unfilter.Image = ((System.Drawing.Image)(resources.GetObject("Unfilter.Image")));
            this.Unfilter.Name = "Unfilter";
            this.Unfilter.Text = "Un Filter";
            this.Unfilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // radRibbonBarButtonGroup5
            // 
            this.radRibbonBarButtonGroup5.Name = "radRibbonBarButtonGroup5";
            this.radRibbonBarButtonGroup5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.radRibbonBarButtonGroup5.Padding = new System.Windows.Forms.Padding(1);
            this.radRibbonBarButtonGroup5.ShowBackColor = false;
            this.radRibbonBarButtonGroup5.ShowBorder = false;
            this.radRibbonBarButtonGroup5.Text = "radRibbonBarButtonGroup4";
            // 
            // ManufacturingSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ManufacturingSetup";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shift (กะทำงาน)";
            this.Load += new System.EventHandler(this.Unit_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Unit_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbShowCapa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup3;
        private Telerik.WinControls.UI.RadButtonElement btnRefresh;
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        private Telerik.WinControls.UI.RadButtonElement btnSave;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem2;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup5;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup3;
        private Telerik.WinControls.UI.RadButtonElement btnFilter1;
        private Telerik.WinControls.UI.RadButtonElement btnUnfilter1;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup2;
        private Telerik.WinControls.UI.RadButtonElement btnFilter;
        private Telerik.WinControls.UI.RadButtonElement Unfilter;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup5;
        private Telerik.WinControls.UI.RibbonBarGroupSeparator ribbonBarGroupSeparator1;
        private Telerik.WinControls.UI.RadDropDownList cbbShowCapa;
        private Telerik.WinControls.UI.RadTimePicker txtEndingTime;
        private Telerik.WinControls.UI.RadTimePicker txtStartingTime;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
