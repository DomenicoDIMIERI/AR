Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public Class CContiOnlineClass
        Inherits CGeneralClass(Of ContoOnline)

        Public Sub New()
            MyBase.New("modContiOnline", GetType(ContoOnlineCursor), 0)
        End Sub

        Public Function GetItemByName(ByVal value As String) As ContoOnline
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim cursor As New ContoOnlineCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Name.Value = value
            Dim ret As ContoOnline = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Function GetItemByAccount(ByVal sito As String, ByVal account As String) As ContoOnline
            sito = Strings.Trim(sito)
            account = Strings.Trim(account)
            If (sito = "" OrElse account = "") Then Return Nothing
            Dim cursor As New ContoOnlineCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Sito.Value = sito
            cursor.Account.Value = account
            cursor.DataInizio.SortOrder = SortEnum.SORT_DESC
            Dim ret As ContoOnline = cursor.Item
            cursor.Dispose()
            Return ret
        End Function


    End Class
End Namespace


Partial Public Class Anagrafica

    Private Shared m_ContiOnline As CContiOnlineClass = Nothing

    Public Shared ReadOnly Property ContiOnline As CContiOnlineClass
        Get
            If m_ContiOnline Is Nothing Then m_ContiOnline = New CContiOnlineClass
            Return m_ContiOnline
        End Get
    End Property
 

End Class