Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    <Serializable> _
    Public Class CommissioniPerUscitaCollection
        Inherits CCollection(Of CommissionePerUscita)

        Private m_Uscita As Uscita

        Public Sub New()
            Me.m_Uscita = Nothing
        End Sub

        Public Sub New(ByVal uscita As Uscita)
            Me.New()
            Me.Load(uscita)
        End Sub

        Public Overloads Function Add(ByVal commissione As Commissione, ByVal operatore As CUser, ByVal esito As String) As CommissionePerUscita
            If Me.m_Uscita Is Nothing Then Throw New ArgumentNullException("Uscita")
            Dim cxu As New CommissionePerUscita
            cxu.Commissione = commissione
            cxu.Uscita = Me.m_Uscita
            cxu.Operatore = operatore
            cxu.DescrizioneEsito = esito
            cxu.Stato = ObjectStatus.OBJECT_VALID
            cxu.Save()
            MyBase.Add(cxu)
            Return cxu
        End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Uscita IsNot Nothing) Then DirectCast(value, CommissionePerUscita).SetUscita(Me.m_Uscita)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Uscita IsNot Nothing) Then DirectCast(newValue, CommissionePerUscita).SetUscita(Me.m_Uscita)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal uscita As Uscita)
            If (uscita Is Nothing) Then Throw New ArgumentNullException("uscita")
            Me.Clear()
            Me.m_Uscita = uscita
            If (GetID(uscita) = 0) Then Return
            Dim cursor As New CommissioniPerUscitaCursor
            cursor.IDUscita.Value = GetID(uscita)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

        Friend Sub SetUscita(ByVal uscita As Uscita)
            Me.m_Uscita = uscita
            For Each item As CommissionePerUscita In Me
                item.SetUscita(uscita)
            Next
        End Sub

    End Class


End Class