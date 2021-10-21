
Imports System.Xml.Serialization

<XmlType("folder")>
Public Class TypeOneFolder
    <XmlAttribute()>
    Public Property Nazwa As String
    <XmlAttribute()>
    Public Property sSrcPath As String
    <XmlAttribute()>
    Public Property sDstPath As String
    <XmlAttribute()>
    Public Property iDelAfterDays As Integer
    <XmlAttribute()>
    Public Property bDisabled As Boolean
End Class

Public Class TypeFoldersCopy
    Private moItems As ObservableCollection(Of TypeOneFolder)
    Private mbDirty = False
    Private ReadOnly msFileName = "foldersy.xml"

End Class
