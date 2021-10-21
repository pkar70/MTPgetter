Public Class Form1


    Private Sub AddLogLine(sTxt As String)
        uiLog.Text = uiLog.Text & vbCrLf & sTxt
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles uiRefresh.Click
        uiRefresh.Enabled = False ' zeby nie bylo podwojnego wywolania (np. przy dlugim kopiowaniu

        App.SetSettingsString("knownDevices", "")

        Dim sTmp As String = App.CheckNewDevices(False)
        If sTmp <> "" Then
            AddLogLine("New devices: " & sTmp)
            Dim aTmp As String()
            aTmp = sTmp.Split(";")
            For Each sNew As String In aTmp
                If MsgBox("New device: " & sNew & ". Configure?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Settingsy.onNavigatedTo(sNew)
                    Settingsy.ShowDialog()
                End If
            Next
        End If

        ' Await App.CheckCopyFromDevices(False)

        uiRefresh.Enabled = True

    End Sub

    Private Async Sub uiCopy_Click(sender As Object, e As EventArgs) Handles uiCopy.Click
        Dim sDevice As String = ""
        Try
            sDevice = uiDevices.SelectedItem.ToString
        Catch ex As Exception
        End Try
        If sDevice = "" Then Exit Sub

        uiCopy.Enabled = False
        AddLogLine(vbCrLf & "Copying from device " & sDevice & "...")

        Dim aMappings As String() = App.GetSettingsString(sDevice).Split(vbLf)

        For Each sMapping As String In aMappings
            Dim aMapFld As String() = sMapping.Trim.Split("|")
            If aMapFld.GetUpperBound(0) < 1 Then Continue For

            Dim sSrcDir, sDstDir As String
            Dim bAutoDel As Boolean = False
            sSrcDir = aMapFld(0).Trim
            sDstDir = aMapFld(1).Trim
            If aMapFld.GetUpperBound(0) > 1 Then
                If aMapFld(2).Trim.ToUpper = "DEL" Then bAutoDel = True
            End If

            Dim sLogLine As String
            sLogLine = Await App.CopyFiles(sDevice, sSrcDir, sDstDir, bAutoDel, uiStatus)
            AddLogLine(sLogLine)
        Next

        uiCopy.Enabled = True
        uiStatus.Text = "."
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        uiDevices.Items.Clear()
        uiCopy.Enabled = False
        uiConfig.Enabled = False

        Dim sTmp As String = App.ConnectedDevices
        If sTmp <> "" Then
            Dim aTmp As String()
            aTmp = sTmp.Split(";")
            For Each sNew As String In aTmp
                uiDevices.Items.Add(sNew)
            Next
            uiConfig.Enabled = True
        End If

    End Sub

    Private Sub uiDevices_SelectedValueChanged(sender As Object, e As EventArgs) Handles uiDevices.SelectedValueChanged
        Dim sKnownDevices As String
        sKnownDevices = App.GetSettingsString("knownDevices")

        If sKnownDevices.IndexOf(uiDevices.SelectedItem.ToString & ";") > -1 Then
            uiCopy.Enabled = True
        Else
            uiCopy.Enabled = False
        End If
    End Sub

    Private Sub uiConfig_Click(sender As Object, e As EventArgs) Handles uiConfig.Click
        Try
            Dim sNew As String = uiDevices.SelectedItem.ToString
            Settingsy.onNavigatedTo(sNew)
            Settingsy.ShowDialog()
        Catch ex As Exception

        End Try

    End Sub
End Class
