Imports System.IO
Imports System.Xml.Serialization

Public NotInheritable Class Dispositivi

    Private Sub New()
    End Sub

    Public Shared Function GetDispositivi() As System.Collections.ArrayList
        Dim ret As New System.Collections.ArrayList
        If (DIALTPLib.Settings.Dispositivi <> "") Then
            Dim m As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(DIALTPLib.Settings.Dispositivi))
            Dim r As New StreamReader(m)
            Dim x As New XmlSerializer(GetType(DispositivoEsterno()))
            Dim a() As DispositivoEsterno = DirectCast(x.Deserialize(r), DispositivoEsterno())
            ret.AddRange(a)
            r.Close()
            m.Dispose()
        End If
        Return ret
    End Function

    Public Shared Sub SetDispositivi(ByVal items As ArrayList)
        'Serialize object to a text file.
        Dim m As New System.IO.MemoryStream
        Dim w As New StreamWriter(m)
        Dim x As New XmlSerializer(GetType(DispositivoEsterno()))
        x.Serialize(w, items.ToArray(GetType(DispositivoEsterno)))
        Dim r As New StreamReader(m)
        m.Position = 0
        DIALTPLib.Settings.Dispositivi = r.ReadToEnd
        w.Dispose()
        r.Dispose()
        m.Dispose()
    End Sub







End Class
