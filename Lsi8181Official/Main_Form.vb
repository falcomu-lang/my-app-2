Public Class Main_Form
    Private _cardInitialized As Boolean
    Private _allowClose As Boolean
    Public CardName(15) As Byte   'LSI8181 or LSI8181A

    Private Sub CompareToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputToolStripMenuItem.Click
        Compare_Form.Hide()
        Compare_Form.Show()
    End Sub
    Private Sub InputToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputToolStripMenuItem.Click
        IO_Form.Hide()
        IO_Form.Show()
    End Sub
    Private Sub SegmentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SegmentToolStripMenuItem.Click
        Segment_Form.Hide()
        Segment_Form.Show()
    End Sub

    Private Sub ExtensionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionToolStripMenuItem.Click
        Extension_Form.Hide()
        Extension_Form.Show()
    End Sub

    Private Sub TimerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerToolStripMenuItem.Click
        Timer_Form.Hide()
        Timer_Form.Show()
    End Sub

    Private Sub InterruptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterruptToolStripMenuItem.Click
        Interrupt_Form.Hide()
        Interrupt_Form.Show()
    End Sub
    Private Sub Main_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitializeCardAndRestoreSettings()
    End Sub

    Public Sub InitializeCardAndRestoreSettings()
        If _cardInitialized Then
            Return
        End If

        Check_Card()
        AttachOfficialSettings()
        ApplyOfficialSettingsToCard()
        _cardInitialized = True
    End Sub

    Public Sub CloseCardAndAllowClose()
        _allowClose = True
        LSI8181_close()
        Close()
    End Sub

    Private Sub AttachOfficialSettings()
        OfficialSettingsStore.Attach(Me)
        OfficialSettingsStore.Attach(Compare_Form)
        OfficialSettingsStore.Attach(IO_Form)
        OfficialSettingsStore.Attach(Segment_Form)
        OfficialSettingsStore.Attach(Extension_Form)
        OfficialSettingsStore.Attach(Timer_Form)
        OfficialSettingsStore.Attach(Interrupt_Form)
    End Sub
    Private Sub ApplyOfficialSettingsToCard()
        If ID_ComboBox.Items.Count = 0 Then
            Return
        End If

        ApplyPageSettings("Compare", Sub() Compare_Form.ApplySavedSettingsToCard())
        ApplyPageSettings("IO", Sub() IO_Form.ApplySavedSettingsToCard())
        ApplyPageSettings("Segment", Sub() Segment_Form.ApplySavedSettingsToCard())
        ApplyPageSettings("Extension", Sub() Extension_Form.ApplySavedSettingsToCard())
        ApplyPageSettings("Timer", Sub() Timer_Form.ApplySavedSettingsToCard())
        ApplyPageSettings("Interrupt", Sub() Interrupt_Form.ApplySavedSettingsToCard())
    End Sub

    Private Sub ApplyPageSettings(ByVal pageName As String, ByVal applyAction As Action)
        Try
            applyAction()
        Catch ex As Exception
            MsgBox("Apply saved LSI8181 " & pageName & " settings failed: " & ex.Message)
        End Try
    End Sub

    Private Sub FileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileToolStripMenuItem.Click
        cloes_function()
    End Sub
    Sub cloes_function()
        Hide()
    End Sub
    Private Sub ExitDoor_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitDoor_Button.Click
        cloes_function()
    End Sub
    Private Sub Check_Card()
        Dim i As Integer
        Dim lngaddress As UInt64
        Dim tc_Address As UInt64

        Status = LSI8181_initial
        If Status <> 0 Then
            MsgBox("LSI8181 Error   (Code#" + Str(Status) + ")")
            Call cloes_function()
        End If

        For i = 0 To LSI8181_CARD_ID_MAX
            Status = LSI8181_info(i, lngaddress, tc_Address)
            If Status = DRV_NO_ERROR Then

                ID_ComboBox.Items.Add(Str(i))
                ID_ComboBox.SelectedIndex = 0
                Address_Label.Text = "Address : " & Hex(lngaddress) & "-" & Hex(tc_Address) & "(H)"
                read_parameter()
            End If
        Next i
    End Sub
    Sub read_parameter()
        'Read current counter
        Status = LSI8181_counter_read(Val(ID_ComboBox.Text), CurrentCounter_Label.Text)
        'Read compare value
        Status = LSI8181_compare_value_read(Val(ID_ComboBox.Text), CompareValue_Label.Text)

        Dim index As Byte
        ''Read counter mode status
        'Status = LSI8181_counter_mode_read(Val(ID_ComboBox.Text), mode)
        ''compare counter mode
        'For index = 0 To 3
        '    If ((mode >> index) And 1) = 0 Then
        '        CompareStatus_CheckedListBox.SetItemChecked(index, False)
        '    Else
        '        CompareStatus_CheckedListBox.SetItemChecked(index, True)
        '    End If
        'Next

        Dim cio_state As Byte
        'Read encoder status
        Status = LSI8181_CIO_read(Val(ID_ComboBox.Text), cio_state)
        'Read A , B , Z phase
        For index = 0 To 2
            If ((cio_state >> index) And 1) = 0 Then
                InputStatus_CheckedListBox.SetItemChecked(index, False)
            Else
                InputStatus_CheckedListBox.SetItemChecked(index, True)
            End If
        Next
        'Read Home In
        If ((cio_state >> 3) And 1) = 0 Then
            IoStatus_CheckedListBox.SetItemChecked(0, False)
        Else
            IoStatus_CheckedListBox.SetItemChecked(0, True)
        End If
        'Read Clear In
        If ((cio_state >> 4) And 1) = 0 Then
            IoStatus_CheckedListBox.SetItemChecked(1, False)
        Else
            IoStatus_CheckedListBox.SetItemChecked(1, True)
        End If
        'Read Z phase toggle flag
        If ((cio_state >> 5) And 1) = 0 Then
            InputStatus_CheckedListBox.SetItemChecked(3, False)
        Else
            InputStatus_CheckedListBox.SetItemChecked(3, True)
        End If

        Dim unused_count As UInteger
        'Read FIFO unused
        Status = LSI8181_compare_FIFO_unused_read(Val(ID_ComboBox.Text), unused_count)
        FifoUnusedNunber_Label.Text = unused_count

        Dim compare_out As Byte
        'Read CMP OUT statue
        Status = LSI8181_CO_read(Val(ID_ComboBox.Text), compare_out)
        If (compare_out = 1) Then
            IoStatus_CheckedListBox.SetItemChecked(3, True)
            'chang compare Form compare out state
            'Compare_Form.CompareOut_CheckBox.Checked = True
        Else
            IoStatus_CheckedListBox.SetItemChecked(3, False)
            'chang compare Form compare out state
            'Compare_Form.CompareOut_CheckBox.Checked = False
        End If
        Dim intInState As Integer
        'Read Gate status(IN00)
        Status = LSI8181_point_read(Val(ID_ComboBox.Text), I_PORT0, 0, intInState)
        If ((intInState And 1) = 1) Then
            IoStatus_CheckedListBox.SetItemChecked(2, True)
        Else
            IoStatus_CheckedListBox.SetItemChecked(2, False)
        End If


        Dim count As UInteger
        Dim single_cont As Byte
        Dim homing_mode As Byte
        'Read homing mode
        Status = LSI8181_HOMING_mode_read(Val(ID_ComboBox.Text), homing_mode, count, single_cont)
        homing_mode_Label.Text = Compare_Form.HomingMode_ComboBox.Items.Item(homing_mode).ToString()
        'HomingMode_ComboBox.SelectedIndex = mode
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        read_parameter()
        Compare_Form.CompareAction()
        Timer_Form.Read_timer()
        If IsOpenIoFormShow = True Then IO_Form.ReadIoState()
        If IsOpenInterruptFormShow = True Then Interrupt_Form.ReadIrqState()
        If IsIoAutoRun = True Then IO_Form.IoAutoRun()
    End Sub

    Private Sub ID_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_ComboBox.SelectedIndexChanged
        CardID = Val(ID_ComboBox.Text)
    End Sub


    Private Sub Main_Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _allowClose Then
            Return
        End If

        e.Cancel = True
        Hide()
    End Sub
    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click
        System.Diagnostics.Process.Start("..\..\..\..\..\API\sw8181.pdf")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        About_Form.Show()
    End Sub
End Class
