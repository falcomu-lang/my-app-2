Public Class IO_Form
    Dim InputPolarity(8) As CheckBox
    Dim OutputPolarity(8) As CheckBox
    Dim Input(8) As CheckBox
    Dim Output(8) As CheckBox
    Dim AutoNumber As Byte = 0
    Sub initial()
        InputPolarity(0) = InputPolarity_CheckBox_0
        InputPolarity(1) = InputPolarity_CheckBox_1
        InputPolarity(2) = InputPolarity_CheckBox_2
        InputPolarity(3) = InputPolarity_CheckBox_3
        InputPolarity(4) = InputPolarity_CheckBox_4
        InputPolarity(5) = InputPolarity_CheckBox_5
        InputPolarity(6) = InputPolarity_CheckBox_6
        InputPolarity(7) = InputPolarity_CheckBox_7

        OutputPolarity(0) = OutputPolarity_CheckBox_0
        OutputPolarity(1) = OutputPolarity_CheckBox_1
        OutputPolarity(2) = OutputPolarity_CheckBox_2
        OutputPolarity(3) = OutputPolarity_CheckBox_3
        OutputPolarity(4) = OutputPolarity_CheckBox_4
        OutputPolarity(5) = OutputPolarity_CheckBox_5
        OutputPolarity(6) = OutputPolarity_CheckBox_6
        OutputPolarity(7) = OutputPolarity_CheckBox_7

        Input(0) = Input_CheckBox_0
        Input(1) = Input_CheckBox_1
        Input(2) = Input_CheckBox_2
        Input(3) = Input_CheckBox_3
        Input(4) = Input_CheckBox_4
        Input(5) = Input_CheckBox_5
        Input(6) = Input_CheckBox_6
        Input(7) = Input_CheckBox_7

        Output(0) = Output_CheckBox_0
        Output(1) = Output_CheckBox_1
        Output(2) = Output_CheckBox_2
        Output(3) = Output_CheckBox_3
        Output(4) = Output_CheckBox_4
        Output(5) = Output_CheckBox_5
        Output(6) = Output_CheckBox_6
        Output(7) = Output_CheckBox_7
    End Sub

    Private Sub SetInputPolarity(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputPolarity_CheckBox_0.CheckedChanged, InputPolarity_CheckBox_7.CheckedChanged, InputPolarity_CheckBox_6.CheckedChanged, InputPolarity_CheckBox_5.CheckedChanged, InputPolarity_CheckBox_4.CheckedChanged, InputPolarity_CheckBox_3.CheckedChanged, InputPolarity_CheckBox_2.CheckedChanged, InputPolarity_CheckBox_1.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim data As Integer
        Dim j As Integer
        Dim status As Integer
        Dim polarity As Byte

        For j = 0 To 7
            If InputPolarity(j).Checked = True Then
                polarity = 1
            Else
                polarity = 0
            End If
            data = polarity * (2 ^ j) + data
        Next j
        'Set input polarity
        status = LSI8181_port_polarity_set(CardID, I_PORT0, data)
        'Call error_code(status)

    End Sub

    Private Sub SetOutputPolarity(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputPolarity_CheckBox_0.CheckedChanged, OutputPolarity_CheckBox_7.CheckedChanged, OutputPolarity_CheckBox_6.CheckedChanged, OutputPolarity_CheckBox_5.CheckedChanged, OutputPolarity_CheckBox_4.CheckedChanged, OutputPolarity_CheckBox_3.CheckedChanged, OutputPolarity_CheckBox_2.CheckedChanged, OutputPolarity_CheckBox_1.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim data As Integer
        Dim j As Integer
        Dim status As Integer
        Dim polarity As Byte

        For j = 0 To 7
            If OutputPolarity(j).Checked = True Then
                polarity = 1
            Else
                polarity = 0
            End If
            data = polarity * (2 ^ j) + data
        Next j
        'Set output polarity
        status = LSI8181_port_polarity_set(CardID, O_PORT0, data)
        'Call error_code(status)
    End Sub

    Private Sub SetOutput(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Output_CheckBox_0.CheckedChanged, Output_CheckBox_7.CheckedChanged, Output_CheckBox_6.CheckedChanged, Output_CheckBox_5.CheckedChanged, Output_CheckBox_4.CheckedChanged, Output_CheckBox_3.CheckedChanged, Output_CheckBox_2.CheckedChanged, Output_CheckBox_1.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim point As Integer
        Dim status As Integer
        Dim state As Byte
        point = Byte.Parse(Microsoft.VisualBasic.Right(DirectCast((sender), CheckBox).Name.ToString(), 1))

        If Output(point).Checked = True Then
            state = 1
        Else
            state = 0
        End If
        'Set output
        status = LSI8181_point_set(CardID, O_PORT0, point, state)
    End Sub

    Private Sub IO_Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        IsOpenIoFormShow = False
    End Sub
    Private Sub IO_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim mode As Byte
        initial()
        ReadIoPolarity()
        'read debounce mode
        Status = LSI8181_debounce_time_read(CardID, mode)
        Debounce_ComboBox.SelectedIndex = mode
        'open flag is true
        IsOpenIoFormShow = True
    End Sub
    Public Sub ReadIoState()
        Dim point As Byte
        Dim state As Byte
        For point = 0 To 7
            Status = LSI8181_point_read(CardID, I_PORT0, point, state)
            Input(point).CheckState = state
            Status = LSI8181_point_read(CardID, O_PORT0, point, state)
            Output(point).CheckState = state
        Next
    End Sub
    Sub ReadIoPolarity()
        Dim point As Byte
        Dim state As Byte
        Status = LSI8181_port_polarity_read(CardID, I_PORT0, state)
        For point = 0 To 7
            InputPolarity(point).CheckState = (state >> point) And 1
        Next
        Status = LSI8181_port_polarity_read(CardID, O_PORT0, state)
        For point = 0 To 7
            OutputPolarity(point).CheckState = (state >> point) And 1
        Next
    End Sub

    Private Sub Debounce_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Debounce_ComboBox.SelectedIndexChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Status = LSI8181_debounce_time_set(CardID, Debounce_ComboBox.SelectedIndex)
    End Sub

    Private Sub ExitDoor_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitDoor_Button.Click
        Me.Close()
    End Sub

    Private Sub ShowIoIrqMask_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowIoIrqMask_Button.Click
        Interrupt_Form.Hide()
        Interrupt_Form.Show()
        Interrupt_Form.BackColorDefine()
        Interrupt_Form.InputIrqMask_CheckBox_0.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_1.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_2.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_3.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_4.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_5.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_6.BackColor = Color.SpringGreen
        Interrupt_Form.InputIrqMask_CheckBox_7.BackColor = Color.SpringGreen
    End Sub
    Public Sub IoAutoRun()
        Status = LSI8181_port_set(CardID, O_PORT0, 0)
        Status = LSI8181_point_set(CardID, O_PORT0, AutoNumber, 1)
        If AutoNumber = 7 Then
            AutoNumber = 0
        Else
            AutoNumber = AutoNumber + 1
        End If
    End Sub
    Private Sub Auto_Button_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Auto_Button.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        If Auto_Button.Checked = True Then
            IsIoAutoRun = True
        Else
            IsIoAutoRun = False
        End If
    End Sub

    Public Sub ApplySavedSettingsToCard()
        initial()
        SetInputPolarity(Me, EventArgs.Empty)
        SetOutputPolarity(Me, EventArgs.Empty)
        For point As Byte = 0 To 7
            Dim state As Byte
            If Output(point).Checked = True Then
                state = 1
            Else
                state = 0
            End If
            Status = LSI8181_point_set(CardID, O_PORT0, point, state)
        Next
        Status = LSI8181_debounce_time_set(CardID, Debounce_ComboBox.SelectedIndex)
        If Auto_Button.Checked = True Then
            IsIoAutoRun = True
        Else
            IsIoAutoRun = False
        End If
    End Sub
End Class