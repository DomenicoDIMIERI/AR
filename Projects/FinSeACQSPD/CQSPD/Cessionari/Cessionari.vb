Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Gestione degli istituti cessionari
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CCessionariClass
        Inherits CGeneralClass(Of CCQSPDCessionarioClass)

        
        Friend Sub New()
            MyBase.New("modFINCessionari", GetType(CCessionariCursor), -1)
        End Sub
  

        ''' <summary>
        ''' Restituisce il cessionario in base al suo nome. 
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CCQSPDCessionarioClass
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CCQSPDCessionarioClass In Me.LoadAll
                If (Strings.Compare(value, ret.Nome, CompareMethod.Text) = 0) Then Return ret
            Next
            Return Nothing
        End Function



        ''' <summary>
        ''' Restituisce un array contenente l'elenco di tutti i cessionari attivi ed inattivi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetArrayCessionari() As CCQSPDCessionarioClass()
            Return GetAllCessionari.ToArray
        End Function

        ''' <summary>
        ''' Restituisce un array base 0 contenente tutti gli oggetti CCessionario validi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetArrayCessionariValidi() As CCQSPDCessionarioClass()
            Dim ret As New CCollection(Of CCQSPDCessionarioClass)
            For Each c As CCQSPDCessionarioClass In Me.LoadAll
                If (c.IsValid) Then ret.Add(c)
            Next
            Return ret.ToArray
        End Function

        Public Function GetAllCessionari() As CCollection(Of CCQSPDCessionarioClass)
            Return New CCollection(Of CCQSPDCessionarioClass)(Me.CachedItems)
        End Function

    End Class

    Private Shared m_Cessionari As CCessionariClass = Nothing

    Public Shared ReadOnly Property Cessionari As CCessionariClass
        Get
            If (m_Cessionari Is Nothing) Then m_Cessionari = New CCessionariClass
            Return m_Cessionari
        End Get
    End Property

End Class