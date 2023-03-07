Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public Class CCarteDiCreditoClass
        Inherits CGeneralClass(Of CartaDiCredito)

        Public Sub New()
            MyBase.New("modCarteDiCredito", GetType(CartaDiCreditoCursor), 0)
        End Sub

        Public Function GetItemByName(ByVal value As String) As CartaDiCredito
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim cursor As New CartaDiCreditoCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Name.Value = value
            Dim ret As CartaDiCredito = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Function GetItemByNumero(ByVal circuito As String, ByVal numeroCarta As String) As CartaDiCredito
            circuito = Strings.Trim(circuito)
            numeroCarta = Strings.Trim(numeroCarta)
            If (circuito = "" OrElse numeroCarta = "") Then Return Nothing
            Dim cursor As New CartaDiCreditoCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.CircuitoCarta.Value = circuito
            cursor.NumeroCarta.Value = numeroCarta
            cursor.DataInizio.SortOrder = SortEnum.SORT_DESC
            Dim ret As CartaDiCredito = cursor.Item
            cursor.Dispose()
            Return ret
        End Function


    End Class
End Namespace


Partial Public Class Anagrafica

    Private Shared m_CarteDiCredito As CCarteDiCreditoClass = Nothing

    Public Shared ReadOnly Property CarteDiCredito As CCarteDiCreditoClass
        Get
            If m_CarteDiCredito Is Nothing Then m_CarteDiCredito = New CCarteDiCreditoClass
            Return m_CarteDiCredito
        End Get
    End Property
 

End Class