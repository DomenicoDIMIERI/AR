Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Class CPersonaFisicaCursor
        Inherits CPersonaCursor

        Private m_Impiego_IDAzienda As New CCursorField(Of Integer)("IMP_IDAzienda")
        Private m_Impiego_NomeAzienda As New CCursorFieldObj(Of String)("IMP_NomeAzienda")
        Private m_Impiego_IDEntePagante As New CCursorField(Of Integer)("IMP_IDEntePagante")
        Private m_Impiego_NomeEntePagante As New CCursorFieldObj(Of String)("IMP_NomeEntePagante")
        Private m_Impiego_Posizione As New CCursorFieldObj(Of String)("IMP_Posizione")
        Private m_Impiego_DataAssunzione As New CCursorField(Of Date)("IMP_DataAssunzione")
        Private m_Impiego_DataLicenziamento As New CCursorField(Of Date)("IMP_DataLicenziamento")
        Private m_Impiego_StipendioNetto As New CCursorField(Of Decimal)("IMP_StipendioNetto")
        Private m_Impiego_StipendioLordo As New CCursorField(Of Decimal)("IMP_StipendioLordo")
        Private m_Impiego_TipoContratto As New CCursorFieldObj(Of String)("IMP_TipoContratto")
        Private m_Impiego_TipoRapporto As New CCursorFieldObj(Of String)("IMP_TipoRapporto")
        Private m_Impiego_TFR As New CCursorField(Of Decimal)("IMP_TFR")
        Private m_Impiego_MensilitaPercepite As New CCursorField(Of Integer)("IMP_MensilitaPercepite")
        Private m_Impiego_PercTFRAzienda As New CCursorField(Of Single)("IMP_PercTFRAzienda")
        Private m_Impiego_NomeFPC As New CCursorFieldObj(Of String)("IMP_NomeFPC")

        Private m_Impiego_CategoriaAzienda As New CCursorFieldObj(Of String)("CategoriaAzienda")
        Private m_Impiego_TipologiaAzienda As New CCursorFieldObj(Of String)("TipologiaAzienda")
        ' Private m_TipoRapportoDiLavoro As New CCursorFieldObj(Of String)("TipologiaRapportoDiLavoro")

        Private m_StatoCivile As New CCursorFieldObj(Of String)("StatoCivile")
        Private m_Disabilita As New CCursorFieldObj(Of String)("Categoria")

        Public Sub New()
            Me.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA
        End Sub

        Public ReadOnly Property Disabilita As CCursorFieldObj(Of String)
            Get
                Return Me.m_Disabilita
            End Get
        End Property

        Public Shadows Property Item As CPersonaFisica
            Get
                Return MyBase.Item
            End Get
            Set(value As CPersonaFisica)
                MyBase.Item = value
            End Set
        End Property

        Public ReadOnly Property Impiego_CategoriaAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_CategoriaAzienda
            End Get
        End Property

        Public ReadOnly Property Impiego_TipologiaAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_TipologiaAzienda
            End Get
        End Property

        'Public ReadOnly Property TipoRapportoDiLavoro As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_TipoRapportoDiLavoro
        '    End Get
        'End Property


        Public ReadOnly Property StatoCivile As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatoCivile
            End Get
        End Property

        Public ReadOnly Property Impiego_IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_Impiego_IDAzienda
            End Get
        End Property

        Public ReadOnly Property Impiego_NomeAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_NomeAzienda
            End Get
        End Property

        Public ReadOnly Property Impiego_IDEntePagante As CCursorField(Of Integer)
            Get
                Return Me.m_Impiego_IDEntePagante
            End Get
        End Property

        Public ReadOnly Property Impiego_NomeEntePagante As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_NomeEntePagante
            End Get
        End Property

        Public ReadOnly Property Impiego_Posizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_Posizione
            End Get
        End Property

        Public ReadOnly Property Impiego_DataAssunzione As CCursorField(Of Date)
            Get
                Return Me.m_Impiego_DataAssunzione
            End Get
        End Property

        Public ReadOnly Property Impiego_DataLicenziamento As CCursorField(Of Date)
            Get
                Return Me.m_Impiego_DataLicenziamento
            End Get
        End Property

        Public ReadOnly Property Impiego_StipendioNetto As CCursorField(Of Decimal)
            Get
                Return Me.m_Impiego_StipendioNetto
            End Get
        End Property

        Public ReadOnly Property Impiego_StipendioLordo As CCursorField(Of Decimal)
            Get
                Return Me.m_Impiego_StipendioLordo
            End Get
        End Property

        Public ReadOnly Property Impiego_TipoContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_TipoContratto
            End Get
        End Property

        Public ReadOnly Property Impiego_TipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_TipoRapporto
            End Get
        End Property

        Public ReadOnly Property Impiego_TFR As CCursorField(Of Decimal)
            Get
                Return Me.m_Impiego_TFR
            End Get
        End Property

        Public ReadOnly Property Impiego_MensilitaPercepite As CCursorField(Of Integer)
            Get
                Return Me.m_Impiego_MensilitaPercepite
            End Get
        End Property

        Public ReadOnly Property Impiego_PercTFRAzienda As CCursorField(Of Single)
            Get
                Return Me.m_Impiego_PercTFRAzienda
            End Get
        End Property

        Public ReadOnly Property Impiego_NomeFPC As CCursorFieldObj(Of String)
            Get
                Return Me.m_Impiego_NomeFPC
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CPersonaFisica
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Persone.Module
        End Function

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As New CKeyCollection(Of CCursorField)(MyBase.GetWhereFields())
            ret.Remove(Me.m_Impiego_CategoriaAzienda)
            ret.Remove(Me.m_Impiego_TipologiaAzienda)
            ret.Remove(Me.m_Impiego_IDAzienda)
            ret.Remove(Me.m_Impiego_NomeAzienda)
            ret.Remove(Me.m_Impiego_IDEntePagante)
            ret.Remove(Me.m_Impiego_NomeEntePagante)
            ret.Remove(Me.m_Impiego_Posizione)
            ret.Remove(Me.m_Impiego_DataAssunzione)
            ret.Remove(Me.m_Impiego_DataLicenziamento)
            ret.Remove(Me.m_Impiego_StipendioNetto)
            ret.Remove(Me.m_Impiego_StipendioLordo)
            ret.Remove(Me.m_Impiego_TipoContratto)
            ret.Remove(Me.m_Impiego_TipoRapporto)
            ret.Remove(Me.m_Impiego_TFR)
            ret.Remove(Me.m_Impiego_MensilitaPercepite)
            ret.Remove(Me.m_Impiego_PercTFRAzienda)
            ret.Remove(Me.m_Impiego_NomeFPC)
            Return ret
        End Function

        Protected Overrides Function GetSortFields() As CKeyCollection(Of CCursorField)
            Dim ret As New CKeyCollection(Of CCursorField)(MyBase.GetSortFields())
            ret.Remove(Me.m_Impiego_CategoriaAzienda)
            ret.Remove(Me.m_Impiego_TipologiaAzienda)
            ret.Remove(Me.m_Impiego_IDAzienda)
            ret.Remove(Me.m_Impiego_NomeAzienda)
            ret.Remove(Me.m_Impiego_IDEntePagante)
            ret.Remove(Me.m_Impiego_NomeEntePagante)
            ret.Remove(Me.m_Impiego_Posizione)
            ret.Remove(Me.m_Impiego_DataAssunzione)
            ret.Remove(Me.m_Impiego_DataLicenziamento)
            ret.Remove(Me.m_Impiego_StipendioNetto)
            ret.Remove(Me.m_Impiego_StipendioLordo)
            ret.Remove(Me.m_Impiego_TipoContratto)
            ret.Remove(Me.m_Impiego_TipoRapporto)
            ret.Remove(Me.m_Impiego_TFR)
            ret.Remove(Me.m_Impiego_MensilitaPercepite)
            ret.Remove(Me.m_Impiego_PercTFRAzienda)
            ret.Remove(Me.m_Impiego_NomeFPC)
            Return ret
        End Function

        'Private Function changeName(ByVal field As CCursorFieldObj(Of String), ByVal newName As String) As String
        '    Dim f As New CCursorFieldObj(Of String)(newName, field.Operator, field.IncludeNulls)
        '    f.Value = field.Value
        '    f.Value1 = field.Value1
        '    f.SortOrder = field.SortOrder
        '    f.SortPriority = field.SortPriority
        '    Return f.GetSQL
        'End Function

        Private Function GetSQLImp() As String
            Dim wherePart As String = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If (Me.m_Impiego_IDAzienda.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_IDAzienda.GetSQL("Azienda"), " AND ")
            If (Me.m_Impiego_NomeAzienda.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_NomeAzienda.GetSQL("NomeAzienda"), " AND ")
            If (Me.m_Impiego_IDEntePagante.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_IDEntePagante.GetSQL("IDEntePagante"), " AND ")
            If (Me.m_Impiego_NomeEntePagante.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_NomeEntePagante.GetSQL("NomeEntePagante"), " AND ")
            If (Me.m_Impiego_Posizione.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_Posizione.GetSQL("Posizione"), " AND ")
            If (Me.m_Impiego_DataAssunzione.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_DataAssunzione.GetSQL("DataAssunzione"), " AND ")
            If (Me.m_Impiego_DataLicenziamento.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_DataLicenziamento.GetSQL("DataLicenziamento"), " AND ")
            If (Me.m_Impiego_StipendioNetto.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_StipendioNetto.GetSQL("StipendioNetto"), " AND ")
            If (Me.m_Impiego_StipendioLordo.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_StipendioLordo.GetSQL("StipendioLordo"), " AND ")
            If (Me.m_Impiego_TipoContratto.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_TipoContratto.GetSQL("TipoContratto"), " AND ")
            If (Me.m_Impiego_TipoRapporto.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_TipoRapporto.GetSQL("TipoRapporto"), " AND ")
            If (Me.m_Impiego_TFR.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_TFR.GetSQL("TFR"), " AND ")
            If (Me.m_Impiego_MensilitaPercepite.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_MensilitaPercepite.GetSQL("MensilitaPercepite"), " AND ")
            If (Me.m_Impiego_PercTFRAzienda.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_PercTFRAzienda.GetSQL("PercTFRAzienda"), " AND ")
            If (Me.m_Impiego_NomeFPC.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_NomeFPC.GetSQL("NomeFPC"), " AND ")

            'Dim timpSQL As String = "SELECT * FROM [tbl_Impiegati] WHERE " & wherePart
            Dim dbSQL As New System.Text.StringBuilder
            dbSQL.Append("SELECT [TPI1].* FROM (SELECT * FROM [tbl_Impiegati] WHERE ")
            dbSQL.Append(wherePart)
            dbSQL.Append(") AS [TPI2] INNER JOIN (")
            dbSQL.Append(MyBase.GetSQL)
            dbSQL.Append(") AS [TPI1] ON [TPI1].[IDImpiego] = [TPI2].[ID]")

            Return dbSQL.ToString
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If (Me.m_Impiego_CategoriaAzienda.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_CategoriaAzienda.GetSQL("Categoria"), " AND ")
            If (Me.m_Impiego_TipologiaAzienda.IsSet) Then wherePart = Strings.Combine(wherePart, Me.m_Impiego_TipologiaAzienda.GetSQL("Tipologia"), " AND ")
            Return wherePart
        End Function

        Private Function IsImpiegoSet() As Boolean
            Return Me.m_Impiego_IDAzienda.IsSet OrElse _
                   Me.m_Impiego_NomeAzienda.IsSet OrElse _
                   Me.m_Impiego_IDEntePagante.IsSet OrElse _
                   Me.m_Impiego_NomeEntePagante.IsSet OrElse _
                   Me.m_Impiego_Posizione.IsSet OrElse _
                   Me.m_Impiego_DataAssunzione.IsSet OrElse _
                   Me.m_Impiego_DataLicenziamento.IsSet OrElse _
                   Me.m_Impiego_StipendioNetto.IsSet OrElse _
                   Me.m_Impiego_StipendioLordo.IsSet OrElse _
                   Me.m_Impiego_TipoContratto.IsSet OrElse _
                   Me.m_Impiego_TipoRapporto.IsSet OrElse _
                   Me.m_Impiego_TFR.IsSet OrElse _
                   Me.m_Impiego_MensilitaPercepite.IsSet OrElse _
                   Me.m_Impiego_PercTFRAzienda.IsSet OrElse _
                   Me.m_Impiego_NomeFPC.IsSet
        End Function

        Public Overrides Function GetSQL() As String
            If Me.IsImpiegoSet Then
                Return Me.GetSQLImp
            Else
                Return MyBase.GetSQL
            End If
        End Function

    End Class


End Class