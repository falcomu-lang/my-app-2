Public Class Compare_Form
    Public Sub ReadCompareMode()
        Dim mode As Byte
        'Read compare mode
        Status = LSI8181_compare_mode_read(Val(Main_Form.ID_ComboBox.Text), mode)
        SetSelectedIndex(CompareMode_ComboBox, mode)

        Dim debounce As Byte
        Dim multiple As Byte
        'Read compare input parameter
        Status = LSI8181_CI_mode_read(Val(Main_Form.ID_ComboBox.Text), mode, debounce, multiple)
        SetSelectedIndex(CompareInput_ComboBox, mode)
        SetSelectedIndex(CompareDebounce_ComboBox, debounce)
        SetSelectedIndex(MultipleRate_ComboBox, multiple)

        Dim count As UInt16
        Dim single_cont As Byte
        'Read homing mode
        Status = LSI8181_HOMING_mode_read(Val(Main_Form.ID_ComboBox.Text), mode, count, single_cont)
        SetSelectedIndex(HomingMode_ComboBox, mode)
        If single_cont = 1 Then
            ContinuousMode_CheckBox.Checked = True
        Else
            ContinuousMode_CheckBox.Checked = False
        End If
        ContinuousCount_TextBox.Text = count

        Dim polarity As Byte
        Dim width As UInt16
        'Read CMP Out mode ,polarity state ,duty cycyle
        Status = LSI8181_compare_CMP_OUT_read(Val(Main_Form.ID_ComboBox.Text), polarity, mode, width)
        SetSelectedIndex(CompareOutput_ComboBox, mode)
        DutyCycle_TextBox.Text = width
        If polarity = 1 Then
            CompareOutPolarity_CheckBox.Checked = True
        Else
            CompareOutPolarity_CheckBox.Checked = False
        End If

        Dim gate As Byte
        'Read Gate Status    
        Status = LSI8181_CO_mode_read(Val(Main_Form.ID_ComboBox.Text), mode, gate, width)
        CompareGate_CheckBox.Checked = (gate <> 0)

        'Read Gate polarity
        Status = LSI8181_port_polarity_read(Val(Main_Form.ID_ComboBox.Text), 0, polarity)
        polarity = polarity And 1
        If polarity = 1 Then
            CompareGatePolarity_CheckBox.Checked = True
        Else
            CompareGatePolarity_CheckBox.Checked = False
        End If

        Dim value As Integer
        'Read auto increment
        Status = LSI8181_compare_increment_read(Val(Main_Form.ID_ComboBox.Text), value)
        AutoIncrement_TextBox.Text = value
        'Read current counter
        Status = LSI8181_counter_read(Val(Main_Form.ID_ComboBox.Text), value)
        PresetValue_TextBox.Text = value
        'Read compare counter
        Status = LSI8181_compare_value_read(Val(Main_Form.ID_ComboBox.Text), value)
        CompareValue_TextBox.Text = value
        'Read thresholdValue
        Dim thresholdValue As UInt16
        Status = LSI8181_compare_FIFO_threshold_read(Val(Main_Form.ID_ComboBox.Text), thresholdValue)
        ThresholdValue_TextBox.Text = thresholdValue
    End Sub

    Public Sub CompareAction()
        'Compare quadrature mode
        If CompareInput_ComboBox.SelectedIndex = 0 Then
            MultipleRate_ComboBox.Enabled = True
        Else
            MultipleRate_ComboBox.Enabled = False
        End If
        'Compare pulse ,duty cycle
        UpdateDutyCycleEnabled()
        'Compare homing Home Z and Z
        If HomingMode_ComboBox.SelectedIndex = 6 Or HomingMode_ComboBox.SelectedIndex = 7 Then
            ContinuousCount_TextBox.Enabled = True
        Else
            ContinuousCount_TextBox.Enabled = False
        End If
        'read counter mode , select Compare mode
        Dim mode As Byte
        Status = LSI8181_counter_mode_read(Val(Main_Form.ID_ComboBox.Text), mode)
        If mode = 0 Then
            'set counter enable
            Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 1)
        ElseIf mode = 9 Then
            If CompareMode_ComboBox.SelectedIndex = 0 Then
                SingleEnable_Button.Enabled = True
            Else
                SingleEnable_Button.Enabled = False
            End If
            If CompareMode_ComboBox.SelectedIndex = 1 Then
                FIOF_GroupBox.Enabled = True
                FifoEnable_Button.Enabled = True
            Else
                FIOF_GroupBox.Enabled = False
                FifoEnable_Button.Enabled = False
            End If
            If CompareMode_ComboBox.SelectedIndex = 2 Then
                AutoIncrement_GroupBox.Enabled = True
                Increment_Button.Enabled = True
            Else
                AutoIncrement_GroupBox.Enabled = False
                Increment_Button.Enabled = False
            End If
            CompareDisable_Button.Enabled = False
        ElseIf mode = 13 Or mode = 11 Then
            CompareDisable_Button.Enabled = True
            Increment_Button.Enabled = False
            FifoEnable_Button.Enabled = False
        End If

    End Sub

    Private Sub AutoIncrement_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoIncrement_Button.Click
        'Set auto increment value
        Status = LSI8181_compare_increment_set(Val(Main_Form.ID_ComboBox.Text), Val(AutoIncrement_TextBox.Text))
    End Sub

    Private Sub AutoIncrementClear_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoIncrementClean_Button.Click
        AutoIncrement_TextBox.Text = 0
        'clean auto increment
        Status = LSI8181_compare_increment_set(Val(Main_Form.ID_ComboBox.Text), Val(AutoIncrement_TextBox.Text))
    End Sub

    Private Sub CompareDisable_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareDisable_Button.Click
        'Start counter , stop compare
        Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 1)
        If Status <> 0 Then
            MsgBox(Status.ToString)
        End If
        CompareDisable_Button.Enabled = False
    End Sub

    Private Sub Compare_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PositionMode_ComboBox.SelectedIndex = 0
        OfficialSettingsStore.RunWithoutSaving(Sub() ReadCompareMode())
        OfficialSettingsStore.Restore(Me)
        RestoreCompareOutCheckedState()
        UpdateDutyCycleEnabled()
    End Sub

    Private Sub ExitDoor_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitDoor_Button.Click
        Me.Close()
    End Sub

    Private Sub PresetValue_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PresetValue_Button.Click
        Dim value As Integer = PresetValue_TextBox.Text
        'Set current counter (Encoder data)
        Status = LSI8181_counter_set(Val(Main_Form.ID_ComboBox.Text), value)
    End Sub

    Private Sub PresetValueClear_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PresetValueClean_Button.Click
        'Clear current counter (Encoder data)
        Status = LSI8181_counter_set(Val(Main_Form.ID_ComboBox.Text), 0)
        PresetValue_TextBox.Text = 0
    End Sub

    Private Sub CompareValue_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareValue_TextBox.Click, CompareValue_Button.Click
        'Set compare counter
        Dim value As Integer = CompareValue_TextBox.Text
        Status = LSI8181_compare_value_set(Val(Main_Form.ID_ComboBox.Text), value)
    End Sub

    Private Sub CompareValueClear_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareValueClean_Button.Click
        'Clear compare counter
        Status = LSI8181_compare_value_set(Val(Main_Form.ID_ComboBox.Text), 0)
        CompareValue_TextBox.Text = 0
    End Sub

    Private Sub SingleEnable_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SingleEnable_Button.Click
        OfficialSettingsStore.SetValue("Compare.LastEnableMode", "Single")
        ApplySingleEnable()
    End Sub

    Private Sub ApplySingleEnable()
        'Enable single mode
        Status = LSI8181_compare_mode_set(Val(Main_Form.ID_ComboBox.Text), 0)
        'Start compare
        Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 2)
        SingleEnable_Button.Enabled = False
    End Sub

    Private Sub FifoEnable_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FifoEnable_Button.Click
        OfficialSettingsStore.SetValue("Compare.LastEnableMode", "Fifo")
        ApplyFifoEnable()
    End Sub

    Private Sub ApplyFifoEnable()
        'Enable FIFO mode
        Status = LSI8181_compare_mode_set(Val(Main_Form.ID_ComboBox.Text), 1)
        'Start compare
        Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 2)
        If Status <> 0 Then
            MsgBox("LSI8181 Error   (Code#" + Str(Status) + ")")
        End If
        FifoEnable_Button.Enabled = False
    End Sub

    Private Sub Increment_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Increment_Button.Click
        OfficialSettingsStore.SetValue("Compare.LastEnableMode", "Increment")
        ApplyIncrementEnable()
    End Sub

    Private Sub ApplyIncrementEnable()
        'Enable single mode
        Status = LSI8181_compare_mode_set(Val(Main_Form.ID_ComboBox.Text), 2)
        'Start compare
        Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 2)
        Increment_Button.Enabled = False
    End Sub
    Private Sub CompareOutSet_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareOutSet_Button.Click
        ApplyCompareOutSettings()
    End Sub

    Private Sub ApplyCompareOutSettings()

        Dim CMP_polarity As Byte
        ForcePulseOutputForAutoIncrement()
        Dim mode As Byte = GetSelectedIndex(CompareOutput_ComboBox)
        Dim dutyCycle As UInt16 = GetUInt16Text(DutyCycle_TextBox)
        If CompareOutPolarity_CheckBox.Checked = True Then
            CMP_polarity = 1
        Else
            CMP_polarity = 0
        End If
        'Set compare output parameter
        Status = LSI8181_compare_CMP_OUT_set(Val(Main_Form.ID_ComboBox.Text), CMP_polarity, mode, dutyCycle)

        'Dim preset As Integer
        'If CompareOut_CheckBox.Checked = True Then
        '    preset = 1
        'Else
        '    preset = 0
        'End If
        ''Set CMP OUT point hight or low
        'Status = LSI8181_toggle_preset(Val(Main_Form.ID_ComboBox.Text), preset)
    End Sub

    Private Sub CompareInput_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareInput_Button.Click
        ApplyCompareInputSettings()
    End Sub

    Private Sub ApplyCompareInputSettings()
        Dim mode As Byte = GetSelectedIndex(CompareInput_ComboBox)
        Dim debounce_time As Byte = GetSelectedIndex(CompareDebounce_ComboBox)
        Dim multiple_rate As Byte = GetSelectedIndex(MultipleRate_ComboBox)
        'Set compare input mode
        Status = LSI8181_CI_mode_set(Val(Main_Form.ID_ComboBox.Text), mode, debounce_time, multiple_rate)
    End Sub

    Private Sub Homing_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Homing_Button.Click
        ApplyHomingSettings()
    End Sub

    Private Sub ApplyHomingSettings()
        Dim mode As Byte = GetSelectedIndex(HomingMode_ComboBox)
        Dim count As UInt16 = GetUInt16Text(ContinuousCount_TextBox)
        Dim continuousMode As Byte
        If ContinuousMode_CheckBox.Checked = True Then
            continuousMode = 1
        Else
            continuousMode = 0
        End If
        'Set homing mode
        Status = LSI8181_HOMING_mode_set(Val(Main_Form.ID_ComboBox.Text), mode, count, continuousMode)

    End Sub

    Private Sub FifoDataApply_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FifoDataApply_Button.Click
        ApplyFifoDataSettings()
    End Sub

    Private Sub ApplyFifoDataSettings()
        Dim positioneMode As Byte = GetSelectedIndex(PositionMode_ComboBox)
        Dim FifoData(0 To 1023) As Int32
        FifoData(0) = Val(FifoData_TextBox.Text)
        Status = LSI8181_compare_FIFO_threshold_set(Val(Main_Form.ID_ComboBox.Text), GetFifoThreshold())

        If Status > 0 Then
            MsgBox("error:" + Status.ToString())
        Else
            'Set FIFO one data
            Status = LSI8181_compare_FIFO_set(Val(Main_Form.ID_ComboBox.Text), FifoData, positioneMode, 1)

        End If
    End Sub

    Private Sub FifoDataClear_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FifoDataClean_Button.Click
        OfficialSettingsStore.SetValue("Compare.LastEnableMode", "Disabled")
        'clean FIFO data
        Status = LSI8181_compare_FIFO_clear(Val(Main_Form.ID_ComboBox.Text))
    End Sub


    Public Sub ApplySavedSettingsToCard()
        'Follow the official Compare Operation screen from upper-left downward, then enable last.
        Status = LSI8181_compare_increment_set(Val(Main_Form.ID_ComboBox.Text), Val(AutoIncrement_TextBox.Text))
        ApplyCompareInputSettings()
        ApplyHomingSettings()
        ApplyCompareOutSettings()
        Status = LSI8181_compare_value_set(Val(Main_Form.ID_ComboBox.Text), Val(CompareValue_TextBox.Text))

        RestoreCompareOutCheckedState()
        If CompareOut_CheckBox.Checked = True Then
            Status = LSI8181_toggle_preset(Val(Main_Form.ID_ComboBox.Text), 1)
        Else
            Status = LSI8181_toggle_preset(Val(Main_Form.ID_ComboBox.Text), 0)
        End If

        Dim lastEnableMode As String = OfficialSettingsStore.GetValue("Compare.LastEnableMode", "")
        If lastEnableMode = "Single" Then
            ApplySingleEnable()
        ElseIf lastEnableMode = "Fifo" Then
            ApplyFifoDataSettings()
            ApplyFifoEnable()
        ElseIf lastEnableMode = "Increment" Then
            ApplyIncrementEnable()
        ElseIf lastEnableMode = "Disabled" Then
            Status = LSI8181_counter_start(Val(Main_Form.ID_ComboBox.Text), 1)
        End If
    End Sub

    Private Function GetSelectedIndex(ByVal comboBox As ComboBox) As Byte
        If comboBox.SelectedIndex < 0 Then
            If comboBox.Items.Count > 0 Then
                comboBox.SelectedIndex = 0
            End If

            Return 0
        End If

        Return CByte(comboBox.SelectedIndex)
    End Function

    Private Sub SetSelectedIndex(ByVal comboBox As ComboBox, ByVal selectedIndex As Integer)
        If selectedIndex < 0 OrElse selectedIndex >= comboBox.Items.Count Then
            If comboBox.Items.Count > 0 Then
                comboBox.SelectedIndex = 0
            End If

            Return
        End If

        comboBox.SelectedIndex = selectedIndex
    End Sub

    Private Function GetUInt16Text(ByVal textBox As TextBox) As UInt16
        Dim value As UInt32
        If Not UInt32.TryParse(textBox.Text, value) Then
            Return 0
        End If

        If value > UInt16.MaxValue Then
            Return UInt16.MaxValue
        End If

        Return CUShort(value)
    End Function

    Private Function GetFifoThreshold() As UInt16
        Dim value As UInt16 = GetUInt16Text(ThresholdValue_TextBox)
        If value = 0 Then
            value = 1
            ThresholdValue_TextBox.Text = value.ToString()
        End If

        Return value
    End Function

    Private Sub RestoreCompareOutCheckedState()
        Dim savedValue As String = OfficialSettingsStore.GetValue("Compare.CompareOutChecked", OfficialSettingsStore.GetValue("Compare_Form.CompareOut_CheckBox.Checked", ""))
        Dim checked As Boolean
        If Boolean.TryParse(savedValue, checked) Then
            CompareOut_CheckBox.Checked = checked
        End If
    End Sub
    Private Sub CompareInput_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareInput_ComboBox.SelectedIndexChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Select Case CompareInput_ComboBox.SelectedIndex
            Case 0
                ToolTip1.SetToolTip(CompareInput_ComboBox, "0: QUADRATURE_MODE A, B phase quadrature signal at A and B input pin")
                MultipleRate_ComboBox.Enabled = True
            Case 1
                ToolTip1.SetToolTip(CompareInput_ComboBox, "1: DUAL_PULSE_MODE CW and CCW signal at A and B input pin")
                MultipleRate_ComboBox.Enabled = False
            Case 2
                ToolTip1.SetToolTip(CompareInput_ComboBox, "2: SINGLE_PULSE_MODE Clock and Direction signal at A and B input pin")
                MultipleRate_ComboBox.Enabled = False
        End Select
    End Sub
    Private Sub CompareMode_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareMode_ComboBox.SelectedIndexChanged
        If OfficialSettingsStore.IsRestoring Then Return
        If CompareMode_ComboBox.SelectedIndex = 0 Then
            SingleEnable_Button.Enabled = True
        Else
            SingleEnable_Button.Enabled = False
        End If
        If CompareMode_ComboBox.SelectedIndex = 1 Then
            FIOF_GroupBox.Enabled = True
            FifoEnable_Button.Enabled = True
        Else
            FIOF_GroupBox.Enabled = False
            FifoEnable_Button.Enabled = False
        End If
        If CompareMode_ComboBox.SelectedIndex = 2 Then
            AutoIncrement_GroupBox.Enabled = True
            Increment_Button.Enabled = True
            ForcePulseOutputForAutoIncrement()
        Else
            AutoIncrement_GroupBox.Enabled = False
            Increment_Button.Enabled = False
        End If

        UpdateDutyCycleEnabled()
    End Sub

    Private Sub CompareOut_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareOut_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        OfficialSettingsStore.SetValue("Compare.CompareOutChecked", CompareOut_CheckBox.Checked.ToString())

        Dim preset As Integer
        If CompareOut_CheckBox.Checked = True Then
            preset = 1
        Else
            preset = 0
        End If
        'Set CMP OUT point hight or low
        Status = LSI8181_toggle_preset(Val(Main_Form.ID_ComboBox.Text), preset)
    End Sub

    Private Sub CompareOutput_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareOutput_ComboBox.SelectedIndexChanged
        If OfficialSettingsStore.IsRestoring Then Return
        UpdateDutyCycleEnabled()
    End Sub

    Private Sub UpdateDutyCycleEnabled()
        DutyCycle_TextBox.Enabled = (CompareOutput_ComboBox.SelectedIndex = 1 OrElse CompareMode_ComboBox.SelectedIndex = 2)
    End Sub

    Private Sub ForcePulseOutputForAutoIncrement()
        If CompareMode_ComboBox.SelectedIndex = 2 AndAlso CompareOutput_ComboBox.Items.Count > 1 AndAlso CompareOutput_ComboBox.SelectedIndex <> 1 Then
            CompareOutput_ComboBox.SelectedIndex = 1
        End If
    End Sub

    Private Sub APhasePolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APhasePolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If APhasePolarity_CheckBox.Checked = True Then
            U16ChageBitX(A_phase, 1, polarity)
        Else
            U16ChageBitX(A_phase, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub

    Private Sub BPhasePolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPhasePolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If BPhasePolarity_CheckBox.Checked = True Then
            U16ChageBitX(B_phase, 1, polarity)
        Else
            U16ChageBitX(B_phase, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub

    Private Sub ZPhasePolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZPhasePolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If ZPhasePolarity_CheckBox.Checked = True Then
            U16ChageBitX(Z_phase, 1, polarity)
        Else
            U16ChageBitX(Z_phase, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub
    Sub U16ChageBitX(ByVal bit As Byte, ByVal state As Byte, ByRef data As UInt16)
        If ((data >> bit) And 1) = state Then
        Else
            If state = 1 Then
                data = data + (1 << bit)
            Else
                data = data - (1 << bit)
            End If
        End If
    End Sub

    Private Sub HomePolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomePolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If HomePolarity_CheckBox.Checked = True Then
            U16ChageBitX(HOME, 1, polarity)
        Else
            U16ChageBitX(HOME, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub

    Private Sub ClrPolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClrPolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If ClrPolarity_CheckBox.Checked = True Then
            U16ChageBitX(CLEAR_IN, 1, polarity)
        Else
            U16ChageBitX(CLEAR_IN, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub

    Private Sub CompareOutPolarity_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareOutPolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim polarity As UInt16
        Status = LSI8181_CIO_polarity_read(Val(Main_Form.ID_ComboBox.Text), polarity)
        If CompareOutPolarity_CheckBox.Checked = True Then
            U16ChageBitX(CMP_OUT, 1, polarity)
        Else
            U16ChageBitX(CMP_OUT, 0, polarity)
        End If
        Status = LSI8181_CIO_polarity_set(Val(Main_Form.ID_ComboBox.Text), polarity)
    End Sub

    Private Sub CompareGate_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareGate_CheckBox.CheckedChanged, CompareGatePolarity_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim Gate_polarity As Byte
        If CompareGatePolarity_CheckBox.Checked = True Then
            Gate_polarity = 1
        Else
            Gate_polarity = 0
        End If
        If CompareGate_CheckBox.Checked = True Then
            'Set Gate , Gate polarity
            Status = LSI8181_compare_GATE_enable(Val(Main_Form.ID_ComboBox.Text), Gate_polarity)
        Else
            Status = LSI8181_compare_GATE_enable(Val(Main_Form.ID_ComboBox.Text), Gate_polarity)
            Status = LSI8181_compare_GATE_disable(Val(Main_Form.ID_ComboBox.Text))
        End If
    End Sub

    Private Sub FifoDataFileSet_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FifoDataFileSet_Button.Click

        'set button color
        FifoDataApply_Button.BackColor = Control.DefaultBackColor
        'set fifo data
        Dim FifoData(0 To 1023) As Int32

        ' show openFileDialog type , txt
        Dim FileNum As Integer
        Dim strTemp As String
        Dim openFileDialog1 As New OpenFileDialog()
        Dim fileNumber As UInt16

        'open fifo data file , set fifo data
        openFileDialog1.Title = "Select fifo data file (*.txt)"
        openFileDialog1.Filter = "text(*.txt)|*.*"
        openFileDialog1.FileName = "*.txt"
        If openFileDialog1.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        Else
            FileNum = FreeFile()
            FileOpen(FileNum, openFileDialog1.FileName, OpenMode.Input)
            fileNumber = 0
            Do Until EOF(FileNum)
                strTemp = LineInput(FileNum)
                FifoData(fileNumber) = Val(strTemp)
                fileNumber = fileNumber + 1
            Loop
            FileClose(FileNum)
        End If
        'if direction or no direction mode
        Status = LSI8181_compare_FIFO_set(CardID, FifoData, Val(PositionMode_ComboBox.SelectedIndex), fileNumber)

        'set fifo threshold value
        Dim threshold_value As Integer = Val(ThresholdValue_TextBox.Text)
        Status = LSI8181_compare_FIFO_threshold_set(CardID, threshold_value)
    End Sub

    Private Sub ShowCompareOutIrq_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCompareOutIrq_Button.Click
        Interrupt_Form.Hide()
        Interrupt_Form.Show()
        Interrupt_Form.BackColorDefine()
        Interrupt_Form.CompareIrqMask_CheckBox.BackColor = Color.SpringGreen
    End Sub

    Private Sub ShowFifoIrqMask_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowFifoIrqMask_Button.Click
        Interrupt_Form.Hide()
        Interrupt_Form.Show()
        Interrupt_Form.BackColorDefine()
        Interrupt_Form.FifoThresholdEmptyIrqMask_CheckBox.BackColor = Color.SpringGreen
        Interrupt_Form.FifoFullIrqMask_CheckBox.BackColor = Color.SpringGreen
        Interrupt_Form.FifoEmptyIrqMask_CheckBox.BackColor = Color.SpringGreen
    End Sub

    Private Sub DutyCycle_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DutyCycle_TextBox.TextChanged
        Try
            out_width_Label.Text = "out width:   " + (Int32.Parse(DutyCycle_TextBox.Text.ToString) + 1).ToString + " us"
        Catch ex As Exception

        End Try
    End Sub

End Class
