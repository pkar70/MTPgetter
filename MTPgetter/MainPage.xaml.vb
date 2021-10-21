' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports MediaDevices

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private Sub uiSetup_Click(sender As Object, e As RoutedEventArgs)
        Me.Frame.Navigate(GetType(Settings))
    End Sub

    Private Async Sub uiRefresh_Click(sender As Object, e As RoutedEventArgs)
        uiRefresh.IsEnabled = False ' zeby nie bylo podwojnego wywolania (np. przy dlugim kopiowaniu

        Dim sTmp As String = Await App.CheckNewDevices(False)
        If sTmp <> "" Then
            Dim aTmp As String()
            aTmp = sTmp.Split(";")
            If Await App.DialogBoxYN("New device: " & aTmp(0) & ". Configure?") Then
                Me.Frame.Navigate(GetType(Settings), aTmp(0))
            End If
            ' sa nowe devicesy (dev;dev;dev;) - proponuj setup 
        End If

        Await App.CheckCopyFromDevices(False)

        uiRefresh.IsEnabled = True
    End Sub

    Private Sub UiClockRead_Checked(sender As Object, e As RoutedEventArgs) Handles uiClockRead.Checked
        ' włącz / wyłącz timer
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        uiVersion.Text = "Build " & Package.Current.Id.Version.Major & "." &
            Package.Current.Id.Version.Minor & "." & Package.Current.Id.Version.Build

        App.SetSettingsString("knownDevices", "")
    End Sub
End Class
