Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 
    ''' <summary>
    ''' Interfaccia implementata dagli oggetto che fungono da generatori di fonti
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IFonteProvider

        ''' <summary>
        ''' Restituisce un array contenente i nomi delle fonti gestiti da questo oggetto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSupportedNames() As String()

        ''' <summary>
        ''' Restituisce un array delle fonti corrispondenti al nome specificato
        ''' </summary>
        ''' <param name="onlyValid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetItemsAsArray(ByVal nome As String, Optional ByVal onlyValid As Boolean = True) As IFonte()

        ''' <summary>
        ''' Restituisce la fonte in base all'ID
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetItemById(ByVal nome As String, ByVal id As Integer) As IFonte

        ''' <summary>
        ''' Restituisce la fonte in base al nome
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetItemByName(ByVal nome As String, ByVal name As String) As IFonte

    End Interface

   


End Class