Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle consulenze
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDConsulenzaCursor
        Inherits DBObjectCursorPO(Of CQSPDConsulenza)

        Private m_IDStudioDiFattibilita As New CCursorField(Of Integer)("IDGruppo")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IDRichiesta As New CCursorField(Of Integer)("IDRichiesta")
        'Private m_IDOffertaCorrente As Integer          'ID dell'offerta corrente
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_NomeConsulente As New CCursorFieldObj(Of String)("NomeConsulente")
        Private m_DataConsulenza As New CCursorField(Of Date)("DataConsulenza")
        Private m_DataConferma As New CCursorField(Of Date)("DataConferma")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Flags As New CCursorField(Of ConsulenzeFlags)("Flags")
        Private m_StatoConsulenza As New CCursorField(Of StatiConsulenza)("StatoConsulenza")
        Private m_IDOffertaCQS As New CCursorField(Of Integer)("IDOffertaCQS")
        Private m_IDOffertaPD As New CCursorField(Of Integer)("IDOffertaPD")
        Private m_DataProposta As New CCursorField(Of Date)("DataProposta")
        Private m_IDPropostaDa As New CCursorField(Of Integer)("IDPropostaDa")
        Private m_IDConfermataDa As New CCursorField(Of Integer)("IDConfermataDa")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_Durata As New CCursorField(Of Double)("Durata")
        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_IDInseritoDa As New CCursorField(Of Integer)("IDInseritoDa")
        Private m_IDRichiestaApprovazione As New CCursorField(Of Integer)("IDRichiestaApprovazione")
        Private m_MotivoAnnullamento As New CCursorFieldObj(Of String)("MotivoAnnullamento")
        Private m_DettaglioAnnullamento As New CCursorFieldObj(Of String)("DettaglioAnnullamento")
        Private m_IDAnnullataDa As New CCursorField(Of Integer)("IDAnnullataDa")
        Private m_NomeAnnullataDa As New CCursorFieldObj(Of String)("NomeAnnullataDa")
        Private m_DataAnnullamento As New CCursorField(Of Date)("DataAnnullamento")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")
        Private m_IDUltimaVerifica As New CCursorField(Of Integer)("IDUltimaVerifica")
        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")

        Private m_IDCessionario As New CCursorField(Of Integer)("IDCessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_IDProfilo As New CCursorField(Of Integer)("IDProfilo")
        Private m_NomeProfilo As New CCursorFieldObj(Of String)("NomeProfilo")
        Private m_IDProdotto As New CCursorField(Of Integer)("IDProdotto")
        Private m_NomeProdotto As New CCursorFieldObj(Of String)("NomeProdotto")
        Private m_CategoriaProdotto As New CCursorFieldObj(Of String)("CategoriaProdotto")
        Private m_Rata As New CCursorField(Of Double)("Rata")
        Private m_NumeroRate As New CCursorField(Of Integer)("NumeroRate")


        Public Sub New()
        End Sub

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_IDCessionario
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property IDProfilo As CCursorField(Of Integer)
            Get
                Return Me.m_IDProfilo
            End Get
        End Property

        Public ReadOnly Property NomeProfilo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProfilo
            End Get
        End Property

        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_IDProdotto
            End Get
        End Property

        Public ReadOnly Property NomeProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdotto
            End Get
        End Property

        Public ReadOnly Property CategoriaProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_CategoriaProdotto
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Double)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property NumeroRate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroRate
            End Get
        End Property


        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property IDUltimaVerifica As CCursorField(Of Integer)
            Get
                Return Me.m_IDUltimaVerifica
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property

        Public ReadOnly Property IDAnnullataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAnnullataDa
            End Get
        End Property

        Public ReadOnly Property NomeAnnullataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAnnullataDa
            End Get
        End Property

        Public ReadOnly Property DataAnnullamento As CCursorField(Of Date)
            Get
                Return Me.m_DataAnnullamento
            End Get
        End Property

        Public ReadOnly Property MotivoAnnullamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoAnnullamento
            End Get
        End Property

        Public ReadOnly Property DettaglioAnnullamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioAnnullamento
            End Get
        End Property


        Public ReadOnly Property IDRichiestaApprovazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property IDInseritoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDInseritoDa
            End Get
        End Property


        Public ReadOnly Property IDStudioDiFattibilita As CCursorField(Of Integer)
            Get
                Return Me.m_IDStudioDiFattibilita
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Double)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property

        Public ReadOnly Property DataProposta As CCursorField(Of Date)
            Get
                Return Me.m_DataProposta
            End Get
        End Property

        Public ReadOnly Property IDPropostaDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPropostaDa
            End Get
        End Property

        Public ReadOnly Property IDConfermataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDConfermataDa
            End Get
        End Property

        Public ReadOnly Property IDOffertaCQS As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaCQS
            End Get
        End Property

        Public ReadOnly Property IDOffertaPD As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaPD
            End Get
        End Property

        Public ReadOnly Property StatoConsulenza As CCursorField(Of StatiConsulenza)
            Get
                Return Me.m_StatoConsulenza
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property IDRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiesta
            End Get
        End Property

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property NomeConsulente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConsulente
            End Get
        End Property

        Public ReadOnly Property DataConsulenza As CCursorField(Of Date)
            Get
                Return Me.m_DataConsulenza
            End Get
        End Property

        Public ReadOnly Property DataConferma As CCursorField(Of Date)
            Get
                Return Me.m_DataConferma
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ConsulenzeFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CQSPDConsulenza
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConsulenze"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Consulenze.Module
        End Function

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As New CKeyCollection(Of CCursorField)(MyBase.GetWhereFields())
            ret.Remove(Me.m_IDCessionario)
            ret.Remove(Me.m_NomeCessionario)
            ret.Remove(Me.m_IDProfilo)
            ret.Remove(Me.m_NomeProfilo)
            ret.Remove(Me.m_IDProdotto)
            ret.Remove(Me.m_NomeProdotto)
            ret.Remove(Me.m_CategoriaProdotto)
            ret.Remove(Me.m_Rata)
            ret.Remove(Me.m_NumeroRate)
            Return ret
        End Function

        Protected Overrides Function GetSortFields() As CKeyCollection(Of CCursorField)
            Dim ret As New CKeyCollection(Of CCursorField)(MyBase.GetWhereFields())
            ret.Remove(Me.m_IDCessionario)
            ret.Remove(Me.m_NomeCessionario)
            ret.Remove(Me.m_IDProfilo)
            ret.Remove(Me.m_NomeProfilo)
            ret.Remove(Me.m_IDProdotto)
            ret.Remove(Me.m_NomeProdotto)
            ret.Remove(Me.m_CategoriaProdotto)
            ret.Remove(Me.m_Rata)
            ret.Remove(Me.m_NumeroRate)
            Return ret
        End Function

        Private Function IsOffertaSet() As Boolean
            Return Me.m_IDCessionario.IsSet OrElse
                    Me.m_NomeCessionario.IsSet OrElse
                    Me.m_IDProfilo.IsSet OrElse
                    Me.m_NomeProfilo.IsSet OrElse
                    Me.m_IDProdotto.IsSet OrElse
                    Me.m_NomeProdotto.IsSet OrElse
                    Me.m_CategoriaProdotto.IsSet OrElse
                    Me.m_Rata.IsSet OrElse
                    Me.m_NumeroRate.IsSet
        End Function

        Public Overrides Function GetWherePart() As String
            Dim ret As String = MyBase.GetWherePart()
            If Me.IsOffertaSet Then
                Dim cursor As New CCQSPDOfferteCursor
                cursor.IDCessionario.InitFrom(Me.m_IDCessionario)
                cursor.NomeCessionario.InitFrom(Me.m_NomeCessionario)
                cursor.IDProfilo.InitFrom(Me.m_IDProfilo)
                cursor.NomeProfilo.InitFrom(Me.m_NomeProfilo)
                cursor.ProdottoID.InitFrom(Me.m_IDProdotto)
                cursor.NomeProdotto.InitFrom(Me.m_NomeProdotto)
                cursor.CategoriaProdotto.InitFrom(Me.m_CategoriaProdotto)
                cursor.Rata.InitFrom(Me.m_Rata)
                cursor.Durata.InitFrom(Me.m_NumeroRate)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True

                Dim arr As Integer() = {}
                Dim dbSQL As String = "SELECT [ID] FROM (" & cursor.GetSQL & ")"
                Dim dbRis As System.Data.IDataReader = cursor.Connection.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim id As Integer = Formats.ToInteger(dbRis("ID"))
                    arr = Arrays.Append(Of Integer)(arr, id)
                End While
                dbRis.Dispose()
                cursor.Dispose()

                If (arr.Length = 0) Then
                    ret = "(0<>0)"
                Else
                    Dim strj As String = Strings.Join(arr, ",")

                    ret = Strings.Combine(ret, "([IDOffertaCQS] In (" & strj & ") OR [IDOffertaPD] In (" & strj & "))", " AND ")
                End If
            End If
            Return ret
        End Function


    End Class


End Class
