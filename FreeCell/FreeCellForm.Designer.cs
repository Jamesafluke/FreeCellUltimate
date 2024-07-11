using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace FreeCell;

partial class FreeCellForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        backgroundPictureBox = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
        SuspendLayout();
        // 
        // backgroundPictureBox
        // 
        backgroundPictureBox.BackColor = Color.DarkGreen;
        backgroundPictureBox.Location = new Point(-5, -5);
        backgroundPictureBox.Name = "backgroundPictureBox";
        backgroundPictureBox.Size = new Size(1564, 1071);
        backgroundPictureBox.TabIndex = 0;
        backgroundPictureBox.TabStop = false;
        // 
        // FreeCellForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Lime;
        ClientSize = new Size(1554, 1061);
        Controls.Add(backgroundPictureBox);
        KeyPreview = true;
        Name = "FreeCellForm";
        Text = "Free Cell Ultimate";
        KeyDown += FreeCellForm_KeyDown;
        KeyPress += FreeCellForm_KeyPress;
        KeyUp += FreeCellForm_KeyUp;
        ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox backgroundPictureBox;
}
