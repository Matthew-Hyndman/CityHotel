namespace CityHotel
{
    partial class AllBookingsReportForm
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
            this.labelBookingID = new System.Windows.Forms.Label();
            this.comboBoxBooking = new System.Windows.Forms.ComboBox();
            this.buttonBooking = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMenuItems = new System.Windows.Forms.TabPage();
            this.crystalReportViewerMenuItems = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.tabTables = new System.Windows.Forms.TabPage();
            this.crystalReportViewerTables = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.tabControl1.SuspendLayout();
            this.tabMenuItems.SuspendLayout();
            this.tabTables.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelBookingID
            // 
            this.labelBookingID.AutoSize = true;
            this.labelBookingID.Font = new System.Drawing.Font("Century Gothic", 10.2F);
            this.labelBookingID.Location = new System.Drawing.Point(5, 72);
            this.labelBookingID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBookingID.Name = "labelBookingID";
            this.labelBookingID.Size = new System.Drawing.Size(98, 21);
            this.labelBookingID.TabIndex = 1;
            this.labelBookingID.Text = "BookingID:";
            // 
            // comboBoxBooking
            // 
            this.comboBoxBooking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBooking.Font = new System.Drawing.Font("Century Gothic", 10.2F);
            this.comboBoxBooking.ForeColor = System.Drawing.Color.Purple;
            this.comboBoxBooking.FormattingEnabled = true;
            this.comboBoxBooking.Location = new System.Drawing.Point(10, 105);
            this.comboBoxBooking.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxBooking.Name = "comboBoxBooking";
            this.comboBoxBooking.Size = new System.Drawing.Size(150, 29);
            this.comboBoxBooking.TabIndex = 2;
            // 
            // buttonBooking
            // 
            this.buttonBooking.Location = new System.Drawing.Point(13, 151);
            this.buttonBooking.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBooking.Name = "buttonBooking";
            this.buttonBooking.Size = new System.Drawing.Size(105, 54);
            this.buttonBooking.TabIndex = 3;
            this.buttonBooking.Text = "Display Reprot";
            this.buttonBooking.UseVisualStyleBackColor = true;
            this.buttonBooking.Click += new System.EventHandler(this.buttonBooking_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabMenuItems);
            this.tabControl1.Controls.Add(this.tabTables);
            this.tabControl1.Location = new System.Drawing.Point(178, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(989, 610);
            this.tabControl1.TabIndex = 4;
            // 
            // tabMenuItems
            // 
            this.tabMenuItems.Controls.Add(this.crystalReportViewerMenuItems);
            this.tabMenuItems.Location = new System.Drawing.Point(4, 30);
            this.tabMenuItems.Name = "tabMenuItems";
            this.tabMenuItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabMenuItems.Size = new System.Drawing.Size(981, 576);
            this.tabMenuItems.TabIndex = 0;
            this.tabMenuItems.Text = "Menu Items";
            this.tabMenuItems.UseVisualStyleBackColor = true;
            // 
            // crystalReportViewerMenuItems
            // 
            this.crystalReportViewerMenuItems.ActiveViewIndex = -1;
            this.crystalReportViewerMenuItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewerMenuItems.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewerMenuItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewerMenuItems.Location = new System.Drawing.Point(3, 3);
            this.crystalReportViewerMenuItems.Name = "crystalReportViewerMenuItems";
            this.crystalReportViewerMenuItems.ShowCloseButton = false;
            this.crystalReportViewerMenuItems.ShowCopyButton = false;
            this.crystalReportViewerMenuItems.ShowGotoPageButton = false;
            this.crystalReportViewerMenuItems.ShowGroupTreeButton = false;
            this.crystalReportViewerMenuItems.ShowLogo = false;
            this.crystalReportViewerMenuItems.ShowParameterPanelButton = false;
            this.crystalReportViewerMenuItems.ShowTextSearchButton = false;
            this.crystalReportViewerMenuItems.Size = new System.Drawing.Size(975, 570);
            this.crystalReportViewerMenuItems.TabIndex = 0;
            this.crystalReportViewerMenuItems.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // tabTables
            // 
            this.tabTables.Controls.Add(this.crystalReportViewerTables);
            this.tabTables.Location = new System.Drawing.Point(4, 30);
            this.tabTables.Name = "tabTables";
            this.tabTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabTables.Size = new System.Drawing.Size(981, 576);
            this.tabTables.TabIndex = 1;
            this.tabTables.Text = "Tables";
            this.tabTables.UseVisualStyleBackColor = true;
            // 
            // crystalReportViewerTables
            // 
            this.crystalReportViewerTables.ActiveViewIndex = -1;
            this.crystalReportViewerTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewerTables.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewerTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewerTables.Location = new System.Drawing.Point(3, 3);
            this.crystalReportViewerTables.Name = "crystalReportViewerTables";
            this.crystalReportViewerTables.ShowCloseButton = false;
            this.crystalReportViewerTables.ShowCopyButton = false;
            this.crystalReportViewerTables.ShowGotoPageButton = false;
            this.crystalReportViewerTables.ShowGroupTreeButton = false;
            this.crystalReportViewerTables.ShowLogo = false;
            this.crystalReportViewerTables.ShowParameterPanelButton = false;
            this.crystalReportViewerTables.ShowTextSearchButton = false;
            this.crystalReportViewerTables.Size = new System.Drawing.Size(975, 570);
            this.crystalReportViewerTables.TabIndex = 0;
            this.crystalReportViewerTables.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // AllBookingsReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 612);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonBooking);
            this.Controls.Add(this.comboBoxBooking);
            this.Controls.Add(this.labelBookingID);
            this.Font = new System.Drawing.Font("Century Gothic", 10.2F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AllBookingsReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AllBookingsReportForm";
            this.Load += new System.EventHandler(this.AllBookingsReportForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabMenuItems.ResumeLayout(false);
            this.tabTables.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelBookingID;
        private System.Windows.Forms.ComboBox comboBoxBooking;
        private System.Windows.Forms.Button buttonBooking;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMenuItems;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewerMenuItems;
        private System.Windows.Forms.TabPage tabTables;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewerTables;
    }
}