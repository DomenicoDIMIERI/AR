Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 
    ''' <summary>
    ''' Provider predefinito delle fonti
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class FontiDefaultProvider
        Implements IFonteProvider

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function GetItemsAsArray(ByVal tipo As String, Optional ByVal onlyValid As Boolean = True) As IFonte() Implements IFonteProvider.GetItemsAsArray
            tipo = Trim(tipo)
            Dim ret As New CCollection(Of IFonte)
            'Dim cursor As New CFontiCursor
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            'cursor.OnlyValid = onlyValid
            'cursor.Nome.SortOrder = SortEnum.SORT_ASC
            'If (nome <> vbNullString) Then cursor.Tipo.Value = nome
            'While Not cursor.EOF
            '    ret.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Reset()
            Dim items As CCollection = Anagrafica.Fonti.LoadAll

            For i As Integer = 0 To items.Count - 1
                Dim f As CFonte = items(i)
                If (tipo = "" OrElse Strings.Compare(f.Tipo, tipo) = 0) Then ret.Add(f)
            Next
            Return ret.ToArray
        End Function



        Public Function GetItemById(ByVal tipo As String, id As Integer) As IFonte Implements IFonteProvider.GetItemById
            Return Fonti.GetItemById(id)
        End Function


        Public Function GetItemById(ByVal tipo As String, ByVal name As String) As IFonte Implements IFonteProvider.GetItemByName
            Return Fonti.GetItemByName(name)
        End Function

        Public Function GetSupportedNames() As String() Implements IFonteProvider.GetSupportedNames

            Dim items As New System.Collections.Hashtable
            For Each f As CFonte In Anagrafica.Fonti.LoadAll
                Dim t As String = f.Tipo
                If (items.ContainsKey(t) = False) Then items.Add(t, t)
            Next

            Dim ret() As String = Nothing
            If (items.Count > 0) Then
                ReDim ret(items.Count - 1)
                items.Keys.CopyTo(ret, 0)
                System.Array.Sort(ret)
            End If
            items = Nothing

            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class