Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti che possiedono collezioni di tipo CSettings
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ISettingsOwner

        ''' <summary>
        ''' Restituisce la collezione delle impostazioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Settings As CSettings


    End Interface


End Class
