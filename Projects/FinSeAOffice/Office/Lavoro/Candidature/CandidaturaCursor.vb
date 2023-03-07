Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle candidature ad un'offerta di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CandidaturaCursor
        Inherits DBObjectCursorPO(Of Candidatura)

        Private m_DataCandidatura As New CCursorField(Of Date)("DataCandidatura")
        Private m_IDCandidato As New CCursorField(Of Integer)("IDCandidato")
        Private m_NomeCandidato As New CCursorFieldObj(Of String)("NomeCandidato")
        Private m_IDCurriculum As New CCursorField(Of Integer)("IDCurriculum")
        Private m_IDOfferta As New CCursorField(Of Integer)("IDOfferta")
        Private m_NomeOfferta As New CCursorFieldObj(Of String)("NomeOfferta")
        Private m_IDCanale As New CCursorField(Of Integer)("IDCanale")
        Private m_NomeCanale As New CCursorFieldObj(Of String)("NomeCanale")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("NomeFonte")
        Private m_DataNascita As New CCursorField(Of Date)("DataNascita")
        Private m_NatoA_Citta As New CCursorFieldObj(Of String)("NatoA_Comune")
        Private m_NatoA_Provincia As New CCursorFieldObj(Of String)("NatoA_Provincia")
        Private m_ResidenteA_Citta As New CCursorFieldObj(Of String)("ResidenteA_Citta")
        Private m_ResidenteA_CAP As New CCursorFieldObj(Of String)("ResidenteA_CAP")
        Private m_ResidenteA_Provincia As New CCursorFieldObj(Of String)("ResidenteA_Provincia")
        Private m_ResidenteA_Via As New CCursorFieldObj(Of String)("ResidenteA_Via")
        Private m_ResidenteA_Civico As New CCursorFieldObj(Of String)("ResidenteA_Civico")
        Private m_Telefono As New CCursorFieldObj(Of String)("Telefono")
        Private m_eMail As New CCursorFieldObj(Of String)("eMail")
        Private m_Valutazione As New CCursorField(Of Integer)("Valutazione")
        Private m_ValutatoDaID As New CCursorField(Of Integer)("ValutatoDaID")
        Private m_ValutatoDaNome As New CCursorFieldObj(Of String)("ValutatoDaNome")
        Private m_ValutatoIl As New CCursorField(Of Date)("ValutatoIl")
        Private m_MotivoValutazione As New CCursorFieldObj(Of String)("MotivoValutazione")

        Public Sub New()
        End Sub

        Public ReadOnly Property DataNascita As CCursorField(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
        End Property

        Public ReadOnly Property NatoA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoA_Citta
            End Get
        End Property

        Public ReadOnly Property NatoA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NatoA_Provincia
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Citta
            End Get
        End Property

        Public ReadOnly Property ResidenteA_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_CAP
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Provincia
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Via
            End Get
        End Property

        Public ReadOnly Property ResidenteA_Civico As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResidenteA_Civico
            End Get
        End Property

        Public ReadOnly Property Telefono As CCursorFieldObj(Of String)
            Get
                Return Me.m_Telefono
            End Get
        End Property

        Public ReadOnly Property eMail As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMail
            End Get
        End Property

        Public ReadOnly Property Valutazione As CCursorField(Of Integer)
            Get
                Return Me.m_Valutazione
            End Get
        End Property

        Public ReadOnly Property ValutatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_ValutatoDaID
            End Get
        End Property

        Public ReadOnly Property ValutatoDaNome As CCursorFieldObj(Of String)
            Get
                Return Me.m_ValutatoDaNome
            End Get
        End Property

        Public ReadOnly Property ValutatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ValutatoIl
            End Get
        End Property

        Public ReadOnly Property MotivoValutazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoValutazione
            End Get
        End Property

        Public ReadOnly Property IDCanale As CCursorField(Of Integer)
            Get
                Return Me.m_IDCanale
            End Get
        End Property

        Public ReadOnly Property NomeCanale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property

        Public ReadOnly Property NomeFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFonte
            End Get
        End Property

        Public ReadOnly Property DataCandidatura As CCursorField(Of Date)
            Get
                Return Me.m_DataCandidatura
            End Get
        End Property

        Public ReadOnly Property IDCandidato As CCursorField(Of Integer)
            Get
                Return Me.m_IDCandidato
            End Get
        End Property

        Public ReadOnly Property NomeCandidato As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCandidato
            End Get
        End Property

        Public ReadOnly Property IDOfferta As CCursorField(Of Integer)
            Get
                Return Me.m_IDOfferta
            End Get
        End Property

        Public ReadOnly Property NomeOfferta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOfferta
            End Get
        End Property

        Public ReadOnly Property IDCurriculum As CCursorField(Of Integer)
            Get
                Return Me.m_IDCurriculum
            End Get
        End Property

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()
            ret.RemoveByKey("Telefono")
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Candidature.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCandidature"
        End Function

        Public Overrides Function GetWherePart() As String
            Dim ret As String = MyBase.GetWherePart()
            If (Me.m_Telefono.IsSet) Then
                Dim fSQL As String = Me.m_Telefono.GetSQL
                Dim f1 As String = Replace(fSQL, "[Telefono]", "[Telefono1]")
                Dim f2 As String = Replace(fSQL, "[Telefono]", "[Telefono2]")
                ret = Strings.Combine(ret, "(" & f1 & " OR " & f2 & ")", " AND ")
            End If
            Return ret
        End Function


    End Class


End Class