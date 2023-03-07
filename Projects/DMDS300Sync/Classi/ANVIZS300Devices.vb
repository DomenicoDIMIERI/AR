Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Databases
Imports DMD.S300
Imports DMD.XML

<Serializable>
Public NotInheritable Class ANVIZS300Devices

    Private Shared m_Items As CCollection = Nothing

    Public Shared ReadOnly Property Items As CCollection
        Get
            If (m_Items Is Nothing) Then m_Items = GetItems()
            Return m_Items
        End Get
    End Property

    Private Shared Function GetItems() As CCollection
        Dim ret As CCollection
        Dim fn As String = GetConfigFileName()
        If System.IO.File.Exists(fn) Then
            Dim text As String = System.IO.File.ReadAllText(fn)
            ret = XML.Utils.Serializer.Deserialize(text)
        Else
            ret = New CCollection
        End If
        Return ret
    End Function

    Public Shared Sub Persist()
        Dim fn As String = GetConfigFileName()
        Dim text As String = XML.Utils.Serializer.Serialize(Items)
        System.IO.File.WriteAllText(fn, text)
    End Sub

    Public Shared Function GetConfigFileName() As String
        Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMDS300Sync\config.dmd")
    End Function


End Class
