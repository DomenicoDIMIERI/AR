Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class MotiviAppuntamentoClass
        Inherits CGeneralClass(Of MotivoAppuntamento)


        Friend Sub New()
            MyBase.New("modCRMMotiviAppuntamento", GetType(MotiviAppuntamentoCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal nome As String) As MotivoAppuntamento
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each item As MotivoAppuntamento In Me.LoadAll
                If Strings.Compare(item.Nome, nome, CompareMethod.Text) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemsByTipoAppuntamento(ByVal tipo As String) As CCollection(Of MotivoAppuntamento)
            Dim ret As New CCollection(Of MotivoAppuntamento)
            tipo = Strings.Trim(tipo)
            If (tipo = "") Then Return ret
            For Each item As MotivoAppuntamento In Me.LoadAll
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso
                        Sistema.TestFlag(item.Flags, MotivoAppuntamentoFlags.Attivo) AndAlso
                        (item.TipoAppuntamento = "" OrElse
                    Strings.Compare(item.TipoAppuntamento, tipo, CompareMethod.Text) = 0) Then
                    ret.Add(item)
                End If
            Next
            Return ret
        End Function

    End Class


End Namespace

Partial Public Class Anagrafica



    Private Shared m_MotiviAppuntamento As MotiviAppuntamentoClass = Nothing

    Public Shared ReadOnly Property MotiviAppuntamento As MotiviAppuntamentoClass
        Get
            If (m_MotiviAppuntamento Is Nothing) Then m_MotiviAppuntamento = New MotiviAppuntamentoClass
            Return m_MotiviAppuntamento
        End Get
    End Property

End Class