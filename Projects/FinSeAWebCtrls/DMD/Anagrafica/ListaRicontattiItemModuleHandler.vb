Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CustomerCalls
Imports DMD.XML

Namespace Forms

    Public Class ListaRicontattiItemModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SDelete)
            Me.UseLocal = True
            'AddHandler CustomerCalls.CRM.NuovoContatto, AddressOf Me.handleNuovaTelefonata
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ListaRicontattoItemCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.ListeRicontatto.Items.GetItemById(id)
        End Function


    End Class



End Namespace