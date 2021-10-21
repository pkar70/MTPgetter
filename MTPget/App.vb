Public Class App
    Public Shared Function GetSettingsString(sName As String, Optional sDefault As String = "") As String

        Return Interaction.GetSetting("MTPget", "settings", sName, sDefault)

    End Function

    Public Shared Sub SetSettingsString(sName As String, sValue As String)

        Interaction.SaveSetting("MTPget", "settings", sName, sValue)

    End Sub



    Public Shared Function XmlSafeString(sInput As String) As String
        Dim sTmp As String
        sTmp = sInput.Replace("&", "&amp;")
        sTmp = sTmp.Replace("<", "&lt;")
        sTmp = sTmp.Replace(">", "&gt;")
        Return sTmp
    End Function

    Public Shared Sub MakeToast(sMsg As String, Optional sMsg1 As String = "")
        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & XmlSafeString(sMsg)
        If sMsg1 <> "" Then sXml = sXml & "</text><text>" & XmlSafeString(sMsg1)
        sXml = sXml & "</text></binding></visual>"
        'Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        'oXml.LoadXml("<toast>" & sXml & "</toast>")
        'Dim oToast = New Windows.UI.Notifications.ToastNotification(oXml)
        'Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(oToast)
    End Sub

    Private Shared msAlreadySeenDevices As String = ""

    Public Shared Function GetDeviceObject(sFriendName As String) As MediaDevices.MediaDevice
        Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        oMDs = MediaDevices.MediaDevice.GetDevices

        Dim sDev As String = sFriendName
        Dim iInd As Integer = sDev.IndexOf(" (")
        If iInd > 0 Then sDev = sDev.Substring(0, iInd)

        Dim sLog As String = ""

        For Each oMD As MediaDevices.MediaDevice In oMDs
            If sDev = oMD.FriendlyName Then Return oMD
        Next

        Return Nothing
    End Function

    Public Shared Function ConnectedDevices() As String
        Dim oMDs As IEnumerable(Of MediaDevices.MediaDevice)
        oMDs = MediaDevices.MediaDevice.GetDevices

        Dim iCnt As Integer = 0
        Dim sList As String = ""

        ' Name: WPD.DEVICE_FRIENDLY_NAME
        ' Description: WPD.STORAGE_DESCRIPTION

        For Each oMD As MediaDevices.MediaDevice In oMDs
            If oMD.DeviceId.ToLower.IndexOf("\\?\usb#") = 0 Then
                sList = sList & oMD.FriendlyName & " (" & oMD.Description & ")" & ";"
            End If  ' if(po USB)
        Next

        Return sList
    End Function

    Public Shared Function CheckNewDevices(bAsBackground As Boolean) As String

        Dim sKnownDevices As String
        sKnownDevices = App.GetSettingsString("knownDevices")
        Dim sConnected As String
        sConnected = ConnectedDevices()

        Dim aDevs As String()
        aDevs = sConnected.Split(";")

        Dim sList As String = ""

        For Each sDev As String In aDevs
            Dim iInd As Integer
            Dim sName As String = sDev

            iInd = sDev.IndexOf(" (")
            If iInd > 0 Then sName = sDev.Substring(0, iInd)

            If sKnownDevices.IndexOf(sName & ";") < 0 Then
                If msAlreadySeenDevices.IndexOf(sName & ";") < 0 Then
                    'nowy, i jeszcze nie widziany
                    msAlreadySeenDevices = msAlreadySeenDevices & sName & ";"

                    Dim sMsg As String = "New device: " & sDev

                    sList = sList & sName & ";"
                    If Not bAsBackground Then
                        ' zapytamy o konfiguracje pietro wyzej
                    Else
                        MakeToast(sMsg)
                    End If
                End If ' i jeszcze nie ignorujemy go (jak na timer, to nie pokazujemy tego samego co timer!
            End If ' nie zapisana konfiguracja
        Next

        Return sList
    End Function

    Private Shared miFilesCnt As Integer
    Private Shared miSkippedCnt As Integer
    Private Shared miDirsCnt As Integer
    Private Shared miFilesLen As Integer
    Private Shared miErrMsg As String

    Private Shared Async Function CopyFilesRecur(oDev As MediaDevices.MediaDevice, sSrcPath As String, oDate As DateTime, sDstPath As String, bAutoDel As Boolean, uiStatus As Label) As Task(Of Boolean)

        miDirsCnt = miDirsCnt + 1
        If uiStatus IsNot Nothing Then uiStatus.Text = "Copying dir " & sSrcPath

        ' jesli nie ma sDstDir, to go stworz
        If Not FileIO.FileSystem.DirectoryExists(sDstPath) Then
            FileIO.FileSystem.CreateDirectory(sDstPath)
        End If

        For Each sDir As String In oDev.EnumerateDirectories(sSrcPath)
            Dim iInd As Integer
            Dim sLastDir As String
            iInd = sDir.LastIndexOf("\")
            If iInd > 0 Then
                sLastDir = sDir.Substring(iInd + 1)
            Else
                Return False ' root dir nie kopiujemy :)
            End If
            If Not Await CopyFilesRecur(oDev, sDir, oDate, sDstPath & "\" & sLastDir, bAutoDel, uiStatus) Then Return False
        Next

        For Each sFile As String In oDev.EnumerateFiles(sSrcPath)
            Dim oFI As MediaDevices.MediaFileInfo = oDev.GetFileInfo(sFile)
            If oFI.LastWriteTime < oDate Then
                miSkippedCnt = miSkippedCnt + 1
            Else
                ' kopiujemy!
                ' dla error: return false

                Dim sDstPathName As String = sDstPath & "\" & oFI.Name

                If FileIO.FileSystem.FileExists(sDstPathName) Then
                    Dim sDstTmp As String
                    Dim iInd As Integer
                    iInd = sDstPathName.LastIndexOf(".")
                    sDstTmp = sDstPathName.Substring(0, iInd) & Date.Now.ToString("yyMMdd") & sDstPathName.Substring(iInd)
                    If FileIO.FileSystem.FileExists(sDstTmp) Then
                        sDstTmp = sDstPathName.Substring(0, iInd) & Date.Now.ToString("yyMMddHHmmss") & sDstPathName.Substring(iInd)
                        If FileIO.FileSystem.FileExists(sDstTmp) Then
                            miErrMsg = miErrMsg & "File '" & sDstPathName & "' already exist! Skipping rest in this dir!" & vbCrLf
                            Return False ' file already exist - np. nagrania nieprzetworzone, nic sie nie da zrobic
                        End If
                    End If
                    miErrMsg = miErrMsg & "File '" & sDstPathName & "' already exist, renaming..." & vbCrLf
                    sDstPathName = sDstTmp
                End If

                Dim iFileLen As ULong = oFI.Length

                Dim bError As Boolean = False

                If iFileLen > 0 Then
                    Dim oBuff As Byte() = New Byte(1024 * 1024) {}
                    Dim oReadStr As IO.Stream = oFI.OpenRead

                    Try
                        While iFileLen > 0
                            ' wczytaj poczatek
                            Dim iCntRead As Integer = Await oReadStr.ReadAsync(oBuff, 0, oBuff.Length)
                            If iCntRead < 1024 * 1024 Then ReDim Preserve oBuff(iCntRead)
                            FileIO.FileSystem.WriteAllBytes(sDstPathName, oBuff, True)
                            iFileLen = iFileLen - iCntRead
                        End While
                    Catch ex As Exception
                        bError = True
                        miErrMsg = miErrMsg & "File '" & sDstPathName & "' copy error: " & ex.Message & vbCrLf
                    End Try

                    oReadStr.Close()
                    If Not bError Then
                        If bAutoDel Then
                            MsgBox("Niby usuwam to:" & vbCrLf & sSrcPath & "\" & sFile)
                            Try
                                oDev.DeleteFile(sSrcPath & "\" & sFile)
                            Catch ex As Exception
                                MsgBox("Nieudane DELETE")
                            End Try
                        End If
                    End If
                Else
                    Try
                        FileIO.FileSystem.WriteAllText(sDstPathName, "", False)
                    Catch ex As Exception
                        bError = True
                        miErrMsg = miErrMsg & "File '" & sDstPathName & "', len 0 - cannot create: " & ex.Message & vbCrLf
                    End Try
                End If

                If bError Then
                    Try
                        FileIO.FileSystem.DeleteFile(sDstPathName)
                    Catch ex As Exception

                    End Try
                Else
                    FileIO.FileSystem.GetFileInfo(sDstPathName).LastWriteTime = oFI.LastWriteTime
                    FileIO.FileSystem.GetFileInfo(sDstPathName).CreationTime = oFI.CreationTime
                End If

                miFilesCnt = miFilesCnt + 1
                miFilesLen = miFilesLen + (oFI.Length \ 1024) + 1
            End If
        Next

        Return True
    End Function

    Private Shared Function CheckNewestFileRecurr(oDir As IO.DirectoryInfo) As DateTime
        Dim oDate As DateTime = New DateTime(1900, 1, 1)

        For Each oSubDir As IO.DirectoryInfo In oDir.EnumerateDirectories
            Dim oDateNew As DateTime = CheckNewestFileRecurr(oSubDir)
            If oDateNew > oDate Then oDate = oDateNew
        Next

        For Each oFile As IO.FileSystemInfo In oDir.EnumerateFileSystemInfos
            If (oFile.Attributes And IO.FileAttributes.Directory) = 0 AndAlso
                oFile.Name.ToLower <> "descript.ion" Then
                If oFile.LastWriteTime > oDate Then oDate = oFile.LastWriteTime
            End If
        Next

        Return oDate

    End Function

    Private Shared Function CheckNewestFile(sDir As String) As DateTime
        Dim oDir As IO.DirectoryInfo = FileIO.FileSystem.GetDirectoryInfo(sDir)

        Return CheckNewestFileRecurr(oDir)
    End Function

    Public Shared Async Function CopyFiles(sDevice As String, sSrcDir As String, sDstDir As String, bAutoDel As Boolean, uiStatus As Label) As Task(Of String)
        Dim oMD As MediaDevices.MediaDevice
        oMD = GetDeviceObject(sDevice)
        If oMD Is Nothing Then Return "ERROR: Cannot find device!"

        ' znajdz katalog sSrcDir
        Try
            oMD.Connect()
        Catch ex As Exception
            Return "ERROR: Cannot open device!"
        End Try

        If Not oMD.DirectoryExists(sSrcDir) Then Return "ERROR: Source directory (" & sSrcDir & ") doesn't exist!"

        ' kontrola dstdir
        If Not FileIO.FileSystem.DirectoryExists(sDstDir) Then Return "ERROR: Destination directory (" & sDstDir & ") doesn't exist!"
        Dim oDate As DateTime = CheckNewestFile(sDstDir)

        Dim sRetVal As String = ""
        sRetVal = vbCrLf & "Copying dir: '" & sSrcDir & "' to '" & sDstDir & "'," & vbCrLf & "  newest file in dst: " & oDate.ToString("yyyy.MM.dd HH:mm:ss") & vbCrLf

        oDate = oDate.AddSeconds(10)     ' tylko nowsze - bez tego kopiowal ostatni plik jeszcze raz

        miFilesCnt = 0
        miFilesLen = 0
        miDirsCnt = 0
        miSkippedCnt = 0
        miErrMsg = ""
        Await CopyFilesRecur(oMD, sSrcDir, oDate, sDstDir, bAutoDel, uiStatus)
        ' aktualizowane liczniki

        sRetVal = sRetVal & "Copied " & miFilesCnt & " files (skipped " & miSkippedCnt & ", in " & miDirsCnt & " dirs), " & miFilesLen \ 1024 & " MiB"
        If miErrMsg <> "" Then sRetVal = sRetVal & vbCrLf & "ERROR messages: " & miErrMsg

        oMD.Disconnect()

        Return sRetVal
    End Function
End Class
