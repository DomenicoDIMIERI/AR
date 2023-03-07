Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    ''' <summary>
    ''' Rappresenta un Fax inviato o ricevuto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class FaxDocument
        Inherits CContattoUtente

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.CRM.Module
        End Function

        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Dim ret As String
                ret = "FAX " & CStr(IIf(Me.Ricevuta, "ricevuto da", "inviato a")) & " " & Me.NomePersona
                If Me.NumeroOIndirizzo <> "" Then ret &= ", tel: " & Formats.FormatPhoneNumber(Me.NumeroOIndirizzo)
                Return ret
            End Get
        End Property

        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
            End Get
        End Property

        Public Overrides Function GetNomeTipoOggetto() As String
            Return "FAX"
        End Function

        Public Property Numero As String
            Get
                Return Me.NumeroOIndirizzo
            End Get
            Set(value As String)
                Me.NumeroOIndirizzo = value
            End Set
        End Property

        Public Overrides Property NumeroOIndirizzo As String
            Get
                Return MyBase.NumeroOIndirizzo
            End Get
            Set(value As String)
                MyBase.NumeroOIndirizzo = Formats.ParsePhoneNumber(value)
            End Set
        End Property

        

    End Class



End Class