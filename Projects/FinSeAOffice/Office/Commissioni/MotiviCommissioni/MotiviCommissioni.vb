Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    Partial Public Class CMotiviCommissioniClass
        Inherits CGeneralClass(Of MotivoCommissione)

        Friend Sub New()
            MyBase.New("modOfficeMotiviCommissioni", GetType(MotivoCommissioneCursor), -1)
        End Sub
 

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeMotiviCommissioni")
                ret.Description = "Motivi Commissioni"
                ret.DisplayName = "Motivi Commissioni"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficeCommissioniM") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeCommissioniM")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("Motivo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function

         
        Public Function GetMotiviAsCollection() As CCollection(Of String)
            'Dim cursor As New MotivoCommissioneCursor
            'Try
            '    Dim ret As New CCollection(Of String)
            '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            '    cursor.Motivo.SortOrder = SortEnum.SORT_ASC
            '    cursor.IgnoreRights = True
            '    While Not cursor.EOF
            '        ret.Add(cursor.Item.Motivo)
            '        cursor.MoveNext()
            '    End While
            '    Return ret
            'Catch ex As Exception
            '    Throw
            'Finally
            '    cursor.Dispose()
            'End Try
            Dim ret As New CCollection(Of String)
            For Each m As MotivoCommissione In Me.LoadAll
                ret.Add(m.Motivo)
            Next
            ret.Sort()
            Return ret
        End Function


    End Class

    Private Shared m_MotiviCommissioni As CMotiviCommissioniClass = Nothing

    Public Shared ReadOnly Property MotiviCommissioni As CMotiviCommissioniClass
        Get
            If (m_MotiviCommissioni Is Nothing) Then m_MotiviCommissioni = New CMotiviCommissioniClass
            Return m_MotiviCommissioni
        End Get
    End Property

End Class