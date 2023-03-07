Imports System.Security.Principal
Imports DMD
Imports DMD.Sistema

Public Class Form1

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        Sistema.Types.Imports.Add("DMD")
    End Sub

    Public Sub Test()
        Dim c As New CKeyCollection
        c.SetItemByKey("ColoreProdotto", "#ec008c")
        c.SetItemByKey("ColoreTabellaFinanziaria", "#ec008c")
        c.SetItemByKey("PromemoriaPrecaricamento", Nothing)
        Dim xml As String = DMD.XML.Utils.Serializer.Serialize(c)

        Debug.Print(xml)

        c = DMD.XML.Utils.Serializer.Deserialize(xml)

        For Each k As String In c.Keys
            Debug.Print(k & " = " & c.GetItemByKey(k))
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Me.Test()
        Dim domain As String = ""
        Dim username As String = "DMD"

        Dim f As New NTAccount(domain & "\" & username)
        Dim s As SecurityIdentifier = f.Translate(GetType(SecurityIdentifier))
        Dim sidString As String = s.ToString()
        Debug.Print(sidString)
    End Sub
End Class
