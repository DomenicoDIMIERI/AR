Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    ''' <summary>
    ''' Gestione degli altri prestiti
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CAltriPrestitiClass
        Inherits CGeneralClass(Of CAltroPrestito)


        Friend Sub New()
            MyBase.New("modAltriPrestiti", GetType(CAltriPrestitiCursor), 0)
        End Sub
           
        Public Function FormatTipo(ByVal tipo As TipoEstinzione) As String
            Select Case tipo
                Case TipoEstinzione.ESTINZIONE_NO : Return ""
                Case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO : Return "CQS"
                Case TipoEstinzione.ESTINZIONE_CQP : Return "CQP"
                Case TipoEstinzione.ESTINZIONE_PRESTITODELEGA : Return "PD"
                Case TipoEstinzione.ESTINZIONE_PRESTITOPERSONALE : Return "Prestito Personale"
                Case TipoEstinzione.ESTINZIONE_PIGNORAMENTO : Return "Pignoramento"
                Case TipoEstinzione.ESTINZIONE_MUTUO : Return "Mutuo"
                Case TipoEstinzione.ESTINZIONE_PROTESTI : Return "Protesti"
                Case TipoEstinzione.ESTINZIONE_ASSICURAZIONE : Return "Assicurazione"
                Case TipoEstinzione.ESTINZIONE_ALIMENTI : Return "Alimenti"
                Case Else : Return "invalid"
            End Select
        End Function


    End Class

    Private Shared m_AltriPrestiti As CAltriPrestitiClass = Nothing

    Public Shared ReadOnly Property AltriPrestiti As CAltriPrestitiClass
        Get
            If (m_AltriPrestiti Is Nothing) Then m_AltriPrestiti = New CAltriPrestitiClass
            Return m_AltriPrestiti
        End Get
    End Property

End Class