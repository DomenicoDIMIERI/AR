Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


 

    <Serializable> _
    Public Class CAziendaUffici
        Inherits CCollection(Of CUfficio)

        <NonSerialized> _
        Private m_Azienda As CAzienda

        Public Sub New()
            Me.m_Azienda = Nothing
        End Sub

        Public Sub New(ByVal azienda As CAzienda)
            Me.New()
            Me.Load(azienda)
        End Sub

        Public ReadOnly Property Azienda As CAzienda
            Get
                Return Me.m_Azienda
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Azienda IsNot Nothing) Then DirectCast(value, CUfficio).SetAzienda(Me.m_Azienda)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Azienda IsNot Nothing) Then DirectCast(newValue, CUfficio).SetAzienda(Me.m_Azienda)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal value As CAzienda)
            If (value Is Nothing) Then Throw New ArgumentNullException("azienda")
            MyBase.Clear()
            Me.m_Azienda = value
            If (GetID(value) = 0) Then Return

            Dim cursor As New CUfficiCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDAzienda.Value = GetID(value)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub


    End Class

End Class