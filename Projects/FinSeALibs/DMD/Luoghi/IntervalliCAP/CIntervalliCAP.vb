Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    ''' <summary>
    ''' Collezione di CAP assegnati ad un comune
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CIntervalliCAP
        Inherits CCollection(Of CIntervalloCAP)

        Private m_Comune As CComune

        Public Sub New()
            Me.m_Comune = Nothing
        End Sub

        Public Sub New(ByVal comune As CComune)
            If (comune Is Nothing) Then Throw New ArgumentNullException("comune")
            Me.m_Comune = comune
            Me.Load()
        End Sub

        Public ReadOnly Property Comune As CComune
            Get
                Return Me.m_Comune
            End Get
        End Property

        Protected Friend Sub SetComune(ByVal value As CComune)
            Me.m_Comune = value
            For i As Integer = 0 To Me.Count - 1
                Dim tmp As CIntervalloCAP = Me(i)
                tmp.SetComune(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Comune IsNot Nothing) Then DirectCast(value, CIntervalloCAP).SetComune(Me.m_Comune)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Comune IsNot Nothing) Then DirectCast(newValue, CIntervalloCAP).SetComune(Me.m_Comune)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub Load()
            Me.Clear()
            If (GetID(Me.m_Comune) = 0) Then Exit Sub
            Dim i As Integer = 0
            Dim t As Boolean = False
            For i = 0 To Luoghi.Comuni.IntervalliCAP.Count - 1
                Dim item As CIntervalloCAP = Luoghi.Comuni.IntervalliCAP(i)
                If (t) Then
                    If (item.IDComune = GetID(Me.m_Comune)) Then
                        Me.Add(item)
                    Else
                        Exit For
                    End If
                Else
                    t = (item.IDComune = GetID(Me.m_Comune))
                    If (t) Then Me.Add(item)
                End If
            Next
            'Dim cursor As New CIntervalloCAPCursor
            'cursor.Da.SortOrder = SortEnum.SORT_ASC
            'cursor.IDComune.Value = GetID(Me.m_Comune)
            'While Not cursor.EOF
            '    Me.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Dispose()
        End Sub

    End Class


End Class