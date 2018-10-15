namespace StockControl
{
    partial class WorkCenters_Cal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkCenters_Cal));
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnRecal = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnCapaView = new Telerik.WinControls.UI.RadButtonElement();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.radMenuItem2 = new Telerik.WinControls.UI.RadMenuItem();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbbWork = new Telerik.WinControls.UI.RadDropDownList();
            this.lbStatus = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dtTo = new Telerik.WinControls.UI.RadDateTimePicker();
            this.dtFrom = new Telerik.WinControls.UI.RadDateTimePicker();
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            this.radRibbonBarButtonGroup2 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            this.btnFilter = new Telerik.WinControls.UI.RadButtonElement();
            this.Unfilter = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarButtonGroup5 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            this.radRibbonBarGroup4 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnImport = new Telerik.WinControls.UI.RadButtonElement();
            this.btnExport = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarButtonGroup1 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbWork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
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
            this.radRibbonBar1.Size = new System.Drawing.Size(549, 160);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.StartMenuItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1,
            this.radMenuSeparatorItem1,
            this.radMenuItem2});
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "Work Center Calculate";
            this.radRibbonBar1.ThemeName = "Office2010Blue";
            this.radRibbonBar1.Click += new System.EventHandler(this.radRibbonBar1_Click);
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup2,
            this.radRibbonBarGroup1});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Action";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnRecal});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Data";
            // 
            // btnRecal
            // 
            this.btnRecal.Image = ((System.Drawing.Image)(resources.GetObject("btnRecal.Image")));
            this.btnRecal.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRecal.Name = "btnRecal";
            this.btnRecal.Text = "Calculate";
            this.btnRecal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRecal.Click += new System.EventHandler(this.btnRecal_Click);
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCapaView});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Page";
            // 
            // btnCapaView
            // 
            this.btnCapaView.Image = ((System.Drawing.Image)(resources.GetObject("btnCapaView.Image")));
            this.btnCapaView.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCapaView.Name = "btnCapaView";
            this.btnCapaView.Text = "<html><p>Cycle Time</p><p>Available</p></html>";
            this.btnCapaView.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCapaView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCapaView.Click += new System.EventHandler(this.btnCapaView_Click);
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
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 428);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(549, 26);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "Status : Work Center Calculate";
            this.radLabelElement1.TextWrap = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cbbWork);
            this.panel1.Controls.Add(this.lbStatus);
            this.panel1.Controls.Add(this.radLabel3);
            this.panel1.Controls.Add(this.radLabel2);
            this.panel1.Controls.Add(this.radLabel1);
            this.panel1.Controls.Add(this.dtTo);
            this.panel1.Controls.Add(this.dtFrom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 160);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(549, 268);
            this.panel1.TabIndex = 2;
            // 
            // cbbWork
            // 
            this.cbbWork.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cbbWork.Location = new System.Drawing.Point(169, 113);
            this.cbbWork.Name = "cbbWork";
            this.cbbWork.Size = new System.Drawing.Size(209, 20);
            this.cbbWork.TabIndex = 2;
            this.cbbWork.ThemeName = "Office2010Blue";
            // 
            // lbStatus
            // 
            this.lbStatus.Location = new System.Drawing.Point(77, 166);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(46, 18);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "lbStatus";
            this.lbStatus.Visible = false;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(77, 113);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(69, 18);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "Work Center";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(79, 74);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(67, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "Ending Date";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(79, 40);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(72, 18);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Starting Date";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd/MM/yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(169, 74);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(114, 20);
            this.dtTo.TabIndex = 0;
            this.dtTo.TabStop = false;
            this.dtTo.Text = "05/09/2018";
            this.dtTo.ThemeName = "Office2010Blue";
            this.dtTo.Value = new System.DateTime(2018, 9, 5, 10, 2, 31, 660);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd/MM/yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(169, 40);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(114, 20);
            this.dtFrom.TabIndex = 0;
            this.dtFrom.TabStop = false;
            this.dtFrom.Text = "05/09/2018";
            this.dtFrom.ThemeName = "Office2010Blue";
            this.dtFrom.Value = new System.DateTime(2018, 9, 5, 10, 2, 31, 660);
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
            // radRibbonBarGroup4
            // 
            this.radRibbonBarGroup4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.radRibbonBarGroup4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnImport,
            this.btnExport,
            this.radRibbonBarButtonGroup1});
            this.radRibbonBarGroup4.Margin = new System.Windows.Forms.Padding(0);
            this.radRibbonBarGroup4.MaxSize = new System.Drawing.Size(0, 0);
            this.radRibbonBarGroup4.MinSize = new System.Drawing.Size(0, 0);
            this.radRibbonBarGroup4.Name = "radRibbonBarGroup4";
            this.radRibbonBarGroup4.Text = "Import / Export";
            this.radRibbonBarGroup4.UseCompatibleTextRendering = false;
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnImport.Name = "btnImport";
            this.btnImport.SmallImage = null;
            this.btnImport.Text = "นำข้อมูลเข้า";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnImport.UseCompatibleTextRendering = false;
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExport.Name = "btnExport";
            this.btnExport.Text = "ส่งข้อมูลออก";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExport.UseCompatibleTextRendering = false;
            // 
            // radRibbonBarButtonGroup1
            // 
            this.radRibbonBarButtonGroup1.Name = "radRibbonBarButtonGroup1";
            this.radRibbonBarButtonGroup1.Padding = new System.Windows.Forms.Padding(1);
            this.radRibbonBarButtonGroup1.ShowBackColor = false;
            this.radRibbonBarButtonGroup1.ShowBorder = false;
            this.radRibbonBarButtonGroup1.Text = "radRibbonBarButtonGroup1";
            this.radRibbonBarButtonGroup1.UseCompatibleTextRendering = false;
            // 
            // WorkCenters_Cal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 454);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "WorkCenters_Cal";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work Center Calculate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkCenters_Cal_FormClosing);
            this.Load += new System.EventHandler(this.Unit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbWork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
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
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem2;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup2;
        private Telerik.WinControls.UI.RadButtonElement btnFilter;
        private Telerik.WinControls.UI.RadButtonElement Unfilter;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup5;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup4;
        private Telerik.WinControls.UI.RadButtonElement btnImport;
        private Telerik.WinControls.UI.RadButtonElement btnExport;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private Telerik.WinControls.UI.RadButtonElement btnRecal;
        private Telerik.WinControls.UI.RadDateTimePicker dtFrom;
        private Telerik.WinControls.UI.RadDateTimePicker dtTo;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadDropDownList cbbWork;
        private Telerik.WinControls.UI.RadLabel lbStatus;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private Telerik.WinControls.UI.RadButtonElement btnCapaView;
    }
}
