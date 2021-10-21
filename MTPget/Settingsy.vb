Public Class Settingsy
    Private msCurrDevice As String

    Public Sub onNavigatedTo(sTmp As String)
        msCurrDevice = sTmp ' uiMiejsca
    End Sub


    Private Sub Settingsy_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated


    End Sub

    Private Sub RecurFillCombo(oDir As MediaDevices.MediaDirectoryInfo)
        uiSrcFold.Items.Add(oDir.FullName)
        uiBrowseSrc.Text = oDir.Name
        For Each oSubDir In oDir.EnumerateDirectories
            RecurFillCombo(oSubDir)
        Next
    End Sub

    Private Sub uiBrowseSrc_Click(sender As Object, e As EventArgs) Handles uiBrowseSrc.Click
        uiAdd.Enabled = False
        uiBrowseSrc.Enabled = False
        If msCurrDevice = "" Then Exit Sub

        Dim sDev As String = msCurrDevice
        Dim iInd As Integer = sDev.IndexOf(" (")
        If iInd > 0 Then sDev = sDev.Substring(0, iInd)

        Dim oMD As MediaDevices.MediaDevice
        oMD = App.GetDeviceObject(sDev)
        If oMD Is Nothing Then
            MsgBox("ERROR: cannot find device?")
            Exit Sub
        End If

        oMD.Connect()

        uiSrcFold.Items.Clear()

        'Dim aFld As String()
        'aFld = oMD.GetDirectories("/")
        Dim oDrives As MediaDevices.MediaDriveInfo() = oMD.GetDrives()

        ' rekurencyjne wypelnienie combo
        For Each oDrive As MediaDevices.MediaDriveInfo In oDrives
            RecurFillCombo(oDrive.RootDirectory)
        Next

        uiBrowseSrc.Text = "Fill..."
        uiAdd.Enabled = True
        uiBrowseSrc.Enabled = True

        oMD.Disconnect()
    End Sub

    Private Sub uiBrowseDst_Click(sender As Object, e As EventArgs) Handles uiBrowseDst.Click

        Dim oPick As Windows.Forms.FolderBrowserDialog
        oPick = New FolderBrowserDialog
        oPick.ShowDialog()
        uiDstFold.Text = oPick.SelectedPath

    End Sub

    Private Sub uiAdd_Click(sender As Object, e As EventArgs) Handles MyBase.Click, uiAdd.Click
        If uiDstFold.Text.Length < 2 Then Exit Sub
        If uiSrcFold.SelectedItem.ToString.Length < 2 Then Exit Sub

        Dim sTxt As String = uiMapping.Text
        sTxt = sTxt & vbCrLf & uiSrcFold.SelectedItem.ToString & "|" & uiDstFold.Text
        uiMapping.Text = sTxt.Trim  ' pierwsze CrLf utnie
        ' jako SetSettings(devName, string() = "src|dst"
    End Sub

    Private Sub uiMapping_TextChanged(sender As Object, e As EventArgs) Handles uiMapping.TextChanged
        uiSave.Enabled = True
    End Sub

    Private Sub Settingsy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        uiAdd.Enabled = False
        uiSave.Enabled = False

        If msCurrDevice <> "" AndAlso App.GetSettingsString("knownDevices").IndexOf(msCurrDevice & ";") < 0 Then
            If MsgBox("Add device " & msCurrDevice & " to known?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                App.SetSettingsString("knownDevices", App.GetSettingsString("knownDevices") & msCurrDevice & ";")
            End If
        End If

        If msCurrDevice = "" Then ' nie dla konkretnego, to znajdz pierwsze
            Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
            oMDs = MediaDevices.MediaDevice.GetDevices

            Dim sKnownDevices As String
            sKnownDevices = App.GetSettingsString("knownDevices")

            For Each oMD As MediaDevices.MediaDevice In oMDs
                If sKnownDevices.IndexOf(oMD.FriendlyName & ";") > -1 Then
                    msCurrDevice = oMD.FriendlyName
                    Exit For
                End If
            Next

        End If

        If msCurrDevice <> "" Then
            uiCurrDevice.Text = "Device: " & msCurrDevice
            Dim sTmp As String
            sTmp = App.GetSettingsString(msCurrDevice)
            sTmp = sTmp.Replace(vbLf, vbCrLf)
            sTmp = sTmp.Replace(vbLf & vbLf, vbLf)
            sTmp = sTmp.Replace(vbCrLf & vbCrLf, vbCrLf)
            uiMapping.Text = sTmp
            uiBrowseSrc.Enabled = True
        Else
            uiCurrDevice.Text = "No device selected"
            uiMapping.Text = ""
            uiBrowseSrc.Enabled = False
        End If

    End Sub

    Private Sub uiSave_Click(sender As Object, e As EventArgs) Handles uiSave.Click
        If msCurrDevice = "" Then Exit Sub
        App.SetSettingsString(msCurrDevice, uiMapping.Text)
        Me.Close()
    End Sub
End Class