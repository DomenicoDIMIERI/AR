Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    Public NotInheritable Class CClassiDispositiviClass
        Inherits CGeneralClass(Of ClasseDispositivo)

        Friend Sub New()
            MyBase.New("modOfficeDevClass", GetType(ClassiDispositivoCursor), -1)
        End Sub

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeDevClass")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeDevClass")
                ret.Description = "Classi Dispositivi"
                ret.DisplayName = "Classi Dispositivi"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficeDevClass") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeDevClass")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("Nome", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Flags", TypeCode.Int32)
                field = table.Fields.Add("Params", TypeCode.String) : field.MaxLength = 0
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function

        Public Function GetItemByName(ByVal nome As String) As ClasseDispositivo
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each cls As ClasseDispositivo In Me.LoadAll
                If String.Compare(cls.Nome, nome, True) = 0 Then Return cls
            Next
            Return Nothing
        End Function
    End Class

    Private Shared m_ClassiDispositivo As CClassiDispositiviClass = Nothing

    Public Shared ReadOnly Property ClassiDispositivi As CClassiDispositiviClass
        Get
            If (m_ClassiDispositivo Is Nothing) Then m_ClassiDispositivo = New CClassiDispositiviClass
            Return m_ClassiDispositivo
        End Get
    End Property

End Class