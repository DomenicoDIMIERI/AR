Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CDistributoriClass
        Inherits CGeneralClass(Of CDistributore)

        Friend Sub New()
            MyBase.New("modDistributori", GetType(CDistributoriCursor), -1)
        End Sub


        Public Function GetItemByName(ByVal value As String) As CDistributore
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each item As CDistributore In Me.LoadAll
                If Strings.Compare(item.Nome, value) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

End Namespace

Partial Public Class Anagrafica



    Private Shared m_Distributori As CDistributoriClass = Nothing

    Public Shared ReadOnly Property Distributori As CDistributoriClass
        Get
            If (m_Distributori Is Nothing) Then m_Distributori = New CDistributoriClass
            Return m_Distributori
        End Get
    End Property

End Class