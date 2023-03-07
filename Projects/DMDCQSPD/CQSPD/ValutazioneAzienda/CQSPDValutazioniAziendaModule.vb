Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals

    <Serializable>
    Public NotInheritable Class CQSPDValutazioniAziendaModule
        Inherits CGeneralClass(Of CQSPDValutazioneAzienda)

        Friend Sub New()
            MyBase.New("modCQSPDValutazioniAzienda", GetType(CQSPDValutazioneAziendaCursor), 0)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.DisplayName = "Valutazioni Aziende"
            m.ClassHandler = "ValutazioniAziendeHandler"
            m.Save()
            Return m
        End Function

        Public Function GetUltimaValutazioneAzienda(ByVal a As CAzienda) As CQSPDValutazioneAzienda
            If (a Is Nothing) Then Throw New ArgumentNullException("a")
            Using cursor As New CQSPDValutazioneAziendaCursor
                cursor.IDAzienda.Value = GetID(a)
                cursor.DataRevisione.SortOrder = SortEnum.SORT_DESC
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Return cursor.Item
            End Using
        End Function

    End Class

End Namespace


Partial Public Class CQSPD

    Private Shared m_ValutazioniAzienda As CQSPDValutazioniAziendaModule = Nothing

    Public Shared ReadOnly Property ValutazioniAzienda As CQSPDValutazioniAziendaModule
        Get
            If (m_ValutazioniAzienda Is Nothing) Then m_ValutazioniAzienda = New CQSPDValutazioniAziendaModule
            Return m_ValutazioniAzienda
        End Get
    End Property


End Class
