Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Public Class Sistema

    Public Class CAttachmentsCursor
        Inherits DBObjectCursor(Of CAttachment)

        Private m_OwnerID As New CCursorField(Of Integer)("OwnerID")
        Private m_OwnerType As New CCursorFieldObj(Of String)("OwnerType")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo") 'Tipo del documento allegato
        Private m_StatoDocumento As New CCursorField(Of AttachmentStatus)("StatoDocumento") 'Valore che indica lo stato del documento (0 NON VALIDATO, 1 VALIDATO, 2 NON LEGGIBILE, 3 NON VALIDO ...)         
        Private m_VerificatoDaID As New CCursorField(Of Integer)("VerificatoDaID") 'ID dell'utente che ha verificato il documento
        Private m_NomeVerificatoDa As New CCursorFieldObj(Of String)("NomeVerificatoDa") 'Nome dell'utente che ha verificato il documento
        Private m_VerificatoIl As New CCursorField(Of Date)("VerificatoIl") 'Data di verifica
        Private m_Testo As New CCursorFieldObj(Of String)("Testo") 'Testo visualizzato
        Private m_URL As New CCursorFieldObj(Of String)("URL")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_Parametro As New CCursorFieldObj(Of String)("Parametro")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_IDDocumento As New CCursorField(Of Integer)("IDDocumento")
        Private m_IDRilasciatoDa As New CCursorField(Of Integer)("IDRilasciatoDa")
        Private m_NomeRilasciatoDa As New CCursorFieldObj(Of String)("RilasciatoDa")
        ' Private m_IDProduttore As New CCursorField(Of Integer)("IDProduttore")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_SottoCategoria As New CCursorFieldObj(Of String)("SottoCategoria")
        Private m_Flags As New CCursorField(Of AttachmentFlags)("Flags")
        Private m_ParentID As New CCursorField(Of Integer)("ParentID")

        Public Sub New()

        End Sub

        Public ReadOnly Property Flags As CCursorField(Of AttachmentFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property ParentID As CCursorField(Of Integer)
            Get
                Return Me.m_ParentID
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property SottoCategoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_SottoCategoria
            End Get
        End Property

        'Public ReadOnly Property IDProduttore As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDProduttore
        '    End Get
        'End Property

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

        Public ReadOnly Property NomeRilasciatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRilasciatoDa
            End Get
        End Property

        Public ReadOnly Property IDRilasciatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDRilasciatoDa
            End Get
        End Property

        Public ReadOnly Property OwnerID As CCursorField(Of Integer)
            Get
                Return Me.m_OwnerID
            End Get
        End Property

        Public ReadOnly Property OwnerType As CCursorFieldObj(Of String)
            Get
                Return Me.m_OwnerType
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property StatoDocumento As CCursorField(Of AttachmentStatus)
            Get
                Return Me.m_StatoDocumento
            End Get
        End Property

        Public ReadOnly Property VerificatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_VerificatoDaID
            End Get
        End Property

        Public ReadOnly Property NomeVerificatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeVerificatoDa
            End Get
        End Property

        Public ReadOnly Property VerificatoIl As CCursorField(Of Date)
            Get
                Return Me.m_VerificatoIl
            End Get
        End Property

        Public ReadOnly Property Testo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Testo
            End Get
        End Property

        Public ReadOnly Property URL As CCursorFieldObj(Of String)
            Get
                Return Me.m_URL
            End Get
        End Property

        Public ReadOnly Property Parametro As CCursorFieldObj(Of String)
            Get
                Return Me.m_Parametro
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property IDDocumento As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumento
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Attachments"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Sistema.Attachments.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Sistema.Attachments.Database
        End Function
    End Class



End Class

