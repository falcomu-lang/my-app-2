<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IO_Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IO_Form))
        Me.Auto_Button = New System.Windows.Forms.CheckBox
        Me.DIO_GroupBox = New System.Windows.Forms.GroupBox
        Me.Input_CheckBox_0 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_1 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_2 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_3 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_4 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_5 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_6 = New System.Windows.Forms.CheckBox
        Me.Input_CheckBox_7 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_0 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_1 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_2 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_3 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_4 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_5 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_6 = New System.Windows.Forms.CheckBox
        Me.Output_CheckBox_7 = New System.Windows.Forms.CheckBox
        Me.Debounce_ComboBox = New System.Windows.Forms.ComboBox
        Me.OutputPolarity_CheckBox_0 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_1 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_2 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_3 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_4 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_5 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_6 = New System.Windows.Forms.CheckBox
        Me.OutputPolarity_CheckBox_7 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_0 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_1 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_2 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_3 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_4 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_5 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_6 = New System.Windows.Forms.CheckBox
        Me.InputPolarity_CheckBox_7 = New System.Windows.Forms.CheckBox
        Me.Debounce_Label = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.ExitDoor_Button = New System.Windows.Forms.Button
        Me.ShowIoIrqMask_Button = New System.Windows.Forms.Button
        Me.DIO_GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'Auto_Button
        '
        Me.Auto_Button.Appearance = System.Windows.Forms.Appearance.Button
        Me.Auto_Button.AutoSize = True
        Me.Auto_Button.BackColor = System.Drawing.SystemColors.Control
        Me.Auto_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.Auto_Button.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Auto_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Auto_Button.Location = New System.Drawing.Point(165, 263)
        Me.Auto_Button.Name = "Auto_Button"
        Me.Auto_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Auto_Button.Size = New System.Drawing.Size(54, 26)
        Me.Auto_Button.TabIndex = 44
        Me.Auto_Button.Text = "AUTO"
        Me.Auto_Button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Auto_Button.UseVisualStyleBackColor = False
        '
        'DIO_GroupBox
        '
        Me.DIO_GroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_0)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_1)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_2)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_3)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_4)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_5)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_6)
        Me.DIO_GroupBox.Controls.Add(Me.Input_CheckBox_7)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_0)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_1)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_2)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_3)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_4)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_5)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_6)
        Me.DIO_GroupBox.Controls.Add(Me.Output_CheckBox_7)
        Me.DIO_GroupBox.Controls.Add(Me.Debounce_ComboBox)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_0)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_1)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_2)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_3)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_4)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_5)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_6)
        Me.DIO_GroupBox.Controls.Add(Me.OutputPolarity_CheckBox_7)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_0)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_1)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_2)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_3)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_4)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_5)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_6)
        Me.DIO_GroupBox.Controls.Add(Me.InputPolarity_CheckBox_7)
        Me.DIO_GroupBox.Controls.Add(Me.Debounce_Label)
        Me.DIO_GroupBox.Controls.Add(Me.Label3)
        Me.DIO_GroupBox.Controls.Add(Me.Label5)
        Me.DIO_GroupBox.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DIO_GroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DIO_GroupBox.Location = New System.Drawing.Point(11, 9)
        Me.DIO_GroupBox.Name = "DIO_GroupBox"
        Me.DIO_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DIO_GroupBox.Size = New System.Drawing.Size(267, 245)
        Me.DIO_GroupBox.TabIndex = 41
        Me.DIO_GroupBox.TabStop = False
        Me.DIO_GroupBox.Text = "DIO"
        '
        'Input_CheckBox_0
        '
        Me.Input_CheckBox_0.AutoSize = True
        Me.Input_CheckBox_0.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_0.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_0.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_0.Location = New System.Drawing.Point(21, 73)
        Me.Input_CheckBox_0.Name = "Input_CheckBox_0"
        Me.Input_CheckBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_0.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_0.TabIndex = 33
        Me.Input_CheckBox_0.Text = "IN 00"
        Me.Input_CheckBox_0.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_1
        '
        Me.Input_CheckBox_1.AutoSize = True
        Me.Input_CheckBox_1.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_1.Location = New System.Drawing.Point(21, 93)
        Me.Input_CheckBox_1.Name = "Input_CheckBox_1"
        Me.Input_CheckBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_1.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_1.TabIndex = 32
        Me.Input_CheckBox_1.Text = "IN 01"
        Me.Input_CheckBox_1.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_2
        '
        Me.Input_CheckBox_2.AutoSize = True
        Me.Input_CheckBox_2.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_2.Location = New System.Drawing.Point(21, 113)
        Me.Input_CheckBox_2.Name = "Input_CheckBox_2"
        Me.Input_CheckBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_2.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_2.TabIndex = 31
        Me.Input_CheckBox_2.Text = "IN 02"
        Me.Input_CheckBox_2.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_3
        '
        Me.Input_CheckBox_3.AutoSize = True
        Me.Input_CheckBox_3.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_3.Location = New System.Drawing.Point(21, 133)
        Me.Input_CheckBox_3.Name = "Input_CheckBox_3"
        Me.Input_CheckBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_3.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_3.TabIndex = 30
        Me.Input_CheckBox_3.Text = "IN 03"
        Me.Input_CheckBox_3.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_4
        '
        Me.Input_CheckBox_4.AutoSize = True
        Me.Input_CheckBox_4.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_4.Location = New System.Drawing.Point(21, 153)
        Me.Input_CheckBox_4.Name = "Input_CheckBox_4"
        Me.Input_CheckBox_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_4.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_4.TabIndex = 29
        Me.Input_CheckBox_4.Text = "IN 04"
        Me.Input_CheckBox_4.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_5
        '
        Me.Input_CheckBox_5.AutoSize = True
        Me.Input_CheckBox_5.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_5.Location = New System.Drawing.Point(21, 173)
        Me.Input_CheckBox_5.Name = "Input_CheckBox_5"
        Me.Input_CheckBox_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_5.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_5.TabIndex = 28
        Me.Input_CheckBox_5.Text = "IN 05"
        Me.Input_CheckBox_5.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_6
        '
        Me.Input_CheckBox_6.AutoSize = True
        Me.Input_CheckBox_6.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_6.Location = New System.Drawing.Point(21, 193)
        Me.Input_CheckBox_6.Name = "Input_CheckBox_6"
        Me.Input_CheckBox_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_6.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_6.TabIndex = 27
        Me.Input_CheckBox_6.Text = "IN 06"
        Me.Input_CheckBox_6.UseVisualStyleBackColor = False
        '
        'Input_CheckBox_7
        '
        Me.Input_CheckBox_7.AutoSize = True
        Me.Input_CheckBox_7.BackColor = System.Drawing.SystemColors.Control
        Me.Input_CheckBox_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Input_CheckBox_7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Input_CheckBox_7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Input_CheckBox_7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Input_CheckBox_7.Location = New System.Drawing.Point(21, 213)
        Me.Input_CheckBox_7.Name = "Input_CheckBox_7"
        Me.Input_CheckBox_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Input_CheckBox_7.Size = New System.Drawing.Size(55, 20)
        Me.Input_CheckBox_7.TabIndex = 26
        Me.Input_CheckBox_7.Text = "IN 07"
        Me.Input_CheckBox_7.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_0
        '
        Me.Output_CheckBox_0.AutoSize = True
        Me.Output_CheckBox_0.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_0.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_0.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Output_CheckBox_0.Location = New System.Drawing.Point(127, 73)
        Me.Output_CheckBox_0.Name = "Output_CheckBox_0"
        Me.Output_CheckBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_0.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_0.TabIndex = 25
        Me.Output_CheckBox_0.Text = "OUT 00"
        Me.Output_CheckBox_0.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_1
        '
        Me.Output_CheckBox_1.AutoSize = True
        Me.Output_CheckBox_1.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_1.Location = New System.Drawing.Point(127, 93)
        Me.Output_CheckBox_1.Name = "Output_CheckBox_1"
        Me.Output_CheckBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_1.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_1.TabIndex = 24
        Me.Output_CheckBox_1.Text = "OUT 01"
        Me.Output_CheckBox_1.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_2
        '
        Me.Output_CheckBox_2.AutoSize = True
        Me.Output_CheckBox_2.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_2.Location = New System.Drawing.Point(127, 113)
        Me.Output_CheckBox_2.Name = "Output_CheckBox_2"
        Me.Output_CheckBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_2.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_2.TabIndex = 23
        Me.Output_CheckBox_2.Text = "OUT 02"
        Me.Output_CheckBox_2.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_3
        '
        Me.Output_CheckBox_3.AutoSize = True
        Me.Output_CheckBox_3.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_3.Location = New System.Drawing.Point(127, 133)
        Me.Output_CheckBox_3.Name = "Output_CheckBox_3"
        Me.Output_CheckBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_3.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_3.TabIndex = 22
        Me.Output_CheckBox_3.Text = "OUT 03"
        Me.Output_CheckBox_3.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_4
        '
        Me.Output_CheckBox_4.AutoSize = True
        Me.Output_CheckBox_4.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_4.Location = New System.Drawing.Point(127, 153)
        Me.Output_CheckBox_4.Name = "Output_CheckBox_4"
        Me.Output_CheckBox_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_4.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_4.TabIndex = 21
        Me.Output_CheckBox_4.Text = "OUT 04"
        Me.Output_CheckBox_4.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_5
        '
        Me.Output_CheckBox_5.AutoSize = True
        Me.Output_CheckBox_5.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_5.Location = New System.Drawing.Point(127, 173)
        Me.Output_CheckBox_5.Name = "Output_CheckBox_5"
        Me.Output_CheckBox_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_5.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_5.TabIndex = 20
        Me.Output_CheckBox_5.Text = "OUT 05"
        Me.Output_CheckBox_5.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_6
        '
        Me.Output_CheckBox_6.AutoSize = True
        Me.Output_CheckBox_6.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_6.Location = New System.Drawing.Point(127, 193)
        Me.Output_CheckBox_6.Name = "Output_CheckBox_6"
        Me.Output_CheckBox_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_6.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_6.TabIndex = 19
        Me.Output_CheckBox_6.Text = "OUT 06"
        Me.Output_CheckBox_6.UseVisualStyleBackColor = False
        '
        'Output_CheckBox_7
        '
        Me.Output_CheckBox_7.AutoSize = True
        Me.Output_CheckBox_7.BackColor = System.Drawing.SystemColors.Control
        Me.Output_CheckBox_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Output_CheckBox_7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output_CheckBox_7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Output_CheckBox_7.Location = New System.Drawing.Point(127, 213)
        Me.Output_CheckBox_7.Name = "Output_CheckBox_7"
        Me.Output_CheckBox_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Output_CheckBox_7.Size = New System.Drawing.Size(72, 20)
        Me.Output_CheckBox_7.TabIndex = 18
        Me.Output_CheckBox_7.Text = "OUT 07"
        Me.Output_CheckBox_7.UseVisualStyleBackColor = False
        '
        'Debounce_ComboBox
        '
        Me.Debounce_ComboBox.BackColor = System.Drawing.SystemColors.Window
        Me.Debounce_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Debounce_ComboBox.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Debounce_ComboBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Debounce_ComboBox.Items.AddRange(New Object() {"No Debounce", "100 Hz", "200 Hz", "1 KHz"})
        Me.Debounce_ComboBox.Location = New System.Drawing.Point(104, 22)
        Me.Debounce_ComboBox.Name = "Debounce_ComboBox"
        Me.Debounce_ComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Debounce_ComboBox.Size = New System.Drawing.Size(121, 23)
        Me.Debounce_ComboBox.TabIndex = 17
        '
        'OutputPolarity_CheckBox_0
        '
        Me.OutputPolarity_CheckBox_0.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_0.Location = New System.Drawing.Point(213, 75)
        Me.OutputPolarity_CheckBox_0.Name = "OutputPolarity_CheckBox_0"
        Me.OutputPolarity_CheckBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_0.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_0.TabIndex = 16
        Me.OutputPolarity_CheckBox_0.Text = "Check2"
        Me.OutputPolarity_CheckBox_0.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_1
        '
        Me.OutputPolarity_CheckBox_1.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_1.Location = New System.Drawing.Point(213, 95)
        Me.OutputPolarity_CheckBox_1.Name = "OutputPolarity_CheckBox_1"
        Me.OutputPolarity_CheckBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_1.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_1.TabIndex = 15
        Me.OutputPolarity_CheckBox_1.Text = "Check2"
        Me.OutputPolarity_CheckBox_1.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_2
        '
        Me.OutputPolarity_CheckBox_2.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_2.Location = New System.Drawing.Point(213, 115)
        Me.OutputPolarity_CheckBox_2.Name = "OutputPolarity_CheckBox_2"
        Me.OutputPolarity_CheckBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_2.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_2.TabIndex = 14
        Me.OutputPolarity_CheckBox_2.Text = "Check2"
        Me.OutputPolarity_CheckBox_2.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_3
        '
        Me.OutputPolarity_CheckBox_3.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_3.Location = New System.Drawing.Point(213, 135)
        Me.OutputPolarity_CheckBox_3.Name = "OutputPolarity_CheckBox_3"
        Me.OutputPolarity_CheckBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_3.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_3.TabIndex = 13
        Me.OutputPolarity_CheckBox_3.Text = "Check2"
        Me.OutputPolarity_CheckBox_3.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_4
        '
        Me.OutputPolarity_CheckBox_4.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_4.Location = New System.Drawing.Point(213, 155)
        Me.OutputPolarity_CheckBox_4.Name = "OutputPolarity_CheckBox_4"
        Me.OutputPolarity_CheckBox_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_4.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_4.TabIndex = 12
        Me.OutputPolarity_CheckBox_4.Text = "Check2"
        Me.OutputPolarity_CheckBox_4.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_5
        '
        Me.OutputPolarity_CheckBox_5.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_5.Location = New System.Drawing.Point(213, 175)
        Me.OutputPolarity_CheckBox_5.Name = "OutputPolarity_CheckBox_5"
        Me.OutputPolarity_CheckBox_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_5.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_5.TabIndex = 11
        Me.OutputPolarity_CheckBox_5.Text = "Check2"
        Me.OutputPolarity_CheckBox_5.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_6
        '
        Me.OutputPolarity_CheckBox_6.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_6.Location = New System.Drawing.Point(213, 195)
        Me.OutputPolarity_CheckBox_6.Name = "OutputPolarity_CheckBox_6"
        Me.OutputPolarity_CheckBox_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_6.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_6.TabIndex = 10
        Me.OutputPolarity_CheckBox_6.Text = "Check2"
        Me.OutputPolarity_CheckBox_6.UseVisualStyleBackColor = False
        '
        'OutputPolarity_CheckBox_7
        '
        Me.OutputPolarity_CheckBox_7.BackColor = System.Drawing.SystemColors.Control
        Me.OutputPolarity_CheckBox_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.OutputPolarity_CheckBox_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OutputPolarity_CheckBox_7.Location = New System.Drawing.Point(213, 215)
        Me.OutputPolarity_CheckBox_7.Name = "OutputPolarity_CheckBox_7"
        Me.OutputPolarity_CheckBox_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OutputPolarity_CheckBox_7.Size = New System.Drawing.Size(17, 17)
        Me.OutputPolarity_CheckBox_7.TabIndex = 9
        Me.OutputPolarity_CheckBox_7.Text = "Check2"
        Me.OutputPolarity_CheckBox_7.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_0
        '
        Me.InputPolarity_CheckBox_0.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_0.Location = New System.Drawing.Point(87, 75)
        Me.InputPolarity_CheckBox_0.Name = "InputPolarity_CheckBox_0"
        Me.InputPolarity_CheckBox_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_0.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_0.TabIndex = 8
        Me.InputPolarity_CheckBox_0.Text = "Check1"
        Me.InputPolarity_CheckBox_0.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_1
        '
        Me.InputPolarity_CheckBox_1.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_1.Location = New System.Drawing.Point(87, 95)
        Me.InputPolarity_CheckBox_1.Name = "InputPolarity_CheckBox_1"
        Me.InputPolarity_CheckBox_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_1.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_1.TabIndex = 7
        Me.InputPolarity_CheckBox_1.Text = "Check1"
        Me.InputPolarity_CheckBox_1.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_2
        '
        Me.InputPolarity_CheckBox_2.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_2.Location = New System.Drawing.Point(87, 115)
        Me.InputPolarity_CheckBox_2.Name = "InputPolarity_CheckBox_2"
        Me.InputPolarity_CheckBox_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_2.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_2.TabIndex = 6
        Me.InputPolarity_CheckBox_2.Text = "Check1"
        Me.InputPolarity_CheckBox_2.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_3
        '
        Me.InputPolarity_CheckBox_3.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_3.Location = New System.Drawing.Point(87, 135)
        Me.InputPolarity_CheckBox_3.Name = "InputPolarity_CheckBox_3"
        Me.InputPolarity_CheckBox_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_3.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_3.TabIndex = 5
        Me.InputPolarity_CheckBox_3.Text = "Check1"
        Me.InputPolarity_CheckBox_3.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_4
        '
        Me.InputPolarity_CheckBox_4.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_4.Location = New System.Drawing.Point(87, 155)
        Me.InputPolarity_CheckBox_4.Name = "InputPolarity_CheckBox_4"
        Me.InputPolarity_CheckBox_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_4.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_4.TabIndex = 4
        Me.InputPolarity_CheckBox_4.Text = "Check1"
        Me.InputPolarity_CheckBox_4.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_5
        '
        Me.InputPolarity_CheckBox_5.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_5.Location = New System.Drawing.Point(87, 175)
        Me.InputPolarity_CheckBox_5.Name = "InputPolarity_CheckBox_5"
        Me.InputPolarity_CheckBox_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_5.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_5.TabIndex = 3
        Me.InputPolarity_CheckBox_5.Text = "Check1"
        Me.InputPolarity_CheckBox_5.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_6
        '
        Me.InputPolarity_CheckBox_6.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_6.Location = New System.Drawing.Point(87, 195)
        Me.InputPolarity_CheckBox_6.Name = "InputPolarity_CheckBox_6"
        Me.InputPolarity_CheckBox_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_6.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_6.TabIndex = 2
        Me.InputPolarity_CheckBox_6.Text = "Check1"
        Me.InputPolarity_CheckBox_6.UseVisualStyleBackColor = False
        '
        'InputPolarity_CheckBox_7
        '
        Me.InputPolarity_CheckBox_7.BackColor = System.Drawing.SystemColors.Control
        Me.InputPolarity_CheckBox_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.InputPolarity_CheckBox_7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InputPolarity_CheckBox_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputPolarity_CheckBox_7.Location = New System.Drawing.Point(87, 215)
        Me.InputPolarity_CheckBox_7.Name = "InputPolarity_CheckBox_7"
        Me.InputPolarity_CheckBox_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputPolarity_CheckBox_7.Size = New System.Drawing.Size(17, 17)
        Me.InputPolarity_CheckBox_7.TabIndex = 1
        Me.InputPolarity_CheckBox_7.Text = "Check1"
        Me.InputPolarity_CheckBox_7.UseVisualStyleBackColor = False
        '
        'Debounce_Label
        '
        Me.Debounce_Label.AutoSize = True
        Me.Debounce_Label.BackColor = System.Drawing.SystemColors.Control
        Me.Debounce_Label.Cursor = System.Windows.Forms.Cursors.Default
        Me.Debounce_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Debounce_Label.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Debounce_Label.Location = New System.Drawing.Point(24, 25)
        Me.Debounce_Label.Name = "Debounce_Label"
        Me.Debounce_Label.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Debounce_Label.Size = New System.Drawing.Size(76, 16)
        Me.Debounce_Label.TabIndex = 36
        Me.Debounce_Label.Text = "Debounce:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(69, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(57, 16)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Polarity"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(194, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(57, 16)
        Me.Label5.TabIndex = 34
        Me.Label5.Text = "Polarity"
        '
        'ExitDoor_Button
        '
        Me.ExitDoor_Button.BackColor = System.Drawing.SystemColors.Control
        Me.ExitDoor_Button.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitDoor_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExitDoor_Button.Image = CType(resources.GetObject("ExitDoor_Button.Image"), System.Drawing.Image)
        Me.ExitDoor_Button.Location = New System.Drawing.Point(225, 260)
        Me.ExitDoor_Button.Name = "ExitDoor_Button"
        Me.ExitDoor_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExitDoor_Button.Size = New System.Drawing.Size(53, 34)
        Me.ExitDoor_Button.TabIndex = 57
        Me.ExitDoor_Button.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ExitDoor_Button.UseVisualStyleBackColor = False
        '
        'ShowIoIrqMask_Button
        '
        Me.ShowIoIrqMask_Button.BackColor = System.Drawing.SystemColors.Control
        Me.ShowIoIrqMask_Button.Cursor = System.Windows.Forms.Cursors.Default
        Me.ShowIoIrqMask_Button.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowIoIrqMask_Button.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ShowIoIrqMask_Button.Location = New System.Drawing.Point(118, 263)
        Me.ShowIoIrqMask_Button.Name = "ShowIoIrqMask_Button"
        Me.ShowIoIrqMask_Button.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowIoIrqMask_Button.Size = New System.Drawing.Size(41, 26)
        Me.ShowIoIrqMask_Button.TabIndex = 131
        Me.ShowIoIrqMask_Button.Text = "IRQ"
        Me.ShowIoIrqMask_Button.UseVisualStyleBackColor = False
        '
        'IO_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 300)
        Me.Controls.Add(Me.ShowIoIrqMask_Button)
        Me.Controls.Add(Me.ExitDoor_Button)
        Me.Controls.Add(Me.Auto_Button)
        Me.Controls.Add(Me.DIO_GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IO_Form"
        Me.Text = "IO_Form"
        Me.DIO_GroupBox.ResumeLayout(False)
        Me.DIO_GroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents Auto_Button As System.Windows.Forms.CheckBox
    Public WithEvents DIO_GroupBox As System.Windows.Forms.GroupBox
    Public WithEvents Input_CheckBox_0 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_1 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_2 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_3 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_4 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_5 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_6 As System.Windows.Forms.CheckBox
    Public WithEvents Input_CheckBox_7 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_0 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_1 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_2 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_3 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_4 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_5 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_6 As System.Windows.Forms.CheckBox
    Public WithEvents Output_CheckBox_7 As System.Windows.Forms.CheckBox
    Public WithEvents Debounce_ComboBox As System.Windows.Forms.ComboBox
    Public WithEvents OutputPolarity_CheckBox_0 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_1 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_2 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_3 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_4 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_5 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_6 As System.Windows.Forms.CheckBox
    Public WithEvents OutputPolarity_CheckBox_7 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_0 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_1 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_2 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_3 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_4 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_5 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_6 As System.Windows.Forms.CheckBox
    Public WithEvents InputPolarity_CheckBox_7 As System.Windows.Forms.CheckBox
    Public WithEvents Debounce_Label As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents ExitDoor_Button As System.Windows.Forms.Button
    Public WithEvents ShowIoIrqMask_Button As System.Windows.Forms.Button
End Class
