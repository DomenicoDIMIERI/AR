Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace CQSPDInternals

    Public Class CMotiviScontoPraticaClass
        Inherits CGeneralClass(Of CQSPD.CMotivoScontoPratica)

        Public Sub New()
            MyBase.New("CQSPDMotiviScontoPratica", GetType(CQSPD.CMotivoScontoPraticaCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce un oggetto in base al suo nome (la ricerca è fatta solo sui motivi attivi)
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CQSPD.CMotivoScontoPratica
            Dim items As CCollection(Of CQSPD.CMotivoScontoPratica) = Me.LoadAll
            value = LCase(Trim(value))
            If (value = "") Then Return Nothing
            For Each m As CQSPD.CMotivoScontoPratica In items
                If (m.Attivo AndAlso LCase(m.Nome) = value) Then Return m
            Next

            Return Nothing
        End Function
    End Class

End Namespace


Partial Public Class CQSPD


    Private Shared m_MotiviSconto As CQSPDInternals.CMotiviScontoPraticaClass

    Public Shared ReadOnly Property MotiviSconto As CQSPDInternals.CMotiviScontoPraticaClass
        Get
            If (m_MotiviSconto Is Nothing) Then m_MotiviSconto = New CQSPDInternals.CMotiviScontoPraticaClass
            Return m_MotiviSconto
        End Get
    End Property



    


End Class
