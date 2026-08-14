Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms

Friend NotInheritable Class OfficialSettingsStore
    Private Shared ReadOnly SettingsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lsi8181_official_settings.ini")
    Private Shared ReadOnly Values As Dictionary(Of String, String) = LoadValues()
    Private Shared ReadOnly AttachedForms As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared _restoring As Boolean

    Public Shared ReadOnly Property IsRestoring As Boolean
        Get
            Return _restoring
        End Get
    End Property

    Private Sub New()
    End Sub

    Public Shared Sub Attach(ByVal form As Form)
        If form Is Nothing OrElse AttachedForms.Contains(form.Name) Then
            Return
        End If

        AttachedForms.Add(form.Name)
        _restoring = True
        Try
            AttachControl(form, form.Name)
        Finally
            _restoring = False
        End Try
    End Sub

    Public Shared Sub SetValue(ByVal key As String, ByVal value As String)
        Values(key) = value
        SaveValues()
    End Sub

    Public Shared Function GetValue(ByVal key As String, ByVal fallback As String) As String
        Dim value As String = Nothing
        If Values.TryGetValue(key, value) Then
            Return value
        End If

        Return fallback
    End Function

    Public Shared Sub Restore(ByVal form As Form)
        If form Is Nothing Then
            Return
        End If

        RunWithoutSaving(Sub() RestoreControl(form, form.Name))
    End Sub

    Public Shared Sub RunWithoutSaving(ByVal action As Action)
        _restoring = True
        Try
            action()
        Finally
            _restoring = False
        End Try
    End Sub

    Private Shared Sub AttachControl(ByVal control As Control, ByVal formName As String)
        RestoreControl(control, formName)
        AddHandler control.ControlAdded, Sub(sender, e) AttachControl(e.Control, formName)

        If TypeOf control Is TextBoxBase Then
            AddHandler DirectCast(control, TextBoxBase).TextChanged, AddressOf PersistChangedControl
        ElseIf TypeOf control Is ComboBox Then
            AddHandler DirectCast(control, ComboBox).SelectedIndexChanged, AddressOf PersistChangedControl
            AddHandler DirectCast(control, ComboBox).TextChanged, AddressOf PersistChangedControl
        ElseIf TypeOf control Is CheckBox Then
            AddHandler DirectCast(control, CheckBox).CheckedChanged, AddressOf PersistChangedControl
        ElseIf TypeOf control Is RadioButton Then
            AddHandler DirectCast(control, RadioButton).CheckedChanged, AddressOf PersistChangedControl
        ElseIf TypeOf control Is NumericUpDown Then
            AddHandler DirectCast(control, NumericUpDown).ValueChanged, AddressOf PersistChangedControl
        ElseIf TypeOf control Is CheckedListBox Then
            AddHandler DirectCast(control, CheckedListBox).ItemCheck, Sub(sender, e)
                                                                          If _restoring Then
                                                                              Return
                                                                          End If

                                                                          BeginInvokeSave(DirectCast(sender, Control))
                                                                      End Sub
        End If

        For Each child As Control In control.Controls
            AttachControl(child, formName)
        Next
    End Sub

    Private Shared Sub RestoreControl(ByVal control As Control, ByVal formName As String)
        Dim key = GetKey(formName, control)

        If TypeOf control Is TextBoxBase Then
            Dim value As String = Nothing
            If Values.TryGetValue(key & ".Text", value) Then
                DirectCast(control, TextBoxBase).Text = value
            End If
        ElseIf TypeOf control Is ComboBox Then
            Dim combo = DirectCast(control, ComboBox)
            Dim selectedIndexText As String = Nothing
            If Values.TryGetValue(key & ".SelectedIndex", selectedIndexText) Then
                Dim selectedIndex As Integer
                If Integer.TryParse(selectedIndexText, NumberStyles.Integer, CultureInfo.InvariantCulture, selectedIndex) AndAlso selectedIndex >= 0 AndAlso selectedIndex < combo.Items.Count Then
                    combo.SelectedIndex = selectedIndex
                End If
            End If

            Dim text As String = Nothing
            If Values.TryGetValue(key & ".Text", text) Then
                combo.Text = text
            End If
        ElseIf TypeOf control Is CheckBox Then
            RestoreChecked(DirectCast(control, CheckBox), key)
        ElseIf TypeOf control Is RadioButton Then
            RestoreChecked(DirectCast(control, RadioButton), key)
        ElseIf TypeOf control Is NumericUpDown Then
            Dim valueText As String = Nothing
            Dim value As Decimal
            Dim numeric = DirectCast(control, NumericUpDown)
            If Values.TryGetValue(key & ".Value", valueText) AndAlso Decimal.TryParse(valueText, NumberStyles.Number, CultureInfo.InvariantCulture, value) Then
                If value < numeric.Minimum Then
                    value = numeric.Minimum
                ElseIf value > numeric.Maximum Then
                    value = numeric.Maximum
                End If

                numeric.Value = value
            End If
        ElseIf TypeOf control Is CheckedListBox Then
            Dim checkedList = DirectCast(control, CheckedListBox)
            Dim value As String = Nothing
            If Values.TryGetValue(key & ".CheckedIndices", value) Then
                Dim selected = New HashSet(Of Integer)()
                For Each part As String In value.Split(","c)
                    Dim index As Integer
                    If Integer.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, index) Then
                        selected.Add(index)
                    End If
                Next

                For itemIndex As Integer = 0 To checkedList.Items.Count - 1
                    checkedList.SetItemChecked(itemIndex, selected.Contains(itemIndex))
                Next
            End If
        End If
    End Sub

    Private Shared Sub RestoreChecked(ByVal button As ButtonBase, ByVal key As String)
        Dim value As String = Nothing
        Dim checked As Boolean
        If Values.TryGetValue(key & ".Checked", value) AndAlso Boolean.TryParse(value, checked) Then
            If TypeOf button Is CheckBox Then
                DirectCast(button, CheckBox).Checked = checked
            ElseIf TypeOf button Is RadioButton Then
                DirectCast(button, RadioButton).Checked = checked
            End If
        End If
    End Sub

    Private Shared Sub PersistChangedControl(ByVal sender As Object, ByVal e As EventArgs)
        If _restoring Then
            Return
        End If

        PersistControl(DirectCast(sender, Control))
        SaveValues()
    End Sub

    Private Shared Sub BeginInvokeSave(ByVal control As Control)
        If control.IsHandleCreated Then
            control.BeginInvoke(New MethodInvoker(Sub()
                                                     PersistControl(control)
                                                     SaveValues()
                                                 End Sub))
        End If
    End Sub

    Private Shared Sub PersistControl(ByVal control As Control)
        Dim form = control.FindForm()
        If form Is Nothing Then
            Return
        End If

        Dim key = GetKey(form.Name, control)
        If TypeOf control Is TextBoxBase Then
            Values(key & ".Text") = DirectCast(control, TextBoxBase).Text
        ElseIf TypeOf control Is ComboBox Then
            Dim combo = DirectCast(control, ComboBox)
            Values(key & ".SelectedIndex") = Convert.ToString(combo.SelectedIndex, CultureInfo.InvariantCulture)
            Values(key & ".Text") = combo.Text
        ElseIf TypeOf control Is CheckBox Then
            Values(key & ".Checked") = DirectCast(control, CheckBox).Checked.ToString()
        ElseIf TypeOf control Is RadioButton Then
            Values(key & ".Checked") = DirectCast(control, RadioButton).Checked.ToString()
        ElseIf TypeOf control Is NumericUpDown Then
            Values(key & ".Value") = Convert.ToString(DirectCast(control, NumericUpDown).Value, CultureInfo.InvariantCulture)
        ElseIf TypeOf control Is CheckedListBox Then
            Dim checkedList = DirectCast(control, CheckedListBox)
            Dim parts As New List(Of String)()
            For Each index As Integer In checkedList.CheckedIndices
                parts.Add(Convert.ToString(index, CultureInfo.InvariantCulture))
            Next

            Values(key & ".CheckedIndices") = String.Join(",", parts.ToArray())
        End If
    End Sub

    Private Shared Function GetKey(ByVal formName As String, ByVal control As Control) As String
        Return formName & "." & control.Name
    End Function

    Private Shared Function LoadValues() As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        If Not File.Exists(SettingsPath) Then
            Return result
        End If

        For Each rawLine As String In File.ReadAllLines(SettingsPath, Encoding.UTF8)
            Dim line = rawLine.Trim()
            If line.Length = 0 OrElse line.StartsWith(";") OrElse line.StartsWith("#") Then
                Continue For
            End If

            Dim separatorIndex = line.IndexOf("="c)
            If separatorIndex <= 0 Then
                Continue For
            End If

            result(line.Substring(0, separatorIndex).Trim()) = line.Substring(separatorIndex + 1).Trim()
        Next

        Return result
    End Function

    Private Shared Sub SaveValues()
        Dim lines As New List(Of String)()
        lines.Add("; LSI-8181 official control settings")
        Dim keys As New List(Of String)(Values.Keys)
        keys.Sort(StringComparer.OrdinalIgnoreCase)

        For Each key As String In keys
            lines.Add(key & "=" & Values(key))
        Next

        File.WriteAllLines(SettingsPath, lines.ToArray(), Encoding.UTF8)
    End Sub
End Class
