Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Rappresenta una visita effettuata o ricevuta
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CAppunto
        Inherits CContattoUtente

        Public Sub New()
        End Sub

        Public Overrides Function GetNomeTipoOggetto() As String
            Return "Appunto"
        End Function
          
        Public Overrides Function GetModule() As CModule
            Return Telefonate.Module
        End Function

        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Return "Appunto"
            End Get
        End Property


        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
            End Get
        End Property

         
        Public Overrides Function GetTableName() As String
            Return "tbl_Telefonate"
        End Function
         
    End Class




End Class

