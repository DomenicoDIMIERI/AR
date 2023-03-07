Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    ''' <summary>
    ''' Collezione di impiegati di un'azienda
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImpiegati
        Inherits CCollection(Of CImpiegato)

        <NonSerialized> _
        Private m_Azienda As CAzienda

        Public Sub New()
            Me.m_Azienda = Nothing
        End Sub

        Public Sub New(ByVal azienda As CAzienda)
            Me.New
            Me.LoadAzienda(azienda)
        End Sub

        Protected Friend Sub SetAzienda(ByVal value As CAzienda)
            Me.m_Azienda = value
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Azienda IsNot Nothing) Then DirectCast(value, CImpiegato).SetAzienda(Me.m_Azienda)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Azienda IsNot Nothing) Then DirectCast(newValue, CImpiegato).SetAzienda(Me.m_Azienda)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Function AddImpiegato(ByVal persona As CPersonaFisica, ByVal ufficio As String, ByVal posizione As String) As CImpiegato
            Dim item As New CImpiegato
            With item
                .Azienda = Me.m_Azienda
                .Persona = persona
                .Ufficio = ufficio
                .Posizione = posizione
                .Stato = ObjectStatus.OBJECT_VALID
            End With
            MyBase.Add(item)
            Return item
        End Function

        Protected Friend Function LoadAzienda(ByVal azienda As CAzienda) As Boolean
            If (azienda Is Nothing) Then Throw New ArgumentNullException("azienda")
            MyBase.Clear()
            Me.m_Azienda = azienda
            If (GetID(azienda) = 0) Then Return True
            Dim conn As CDBConnection = azienda.GetConnection
            If (conn.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Aziende&_a=getImpiegati", "aid", RPC.int2n(GetID(azienda)))
                If (ret <> "") Then Me.AddRange(XML.Utils.Serializer.Deserialize(ret))
            Else
                Dim cursor As New CImpiegatiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.AziendaID.Value = GetID(azienda)
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                While Not cursor.EOF
                    If Not cursor.Item Is Nothing Then MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If

            Return True
        End Function

    End Class


End Class
