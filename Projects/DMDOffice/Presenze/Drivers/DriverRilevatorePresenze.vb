Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office



    Public MustInherit Class DriverRilevatorePresenze
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce il nome del driver
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetName() As String

        ''' <summary>
        ''' Restituisce la versione del driver
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetVersion() As String

        ''' <summary>
        ''' Restituisce vero se il driver supporta il rilevatore 
        ''' </summary>
        ''' <returns></returns>
        Protected Friend MustOverride Function Supports(ByVal rilevatore As RilevatorePresenze) As Boolean

        ''' <summary>
        ''' Imposta l'orario sul dispositivo di rilavore presenze in modo che corrisponde con l'orario di questo sistema
        ''' </summary>
        ''' <param name="rilevatore"></param>
        Protected Friend MustOverride Sub SincronizzaOrario(ByVal rilevatore As RilevatorePresenze)

        ''' <summary>
        ''' Scarica le marcature dal rilevatore
        ''' </summary>
        ''' <param name="rilevatore"></param>
        Protected Friend MustOverride Function ScaricaNuoveMarcature(ByVal rilevatore As RilevatorePresenze) As CCollection(Of MarcaturaIngressoUscita)

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class