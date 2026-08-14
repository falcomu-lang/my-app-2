Public Class Segment_Form
    Private _initialized As Boolean
    Dim s_control(3) As ComboBox
    Dim s_start(3) As TextBox
    Dim s_stop(3) As TextBox
    Private Sub Segment_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initial()
        Read_parameter()
    End Sub
    Sub Initial()
        If _initialized Then Return

        s_control(0) = Control_ComboBox_0
        s_control(1) = Control_ComboBox_1
        s_control(2) = Control_ComboBox_2

        s_start(0) = Start_TextBox_0
        s_start(1) = Start_TextBox_1
        s_start(2) = Start_TextBox_2

        s_stop(0) = Stop_TextBox_0
        s_stop(1) = Stop_TextBox_1
        s_stop(2) = Stop_TextBox_2

        _initialized = True
    End Sub
    Sub Apply()
        Dim i As Byte

        For i = 0 To 2
            Status = LSI8181_segment_control_write(CardID, i, GetSelectedIndex(s_control(i)))
            Status = LSI8181_cmp_segment_write(CardID, i, Val(s_start(i).Text), Val(s_stop(i).Text))
        Next
    End Sub
    Sub Read_parameter()
        Dim value As Byte
        Dim start_32 As Integer
        Dim stop_32 As Integer
        Dim i As Byte

        For i = 0 To 2
            Status = LSI8181_segment_control_read(CardID, i, value)
            s_control(i).SelectedIndex = value
            Status = LSI8181_cmp_segment_read(CardID, i, start_32, stop_32)
            s_start(i).Text = start_32
            s_stop(i).Text = stop_32
        Next
        Status = LSI8181_mask_off_read(CardID, value)
        Mask_ComboBox.SelectedIndex = value
    End Sub

    Private Sub Apply_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Apply_Button.Click
        Apply()
    End Sub

    Private Sub Ok_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ok_Button.Click
        Apply()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Mask_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mask_ComboBox.SelectedIndexChanged
        If OfficialSettingsStore.IsRestoring Then Return
        Dim mode As Byte = GetSelectedIndex(Mask_ComboBox)
        Status = LSI8181_mask_off_write(CardID, mode)
    End Sub

    Public Sub ApplySavedSettingsToCard()
        Initial()
        Apply()
        Status = LSI8181_mask_off_write(CardID, GetSelectedIndex(Mask_ComboBox))
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
End Class
