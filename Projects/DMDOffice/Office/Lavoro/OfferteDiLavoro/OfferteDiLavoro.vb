Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    Public NotInheritable Class COfferteDiLavoroClass
        Inherits CGeneralClass(Of OffertaDiLavoro)

        Friend Sub New()
            MyBase.New("modOfficeOfferteDiLavoro", GetType(OffertaDiLavoroCursor))
        End Sub

        Public Function GetCandidature(ByVal oid As Integer) As CCollection(Of Candidatura)
            Dim ret As New CCollection(Of Candidatura)
            If (oid = 0) Then Return ret
            Dim cursor As New CandidaturaCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDOfferta.Value = oid
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
            Return ret
        End Function

        Public Function GetCandidature(ByVal offerta As OffertaDiLavoro) As CCollection(Of Candidatura)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")
            Return Me.GetCandidature(GetID(offerta))
        End Function

    End Class

    Private Shared m_OfferteDiLavoro As COfferteDiLavoroClass = Nothing

    Public Shared ReadOnly Property OfferteDiLavoro As COfferteDiLavoroClass
        Get
            If (m_OfferteDiLavoro Is Nothing) Then m_OfferteDiLavoro = New COfferteDiLavoroClass
            Return m_OfferteDiLavoro
        End Get
    End Property


End Class