Module LSI_global

    Public Function show_error(ByVal Status As UInt32)
        If Status <> 0 Then
            MsgBox("LSI8181 Error   (Code#" + Str(Status) + ")")
        End If
        Return 0
    End Function
End Module
