<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Interrupt_Form
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
        Me.IrqCounter_label = New System.Windows.Forms.Label
        Me.IrqStatus_CheckedListBox = New System.Windows.Forms.CheckedListBox
        Me.XIrqStatus_GroupBox = New System.Windows.Forms.GroupBox
        Me.IrqEnable_CheckBox = New System.Windows.Forms.CheckBox
        Me.IrqStatus_Label = New System.Windows.Forms.Label
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.ClearIrqCounter_Button = New System.Windows.Forms.Button
        Me.Apple_Button = New System.Windows.Forms.Button
        Me.FifoThresholdEmptyIrqMask_CheckBox = New System.Windows.Forms.CheckBox
        Me.IrqMask_GroupBox = New System.Windows.Forms.GroupBox
        Me.InputIrqMask_CheckBox_0 = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_3 = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_2 = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_5 = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_6 = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_1 = New System.Windows.Forms.CheckBox
        Me.FifoEmptyIrqMask_CheckBox = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_4 = New System.Windows.Forms.CheckBox
        Me.CompareIrqMask_CheckBox = New System.Windows.Forms.CheckBox
        Me.InputIrqMask_CheckBox_7 = New System.Windows.Forms.CheckBox
        Me.FifoFullIrqMask_CheckBox = New System.Windows.Forms.CheckBox
        Me.TimerIrqMask_CheckBox = New System.Windows.Forms.CheckBox
        Me.XIrqStatus_GroupBox.SuspendLayout()
        Me.IrqMask_GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'IrqCounter_label
        '
        Me.IrqCounter_label.BackColor = System.Drawing.SystemColors.Control
        Me.IrqCounter_label.Cursor = System.Windows.Forms.Cursors.Default
        Me.IrqCounter_label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.IrqCounter_label.Location = New System.Drawing.Point(305, 256)
        Me.IrqCounter_label.Name = "IrqCounter_label"
        Me.IrqCounter_label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.IrqCounter_label.Size = New System.Drawing.Size(113, 17)
        Me.IrqCounter_label.TabIndex = 102
        Me.IrqCounter_label.Text = "IRQ Counter:"
        '
        'IrqStatus_CheckedListBox
        '
        Me.IrqStatus_CheckedListBox.BackColor = System.Drawing.SystemColors.Control
        Me.IrqStatus_CheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.IrqStatus_CheckedListBox.CheckOnClick = True
        Me.IrqStatus_CheckedListBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IrqStatus_CheckedListBox.FormattingEnabled = True
        Me.IrqStatus_CheckedListBox.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "1", "2", "3", "4", "5", "6", "7", "8"})
        Me.IrqStatus_CheckedListBox.Location = New System.Drawing.Point(5, 15)
        Me.IrqStatus_CheckedListBox.Name = "IrqStatus_CheckedListBox"
        Me.IrqStatus_CheckedListBox.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.IrqStatus_CheckedListBox.Size = New System.Drawing.Size(18, 221)
        Me.IrqStatus_CheckedListBox.TabIndex = 92
        '
        'XIrqStatus_GroupBox
        '
        Me.XIrqStatus_GroupBox.Controls.Add(Me.IrqStatus_CheckedListBox)
        Me.XIrqStatus_GroupBox.Location = New System.Drawing.Point(271, 38)
        Me.XIrqStatus_GroupBox.Name = "XIrqStatus_GroupBox"
        Me.XIrqStatus_GroupBox.Size = New System.Drawing.Size(27, 261)
        Me.XIrqStatus_GroupBox.TabIndex = 97
        Me.XIrqStatus_GroupBox.TabStop = False
        Me.XIrqStatus_GroupBox.Text = "X"
        '
        'IrqEnable_CheckBox
        '
        Me.IrqEnable_CheckBox.AutoSize = True
        Me.IrqEnable_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.IrqEnable_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.IrqEnable_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IrqEnable_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.IrqEnable_CheckBox.Location = New System.Drawing.Point(12, 12)
        Me.IrqEnable_CheckBox.Name = "IrqEnable_CheckBox"
        Me.IrqEnable_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.IrqEnable_CheckBox.Size = New System.Drawing.Size(98, 20)
        Me.IrqEnable_CheckBox.TabIndex = 95
        Me.IrqEnable_CheckBox.Text = "IRQ Enable"
        Me.IrqEnable_CheckBox.UseVisualStyleBackColor = False
        '
        'IrqStatus_Label
        '
        Me.IrqStatus_Label.AutoSize = True
        Me.IrqStatus_Label.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IrqStatus_Label.Location = New System.Drawing.Point(237, 12)
        Me.IrqStatus_Label.Name = "IrqStatus_Label"
        Me.IrqStatus_Label.Size = New System.Drawing.Size(107, 22)
        Me.IrqStatus_Label.TabIndex = 101
        Me.IrqStatus_Label.Text = "IRQ Status"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.AutoSize = True
        Me.Cancel_Button.Location = New System.Drawing.Point(206, 239)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(47, 22)
        Me.Cancel_Button.TabIndex = 88
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'ClearIrqCounter_Button
        '
        Me.ClearIrqCounter_Button.AutoSize = True
        Me.ClearIrqCounter_Button.Location = New System.Drawing.Point(301, 276)
        Me.ClearIrqCounter_Button.Name = "ClearIrqCounter_Button"
        Me.ClearIrqCounter_Button.Size = New System.Drawing.Size(95, 23)
        Me.ClearIrqCounter_Button.TabIndex = 103
        Me.ClearIrqCounter_Button.Text = "Clear Irq counter"
        Me.ClearIrqCounter_Button.UseVisualStyleBackColor = True
        '
        'Apple_Button
        '
        Me.Apple_Button.AutoSize = True
        Me.Apple_Button.Location = New System.Drawing.Point(157, 239)
        Me.Apple_Button.Name = "Apple_Button"
        Me.Apple_Button.Size = New System.Drawing.Size(44, 22)
        Me.Apple_Button.TabIndex = 88
        Me.Apple_Button.Text = "Apply"
        Me.Apple_Button.UseVisualStyleBackColor = True
        '
        'FifoThresholdEmptyIrqMask_CheckBox
        '
        Me.FifoThresholdEmptyIrqMask_CheckBox.AutoSize = True
        Me.FifoThresholdEmptyIrqMask_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.FifoThresholdEmptyIrqMask_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.FifoThresholdEmptyIrqMask_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FifoThresholdEmptyIrqMask_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FifoThresholdEmptyIrqMask_CheckBox.Location = New System.Drawing.Point(6, 151)
        Me.FifoThresholdEmptyIrqMask_CheckBox.Name = "FifoThresholdEmptyIrqMask_CheckBox"
        Me.FifoThresholdEmptyIrqMask_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FifoThresholdEmptyIrqMask_CheckBox.Size = New System.Drawing.Size(227, 20)
        Me.FifoThresholdEmptyIrqMask_CheckBox.TabIndex = 82
        Me.FifoThresholdEmptyIrqMask_CheckBox.Text = "FIFO threshold empty IRQ mask"
        Me.FifoThresholdEmptyIrqMask_CheckBox.UseVisualStyleBackColor = False
        '
        'IrqMask_GroupBox
        '
        Me.IrqMask_GroupBox.Controls.Add(Me.Cancel_Button)
        Me.IrqMask_GroupBox.Controls.Add(Me.Apple_Button)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_0)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_3)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_2)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_5)
        Me.IrqMask_GroupBox.Controls.Add(Me.FifoThresholdEmptyIrqMask_CheckBox)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_6)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_1)
        Me.IrqMask_GroupBox.Controls.Add(Me.FifoEmptyIrqMask_CheckBox)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_4)
        Me.IrqMask_GroupBox.Controls.Add(Me.CompareIrqMask_CheckBox)
        Me.IrqMask_GroupBox.Controls.Add(Me.InputIrqMask_CheckBox_7)
        Me.IrqMask_GroupBox.Controls.Add(Me.FifoFullIrqMask_CheckBox)
        Me.IrqMask_GroupBox.Controls.Add(Me.TimerIrqMask_CheckBox)
        Me.IrqMask_GroupBox.Location = New System.Drawing.Point(6, 38)
        Me.IrqMask_GroupBox.Name = "IrqMask_GroupBox"
        Me.IrqMask_GroupBox.Size = New System.Drawing.Size(259, 261)
        Me.IrqMask_GroupBox.TabIndex = 96
        Me.IrqMask_GroupBox.TabStop = False
        Me.IrqMask_GroupBox.Text = "IRQ Mask"
        '
        'InputIrqMask_CheckBox_0
        '
        Me.InputIrqMask_CheckBox_0.AutoSize = True
        Me.InputIrqMask_CheckBox_0.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_0.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_0.Location = New System.Drawing.Point(6, 15)
        Me.InputIrqMask_CheckBox_0.Name = "InputIrqMask_CheckBox_0"
        Me.InputIrqMask_CheckBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_0.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_0.TabIndex = 82
        Me.InputIrqMask_CheckBox_0.Text = "IN 00 IRQ mask"
        Me.InputIrqMask_CheckBox_0.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_3
        '
        Me.InputIrqMask_CheckBox_3.AutoSize = True
        Me.InputIrqMask_CheckBox_3.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_3.Location = New System.Drawing.Point(6, 66)
        Me.InputIrqMask_CheckBox_3.Name = "InputIrqMask_CheckBox_3"
        Me.InputIrqMask_CheckBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_3.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_3.TabIndex = 82
        Me.InputIrqMask_CheckBox_3.Text = "IN 03 IRQ mask"
        Me.InputIrqMask_CheckBox_3.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_2
        '
        Me.InputIrqMask_CheckBox_2.AutoSize = True
        Me.InputIrqMask_CheckBox_2.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_2.Location = New System.Drawing.Point(6, 49)
        Me.InputIrqMask_CheckBox_2.Name = "InputIrqMask_CheckBox_2"
        Me.InputIrqMask_CheckBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_2.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_2.TabIndex = 78
        Me.InputIrqMask_CheckBox_2.Text = "IN 02 IRQ mask"
        Me.InputIrqMask_CheckBox_2.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_5
        '
        Me.InputIrqMask_CheckBox_5.AutoSize = True
        Me.InputIrqMask_CheckBox_5.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_5.Location = New System.Drawing.Point(6, 100)
        Me.InputIrqMask_CheckBox_5.Name = "InputIrqMask_CheckBox_5"
        Me.InputIrqMask_CheckBox_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_5.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_5.TabIndex = 78
        Me.InputIrqMask_CheckBox_5.Text = "IN 05 IRQ mask"
        Me.InputIrqMask_CheckBox_5.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_6
        '
        Me.InputIrqMask_CheckBox_6.AutoSize = True
        Me.InputIrqMask_CheckBox_6.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_6.Location = New System.Drawing.Point(6, 117)
        Me.InputIrqMask_CheckBox_6.Name = "InputIrqMask_CheckBox_6"
        Me.InputIrqMask_CheckBox_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_6.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_6.TabIndex = 85
        Me.InputIrqMask_CheckBox_6.Text = "IN 06 IRQ mask"
        Me.InputIrqMask_CheckBox_6.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_1
        '
        Me.InputIrqMask_CheckBox_1.AutoSize = True
        Me.InputIrqMask_CheckBox_1.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_1.Location = New System.Drawing.Point(6, 32)
        Me.InputIrqMask_CheckBox_1.Name = "InputIrqMask_CheckBox_1"
        Me.InputIrqMask_CheckBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_1.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_1.TabIndex = 80
        Me.InputIrqMask_CheckBox_1.Text = "IN 01 IRQ mask"
        Me.InputIrqMask_CheckBox_1.UseVisualStyleBackColor = False
        '
        'FifoEmptyIrqMask_CheckBox
        '
        Me.FifoEmptyIrqMask_CheckBox.AutoSize = True
        Me.FifoEmptyIrqMask_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.FifoEmptyIrqMask_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.FifoEmptyIrqMask_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FifoEmptyIrqMask_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FifoEmptyIrqMask_CheckBox.Location = New System.Drawing.Point(6, 185)
        Me.FifoEmptyIrqMask_CheckBox.Name = "FifoEmptyIrqMask_CheckBox"
        Me.FifoEmptyIrqMask_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FifoEmptyIrqMask_CheckBox.Size = New System.Drawing.Size(164, 20)
        Me.FifoEmptyIrqMask_CheckBox.TabIndex = 78
        Me.FifoEmptyIrqMask_CheckBox.Text = "FIFO empty IRQ mask"
        Me.FifoEmptyIrqMask_CheckBox.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_4
        '
        Me.InputIrqMask_CheckBox_4.AutoSize = True
        Me.InputIrqMask_CheckBox_4.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_4.Location = New System.Drawing.Point(6, 83)
        Me.InputIrqMask_CheckBox_4.Name = "InputIrqMask_CheckBox_4"
        Me.InputIrqMask_CheckBox_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_4.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_4.TabIndex = 80
        Me.InputIrqMask_CheckBox_4.Text = "IN 04 IRQ mask"
        Me.InputIrqMask_CheckBox_4.UseVisualStyleBackColor = False
        '
        'CompareIrqMask_CheckBox
        '
        Me.CompareIrqMask_CheckBox.AutoSize = True
        Me.CompareIrqMask_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.CompareIrqMask_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.CompareIrqMask_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompareIrqMask_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CompareIrqMask_CheckBox.Location = New System.Drawing.Point(6, 202)
        Me.CompareIrqMask_CheckBox.Name = "CompareIrqMask_CheckBox"
        Me.CompareIrqMask_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CompareIrqMask_CheckBox.Size = New System.Drawing.Size(149, 20)
        Me.CompareIrqMask_CheckBox.TabIndex = 85
        Me.CompareIrqMask_CheckBox.Text = "Compare IRQ mask"
        Me.CompareIrqMask_CheckBox.UseVisualStyleBackColor = False
        '
        'InputIrqMask_CheckBox_7
        '
        Me.InputIrqMask_CheckBox_7.AutoSize = True
        Me.InputIrqMask_CheckBox_7.BackColor = System.Drawing.SystemColors.Control
        Me.InputIrqMask_CheckBox_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputIrqMask_CheckBox_7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputIrqMask_CheckBox_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InputIrqMask_CheckBox_7.Location = New System.Drawing.Point(6, 134)
        Me.InputIrqMask_CheckBox_7.Name = "InputIrqMask_CheckBox_7"
        Me.InputIrqMask_CheckBox_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputIrqMask_CheckBox_7.Size = New System.Drawing.Size(122, 20)
        Me.InputIrqMask_CheckBox_7.TabIndex = 81
        Me.InputIrqMask_CheckBox_7.Text = "IN 07 IRQ mask"
        Me.InputIrqMask_CheckBox_7.UseVisualStyleBackColor = False
        '
        'FifoFullIrqMask_CheckBox
        '
        Me.FifoFullIrqMask_CheckBox.AutoSize = True
        Me.FifoFullIrqMask_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.FifoFullIrqMask_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.FifoFullIrqMask_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FifoFullIrqMask_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FifoFullIrqMask_CheckBox.Location = New System.Drawing.Point(6, 168)
        Me.FifoFullIrqMask_CheckBox.Name = "FifoFullIrqMask_CheckBox"
        Me.FifoFullIrqMask_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FifoFullIrqMask_CheckBox.Size = New System.Drawing.Size(145, 20)
        Me.FifoFullIrqMask_CheckBox.TabIndex = 80
        Me.FifoFullIrqMask_CheckBox.Text = "FIFO full IRQ mask"
        Me.FifoFullIrqMask_CheckBox.UseVisualStyleBackColor = False
        '
        'TimerIrqMask_CheckBox
        '
        Me.TimerIrqMask_CheckBox.AutoSize = True
        Me.TimerIrqMask_CheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.TimerIrqMask_CheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.TimerIrqMask_CheckBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerIrqMask_CheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TimerIrqMask_CheckBox.Location = New System.Drawing.Point(6, 219)
        Me.TimerIrqMask_CheckBox.Name = "TimerIrqMask_CheckBox"
        Me.TimerIrqMask_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TimerIrqMask_CheckBox.Size = New System.Drawing.Size(128, 20)
        Me.TimerIrqMask_CheckBox.TabIndex = 81
        Me.TimerIrqMask_CheckBox.Text = "Timer IRQ mask"
        Me.TimerIrqMask_CheckBox.UseVisualStyleBackColor = False
        '
        'Interrupt_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 305)
        Me.Controls.Add(Me.IrqCounter_label)
        Me.Controls.Add(Me.XIrqStatus_GroupBox)
        Me.Controls.Add(Me.IrqEnable_CheckBox)
        Me.Controls.Add(Me.IrqStatus_Label)
        Me.Controls.Add(Me.ClearIrqCounter_Button)
        Me.Controls.Add(Me.IrqMask_GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Interrupt_Form"
        Me.Text = "Interrupt_Form"
        Me.XIrqStatus_GroupBox.ResumeLayout(False)
        Me.IrqMask_GroupBox.ResumeLayout(False)
        Me.IrqMask_GroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents IrqCounter_label As System.Windows.Forms.Label
    Public WithEvents IrqStatus_CheckedListBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents XIrqStatus_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents IrqEnable_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IrqStatus_Label As System.Windows.Forms.Label
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ClearIrqCounter_Button As System.Windows.Forms.Button
    Friend WithEvents Apple_Button As System.Windows.Forms.Button
    Public WithEvents FifoThresholdEmptyIrqMask_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IrqMask_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents FifoEmptyIrqMask_CheckBox As System.Windows.Forms.CheckBox
    Public WithEvents CompareIrqMask_CheckBox As System.Windows.Forms.CheckBox
    Public WithEvents FifoFullIrqMask_CheckBox As System.Windows.Forms.CheckBox
    Public WithEvents TimerIrqMask_CheckBox As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_0 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_3 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_2 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_5 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_6 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_1 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_4 As System.Windows.Forms.CheckBox
    Public WithEvents InputIrqMask_CheckBox_7 As System.Windows.Forms.CheckBox
End Class
