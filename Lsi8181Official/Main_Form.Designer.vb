<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main_Form
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main_Form))
        Me.CompareValue_Label = New System.Windows.Forms.Label
        Me.ID_ComboBox = New System.Windows.Forms.ComboBox
        Me._Label1_2 = New System.Windows.Forms.Label
        Me.Address_Label = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OperationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OutputToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtensionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SegmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InterruptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TimerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InputToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ExitDoor_Button = New System.Windows.Forms.Button
        Me.X_Encoder_GroupBox = New System.Windows.Forms.GroupBox
        Me.X_Input_GroupBox = New System.Windows.Forms.GroupBox
        Me.InputStatus_CheckedListBox = New System.Windows.Forms.CheckedListBox
        Me.CurrentCounter_Label = New System.Windows.Forms.Label
        Me.CurrentCounterShow_Label = New System.Windows.Forms.Label
        Me.X_Compare_GroupBox = New System.Windows.Forms.GroupBox
        Me.CompareValueShow_Label = New System.Windows.Forms.Label
        Me.IoStatus_GroupBox = New System.Windows.Forms.GroupBox
        Me.IoStatus_CheckedListBox = New System.Windows.Forms.CheckedListBox
        Me.FifoUnusedNunberShow_Label = New System.Windows.Forms.Label
        Me.FifoUnusedNunber_Label = New System.Windows.Forms.Label
        Me.homing_mode_Label = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.X_Encoder_GroupBox.SuspendLayout()
        Me.X_Input_GroupBox.SuspendLayout()
        Me.X_Compare_GroupBox.SuspendLayout()
        Me.IoStatus_GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'CompareValue_Label
        '
        Me.CompareValue_Label.BackColor = System.Drawing.SystemColors.Info
        Me.CompareValue_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CompareValue_Label.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompareValue_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CompareValue_Label.Location = New System.Drawing.Point(12, 39)
        Me.CompareValue_Label.Name = "CompareValue_Label"
        Me.CompareValue_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CompareValue_Label.Size = New System.Drawing.Size(111, 25)
        Me.CompareValue_Label.TabIndex = 15
        Me.CompareValue_Label.Text = "0"
        Me.CompareValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ID_ComboBox
        '
        Me.ID_ComboBox.BackColor = System.Drawing.SystemColors.Window
        Me.ID_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ID_ComboBox.Font = New System.Drawing.Font("PMingLiU", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ID_ComboBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ID_ComboBox.Location = New System.Drawing.Point(68, 30)
        Me.ID_ComboBox.Name = "ID_ComboBox"
        Me.ID_ComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ID_ComboBox.Size = New System.Drawing.Size(65, 20)
        Me.ID_ComboBox.TabIndex = 25
        '
        '_Label1_2
        '
        Me._Label1_2.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_2.Location = New System.Drawing.Point(12, 33)
        Me._Label1_2.Name = "_Label1_2"
        Me._Label1_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_2.Size = New System.Drawing.Size(57, 19)
        Me._Label1_2.TabIndex = 27
        Me._Label1_2.Text = "Card ID"
        '
        'Address_Label
        '
        Me.Address_Label.AutoSize = True
        Me.Address_Label.BackColor = System.Drawing.SystemColors.Control
        Me.Address_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.Address_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Address_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Address_Label.Location = New System.Drawing.Point(12, 52)
        Me.Address_Label.Name = "Address_Label"
        Me.Address_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Address_Label.Size = New System.Drawing.Size(66, 16)
        Me.Address_Label.TabIndex = 26
        Me.Address_Label.Text = "Address :"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OperationToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(294, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.FileToolStripMenuItem.Text = "Quit"
        '
        'OperationToolStripMenuItem
        '
        Me.OperationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OutputToolStripMenuItem, Me.ExtensionToolStripMenuItem, Me.SegmentToolStripMenuItem, Me.InterruptToolStripMenuItem, Me.TimerToolStripMenuItem, Me.InputToolStripMenuItem})
        Me.OperationToolStripMenuItem.Name = "OperationToolStripMenuItem"
        Me.OperationToolStripMenuItem.Size = New System.Drawing.Size(78, 20)
        Me.OperationToolStripMenuItem.Text = "Operation"
        '
        'OutputToolStripMenuItem
        '
        Me.OutputToolStripMenuItem.Name = "OutputToolStripMenuItem"
        Me.OutputToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OutputToolStripMenuItem.Text = "Compare"
        '
        'ExtensionToolStripMenuItem
        '
        Me.ExtensionToolStripMenuItem.Name = "ExtensionToolStripMenuItem"
        Me.ExtensionToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExtensionToolStripMenuItem.Text = "Extension"
        '
        'SegmentToolStripMenuItem
        '
        Me.SegmentToolStripMenuItem.Name = "SegmentToolStripMenuItem"
        Me.SegmentToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SegmentToolStripMenuItem.Text = "Segment"
        '
        'InterruptToolStripMenuItem
        '
        Me.InterruptToolStripMenuItem.Name = "InterruptToolStripMenuItem"
        Me.InterruptToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.InterruptToolStripMenuItem.Text = "Interrupt"
        '
        'TimerToolStripMenuItem
        '
        Me.TimerToolStripMenuItem.Name = "TimerToolStripMenuItem"
        Me.TimerToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.TimerToolStripMenuItem.Text = "Timer"
        '
        'InputToolStripMenuItem
        '
        Me.InputToolStripMenuItem.Name = "InputToolStripMenuItem"
        Me.InputToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.InputToolStripMenuItem.Text = "IO"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.HelpToolStripMenuItem1})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'HelpToolStripMenuItem1
        '
        Me.HelpToolStripMenuItem1.Name = "HelpToolStripMenuItem1"
        Me.HelpToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.HelpToolStripMenuItem1.Text = "help"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 80
        '
        'ExitDoor_Button
        '
        Me.ExitDoor_Button.BackColor = System.Drawing.SystemColors.Control
        Me.ExitDoor_Button.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitDoor_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExitDoor_Button.Image = CType(resources.GetObject("ExitDoor_Button.Image"), System.Drawing.Image)
        Me.ExitDoor_Button.Location = New System.Drawing.Point(235, 307)
        Me.ExitDoor_Button.Name = "ExitDoor_Button"
        Me.ExitDoor_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExitDoor_Button.Size = New System.Drawing.Size(53, 34)
        Me.ExitDoor_Button.TabIndex = 56
        Me.ExitDoor_Button.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ExitDoor_Button, "Exit")
        Me.ExitDoor_Button.UseVisualStyleBackColor = False
        '
        'X_Encoder_GroupBox
        '
        Me.X_Encoder_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.X_Encoder_GroupBox.Controls.Add(Me.X_Input_GroupBox)
        Me.X_Encoder_GroupBox.Controls.Add(Me.CurrentCounter_Label)
        Me.X_Encoder_GroupBox.Controls.Add(Me.CurrentCounterShow_Label)
        Me.X_Encoder_GroupBox.Font = New System.Drawing.Font("Arial", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.X_Encoder_GroupBox.ForeColor = System.Drawing.Color.Black
        Me.X_Encoder_GroupBox.Location = New System.Drawing.Point(153, 124)
        Me.X_Encoder_GroupBox.Name = "X_Encoder_GroupBox"
        Me.X_Encoder_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.X_Encoder_GroupBox.Size = New System.Drawing.Size(135, 164)
        Me.X_Encoder_GroupBox.TabIndex = 48
        Me.X_Encoder_GroupBox.TabStop = False
        Me.X_Encoder_GroupBox.Text = "Encoder"
        '
        'X_Input_GroupBox
        '
        Me.X_Input_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.X_Input_GroupBox.Controls.Add(Me.InputStatus_CheckedListBox)
        Me.X_Input_GroupBox.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.X_Input_GroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.X_Input_GroupBox.Location = New System.Drawing.Point(6, 64)
        Me.X_Input_GroupBox.Name = "X_Input_GroupBox"
        Me.X_Input_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.X_Input_GroupBox.Size = New System.Drawing.Size(123, 97)
        Me.X_Input_GroupBox.TabIndex = 129
        Me.X_Input_GroupBox.TabStop = False
        Me.X_Input_GroupBox.Text = "Input status"
        '
        'InputStatus_CheckedListBox
        '
        Me.InputStatus_CheckedListBox.BackColor = System.Drawing.SystemColors.Control
        Me.InputStatus_CheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.InputStatus_CheckedListBox.CheckOnClick = True
        Me.InputStatus_CheckedListBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputStatus_CheckedListBox.FormattingEnabled = True
        Me.InputStatus_CheckedListBox.Items.AddRange(New Object() {"A Phase", "B Phase", "Z Phase", "Zero toggle"})
        Me.InputStatus_CheckedListBox.Location = New System.Drawing.Point(13, 22)
        Me.InputStatus_CheckedListBox.Name = "InputStatus_CheckedListBox"
        Me.InputStatus_CheckedListBox.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.InputStatus_CheckedListBox.Size = New System.Drawing.Size(104, 68)
        Me.InputStatus_CheckedListBox.TabIndex = 16
        '
        'CurrentCounter_Label
        '
        Me.CurrentCounter_Label.BackColor = System.Drawing.SystemColors.Info
        Me.CurrentCounter_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentCounter_Label.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentCounter_Label.ForeColor = System.Drawing.Color.Black
        Me.CurrentCounter_Label.Location = New System.Drawing.Point(12, 36)
        Me.CurrentCounter_Label.Name = "CurrentCounter_Label"
        Me.CurrentCounter_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentCounter_Label.Size = New System.Drawing.Size(111, 25)
        Me.CurrentCounter_Label.TabIndex = 14
        Me.CurrentCounter_Label.Text = "0"
        Me.CurrentCounter_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CurrentCounterShow_Label
        '
        Me.CurrentCounterShow_Label.BackColor = System.Drawing.Color.Plum
        Me.CurrentCounterShow_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CurrentCounterShow_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentCounterShow_Label.ForeColor = System.Drawing.Color.Black
        Me.CurrentCounterShow_Label.Location = New System.Drawing.Point(12, 20)
        Me.CurrentCounterShow_Label.Name = "CurrentCounterShow_Label"
        Me.CurrentCounterShow_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentCounterShow_Label.Size = New System.Drawing.Size(111, 17)
        Me.CurrentCounterShow_Label.TabIndex = 11
        Me.CurrentCounterShow_Label.Text = "Current counter"
        Me.CurrentCounterShow_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'X_Compare_GroupBox
        '
        Me.X_Compare_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.X_Compare_GroupBox.Controls.Add(Me.CompareValueShow_Label)
        Me.X_Compare_GroupBox.Controls.Add(Me.IoStatus_GroupBox)
        Me.X_Compare_GroupBox.Controls.Add(Me.CompareValue_Label)
        Me.X_Compare_GroupBox.Controls.Add(Me.FifoUnusedNunberShow_Label)
        Me.X_Compare_GroupBox.Controls.Add(Me.FifoUnusedNunber_Label)
        Me.X_Compare_GroupBox.Font = New System.Drawing.Font("Arial", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.X_Compare_GroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.X_Compare_GroupBox.Location = New System.Drawing.Point(12, 124)
        Me.X_Compare_GroupBox.Name = "X_Compare_GroupBox"
        Me.X_Compare_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.X_Compare_GroupBox.Size = New System.Drawing.Size(135, 217)
        Me.X_Compare_GroupBox.TabIndex = 49
        Me.X_Compare_GroupBox.TabStop = False
        Me.X_Compare_GroupBox.Text = "Compare"
        '
        'CompareValueShow_Label
        '
        Me.CompareValueShow_Label.BackColor = System.Drawing.Color.Plum
        Me.CompareValueShow_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.CompareValueShow_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompareValueShow_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CompareValueShow_Label.Location = New System.Drawing.Point(12, 22)
        Me.CompareValueShow_Label.Name = "CompareValueShow_Label"
        Me.CompareValueShow_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CompareValueShow_Label.Size = New System.Drawing.Size(111, 17)
        Me.CompareValueShow_Label.TabIndex = 12
        Me.CompareValueShow_Label.Text = "Compare value"
        Me.CompareValueShow_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'IoStatus_GroupBox
        '
        Me.IoStatus_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.IoStatus_GroupBox.Controls.Add(Me.IoStatus_CheckedListBox)
        Me.IoStatus_GroupBox.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IoStatus_GroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.IoStatus_GroupBox.Location = New System.Drawing.Point(6, 67)
        Me.IoStatus_GroupBox.Name = "IoStatus_GroupBox"
        Me.IoStatus_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.IoStatus_GroupBox.Size = New System.Drawing.Size(123, 96)
        Me.IoStatus_GroupBox.TabIndex = 129
        Me.IoStatus_GroupBox.TabStop = False
        Me.IoStatus_GroupBox.Text = "IO status"
        '
        'IoStatus_CheckedListBox
        '
        Me.IoStatus_CheckedListBox.BackColor = System.Drawing.SystemColors.Control
        Me.IoStatus_CheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.IoStatus_CheckedListBox.CheckOnClick = True
        Me.IoStatus_CheckedListBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IoStatus_CheckedListBox.FormattingEnabled = True
        Me.IoStatus_CheckedListBox.Items.AddRange(New Object() {"Home IN", "CLR IN", "IN 00", "CMP OUT"})
        Me.IoStatus_CheckedListBox.Location = New System.Drawing.Point(13, 22)
        Me.IoStatus_CheckedListBox.Name = "IoStatus_CheckedListBox"
        Me.IoStatus_CheckedListBox.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.IoStatus_CheckedListBox.Size = New System.Drawing.Size(104, 68)
        Me.IoStatus_CheckedListBox.TabIndex = 124
        '
        'FifoUnusedNunberShow_Label
        '
        Me.FifoUnusedNunberShow_Label.BackColor = System.Drawing.Color.Plum
        Me.FifoUnusedNunberShow_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.FifoUnusedNunberShow_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FifoUnusedNunberShow_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FifoUnusedNunberShow_Label.Location = New System.Drawing.Point(12, 170)
        Me.FifoUnusedNunberShow_Label.Name = "FifoUnusedNunberShow_Label"
        Me.FifoUnusedNunberShow_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FifoUnusedNunberShow_Label.Size = New System.Drawing.Size(111, 17)
        Me.FifoUnusedNunberShow_Label.TabIndex = 126
        Me.FifoUnusedNunberShow_Label.Text = "FIFO unused No."
        Me.FifoUnusedNunberShow_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'FifoUnusedNunber_Label
        '
        Me.FifoUnusedNunber_Label.BackColor = System.Drawing.SystemColors.Info
        Me.FifoUnusedNunber_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.FifoUnusedNunber_Label.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FifoUnusedNunber_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FifoUnusedNunber_Label.Location = New System.Drawing.Point(12, 186)
        Me.FifoUnusedNunber_Label.Name = "FifoUnusedNunber_Label"
        Me.FifoUnusedNunber_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FifoUnusedNunber_Label.Size = New System.Drawing.Size(111, 25)
        Me.FifoUnusedNunber_Label.TabIndex = 127
        Me.FifoUnusedNunber_Label.Text = "0"
        Me.FifoUnusedNunber_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'homing_mode_Label
        '
        Me.homing_mode_Label.AutoSize = True
        Me.homing_mode_Label.Font = New System.Drawing.Font("PMingLiU", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.homing_mode_Label.Location = New System.Drawing.Point(15, 96)
        Me.homing_mode_Label.Name = "homing_mode_Label"
        Me.homing_mode_Label.Size = New System.Drawing.Size(221, 16)
        Me.homing_mode_Label.TabIndex = 57
        Me.homing_mode_Label.Text = "NORMAL/DISABLE (default)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("PMingLiU", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 16)
        Me.Label1.TabIndex = 58
        Me.Label1.Text = "Homing mode:"
        '
        'Main_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 352)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.homing_mode_Label)
        Me.Controls.Add(Me.X_Encoder_GroupBox)
        Me.Controls.Add(Me.X_Compare_GroupBox)
        Me.Controls.Add(Me.ExitDoor_Button)
        Me.Controls.Add(Me.ID_ComboBox)
        Me.Controls.Add(Me._Label1_2)
        Me.Controls.Add(Me.Address_Label)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main_Form"
        Me.Text = "LSI-8181 Test Program"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.X_Encoder_GroupBox.ResumeLayout(False)
        Me.X_Input_GroupBox.ResumeLayout(False)
        Me.X_Compare_GroupBox.ResumeLayout(False)
        Me.IoStatus_GroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents CompareValue_Label As System.Windows.Forms.Label
    Public WithEvents ID_ComboBox As System.Windows.Forms.ComboBox
    Public WithEvents _Label1_2 As System.Windows.Forms.Label
    Public WithEvents Address_Label As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents ExitDoor_Button As System.Windows.Forms.Button
    Public WithEvents X_Encoder_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents X_Input_GroupBox As System.Windows.Forms.GroupBox
    Private WithEvents InputStatus_CheckedListBox As System.Windows.Forms.CheckedListBox
    Public WithEvents CurrentCounter_Label As System.Windows.Forms.Label
    Public WithEvents CurrentCounterShow_Label As System.Windows.Forms.Label
    Public WithEvents X_Compare_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents CompareValueShow_Label As System.Windows.Forms.Label
    Public WithEvents IoStatus_GroupBox As System.Windows.Forms.GroupBox
    Private WithEvents IoStatus_CheckedListBox As System.Windows.Forms.CheckedListBox
    Public WithEvents FifoUnusedNunberShow_Label As System.Windows.Forms.Label
    Public WithEvents FifoUnusedNunber_Label As System.Windows.Forms.Label
    Friend WithEvents OperationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InputToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OutputToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InterruptToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SegmentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtensionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents homing_mode_Label As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem

End Class
