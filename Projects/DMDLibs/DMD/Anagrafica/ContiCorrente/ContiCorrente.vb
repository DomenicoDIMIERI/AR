Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CContiCorrenteClass
        Inherits CGeneralClass(Of ContoCorrente)

        Friend Sub New()
            MyBase.New("modContiCorrente", GetType(ContoCorrenteCursor), 0)
        End Sub

        Function ParseIBAN(value As String) As String
            Return Strings.Replace(value, "  ", "")
        End Function

        Function ParseNumero(value As String) As String
            Return Strings.Replace(value, "  ", "")
        End Function

        Public Function GetItemByName(ByVal name As String) As ContoCorrente
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim cursor As New ContoCorrenteCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.Value = name
            cursor.StatoContoCorrente.SortOrder = SortEnum.SORT_ASC
            Dim ret As ContoCorrente = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Function GetItemByNumero(ByVal banca As CBanca, ByVal numero As String) As ContoCorrente
            If (GetID(banca) = 0) Then Return Nothing
            numero = Me.ParseNumero(numero)
            If (numero = "") Then Return Nothing
            Dim cursor As New ContoCorrenteCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Numero.Value = numero
            cursor.IDBanca.Value = GetID(banca)
            Dim ret As ContoCorrente = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Function Find(text As String) As CCollection(Of ContoCorrente)
            Dim ret As New CCollection(Of ContoCorrente)
            text = Trim(text)
            If (text = "") Then Return ret

            Dim cursor As New ContoCorrenteCursor
            cursor.Nome.Value = text & "%"
            cursor.Nome.Operator = OP.OP_LIKE
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Reset1()

            If (ret.Count = 0) Then
                cursor.Clear()
                cursor.Numero.Value = "%" & text & "%"
                cursor.Numero.Operator = OP.OP_LIKE
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End If

            cursor.Dispose()
            cursor = Nothing

            Return ret
        End Function

    End Class


End Namespace

Partial Public Class Anagrafica

    Private Shared m_ContiCorrente As CContiCorrenteClass = Nothing

    Public Shared ReadOnly Property ContiCorrente As CContiCorrenteClass
        Get
            If (m_ContiCorrente Is Nothing) Then m_ContiCorrente = New CContiCorrenteClass
            Return m_ContiCorrente
        End Get
    End Property



End Class