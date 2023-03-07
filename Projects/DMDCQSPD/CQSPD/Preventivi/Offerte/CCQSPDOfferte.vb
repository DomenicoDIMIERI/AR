Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags>
    Public Enum TEGCalcFlag As Integer
        SPREAD = 1
        PROVVIGIONE = 2
        PREMIOVITA = 4
        PREMIOIMPIEGO = 8
        PREMIOCREDITO = 16
        COMMISSIONI = 32
        INTERESSI = 64
        IMPOSTASOSTITUTIVA = 128
        ONERIERARIALI = 256
        ALTREIMPOSTE = 512
        SPESECONVENZIONI = 1024
        ALTRESPESE = 2048
        RIVALSA = 4096

        CALCOLOTEG_SPESEALL = SPESECONVENZIONI Or ALTRESPESE
    End Enum


    <Serializable> _
    Public Class CCQSPDOfferte
        Inherits CCollection(Of COffertaCQS)

        <NonSerialized> Private m_Preventivo As CPreventivo

        Public Sub New()
            Me.m_Preventivo = Nothing
        End Sub

        Public Sub New(ByVal owner As CPreventivo)
            Me.Load(owner)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Preventivo IsNot Nothing) Then DirectCast(value, COffertaCQS).SetPreventivo(Me.m_Preventivo)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Preventivo IsNot Nothing) Then DirectCast(newValue, COffertaCQS).SetPreventivo(Me.m_Preventivo)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal value As CPreventivo)
            Me.m_Preventivo = value
            For Each o As COffertaCQS In Me
                o.SetPreventivo(value)
            Next
        End Sub

        Protected Friend Sub Load(ByVal preventivo As CPreventivo)
            If (preventivo Is Nothing) Then Throw New ArgumentNullException("preventivo")
            MyBase.Clear()
            Me.m_Preventivo = preventivo
            If (GetID(preventivo) = 0) Then Exit Sub
            Dim cursor As New CCQSPDOfferteCursor
            Try
                cursor.PreventivoID.Value = GetID(preventivo)
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            Me.Comparer = Me.m_Preventivo
            Me.Sort()
        End Sub

    End Class

End Class
