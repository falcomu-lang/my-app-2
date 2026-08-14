Friend Class Interrupt_Form
    Dim IoMask(8) As CheckBox
    Dim TimerCounterMask(5) As CheckBox
    Sub InitialObject()
        IoMask(0) = InputIrqMask_CheckBox_0
        IoMask(1) = InputIrqMask_CheckBox_1
        IoMask(2) = InputIrqMask_CheckBox_2
        IoMask(3) = InputIrqMask_CheckBox_3
        IoMask(4) = InputIrqMask_CheckBox_4
        IoMask(5) = InputIrqMask_CheckBox_5
        IoMask(6) = InputIrqMask_CheckBox_6
        IoMask(7) = InputIrqMask_CheckBox_7
        TimerCounterMask(0) = FifoThresholdEmptyIrqMask_CheckBox
        TimerCounterMask(1) = FifoFullIrqMask_CheckBox
        TimerCounterMask(2) = FifoEmptyIrqMask_CheckBox
        TimerCounterMask(3) = CompareIrqMask_CheckBox
        TimerCounterMask(4) = TimerIrqMask_CheckBox
    End Sub
    Public Sub ReadIrqState()
        Dim mask As Byte
        Dim i As Byte
        Status = LSI8181_IRQ_status_read(CardID, IO, mask)
        For i = 0 To 7
            IrqStatus_CheckedListBox.SetItemCheckState(i, mask >> i And 1)
        Next
        Status = LSI8181_IRQ_status_read(CardID, TIMER_COUNTER, mask)
        For i = 8 To 12
            IrqStatus_CheckedListBox.SetItemCheckState(i, mask >> i And 1)
        Next
        IrqCounter_label.Text = "IRQ Counter : " + Str(irq_count)
    End Sub
    Sub ReadIrqMask()
        Dim mask As Byte
        Dim i As Byte
        Status = LSI8181_IRQ_mask_read(CardID, IO, mask)
        For i = 0 To 7
            IoMask(i).CheckState = mask >> i And 1
        Next
        Status = LSI8181_IRQ_mask_read(CardID, TIMER_COUNTER, mask)
        For i = 0 To 4
            TimerCounterMask(i).CheckState = mask >> i And 1
        Next
    End Sub

    Private Sub Interrupt_Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        IsOpenInterruptFormShow = False
    End Sub

    Private Sub Interrupt_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitialObject()
        ReadIrqMask()
        IsOpenInterruptFormShow = True
    End Sub

    Private Sub Apple_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Apple_Button.Click
        Dim data As Integer
        Dim j As Integer
        Dim status As Integer
        Dim mask As Byte

        For j = 0 To 7
            If IoMask(j).Checked = True Then
                mask = 1
            Else
                mask = 0
            End If
            data = mask * (2 ^ j) + data
        Next j
        'Set IO mask
        status = LSI8181_IRQ_mask_set(CardID, IO, data)
        data = 0
        For j = 0 To 4
            If TimerCounterMask(j).Checked = True Then
                mask = 1
            Else
                mask = 0
            End If
            data = mask * (2 ^ j) + data
        Next j
        'Set IO mask
        status = LSI8181_IRQ_mask_set(CardID, TIMER_COUNTER, data)
    End Sub
    Public callback As New IRQ_PROCESS_Delegate(AddressOf IRQ_testprocess)
    Private Sub IrqEnable_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IrqEnable_CheckBox.CheckedChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim hevent As Long
        If IrqEnable_CheckBox.Checked = True Then
            LSI8181_IRQ_enable(CardID, hevent)
            LSI8181_IRQ_process_link(CardID, callback)
        Else
            LSI8181_IRQ_disable(CardID)
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub ClearIrqCounter_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearIrqCounter_Button.Click
        irq_count = 0
    End Sub
    Public Sub BackColorDefine()
        FifoThresholdEmptyIrqMask_CheckBox.BackColor = Control.DefaultBackColor
        FifoFullIrqMask_CheckBox.BackColor = Control.DefaultBackColor
        FifoEmptyIrqMask_CheckBox.BackColor = Control.DefaultBackColor
        CompareIrqMask_CheckBox.BackColor = Control.DefaultBackColor
        TimerIrqMask_CheckBox.BackColor = Control.DefaultBackColor
        IoMask(0).BackColor = Control.DefaultBackColor
        IoMask(1).BackColor = Control.DefaultBackColor
        IoMask(2).BackColor = Control.DefaultBackColor
        IoMask(3).BackColor = Control.DefaultBackColor
        IoMask(4).BackColor = Control.DefaultBackColor
        IoMask(5).BackColor = Control.DefaultBackColor
        IoMask(6).BackColor = Control.DefaultBackColor
        IoMask(7).BackColor = Control.DefaultBackColor

    End Sub

    Public Sub ApplySavedSettingsToCard()
        InitialObject()
        Apple_Button_Click(Me, EventArgs.Empty)
        If IrqEnable_CheckBox.Checked = True Then
            Dim hevent As Long
            LSI8181_IRQ_enable(CardID, hevent)
            LSI8181_IRQ_process_link(CardID, callback)
        Else
            LSI8181_IRQ_disable(CardID)
        End If
    End Sub
End Class