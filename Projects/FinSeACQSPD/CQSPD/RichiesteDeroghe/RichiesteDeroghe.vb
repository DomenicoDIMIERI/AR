Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.CQSPD

Namespace Internals

    ''' <summary>
    ''' Modulo per la gestione delle richieste deroghe
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CRichiesteDerogheClass
        Inherits CGeneralClass(Of CRichiestaDeroga)

        Public Event Inviata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event Rivuta(ByVal sender As Object, ByVal e As ItemEventArgs)


        Public Sub New()
            MyBase.New("modCQSPDRichDeroghe", GetType(CRichiestaConteggioCursor), 0)
        End Sub

        Friend Sub doOnInviata(ByVal e As ItemEventArgs)
            RaiseEvent Inviata(Me, e)
            Me.Module.DispatchEvent(New EventDescription("inviata", "Richiesta Deroga Inviata", e.Item))
        End Sub

        Friend Sub doOnRicevuta(ByVal e As ItemEventArgs)
            RaiseEvent Rivuta(Me, e)
            Me.Module.DispatchEvent(New EventDescription("ricevuta", "Richiesta Deroga Ricevuta", e.Item))
        End Sub

        Protected Friend Shadows Sub doItemCreated(e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub


    End Class


End Namespace

Partial Public Class CQSPD



    Private Shared m_RichiesteDeroghe As CRichiesteDerogheClass = Nothing

    ''' <summary>
    ''' Modulo per la gestione delle richieste di deroghe alle offerte standard
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property RichiesteDeroghe As CRichiesteDerogheClass
        Get
            If m_RichiesteDeroghe Is Nothing Then m_RichiesteDeroghe = New CRichiesteDerogheClass
            Return m_RichiesteDeroghe
        End Get
    End Property
End Class
