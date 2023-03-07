Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Gestione dei veicoli
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CVeicoliClass
        Inherits CGeneralClass(Of Veicolo)

        Friend Sub New()
            MyBase.New("modOfficeVeicoli", GetType(VeicoliCursor), -1)
        End Sub
 

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeVeicoli")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeVeicoli")
                ret.Description = "Veicoli"
                ret.DisplayName = "Veicoli"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficeVeicoli") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeVeicoli")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("Nome", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Tipo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Modello", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("DataAcquisto", TypeCode.DateTime)
                field = table.Fields.Add("DataDismissione", TypeCode.DateTime)
                field = table.Fields.Add("StatoVeicolo", TypeCode.Int32)
                field = table.Fields.Add("Alimentazione", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Seriale", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("IconURL", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("KmALitro", TypeCode.Single)
                field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32)
                field = table.Fields.Add("NomePuntoOperativo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function

 

        Public Function GetItemBySeriale(ByVal seriale As String) As Veicolo
            seriale = Trim(seriale)
            If (seriale = vbNullString) Then Return Nothing
            For Each item As Veicolo In Me.LoadAll
                If Strings.Compare(item.Seriale, seriale) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Function ParseTarga(ByVal targa As String) As String
            Return Strings.UCase(Strings.Replace(targa, " ", ""))
        End Function

        Function FormatTarga(ByVal targa As String) As String
            Return Strings.UCase(Strings.Replace(targa, " ", ""))
        End Function

    End Class


    Private Shared m_Veicoli As CVeicoliClass = Nothing

    Public Shared ReadOnly Property Veicoli As CVeicoliClass
        Get
            If (m_Veicoli Is Nothing) Then m_Veicoli = New CVeicoliClass
            Return m_Veicoli
        End Get
    End Property

End Class