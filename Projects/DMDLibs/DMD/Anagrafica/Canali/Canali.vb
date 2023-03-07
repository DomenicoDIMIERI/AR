Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals


    ''' <summary>
    ''' Gestione dei canali
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CCanaliClass
        Inherits CGeneralClass(Of CCanale)

        Friend Sub New()
            MyBase.New("modCanali", GetType(CCanaleCursor), -1)
        End Sub


        Public Function GetItemByName(ByVal value As String) As CCanale
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CCanale In Me.LoadAll
                If Strings.Compare(ret.Nome, value) = 0 Then Return ret
            Next
            Return Nothing
        End Function

        Public Function GetTipiCanale(Optional ByVal onlyValid As Boolean = True) As String()
            Dim ret() As String = {}
            For Each item As CCanale In Me.LoadAll
                If (Arrays.BinarySearch(ret, item.Tipo) < 0) Then ret = Arrays.InsertSorted(ret, item.Tipo)
            Next
            Return ret
        End Function

    End Class

End Namespace

Partial Public Class Anagrafica

    Private Shared m_Canali As CCanaliClass = Nothing

    Public Shared ReadOnly Property Canali As CCanaliClass
        Get
            If (m_Canali Is Nothing) Then m_Canali = New CCanaliClass
            Return m_Canali
        End Get
    End Property



End Class