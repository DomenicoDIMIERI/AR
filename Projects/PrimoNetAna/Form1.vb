Imports DMD.Sistema
Imports DMD
Imports DMD.Databases

Public Class Form1

    Private m_Conn As CDBConnection

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Public ReadOnly Property Conn As CDBConnection
        Get
            If (Me.m_Conn Is Nothing) Then
                Me.m_Conn = New CMdbDBConnection("db.mdb")
                Me.m_Conn.OpenDB()
            End If
            Return Me.m_Conn
        End Get
    End Property

End Class
