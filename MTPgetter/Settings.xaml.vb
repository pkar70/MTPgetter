' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class Settings
    Inherits Page

    Private msCurrDevice As String

    Protected Overrides Sub onNavigatedTo(e As NavigationEventArgs)
        msCurrDevice = e.Parameter.ToString ' uiMiejsca
    End Sub
    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        If msCurrDevice <> "" AndAlso App.GetSettingsString("knownDevices").IndexOf(msCurrDevice & ";") < 0 Then
            If Await App.DialogBoxYN("Add device " & msCurrDevice & " to known?") Then
                'App.SetSettingsString("knownDevices", App.GetSettingsString("knownDevices") & msCurrDevice & ";")
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

        uiCurrDevice.Text = msCurrDevice
        ' oraz lista aktualnych konfiguracji
        ' tu gdzies dodanie do App.SetSettingsString("knownDevices") aktualnego

    End Sub
    Private Sub uiBrowseSrc_Click(sender As Object, e As RoutedEventArgs)
        If msCurrDevice = "" Then Exit Sub
        Dim oMD As MediaDevices.MediaDevice
        oMD = App.GetDeviceObject(msCurrDevice)
        If oMD Is Nothing Then Exit Sub

        oMD.Connect()

        'Dim aFld As String()
        'aFld = oMD.GetDirectories("/")
        Dim oDrives As MediaDevices.MediaDriveInfo() = oMD.GetDrives()




        oMD.Disconnect()
    End Sub

    Private Async Sub uiBrowseDst_Click(sender As Object, e As RoutedEventArgs)
        ' browser
        Dim oPick As Windows.Storage.Pickers.FolderPicker = New Windows.Storage.Pickers.FolderPicker
        oPick.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.ComputerFolder
        oPick.FileTypeFilter.Add("*")
        Dim oFold As Windows.Storage.StorageFolder = Await oPick.PickSingleFolderAsync
        Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(oFold)

        uiDstFold.Text = oFold.Path

    End Sub


End Class
