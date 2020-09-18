namespace NetEti.DemoApplications
{
  partial class Form1
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
      this.tbxKey = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.btnGo = new System.Windows.Forms.Button();
      this.lbxValues = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // tbxKey
      // 
      this.tbxKey.Location = new System.Drawing.Point(47, 48);
      this.tbxKey.Name = "tbxKey";
      this.tbxKey.Size = new System.Drawing.Size(491, 20);
      this.tbxKey.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(44, 32);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Schlüssel";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(44, 82);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(36, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Werte";
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(244, 421);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new System.Drawing.Size(63, 23);
      this.btnGo.TabIndex = 4;
      this.btnGo.Text = "go!";
      this.btnGo.UseVisualStyleBackColor = true;
      this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
      // 
      // lbxValues
      // 
      this.lbxValues.FormattingEnabled = true;
      this.lbxValues.Location = new System.Drawing.Point(47, 127);
      this.lbxValues.Name = "lbxValues";
      this.lbxValues.Size = new System.Drawing.Size(491, 264);
      this.lbxValues.TabIndex = 5;
      this.lbxValues.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxValues_MouseDoubleClick);
      // 
      // FrmMain
      // 
      this.AcceptButton = this.btnGo;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(581, 471);
      this.Controls.Add(this.lbxValues);
      this.Controls.Add(this.btnGo);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbxKey);
      this.Name = "FrmMain";
      this.Text = "RegistryAccessDemo";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tbxKey;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnGo;
    private System.Windows.Forms.ListBox lbxValues;
  }
}

