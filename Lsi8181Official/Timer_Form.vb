Public Class Timer_Form

    Private Sub CurrentValue_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentValue_Button.Click
        'Set timer data
        Status = LSI8181_timer_set(CardID, Val(CurrentValue_TextBox.Text))
    End Sub

    Private Sub CurrentValueClean_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentValueClean_Button.Click
        OfficialSettingsStore.SetValue("Timer.LastState", "Stopped")
        'clean timer data and stop timer
        Status = LSI8181_timer_set(CardID, 0)
        Status = LSI8181_timer_stop(CardID)
    End Sub

    Private Sub Start_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Start_Button.Click
        OfficialSettingsStore.SetValue("Timer.LastState", "Started")
        'start timer
        Status = LSI8181_timer_start(CardID)
    End Sub

    Private Sub Stop_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Stop_Button.Click
        OfficialSettingsStore.SetValue("Timer.LastState", "Stopped")
        'stop timer
        Status = LSI8181_timer_stop(CardID)
    End Sub
    Sub Read_timer()
        Dim data As Integer
        Status = LSI8181_TC_read(CardID, 2, data)
        CurrentCounter_Label.Text = data
    End Sub

    Private Sub Exit_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Exit_CheckBox.CheckedChanged
        Me.Close()
    End Sub

    Private Sub CurrentValue_TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentValue_TextBox.TextChanged
        'period_str = Format(((Val(CurrentValue_TextBox.Text) + 1) * 0.000001), "0.0000")
        CurrentPeriod_Label.Text = "T=" + Format(((Val(CurrentValue_TextBox.Text) + 1) * 0.000001), "0.0000") + "S"
    End Sub

    Private Sub Timer_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim data As Integer
        Status = LSI8181_TC_read(CardID, 1, data)
        CurrentValue_TextBox.Text = data
    End Sub

    Private Sub ShowTimerIrqMask_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowTimerIrqMask_Button.Click
        Interrupt_Form.Hide()
        Interrupt_Form.Show()
        Interrupt_Form.BackColorDefine()
        Interrupt_Form.TimerIrqMask_CheckBox.BackColor = Color.SpringGreen
    End Sub

    Public Sub ApplySavedSettingsToCard()
        Status = LSI8181_timer_set(CardID, Val(CurrentValue_TextBox.Text))
        Dim lastState As String = OfficialSettingsStore.GetValue("Timer.LastState", "")
        If lastState = "Started" Then
            Status = LSI8181_timer_start(CardID)
        ElseIf lastState = "Stopped" Then
            Status = LSI8181_timer_stop(CardID)
        End If
    End Sub
End Class