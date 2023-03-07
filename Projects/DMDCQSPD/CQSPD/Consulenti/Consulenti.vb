Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica




Partial Public Class CQSPD


    Public NotInheritable Class CConsulentiClass
        Inherits CGeneralClass(Of CConsulentePratica)

        Friend Sub New()
            MyBase.New("CQSPDConsulenti", GetType(CConsulentiPraticaCursor), -1)
        End Sub
         
        Public Function GetItemByUser(ByVal user As CUser) As CConsulentePratica
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return GetItemByUser(GetID(user))
        End Function

        Public Function GetItemByUser(ByVal id As Integer) As CConsulentePratica
            If id = 0 Then Return Nothing

            For Each item As CConsulentePratica In Me.LoadAll
                If (item.IDUser = id) Then Return item
            Next


            Return Nothing
        End Function
         

         
    End Class

    Private Shared m_Consulenti As CConsulentiClass = Nothing

    Public Shared ReadOnly Property Consulenti As CConsulentiClass
        Get
            If (m_Consulenti Is Nothing) Then m_Consulenti = New CConsulentiClass
            Return m_Consulenti
        End Get
    End Property

End Class