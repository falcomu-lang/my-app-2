Public Class Extension_Form
    Dim Offset_Text(8) As TextBox
    Dim Pulse_Text(8) As TextBox
    Dim State_Check(8) As CheckBox
    Dim Mask_Check(8) As CheckBox
    Dim Status_Check(8) As CheckBox
    Sub initial()
        Offset_Text(0) = OffsetCompare_TextBox_0
        Offset_Text(1) = OffsetCompare_TextBox_1
        Offset_Text(2) = OffsetCompare_TextBox_2
        Offset_Text(3) = OffsetCompare_TextBox_3
        Offset_Text(4) = OffsetCompare_TextBox_4
        Offset_Text(5) = OffsetCompare_TextBox_5
        Offset_Text(6) = OffsetCompare_TextBox_6
        Offset_Text(7) = OffsetCompare_TextBox_7

        Pulse_Text(0) = PulseWidth_TextBox_0
        Pulse_Text(1) = PulseWidth_TextBox_1
        Pulse_Text(2) = PulseWidth_TextBox_2
        Pulse_Text(3) = PulseWidth_TextBox_3
        Pulse_Text(4) = PulseWidth_TextBox_4
        Pulse_Text(5) = PulseWidth_TextBox_5
        Pulse_Text(6) = PulseWidth_TextBox_6
        Pulse_Text(7) = PulseWidth_TextBox_7

        State_Check(0) = State_CheckBox_0
        State_Check(1) = State_CheckBox_1
        State_Check(2) = State_CheckBox_2
        State_Check(3) = State_CheckBox_3
        State_Check(4) = State_CheckBox_4
        State_Check(5) = State_CheckBox_5
        State_Check(6) = State_CheckBox_6
        State_Check(7) = State_CheckBox_7

        Status_Check(0) = Status_CheckBox_0
        Status_Check(1) = Status_CheckBox_1
        Status_Check(2) = Status_CheckBox_2
        Status_Check(3) = Status_CheckBox_3
        Status_Check(4) = Status_CheckBox_4
        Status_Check(5) = Status_CheckBox_5
        Status_Check(6) = Status_CheckBox_6
        Status_Check(7) = Status_CheckBox_7

        Mask_Check(0) = Mask_CheckBox_0
        Mask_Check(1) = Mask_CheckBox_1
        Mask_Check(2) = Mask_CheckBox_2
        Mask_Check(3) = Mask_CheckBox_3
        Mask_Check(4) = Mask_CheckBox_4
        Mask_Check(5) = Mask_CheckBox_5
        Mask_Check(6) = Mask_CheckBox_6
        Mask_Check(7) = Mask_CheckBox_7
    End Sub
    Private Sub Apply_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Apply_Button.Click
        Call apply()
    End Sub
    Sub apply()
        Dim i As Byte
        Dim mask As Byte
        Dim state As Byte
        Dim offset As Integer
        Dim out_width As Integer
        Dim mask_temp As Byte

        For i = 0 To 7
            If (Offset_Text(i).Text > 32767) Then
                offset = Offset_Text(i).Text - 65536
            Else
                offset = Offset_Text(i).Text
            End If
            Status = LSI8181_compare_offset_set(CardID, i, offset)

            out_width = Pulse_Text(i).Text
            Status = LSI8181_compare_offset_out_width_set(CardID, i, out_width)
            If State_Check(i).Checked = True Then
                state = 1
            Else
                state = 0
            End If
            Status = LSI8181_compare_offset_output_point_set(CardID, i, state)
            If Mask_Check(i).Checked = True Then
                mask_temp = 1
            Else
                mask_temp = 0
            End If
            mask = mask + mask_temp * (2 ^ i)
        Next i
        Status = LSI8181_compare_offset_mask_set(CardID, mask)
    End Sub
    Sub Parameter_read_formload()
        Dim i As Byte
        Dim mask As Byte
        Dim state As Byte
        Dim offset As Int16
        Dim out_width As UInt16
        'Read offset mask data
        Status = LSI8181_compare_offset_mask_read(CardID, mask)

        For i = 0 To 7
            'Mask_Check(i).value = (mask And (2 ^ i)) / (2 ^ i)
            If (mask And (2 ^ i)) / (2 ^ i) = 1 Then
                Mask_Check(i).Checked = True
            Else
                Mask_Check(i).Checked = False
            End If
            Status = LSI8181_compare_offset_read(CardID, i, offset)
            Offset_Text(i).Text = offset.ToString()

            Status = LSI8181_compare_offset_out_width_read(CardID, i, out_width)
            Pulse_Text(i).Text = out_width
            Status = LSI8181_compare_offset_output_point_read(CardID, i, state)
            'State_Check(i).value = state
            If state = 1 Then
                State_Check(i).Checked = True
            Else
                State_Check(i).Checked = False
            End If
        Next i
    End Sub

    Sub Parameter_read()
        Dim i As Byte
        Dim state As Byte
        For i = 0 To 7
            Status = LSI8181_compare_offset_output_point_read(CardID, i, state)
            'State_Check(i).value = state
            If state = 1 Then
                Status_Check(i).Checked = True
            Else
                Status_Check(i).Checked = False
            End If
        Next i
    End Sub
    Private Sub Extension_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initial()
        Parameter_read_formload()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Ok_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ok_Button.Click
        Call apply()
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim i As Byte
        Parameter_read()
        For i = 0 To 7
            If Mask_Check(i).Checked = True Then
                State_Check(i).Checked = False
                State_Check(i).Enabled = False
            Else
                State_Check(i).Enabled = True
            End If
        Next i
    End Sub

    Public Sub ApplySavedSettingsToCard()
        initial()
        apply()
    End Sub
End Class