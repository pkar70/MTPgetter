''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Class App
    Inherits Application

    ''' <summary>
    ''' Invoked when the application is launched normally by the end user.  Other entry points
    ''' will be used when the application is launched to open a specific file, to display
    ''' search results, and so forth.
    ''' </summary>
    ''' <param name="e">Details about the launch request and process.</param>
    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' Do not repeat app initialization when the Window already has content,
        ' just ensure that the window is active

        If rootFrame Is Nothing Then
            ' Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed
            ' PKAR added wedle https://stackoverflow.com/questions/39262926/uwp-hardware-back-press-work-correctly-in-mobile-but-error-with-pc
            AddHandler rootFrame.Navigated, AddressOf OnNavigatedAddBackButton
            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf OnBackButtonPressed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Load state from previously suspended application
            End If
            ' Place the frame in the current Window
            Window.Current.Content = rootFrame
        End If

        If e.PrelaunchActivated = False Then
            If rootFrame.Content Is Nothing Then
                ' When the navigation stack isn't restored navigate to the first page,
                ' configuring the new page by passing required information as a navigation
                ' parameter
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            ' Ensure the current window is active
            Window.Current.Activate()
        End If
    End Sub

    ''' <summary>
    ''' Invoked when Navigation to a certain page fails
    ''' </summary>
    ''' <param name="sender">The Frame which failed navigation</param>
    ''' <param name="e">Details about the navigation failure</param>
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ''' <summary>
    ''' Invoked when application execution is being suspended.  Application state is saved
    ''' without knowing whether the application will be terminated or resumed with the contents
    ''' of memory still intact.
    ''' </summary>
    ''' <param name="sender">The source of the suspend request.</param>
    ''' <param name="e">Details about the suspend request.</param>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' TODO: Save application state and stop any background activity
        deferral.Complete()
    End Sub
#Region "BackButton"
    ' PKAR added wedle https://stackoverflow.com/questions/39262926/uwp-hardware-back-press-work-correctly-in-mobile-but-error-with-pc
    Private Sub OnNavigatedAddBackButton(sender As Object, e As NavigationEventArgs)
#If CONFIG = "Debug" Then
        ' próba wylapywania errorów gdy nic innego tego nie złapie
        Dim sDebugCatch As String = ""
        Try
#End If
            Dim oFrame As Frame = TryCast(sender, Frame)
            If oFrame Is Nothing Then Exit Sub

            Dim oNavig As Windows.UI.Core.SystemNavigationManager = Windows.UI.Core.SystemNavigationManager.GetForCurrentView

            If oFrame.CanGoBack Then
                oNavig.AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible
            Else
                oNavig.AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed
            End If


#If CONFIG = "Debug" Then
        Catch ex As Exception
            sDebugCatch = ex.Message
        End Try

        If sDebugCatch <> "" Then
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
            App.DialogBox("DebugCatch in OnNavigatedAddBackButton:" & vbCrLf & sDebugCatch)
#Enable Warning BC42358
        End If
#End If

    End Sub

    Private Sub OnBackButtonPressed(sender As Object, e As Windows.UI.Core.BackRequestedEventArgs)
        Try
            TryCast(Window.Current.Content, Frame).GoBack()
            e.Handled = True
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Get/Set settings"

#Region "String"

    Public Shared Function GetSettingsString(sName As String, Optional sDefault As String = "") As String
        Dim sTmp As String

        sTmp = sDefault

        With Windows.Storage.ApplicationData.Current
            If .RoamingSettings.Values.ContainsKey(sName) Then
                sTmp = .RoamingSettings.Values(sName).ToString
            End If
            If .LocalSettings.Values.ContainsKey(sName) Then
                sTmp = .LocalSettings.Values(sName).ToString
            End If
        End With

        Return sTmp

    End Function

    Public Shared Sub SetSettingsString(sName As String, sValue As String)
        SetSettingsString(sName, sValue, False)
    End Sub

    Public Shared Sub SetSettingsString(sName As String, sValue As String, bRoam As Boolean)
        With Windows.Storage.ApplicationData.Current
            If bRoam Then .RoamingSettings.Values(sName) = sValue
            .LocalSettings.Values(sName) = sValue
        End With
    End Sub
#End Region
#Region "Int"
    Public Shared Function GetSettingsInt(sName As String, Optional iDefault As Integer = 0) As Integer
        Dim sTmp As Integer

        sTmp = iDefault

        With Windows.Storage.ApplicationData.Current
            If .RoamingSettings.Values.ContainsKey(sName) Then
                sTmp = CInt(.RoamingSettings.Values(sName).ToString)
            End If
            If .LocalSettings.Values.ContainsKey(sName) Then
                sTmp = CInt(.LocalSettings.Values(sName).ToString)
            End If
        End With

        Return sTmp

    End Function

    Public Shared Sub SetSettingsInt(sName As String, sValue As Integer)
        SetSettingsInt(sName, sValue, False)
    End Sub

    Public Shared Sub SetSettingsInt(sName As String, sValue As Integer, bRoam As Boolean)
        With Windows.Storage.ApplicationData.Current
            If bRoam Then .RoamingSettings.Values(sName) = sValue.ToString
            .LocalSettings.Values(sName) = sValue.ToString
        End With
    End Sub
#End Region
#Region "Bool"
    Public Shared Function GetSettingsBool(sName As String, Optional iDefault As Boolean = False) As Boolean
        Dim sTmp As Boolean

        sTmp = iDefault
        With Windows.Storage.ApplicationData.Current
            If .RoamingSettings.Values.ContainsKey(sName) Then
                sTmp = CBool(.RoamingSettings.Values(sName).ToString)
            End If
            If .LocalSettings.Values.ContainsKey(sName) Then
                sTmp = CBool(.LocalSettings.Values(sName).ToString)
            End If
        End With

        Return sTmp

    End Function
    Public Shared Sub SetSettingsBool(sName As String, sValue As Boolean)
        SetSettingsBool(sName, sValue, False)
    End Sub

    Public Shared Sub SetSettingsBool(sName As String, sValue As Boolean, bRoam As Boolean)
        With Windows.Storage.ApplicationData.Current
            If bRoam Then .RoamingSettings.Values(sName) = sValue.ToString
            .LocalSettings.Values(sName) = sValue.ToString
        End With
    End Sub
#End Region

#End Region



    ' -- Testy sieciowe ---------------------------------------------

#Region "testy sieciowe"

    Public Shared Function IsMobile() As Boolean
        Return (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Mobile")
    End Function

    Public Shared Function IsNetIPavailable(bMsg As Boolean) As Boolean
        If App.GetSettingsBool("offline") Then Return False

        If Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() Then Return True
        If bMsg Then
#Disable Warning BC42358
            DialogBox("ERROR: no IP network available")
#Enable Warning BC42358
        End If
        Return False
    End Function

    Public Shared Function IsCellInet() As Boolean
        Return Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile().IsWwanConnectionProfile
    End Function
#End Region


    ' -- DialogBoxy ---------------------------------------------

#Region "DialogBoxy"


    Public Shared Async Function DialogBox(sMsg As String) As Task
        Dim oMsg As Windows.UI.Popups.MessageDialog = New Windows.UI.Popups.MessageDialog(sMsg)
        Await oMsg.ShowAsync
    End Function

    Public Shared Function GetLangString(sMsg As String) As String
        If sMsg = "" Then Return ""

        Dim sRet As String = sMsg
        Try
            sRet = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString(sMsg)
        Catch
        End Try
        Return sRet
    End Function

    Public Shared Async Sub DialogBoxRes(sMsg As String)
        sMsg = GetLangString(sMsg)
        Await DialogBox(sMsg)
    End Sub


    Public Shared Async Sub DialogBoxError(iNr As Integer, sMsg As String)
        Dim sTxt As String = GetLangString("errAnyError")
        sTxt = sTxt & " (" & iNr & ")" & vbCrLf & sMsg
        Await DialogBox(sMsg)
    End Sub

    Public Shared Async Function DialogBoxYN(sMsg As String, Optional sYes As String = "Tak", Optional sNo As String = "Nie") As Task(Of Boolean)
        Dim oMsg As Windows.UI.Popups.MessageDialog = New Windows.UI.Popups.MessageDialog(sMsg)
        Dim oYes As Windows.UI.Popups.UICommand = New Windows.UI.Popups.UICommand(sYes)
        Dim oNo As Windows.UI.Popups.UICommand = New Windows.UI.Popups.UICommand(sNo)
        oMsg.Commands.Add(oYes)
        oMsg.Commands.Add(oNo)
        oMsg.DefaultCommandIndex = 1    ' default: No
        oMsg.CancelCommandIndex = 1
        Dim oCmd As Windows.UI.Popups.IUICommand = Await oMsg.ShowAsync
        If oCmd Is Nothing Then Return False
        If oCmd.Label = sYes Then Return True

        Return False
    End Function

    Public Shared Async Function DialogBoxResYN(sMsgResId As String, Optional sYesResId As String = "resDlgYes", Optional sNoResId As String = "resDlgNo") As Task(Of Boolean)
        Dim sMsg, sYes, sNo As String

        With Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView()
            sMsg = .GetString(sMsgResId)
            sYes = .GetString(sYesResId)
            sNo = .GetString(sNoResId)
        End With

        If sMsg = "" Then sMsg = sMsgResId  ' zabezpieczenie na brak string w resource
        If sYes = "" Then sYes = sYesResId
        If sNo = "" Then sNo = sNoResId

        Return Await DialogBoxYN(sMsg, sYes, sNo)
    End Function


    Public Shared Async Function DialogBoxInput(sMsgResId As String, Optional sDefaultResId As String = "", Optional sYesResId As String = "resDlgContinue", Optional sNoResId As String = "resDlgCancel") As Task(Of String)
        Dim sMsg, sYes, sNo, sDefault As String

        sDefault = ""

        With Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView()
            sMsg = .GetString(sMsgResId)
            sYes = .GetString(sYesResId)
            sNo = .GetString(sNoResId)
            If sDefaultResId <> "" Then sDefault = .GetString(sDefaultResId)
        End With

        If sMsg = "" Then sMsg = sMsgResId  ' zabezpieczenie na brak string w resource
        If sYes = "" Then sYes = sYesResId
        If sNo = "" Then sNo = sNoResId
        If sDefault = "" Then sDefault = sDefaultResId

        Dim oInputTextBox = New TextBox
        oInputTextBox.AcceptsReturn = False
        oInputTextBox.Text = sDefault
        Dim oDlg As New ContentDialog
        oDlg.Content = oInputTextBox
        oDlg.PrimaryButtonText = sYes
        oDlg.SecondaryButtonText = sNo
        oDlg.Title = sMsg

        Dim oCmd = Await oDlg.ShowAsync
        If oCmd <> ContentDialogResult.Primary Then Return ""

        Return oInputTextBox.Text

    End Function



#End Region
    Public Shared Function XmlSafeString(sInput As String) As String
        Dim sTmp As String
        sTmp = sInput.Replace("&", "&amp;")
        sTmp = sTmp.Replace("<", "&lt;")
        sTmp = sTmp.Replace(">", "&gt;")
        Return sTmp
    End Function

    Public Shared Function XmlSafeStringQt(sInput As String) As String
        Dim sTmp As String
        sTmp = XmlSafeString(sInput)
        sTmp = sTmp.Replace("""", "&quote;")
        sTmp = sTmp.Replace("'", "&apos;")
        Return sTmp
    End Function

    Public Shared Sub MakeToast(sMsg As String, Optional sMsg1 As String = "")
        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & XmlSafeString(sMsg)
        If sMsg1 <> "" Then sXml = sXml & "</text><text>" & XmlSafeString(sMsg1)
        sXml = sXml & "</text></binding></visual>"
        Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        oXml.LoadXml("<toast>" & sXml & "</toast>")
        Dim oToast = New Windows.UI.Notifications.ToastNotification(oXml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(oToast)
    End Sub
    Public Shared Function WinVer() As Integer
        'Unknown = 0,
        'Threshold1 = 1507,   // 10240
        'Threshold2 = 1511,   // 10586
        'Anniversary = 1607,  // 14393 Redstone 1
        'Creators = 1703,     // 15063 Redstone 2
        'FallCreators = 1709 // 16299 Redstone 3
        'April = 1803		// 17134
        'October = 1809		// 17763

        Dim u As ULong = ULong.Parse(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion)
        u = (u And &HFFFF0000L) >> 16
        Return u
        'For i As Integer = 5 To 1 Step -1
        '    If Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", i) Then Return i
        'Next

        'Return 0
    End Function

    Private Shared msAlreadySeenDevices As String = ""

#Const USELIB = "git"
    ' #Const USELIB = "git"

#If USELIB = "win" Then
    Public Shared Async Function CheckNewDevices(bAsBackground As Boolean) As Task(Of String)

        Dim sKnownDevices As String
        sKnownDevices = App.GetSettingsString("knownDevices")

        'Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        'oMDs = MediaDevices.MediaDevice.GetDevices

        Dim oMDs As Windows.Devices.Enumeration.DeviceInformationCollection
        oMDs = Await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(
            Windows.Devices.Portable.StorageDevice.GetDeviceSelector)

        Dim iCnt As Integer = 0
        Dim sList As String = ""

        ' Name: WPD.DEVICE_FRIENDLY_NAME
        ' Description: WPD.STORAGE_DESCRIPTION

        For Each oMD As Windows.Devices.Enumeration.DeviceInformation In oMDs
            If oMD.Id.ToLower.IndexOf("\\?\usb#") = 0 Then
                If sKnownDevices.IndexOf(oMD.Name & ";") < 0 Then
                    If msAlreadySeenDevices.IndexOf(oMD.Name & ";") < 0 Then
                        'nowy, i jeszcze nie widziany
                        msAlreadySeenDevices = msAlreadySeenDevices & oMD.Name & ";"
                        sList = sList & oMD.Name & ";"

                        'For Each oCos In oMD.Properties
                        '    Debug.Print(oCos.Key.ToString)
                        '    Debug.Print(oCos.Value)
                        'Next
                        '[Key]   "System.ItemNameDisplay"	String
                        '"Lumia650_pkar"

                        '"System.Devices.ContainerId"
                        '{50957dac-a9f3-51a9-baf6-36a0dd1bf5bc}


                        Dim sMsg As String = "New device: " & oMD.Name
                        If Not bAsBackground Then
                            ' Await App.DialogBox(sMsg) ' zapytamy o konfiguracje pietro wyzej
                        Else
                            MakeToast(sMsg)
                        End If

                    End If ' i jeszcze nie ignorujemy go (jak na timer, to nie pokazujemy tego samego co timer!
                End If ' nie zapisana konfiguracja
            End If  ' if(po USB)
        Next

        Return sList
    End Function
#Else
    Public Shared Async Function CheckNewDevices(bAsBackground As Boolean) As Task(Of String)

        Dim sKnownDevices As String
        sKnownDevices = App.GetSettingsString("knownDevices")

        Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        oMDs = MediaDevices.MediaDevice.GetDevices

        Dim iCnt As Integer = 0
        Dim sList As String = ""

        ' Name: WPD.DEVICE_FRIENDLY_NAME
        ' Description: WPD.STORAGE_DESCRIPTION

        For Each oMD As MediaDevices.MediaDevice In oMDs
            If oMD.DeviceId.ToLower.IndexOf("\\?\usb#") = 0 Then
                If sKnownDevices.IndexOf(oMD.FriendlyName & ";") < 0 Then
                    If msAlreadySeenDevices.IndexOf(oMD.FriendlyName & ";") < 0 Then

                        'nowy, i jeszcze nie widziany
                        msAlreadySeenDevices = msAlreadySeenDevices & oMD.FriendlyName & ";"
                        sList = sList & oMD.FriendlyName & ";"

                        Dim sMsg As String = "New device: " & oMD.FriendlyName & " (" & oMD.Description & ")"

                        If Not bAsBackground Then
                            ' Await App.DialogBox(sMsg) ' zapytamy o konfiguracje pietro wyzej
                        Else
                            MakeToast(sMsg)
                        End If

                    End If ' i jeszcze nie ignorujemy go (jak na timer, to nie pokazujemy tego samego co timer!
                End If ' nie zapisana konfiguracja
            End If  ' if(po USB)
        Next

        Return sList
    End Function
#End If

    Private Shared Async Function CopyFromDevice(oMD As MediaDevices.MediaDevice) As Task(Of Boolean)
        ' foreach TypeFoldersCopy.getfoldersfor(sFriendlyName)
        ' kopiuj jeden folder
    End Function


    Public Shared Function GetDeviceObject(sFriendName As String) As MediaDevices.MediaDevice
        Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        oMDs = MediaDevices.MediaDevice.GetDevices

        Dim sLog As String = ""

        For Each oMD As MediaDevices.MediaDevice In oMDs
            If sFriendName = oMD.FriendlyName Then Return oMD
        Next

        Return Nothing
    End Function

    Public Shared Async Function CheckCopyFromDevices(bAsBackground As Boolean) As Task(Of String)

        Dim sKnownDevices As String
        sKnownDevices = App.GetSettingsString("knownDevices")

        Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        oMDs = MediaDevices.MediaDevice.GetDevices

        Dim sLog As String = ""

        For Each oMD As MediaDevices.MediaDevice In oMDs
            If sKnownDevices.IndexOf(oMD.FriendlyName & ";") > -1 Then
                Await CopyFromDevice(oMD)
            End If
        Next

        Return sLog
    End Function


End Class
