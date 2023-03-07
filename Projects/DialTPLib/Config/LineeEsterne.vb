Imports System.IO
Imports System.Xml.Serialization

Public NotInheritable Class LineeEsterne

    Private Sub New()
    End Sub

    Public Shared Function GetLinee() As System.Collections.ArrayList
        Dim ret As New System.Collections.ArrayList
        If (DIALTPLib.Settings.Linee <> "") Then
            Dim m As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(DIALTPLib.Settings.Linee))
            Dim r As New StreamReader(m)
            Dim x As New XmlSerializer(GetType(LineaEsterna()))
            Dim a() As LineaEsterna = DirectCast(x.Deserialize(r), LineaEsterna())
            ret.AddRange(a)
            r.Close()
            m.Dispose()
        
        End If
        Return ret
    End Function

    Public Shared Sub SetLinee(ByVal items As ArrayList)
        'Serialize object to a text file.
        Dim m As New System.IO.MemoryStream
        Dim w As New StreamWriter(m)
        Dim x As New XmlSerializer(GetType(LineaEsterna()))
        x.Serialize(w, items.ToArray(GetType(LineaEsterna)))
        Dim r As New StreamReader(m)
        m.Position = 0
        DIALTPLib.Settings.Linee = r.ReadToEnd
        'DIALTPLib.Settings.Save()
        w.Dispose()
        r.Dispose()
        m.Dispose()
    End Sub







End Class
