Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    Public Class ConsensoInformatoCursor
        Inherits DBObjectCursorBase(Of ConsensoInformato)

        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_DataConsenso As New CCursorField(Of Date)("DataConsenso")
        Private m_Consenso As New CCursorField(Of Boolean)("Consenso")
        Private m_Richiesto As New CCursorField(Of Boolean)("Richiesto")
        Private m_NomeDocumento As New CCursorFieldObj(Of String)("NomeDocumento")
        Private m_DescrizioneDocumento As New CCursorFieldObj(Of String)("DescrizioneDocumento")
        Private m_LinkDocumentoVisionato As New CCursorFieldObj(Of String)("LinkVisionato")
        Private m_LinkDocumentoFirmato As New CCursorFieldObj(Of String)("LinkFirmato")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property Consenso As CCursorField(Of Boolean)
            Get
                Return Me.m_Consenso
            End Get
        End Property

        Public ReadOnly Property Richiesto As CCursorField(Of Boolean)
            Get
                Return Me.m_Richiesto
            End Get
        End Property

        Public ReadOnly Property DataConsenso As CCursorField(Of Date)
            Get
                Return Me.m_DataConsenso
            End Get
        End Property

        Public ReadOnly Property NomeDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDocumento
            End Get
        End Property

        Public ReadOnly Property DescrizioneDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneDocumento
            End Get
        End Property

        Public ReadOnly Property LinkDocumentoVisionato As CCursorFieldObj(Of String)
            Get
                Return Me.m_LinkDocumentoVisionato
            End Get
        End Property

        Public ReadOnly Property LinkDocumentoFirmato As CCursorFieldObj(Of String)
            Get
                Return Me.m_LinkDocumentoFirmato
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.ConsensiInformati.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneConsensi"
        End Function

    End Class




End Class