﻿namespace Monoedit
{
    partial class AddEditItem
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
            this._pnlSpriteListView = new System.Windows.Forms.Panel();
            this._btnAdd = new MetroFramework.Controls.MetroButton();
            this._btnRemove = new MetroFramework.Controls.MetroButton();
            this._btnCopy = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // _pnlSpriteListView
            // 
            this._pnlSpriteListView.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlSpriteListView.Location = new System.Drawing.Point(20, 60);
            this._pnlSpriteListView.Name = "_pnlSpriteListView";
            this._pnlSpriteListView.Size = new System.Drawing.Size(289, 270);
            this._pnlSpriteListView.TabIndex = 0;
            // 
            // _btnAdd
            // 
            this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._btnAdd.Location = new System.Drawing.Point(201, 22);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(32, 32);
            this._btnAdd.TabIndex = 1;
            this._btnAdd.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._btnAdd.UseSelectable = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._btnRemove.Location = new System.Drawing.Point(239, 22);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(32, 32);
            this._btnRemove.TabIndex = 1;
            this._btnRemove.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._btnRemove.UseSelectable = true;
            this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _btnCopy
            // 
            this._btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._btnCopy.Location = new System.Drawing.Point(277, 22);
            this._btnCopy.Name = "_btnCopy";
            this._btnCopy.Size = new System.Drawing.Size(32, 32);
            this._btnCopy.TabIndex = 1;
            this._btnCopy.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._btnCopy.UseSelectable = true;
            this._btnCopy.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // AddEditItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 378);
            this.Controls.Add(this._btnCopy);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnAdd);
            this.Controls.Add(this._pnlSpriteListView);
            this.Name = "AddEditItem";
            this.Text = "Add/Edit";
            this.Load += new System.EventHandler(this.AddEditItem_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlSpriteListView;
        private MetroFramework.Controls.MetroButton _btnAdd;
        private MetroFramework.Controls.MetroButton _btnRemove;
        private MetroFramework.Controls.MetroButton _btnCopy;
    }
}