Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica


Namespace Internals

    Public NotInheritable Class CFormeGiuridicheAziendaClass
        Inherits CGeneralClass(Of CFormaGiuridicaAzienda)

        Friend Sub New()
            MyBase.New("modFormeGiuridicheAzienda", GetType(CFormeGiuridicheAziendaCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal value As String) As CFormaGiuridicaAzienda
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CFormaGiuridicaAzienda) = Me.LoadAll
            For Each f As CFormaGiuridicaAzienda In items
                If (f.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(f.Nome, value, CompareMethod.Text) = 0) Then Return f
            Next

            Return Nothing
        End Function

    End Class

End Namespace


Partial Public Class Anagrafica



    Private Shared m_FormeGiuridicheAzienda As CFormeGiuridicheAziendaClass = Nothing

    Public Shared ReadOnly Property FormeGiuridicheAzienda As CFormeGiuridicheAziendaClass
        Get
            If (m_FormeGiuridicheAzienda Is Nothing) Then m_FormeGiuridicheAzienda = New CFormeGiuridicheAziendaClass
            Return m_FormeGiuridicheAzienda
        End Get
    End Property

End Class