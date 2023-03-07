Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    ''' <summary>
    ''' Gestione dei dispositivi hardware assegnati ad un utente
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CDispositiviClass
        Inherits CGeneralClass(Of Dispositivo)

        Friend Sub New()
            MyBase.New("modOfficeDevices", GetType(DispositivoCursor), -1)
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeDevices")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeDevices")
                ret.Description = "Dispositivi"
                ret.DisplayName = "Dispositivi"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not DMD.Office.Database.Tables.ContainsKey("tbl_OfficeDevices") Then
                Dim table As CDBTable = DMD.Office.Database.Tables.Add("tbl_OfficeDevices")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("UserID", TypeCode.Int32)
                field = table.Fields.Add("Nome", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Tipo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Modello", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("DataAcquisto", TypeCode.DateTime)
                field = table.Fields.Add("DataDismissione", TypeCode.DateTime)
                field = table.Fields.Add("StatoDispositivo", TypeCode.Int32)
                field = table.Fields.Add("Seriale", TypeCode.String) : field.MaxLength = 255
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



        Public Function GetItemBySeriale(ByVal seriale As String) As Dispositivo
            seriale = Trim(seriale)
            If (seriale = vbNullString) Then Return Nothing
            Dim ret As Dispositivo
            For Each ret In Me.LoadAll
                If (Strings.Compare(ret.Seriale, seriale) = 0) Then Return ret
            Next
            Return Nothing
        End Function

        Public Function GetItemByName(ByVal name As String) As Dispositivo
            name = Trim(name)
            If (name = vbNullString) Then Return Nothing
            Dim ret As Dispositivo
            For Each ret In Me.LoadAll
                If (Strings.Compare(ret.Nome, name) = 0) Then Return ret
            Next
            Return Nothing
        End Function

    End Class


End Namespace

Partial Class Office



    Private Shared m_Dispotivi As CDispositiviClass = Nothing

    Public Shared ReadOnly Property Dispositivi As CDispositiviClass
        Get
            If (m_Dispotivi Is Nothing) Then m_Dispotivi = New CDispositiviClass
            Return m_Dispotivi
        End Get
    End Property


End Class