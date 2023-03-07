Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    Public Class CPersonStatsCursor
        Inherits DBObjectCursorBase(Of CPersonStats)

        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_IDUltimaVisita As New CCursorField(Of Integer)("IDUltimaVisita")
        Private m_DataUltimaVisita As New CCursorField(Of Date)("UltimaVisita")
        Private m_UltimoContattoNoID As New CCursorField(Of Integer)("IDTelefonata")
        Private m_UltimoContattoOkID As New CCursorField(Of Integer)("IDTelefonataOk")
        Private m_DataUltimoContattoNo As New CCursorField(Of Date)("DataUltimaTelefonata")
        Private m_DataUltimoContattoOk As New CCursorField(Of Date)("DataUltimaTelefonataOk")
        Private m_ConteggioRisp As New CCursorField(Of Integer)("ConteggioRisp")
        Private m_ConteggioNoRispo As New CCursorField(Of Integer)("ConteggioNoRispo")
        Private m_ConteggioTelefonate As New CCursorField(Of Integer)("[ConteggioRisp] + [ConteggioNoRispo]")
        Private m_FrequenzaRisposta As New CCursorField(Of Double)("IIF([ConteggioRisp] + [ConteggioNoRispo]>0, [ConteggioRisp]/([ConteggioRisp] + [ConteggioNoRispo]), 0)")
        Private m_Flags As New CCursorField(Of PersonFlags)("Flags")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_IDPuntoOperativo As New CCursorField(Of Integer)("IDPuntoOperativo")
        Private m_MotivoProssimoRicontatto As New CCursorFieldObj(Of String)("MotivoProssimoRicontatto")
        Private m_DataProssimoRicontatto As New CCursorField(Of Date)("DataProssimoRicontatto")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        'Private m_DataAggiornamento As New CCursorField(Of Date)("DataAggiornamento")
        Private m_DataAggiornamento As New CCursorFieldDBDate("DataAggiornamento1")
        Private m_DataUltimaOperazione As New CCursorField(Of Date)("DataUltimaOperazione")
        Private m_DataUltimoContatto As New CCursorField(Of Date)("IIF([DataUltimaTelefonata]>[DataUltimaTelefonataOk],[DataUltimaTelefonata],[DataUltimaTelefonataOk])")
        Private m_Gravita As New CCursorField(Of Integer)("Gravita")
        Private m_ConteggioCondizioni As New CCursorField(Of Integer)("ContaCondizioni")
        Private m_DettaglioEsito As New CCursorFieldObj(Of String)("DettaglioEsito")
        Private m_DettaglioEsito1 As New CCursorFieldObj(Of String)("DettaglioEsito1")

        Public Sub New()
        End Sub

        Public ReadOnly Property ConteggioTelefonate As CCursorField(Of Integer)
            Get
                Return Me.m_ConteggioTelefonate
            End Get
        End Property

        Public ReadOnly Property FrequenzaRisposta As CCursorField(Of Double)
            Get
                Return Me.m_FrequenzaRisposta
            End Get
        End Property


        Public ReadOnly Property DettaglioEsito As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsito
            End Get
        End Property

        Public ReadOnly Property DettaglioEsito1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsito1
            End Get
        End Property


        Public ReadOnly Property IDUltimaVisita As CCursorField(Of Integer)
            Get
                Return Me.m_IDUltimaVisita
            End Get
        End Property

        Public ReadOnly Property DataUltimaVisita As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimaVisita
            End Get
        End Property

        Public ReadOnly Property Gravita As CCursorField(Of Integer)
            Get
                Return Me.m_Gravita
            End Get
        End Property

        Public Overrides Function GetSQL() As String
            Return MyBase.GetSQL()
        End Function

        Public ReadOnly Property ConteggioCondizioni As CCursorField(Of Integer)
            Get
                Return Me.m_ConteggioCondizioni
            End Get
        End Property


        Public ReadOnly Property DataUltimaOperazione As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimaOperazione
            End Get
        End Property

        Public ReadOnly Property DataUltimoContatto As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoContatto
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property


        Public ReadOnly Property MotivoProssimoRicontatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoProssimoRicontatto
            End Get
        End Property

        Public ReadOnly Property DataProssimoRicontatto As CCursorField(Of Date)
            Get
                Return Me.m_DataProssimoRicontatto
            End Get
        End Property

        'Public ReadOnly Property DataAggiornamento As CCursorField(Of Date)
        '    Get
        '        Return Me.m_DataAggiornamento
        '    End Get
        'End Property

        Public ReadOnly Property DataAggiornamento As CCursorFieldDBDate
            Get
                Return Me.m_DataAggiornamento
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property UltimoContattoOkID As CCursorField(Of Integer)
            Get
                Return Me.m_UltimoContattoOkID
            End Get
        End Property

        Public ReadOnly Property UltimoContattoNoID As CCursorField(Of Integer)
            Get
                Return Me.m_UltimoContattoNoID
            End Get
        End Property

        Public ReadOnly Property DataUltimoContattoOk As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoContattoOk
            End Get
        End Property

        Public ReadOnly Property DataUltimoContattoNo As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoContattoNo
            End Get
        End Property

        Public ReadOnly Property ConteggioRisp As CCursorField(Of Integer)
            Get
                Return Me.m_ConteggioRisp
            End Get
        End Property

        Public ReadOnly Property ConteggioNoRispo As CCursorField(Of Integer)
            Get
                Return Me.m_ConteggioNoRispo
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of PersonFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property IDPuntoOperativo As CCursorField(Of Integer)
            Get
                Return Me.m_IDPuntoOperativo
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.StatsDB
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UltimaChiamata"
        End Function

        'Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
        '    Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()
        '    Dim f As CCursorField(Of Date) = ret.GetItemByKey("DataAggiornamento")
        '    ret.Remove(f)
        '    Dim field As New CCursorFieldObj(Of String)("DataAggiornamento1")
        '    ret.Add("DataAggiornamento1", field)
        '    If f.IsSet Then
        '        field.Value = DBUtils.ToDBDateStr(f.Value)
        '        field.Value1 = DBUtils.ToDBDateStr(f.Value1)
        '        field.SortOrder = f.SortOrder
        '        field.IncludeNulls = f.IncludeNulls
        '        field.Operator = f.Operator
        '    End If
        '    Return ret
        'End Function

        'Protected Overrides Function GetSortFields() As CKeyCollection(Of CCursorField)
        '    Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()
        '    Dim f As CCursorField(Of Date) = ret.GetItemByKey("DataAggiornamento")
        '    ret.Remove(f)
        '    Dim field As New CCursorFieldObj(Of String)("DataAggiornamento1")
        '    ret.Add("DataAggiornamento1", field)
        '    If f.IsSet Then
        '        field.SortOrder = f.SortOrder
        '    End If
        '    Return ret
        'End Function

        Public Overrides Function GetWherePart() As String
            Return MyBase.GetWherePart()
        End Function

    End Class


End Class