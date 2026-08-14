Public Class Security_Form
    Dim Old_Security(0 To 4) As TextBox
    Dim New_Security(0 To 4) As TextBox
    Dim GroupBox(0 To 1) As GroupBox
    Dim Lock_ico(0 To 2) As PictureBox
    Dim Open_SEC As Button
    Dim Set_SEC As Button
    Dim Change_SEC As Button
    Dim Disable_SEC As Button
    Dim Status_lab As Label

    Dim NewPW(0 To 4) As UInt16
    Dim OldPW(0 To 4) As UInt16
    Dim i As Integer
    Dim Enable_SEC As Integer
    Dim Status
    Dim MSGSTR, EStr As String
    Private Sub Security_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        NewPW.Initialize()
        OldPW.Initialize()

        Read_AIO_Security_Status()
    End Sub
    Sub Security_Initial()
        Dim i As Byte
        Open_SEC = btn_Open_SEC
        Set_SEC = btn_Set_SEC
        Change_SEC = btn_Change_SEC
        Disable_SEC = btn_Disable_SEC

        GroupBox(0) = GroupBox1
        GroupBox(1) = GroupBox2
        Status_lab = lab_Status
        For i = 0 To 4
            Old_Security(i) = GroupBox(0).Controls("txt_OldSecurity" + i.ToString())
            New_Security(i) = GroupBox(1).Controls("txt_NewSecurity" + i.ToString())
        Next
    End Sub

    Private Sub btn_set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Set_SEC.Click
        Dim MSGSTR As String

        If (New_Security(0).Text = "" Or New_Security(1).Text = "" Or New_Security(2).Text = "" Or New_Security(3).Text = "" Or New_Security(4).Text = "") Then
            MsgBox("Please input new security(Data0 ~ Data4)!")
        Else
            MSGSTR = CheckNewData
            If MSGSTR <> "" Then
                MsgBox(MSGSTR)
            Else
                PassWordTransfer()
                Status = LSI8181_password_set(CardID, NewPW(0))
                If Status <> 0 Then
                    MsgBox("Error  #" + Str(Status))
                End If
                clear_SEC_data()
                Read_AIO_Security_Status()
            End If
        End If
    End Sub

    Private Function CheckNewData() As String
        Dim EStr As String
        Dim i As Byte
        EStr = ""
        For i = 0 To 4
            If Val(New_Security(i).Text) < 0 Or Val(New_Security(i).Text) > 65535 Then
                EStr = EStr + "Data" + Str(i) + ","
            End If
        Next i

        If EStr <> "" Then
            CheckNewData = "New_security Data error (" + EStr + ")"
        Else
            CheckNewData = ""
        End If

    End Function

    Private Sub PassWordTransfer()
        For i = 0 To 4
            NewPW(i) = 0
            'If Val(New_Security(i).Text) > 32767 Then
            'NewPW(i) = Val(New_Security(i).Text) - 65536
            'Else
            NewPW(i) = Val(New_Security(i).Text)
            'End If

            'If Val(Old_Security(i)) > 32767 Then
            'OldPW(i) = Val(Old_Security(i).Text) - 65536
            'Else
            OldPW(i) = Val(Old_Security(i).Text)
            'End If
        Next i

    End Sub
    Private Sub clear_SEC_data()
        For i = 0 To 4
            Old_Security(i).Text = ""
            New_Security(i).Text = ""
        Next i
    End Sub

    Public Sub Read_AIO_Security_Status()
        Dim Sopen, Sena As Byte
        Security_Initial()
        Lock_ico(0) = ptb_Lock_ico0
        Lock_ico(1) = ptb_Lock_ico1
        Lock_ico(2) = ptb_Lock_ico2
        Lock_ico(0).Visible = False
        Lock_ico(1).Visible = False
        Lock_ico(2).Visible = False
        Main_Form.Lock_PictureBox_0.Visible = False
        Main_Form.Lock_PictureBox_1.Visible = False
        Main_Form.Lock_PictureBox_2.Visible = False

        '       mLock_ico.Visible = False

        Status = LSI8181_security_status_read(CardID, Sopen, Sena)
        For i = 0 To 4
            Old_Security(i).Visible = True
            New_Security(i).Visible = True
        Next i
        If Status <> 0 Then
            MsgBox("Error  #" + Str(Status))
        ElseIf Sena Then
            Open_SEC.Visible = True
            Default_Button.Visible = True
            Set_SEC.Visible = False
            Change_SEC.Visible = True
            Disable_SEC.Visible = True

            Select Case Sopen
                Case LOCK_RELEASE
                    Lock_ico(1).Visible = True  'show open.ico
                    '                 mLock_ico.Visible = True  'show open.ico
                    '                  mLock_ico.Image = Main.ImageList1.Images(2)
                    Status_lab.Text = "Security open"
                    Main_Form.Lock_PictureBox_1.Visible = True

                Case LOCKED
                    Lock_ico(0).Visible = True  'show close.ico
                    '                  mLock_ico.Visible = True  'show open.ico
                    '                  mLock_ico.Image = Main.ImageList1.Images(3)

                    Status_lab.Text = "Security close"
                    Main_Form.Lock_PictureBox_0.Visible = True
                Case CARD_LOCKED_OVER
                    Lock_ico(2).Visible = True  'show lock.ico
                    '                mLock_ico.Visible = True  'show open.ico
                    '                mLock_ico.Image = Main.ImageList1.Images(4)
                    Main_Form.Lock_PictureBox_2.Visible = True
            End Select
        Else
            Status_lab.Text = "Security disable"
            Open_SEC.Visible = False
            Default_Button.Visible = False
            Set_SEC.Visible = True
            Change_SEC.Visible = False
            Disable_SEC.Visible = False

            For i = 0 To 4
                Old_Security(i).Visible = False
                New_Security(i).Visible = True
            Next i

        End If

    End Sub

    Private Sub btn_Open_SEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open_SEC.Click
        If (Old_Security(0).Text = "" Or Old_Security(1).Text = "" Or Old_Security(2).Text = "" Or Old_Security(3).Text = "" Or Old_Security(4).Text = "") Then
            MsgBox("Please input old security(Data0 ~ Data4)!")
        Else
            MSGSTR = CheckOldData()
            If MSGSTR <> "" Then
                MsgBox(MSGSTR)
            Else
                PassWordTransfer()
                Status = LSI8181_security_unlock(CardID, OldPW(0))
                If Status <> 0 Then
                    MsgBox("Error  #" + Str(Status))
                End If
                Read_AIO_Security_Status()
                clear_SEC_data()
            End If
        End If
    End Sub
    Private Function CheckOldData() As String
        Dim EStr As String
        EStr = ""
        For i = 0 To 4
            If Val(Old_Security(i).Text) < 0 Or Val(Old_Security(i).Text) > 65535 Then
                EStr = EStr + "Data" + Str(i) + ","
            End If
        Next i

        If EStr <> "" Then
            CheckOldData = "Old_security Data error (" + EStr + ")"
        Else
            CheckOldData = ""
        End If

    End Function

    Private Sub btn_Change_SEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Change_SEC.Click
        If (Old_Security(0).Text = "" Or Old_Security(1).Text = "" Or Old_Security(2).Text = "" Or Old_Security(3).Text = "" Or Old_Security(4).Text = "") Then
            MsgBox("Please input old security(Data0 ~ Data4)!")
        Else
            If (New_Security(0).Text = "" Or New_Security(1).Text = "" Or New_Security(2).Text = "" Or New_Security(3).Text = "" Or New_Security(4).Text = "") Then
                MsgBox("Please input new security(Data0 ~ Data4)!")
            Else
                MSGSTR = CheckOldData()
                If MSGSTR <> "" Then
                    MsgBox(MSGSTR)
                Else
                    MSGSTR = CheckNewData()
                    If MSGSTR <> "" Then
                        MsgBox(MSGSTR)
                    Else
                        PassWordTransfer()
                        Status = LSI8181_password_change(CardID, OldPW(0), NewPW(0))
                        If Status <> 0 Then
                            MsgBox("Error  #" + Str(Status))
                        End If
                        Read_AIO_Security_Status()
                        clear_SEC_data()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btn_Disable_SEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Disable_SEC.Click
        If (Old_Security(0).Text = "" Or Old_Security(1).Text = "" Or Old_Security(2).Text = "" Or Old_Security(3).Text = "" Or Old_Security(4).Text = "") Then
            MsgBox("Please input old security(Data0 ~ Data4)!")
        Else

            MSGSTR = CheckOldData()
            If MSGSTR <> "" Then
                MsgBox(MSGSTR)
            Else
                PassWordTransfer()
                Status = LSI8181_password_clear(CardID, OldPW(0))
                Read_AIO_Security_Status()
                clear_SEC_data()
            End If

        End If
    End Sub

    Private Sub btn_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_exit.Click
        Me.Close()
    End Sub

    Private Sub Default_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Default_Button.Click
        Status = LSI8181_password_set_default(CardID)
        Read_AIO_Security_Status()
    End Sub
End Class