Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals

Namespace Internals

    ''' <summary>
    ''' Gestione dei luoghi "attraversati" dall'operatore per effettuare la commissione
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class GPSRecordsClass
        Inherits CGeneralClass(Of GPSRecord)

        Public Sub New()
            MyBase.New("modOfficeGPSRecords", GetType(GPSRecordCursor), 0)
        End Sub

        'Protected Overrides Function InitializeModule() As CModule
        '    Return MyBase.InitializeModule()
        '    Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeGPSRecords")
        '    If (ret Is Nothing) Then
        '        ret = New CModule("modOfficeGPSRecords")
        '        ret.Description = "GPS Records"
        '        ret.DisplayName = "GPS Records"
        '        ret.Parent = Office.Module
        '        ret.Stato = ObjectStatus.OBJECT_VALID
        '        ret.Save()
        '        ret.InitializeStandardActions()
        '    End If
        '    If Not Office.Database.Tables.ContainsKey("tbl_OfficeLuoghiV") Then
        '        Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeLuoghiV")
        '        Dim field As CDBEntityField
        '        field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
        '        field = table.Fields.Add("UserID", TypeCode.Int32)
        '        field = table.Fields.Add("IDDispositivo", TypeCode.Int32)
        '        field = table.Fields.Add("ContextType", TypeCode.String) : field.MaxLength = 255
        '        field = table.Fields.Add("ContextID", TypeCode.Int32)
        '        field = table.Fields.Add("Istante1", TypeCode.DateTime)
        '        field = table.Fields.Add("Istante2", TypeCode.DateTime)
        '        field = table.Fields.Add("Latitudine", TypeCode.Double)
        '        field = table.Fields.Add("Longitudine", TypeCode.Double)
        '        field = table.Fields.Add("Altitudine", TypeCode.Double)
        '        table.Create()
        '    End If
        '    Return ret
        'End Function

        Public Function GetPosizioniDispositivo(ByVal dispositivo As Dispositivo, ByVal daIstante As Nullable(Of Date), ByVal aIstante As Nullable(Of Date)) As CCollection(Of GPSRecord)
            If (dispositivo Is Nothing) Then Throw New ArgumentNullException("dispositivo")
            Dim ret As New CCollection(Of GPSRecord)
            Dim cursor As New GPSRecordCursor
            cursor.IDDispositivo.Value = GetID(dispositivo)
            cursor.Istante1.SortOrder = SortEnum.SORT_ASC
            If (daIstante.HasValue) Then
                cursor.Istante1.Value = daIstante.Value
                If (aIstante.HasValue) Then
                    cursor.Istante1.Value1 = aIstante.Value
                    cursor.Istante1.Operator = OP.OP_BETWEEN
                Else
                    cursor.Istante1.Operator = OP.OP_GE
                End If
            Else
                If (aIstante.HasValue) Then
                    cursor.Istante1.Value = aIstante.Value
                    cursor.Istante1.Operator = OP.OP_LT
                End If
            End If
            While Not cursor.EOF
                Dim rec As GPSRecord = cursor.Item
                rec.SetDispositivo(dispositivo)
                ret.Add(rec)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function

    End Class

End Namespace

Partial Class Office

    Private Shared m_GPSRecords As GPSRecordsClass = Nothing

    ''' <summary>
    ''' Gestione dei luoghi "attraversati" dall'operatore per effettuare la commissione
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GPSRecords As GPSRecordsClass
        Get
            If (m_GPSRecords Is Nothing) Then m_GPSRecords = New GPSRecordsClass
            Return m_GPSRecords
        End Get
    End Property



End Class