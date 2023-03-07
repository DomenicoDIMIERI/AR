Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    <Serializable> _
    Public Class CIntestatariContoCorrente
        Inherits CCollection(Of IntestatarioContoCorrente)

        Private m_ContoCorrente As ContoCorrente

        Public Sub New()
            Me.m_ContoCorrente = Nothing
        End Sub

        Public Sub New(ByVal conto As ContoCorrente)
            Me.New()
            Me.Load(conto)
        End Sub

        Public ReadOnly Property ContoCorrente As ContoCorrente
            Get
                Return Me.m_ContoCorrente
            End Get
        End Property

        Protected Friend Overridable Sub SetContoCorrente(ByVal value As ContoCorrente)
            Me.m_ContoCorrente = value
            If (value IsNot Nothing) Then
                For Each c As IntestatarioContoCorrente In Me
                    c.SetContoCorrente(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_ContoCorrente IsNot Nothing) Then DirectCast(value, IntestatarioContoCorrente).SetContoCorrente(Me.m_ContoCorrente)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_ContoCorrente IsNot Nothing) Then DirectCast(newValue, IntestatarioContoCorrente).SetContoCorrente(Me.m_ContoCorrente)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal conto As ContoCorrente)
            If (conto Is Nothing) Then Throw New ArgumentNullException("conto")
            Me.Clear()
            Me.m_ContoCorrente = conto
            If (GetID(conto) = 0) Then Exit Sub
            Dim cursor As New IntestatarioContoCorrenteCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.IDContoCorrente.Value = GetID(conto)
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            Dim i As Integer = 0
            For Each Item As IntestatarioContoCorrente In Me
                If (i > 0) Then ret.Append(", ")
                ret.Append(Item.NomePersona)
                i += 1
            Next
            Return ret.ToString
        End Function
         
    End Class

End Class