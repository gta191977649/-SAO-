namespace SAP_WPF
{
    partial class Changelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Changelog));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SAO_CHANGLOG_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(16, 14);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(448, 274);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // SAO_CHANGLOG_OK
            // 
            this.SAO_CHANGLOG_OK.Location = new System.Drawing.Point(319, 296);
            this.SAO_CHANGLOG_OK.Margin = new System.Windows.Forms.Padding(4);
            this.SAO_CHANGLOG_OK.Name = "SAO_CHANGLOG_OK";
            this.SAO_CHANGLOG_OK.Size = new System.Drawing.Size(145, 45);
            this.SAO_CHANGLOG_OK.TabIndex = 1;
            this.SAO_CHANGLOG_OK.Text = "OK";
            this.SAO_CHANGLOG_OK.UseVisualStyleBackColor = true;
            this.SAO_CHANGLOG_OK.Click += new System.EventHandler(this.SAO_CHANGLOG_OK_Click);
            // 
            // Changelog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 352);
            this.ControlBox = false;
            this.Controls.Add(this.SAO_CHANGLOG_OK);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Changelog";
            this.Text = "更新历史";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button SAO_CHANGLOG_OK;
    }
}