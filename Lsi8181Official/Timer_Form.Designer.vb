<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Timer_Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Timer_Form))
        Me.Timer_GroupBox = New System.Windows.Forms.GroupBox
        Me.ShowTimerIrqMask_Button = New System.Windows.Forms.Button
        Me.CurrentValue_TextBox = New System.Windows.Forms.TextBox
        Me.CurrentValueClean_Button = New System.Windows.Forms.Button
        Me.Stop_Button = New System.Windows.Forms.Button
        Me.Start_Button = New System.Windows.Forms.Button
        Me.CurrentValue_Button = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.CurrentPeriod_Label = New System.Windows.Forms.Label
        Me.CurrentCounter_Label = New System.Windows.Forms.Label
        Me.ShowCurrentCounter_Label = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.CurrentValue_Label = New System.Windows.Forms.Label
        Me.Exit_CheckBox = New System.Windows.Forms.CheckBox
        Me.Timer_GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer_GroupBox
        '
        Me.Timer_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.Timer_GroupBox.Controls.Add(Me.ShowTimerIrqMask_Button)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentValue_TextBox)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentValueClean_Button)
        Me.Timer_GroupBox.Controls.Add(Me.Stop_Button)
        Me.Timer_GroupBox.Controls.Add(Me.Start_Button)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentValue_Button)
        Me.Timer_GroupBox.Controls.Add(Me.Label2)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentPeriod_Label)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentCounter_Label)
        Me.Timer_GroupBox.Controls.Add(Me.ShowCurrentCounter_Label)
        Me.Timer_GroupBox.Controls.Add(Me.Label4)
        Me.Timer_GroupBox.Controls.Add(Me.CurrentValue_Label)
        Me.Timer_GroupBox.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Timer_GroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Timer_GroupBox.Location = New System.Drawing.Point(9, 7)
        Me.Timer_GroupBox.Name = "Timer_GroupBox"
        Me.Timer_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Timer_GroupBox.Size = New System.Drawing.Size(281, 246)
        Me.Timer_GroupBox.TabIndex = 1
        Me.Timer_GroupBox.TabStop = False
        Me.Timer_GroupBox.Text = "Timer "
        '
        'ShowTimerIrqMask_Button
        '
        Me.ShowTimerIrqMask_Button.BackColor = System.Drawing.SystemColors.Control
        Me.ShowTimerIrqMask_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.ShowTimerIrqMask_Button.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowTimerIrqMask_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ShowTimerIrqMask_Button.Location = New System.Drawing.Point(49, 213)
        Me.ShowTimerIrqMask_Button.Name = "ShowTimerIrqMask_Button"
        Me.ShowTimerIrqMask_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowTimerIrqMask_Button.Size = New System.Drawing.Size(41, 25)
        Me.ShowTimerIrqMask_Button.TabIndex = 130
        Me.ShowTimerIrqMask_Button.Text = "IRQ"
        Me.ShowTimerIrqMask_Button.UseVisualStyleBackColor = False
        '
        'CurrentValue_TextBox
        '
        Me.CurrentValue_TextBox.AcceptsReturn = True
        Me.CurrentValue_TextBox.BackColor = System.Drawing.SystemColors.Window
        Me.CurrentValue_TextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.CurrentValue_TextBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentValue_TextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CurrentValue_TextBox.Location = New System.Drawing.Point(22, 142)
        Me.CurrentValue_TextBox.MaxLength = 0
        Me.CurrentValue_TextBox.Name = "CurrentValue_TextBox"
        Me.CurrentValue_TextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentValue_TextBox.Size = New System.Drawing.Size(89, 22)
        Me.CurrentValue_TextBox.TabIndex = 42
        Me.CurrentValue_TextBox.Text = "0"
        Me.CurrentValue_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CurrentValueClean_Button
        '
        Me.CurrentValueClean_Button.BackColor = System.Drawing.SystemColors.Control
        Me.CurrentValueClean_Button.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentValueClean_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CurrentValueClean_Button.Image = CType(resources.GetObject("CurrentValueClean_Button.Image"), System.Drawing.Image)
        Me.CurrentValueClean_Button.Location = New System.Drawing.Point(252, 140)
        Me.CurrentValueClean_Button.Name = "CurrentValueClean_Button"
        Me.CurrentValueClean_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentValueClean_Button.Size = New System.Drawing.Size(20, 26)
        Me.CurrentValueClean_Button.TabIndex = 52
        Me.CurrentValueClean_Button.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.CurrentValueClean_Button.UseVisualStyleBackColor = False
        '
        'Stop_Button
        '
        Me.Stop_Button.AutoEllipsis = True
        Me.Stop_Button.BackColor = System.Drawing.Color.Red
        Me.Stop_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.Stop_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Stop_Button.Location = New System.Drawing.Point(183, 209)
        Me.Stop_Button.Name = "Stop_Button"
        Me.Stop_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Stop_Button.Size = New System.Drawing.Size(89, 30)
        Me.Stop_Button.TabIndex = 6
        Me.Stop_Button.Text = "Stop TC"
        Me.Stop_Button.UseVisualStyleBackColor = False
        '
        'Start_Button
        '
        Me.Start_Button.AutoSize = True
        Me.Start_Button.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Start_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.Start_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Start_Button.Location = New System.Drawing.Point(96, 209)
        Me.Start_Button.Name = "Start_Button"
        Me.Start_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Start_Button.Size = New System.Drawing.Size(81, 30)
        Me.Start_Button.TabIndex = 5
        Me.Start_Button.Text = "Start TC"
        Me.Start_Button.UseVisualStyleBackColor = False
        '
        'CurrentValue_Button
        '
        Me.CurrentValue_Button.AutoSize = True
        Me.CurrentValue_Button.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CurrentValue_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentValue_Button.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentValue_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CurrentValue_Button.Location = New System.Drawing.Point(153, 139)
        Me.CurrentValue_Button.Name = "CurrentValue_Button"
        Me.CurrentValue_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentValue_Button.Size = New System.Drawing.Size(96, 29)
        Me.CurrentValue_Button.TabIndex = 3
        Me.CurrentValue_Button.Text = "Set timer"
        Me.CurrentValue_Button.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(9, 170)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(250, 17)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Period T=(time constant+1)*1us"
        '
        'CurrentPeriod_Label
        '
        Me.CurrentPeriod_Label.BackColor = System.Drawing.SystemColors.Control
        Me.CurrentPeriod_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentPeriod_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CurrentPeriod_Label.Location = New System.Drawing.Point(9, 187)
        Me.CurrentPeriod_Label.Name = "CurrentPeriod_Label"
        Me.CurrentPeriod_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentPeriod_Label.Size = New System.Drawing.Size(185, 19)
        Me.CurrentPeriod_Label.TabIndex = 21
        Me.CurrentPeriod_Label.Text = "T="
        '
        'CurrentCounter_Label
        '
        Me.CurrentCounter_Label.BackColor = System.Drawing.Color.Black
        Me.CurrentCounter_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentCounter_Label.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentCounter_Label.ForeColor = System.Drawing.Color.Yellow
        Me.CurrentCounter_Label.Location = New System.Drawing.Point(9, 39)
        Me.CurrentCounter_Label.Name = "CurrentCounter_Label"
        Me.CurrentCounter_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentCounter_Label.Size = New System.Drawing.Size(264, 25)
        Me.CurrentCounter_Label.TabIndex = 20
        Me.CurrentCounter_Label.Text = "0"
        Me.CurrentCounter_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ShowCurrentCounter_Label
        '
        Me.ShowCurrentCounter_Label.BackColor = System.Drawing.Color.Black
        Me.ShowCurrentCounter_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.ShowCurrentCounter_Label.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowCurrentCounter_Label.ForeColor = System.Drawing.Color.Yellow
        Me.ShowCurrentCounter_Label.Location = New System.Drawing.Point(9, 23)
        Me.ShowCurrentCounter_Label.Name = "ShowCurrentCounter_Label"
        Me.ShowCurrentCounter_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowCurrentCounter_Label.Size = New System.Drawing.Size(264, 17)
        Me.ShowCurrentCounter_Label.TabIndex = 19
        Me.ShowCurrentCounter_Label.Text = "Current counter"
        Me.ShowCurrentCounter_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(9, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(233, 54)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Warning:To preload system hanged , use time constant greater than 65535"
        '
        'CurrentValue_Label
        '
        Me.CurrentValue_Label.AutoSize = True
        Me.CurrentValue_Label.BackColor = System.Drawing.SystemColors.Control
        Me.CurrentValue_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentValue_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CurrentValue_Label.Location = New System.Drawing.Point(9, 123)
        Me.CurrentValue_Label.Name = "CurrentValue_Label"
        Me.CurrentValue_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentValue_Label.Size = New System.Drawing.Size(113, 19)
        Me.CurrentValue_Label.TabIndex = 2
        Me.CurrentValue_Label.Text = "Current Value"
        '
        'Exit_CheckBox
        '
        Me.Exit_CheckBox.Appearance = System.Windows.Forms.Appearance.Button
        Me.Exit_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.Exit_CheckBox.Font = New System.Drawing.Font("PMingLiU", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Exit_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Exit_CheckBox.Image = CType(resources.GetObject("Exit_CheckBox.Image"), System.Drawing.Image)
        Me.Exit_CheckBox.Location = New System.Drawing.Point(240, 259)
        Me.Exit_CheckBox.Name = "Exit_CheckBox"
        Me.Exit_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Exit_CheckBox.Size = New System.Drawing.Size(41, 37)
        Me.Exit_CheckBox.TabIndex = 11
        Me.Exit_CheckBox.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Exit_CheckBox.UseVisualStyleBackColor = False
        '
        'Timer_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(297, 301)
        Me.Controls.Add(Me.Exit_CheckBox)
        Me.Controls.Add(Me.Timer_GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Timer_Form"
        Me.Text = "Timer_Form"
        Me.Timer_GroupBox.ResumeLayout(False)
        Me.Timer_GroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Timer_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents Stop_Button As System.Windows.Forms.Button
    Public WithEvents Start_Button As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents CurrentPeriod_Label As System.Windows.Forms.Label
    Public WithEvents CurrentCounter_Label As System.Windows.Forms.Label
    Public WithEvents ShowCurrentCounter_Label As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents CurrentValue_Button As System.Windows.Forms.Button
    Public WithEvents CurrentValue_Label As System.Windows.Forms.Label
    Public WithEvents Exit_CheckBox As System.Windows.Forms.CheckBox
    Public WithEvents CurrentValueClean_Button As System.Windows.Forms.Button
    Public WithEvents CurrentValue_TextBox As System.Windows.Forms.TextBox
    Public WithEvents ShowTimerIrqMask_Button As System.Windows.Forms.Button
End Class
