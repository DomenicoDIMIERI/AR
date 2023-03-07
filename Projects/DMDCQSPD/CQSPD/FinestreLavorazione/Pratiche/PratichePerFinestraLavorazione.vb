#Const CaricaDocumentiOnLoad = True

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Office

Partial Public Class CQSPD

    <Serializable>
    Public Class PratichePerFinestraLavorazione
        Inherits CCollection(Of CRapportino)

        Private m_W As FinestraLavorazione

        Public Sub New()
            Me.m_W = Nothing
        End Sub

        Public Sub New(ByVal w As FinestraLavorazione)
            Me.New
            Me.Load(w)
        End Sub

        Public ReadOnly Property Finestra As FinestraLavorazione
            Get
                Return Me.m_W
            End Get
        End Property

        Protected Friend Sub SetFinestra(ByVal value As FinestraLavorazione)
            Me.m_W = value
            For Each p As CRapportino In Me
                p.SetFinestraLavorazione(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_W IsNot Nothing) Then DirectCast(value, CRapportino).SetFinestraLavorazione(Me.m_W)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_W IsNot Nothing) Then DirectCast(newValue, CRapportino).SetFinestraLavorazione(Me.m_W)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Protected Sub Load(ByVal w As FinestraLavorazione)
            If (w Is Nothing) Then Throw New ArgumentNullException("w")
            Me.SetFinestra(w)
            If (GetID(w) = 0) Then Return
            Dim cursor As New CRapportiniCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDFinestraLavorazione.Value = GetID(w)
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub



    End Class

End Class
