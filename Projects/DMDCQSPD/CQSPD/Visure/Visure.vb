Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    Public Class VisureEventArgs
        Inherits System.EventArgs

        Private m_Item As Visura

        Public Sub New(ByVal item As Visura)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Item = item
        End Sub

        Public ReadOnly Property Item As Visura
            Get
                Return Me.m_Item
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    ''' <summary>
    ''' Gestione delle richieste di visure
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CVisureClass
        Inherits CGeneralClass(Of Visura)

      

        ''' <summary>
        ''' Evento generato quando viene memorizzata una nuova richiesta
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaCreata(ByVal e As VisureEventArgs)

        ''' <summary>
        ''' Evento generato quando viene effettuata una nuova richiesta ad un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaEffettuata(ByVal e As VisureEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ritirata un richiesta ad un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRitirata(ByVal e As VisureEventArgs)

        ''' <summary>
        ''' Evento generato quando viene rifiutata un richiesta da un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRifiutata(ByVal e As VisureEventArgs)

        ''' <summary>
        ''' Evento generato quando viene annullata una richiesta da un utente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaAnnullata(ByVal e As VisureEventArgs)

        Friend Sub New()
            MyBase.New("modCQSPDVisure", GetType(VisureCursor))
        End Sub
     
        '        ''' <summary>
        '        ''' Inizializza modulo e tabelle
        '        ''' </summary>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Private  Function InitModule() As CModule
        '            Dim ret As CModule = Sistema.Modules.GetItemByName("")
        '            If (ret Is Nothing) Then
        '                ret = New CModule("modCQSPDVisure")
        '                ret.Stato = ObjectStatus.OBJECT_VALID
        '                ret.Parent = DMD.CQSPD.Module
        '                ret.Save()
        '                ret.InitializeStandardActions()

        '#If 0 Then
        '                create table tbl_CQSPDVisure
        '(
        '[ID] counter Primary Key,
        '[Data] Date,
        '[IDRichiedente] Int,
        '[NomeRichiedente] Text(255),
        '[IDPresaInCaricoDa] Int,
        '[NomePresaInCaricoDa] Text(255),
        '[DataPresaInCarico] Date,
        '[DataCompletamento] Date,
        '[StatoVisura] Int,
        '[VALAMM] Bit,
        '[CENSDATLAV] bit,
        '[CENSSEDOP] bit,
        '[VARIAZDENOM] bit,
        '[SBLOCCO] bit,
        '[IDAmministrazione] Int,
        '[RagioneSociale] Text(255),
        '[OggettoSociale] Memo,
        '[CodiceFiscale] Text(32),
        '[PartitaIVA] Text(32),
        '[NomeResponsabile] Text(255),
        '[QualificaResponsabile] Text(255),
        '[Indirizzo_Provincia] Text(255),
        '[Indirizzo_Citta] text(255),
        '[Indirizzo_CAP] Text(16),
        '[Indirizzo_Via] Text(255),
        '[Telefono] Text(32),
        '[Fax] Text(32),
        '[eMail] Text(255),
        '[IndirizzoN_Provincia] Text(255),
        '[IndirizzoN_Citta] Text(255),
        '[IndirizzoN_CAP] Text(16),
        '[IndirizzoN_Via] Text(255),
        '[TelefonoN] Text(32),
        '[FaxN] Text(32),
        '[CONVSINO] bit,
        '[CODCONV] Text(255),
        '[NumeroDipendenti] int,
        '[AMMMODPRST008] bit,
        '[NoteSocieta] Memo,
        '[IDBustaPaga] Int,
        '[IDMotivoSblocco] int,
        '[IDPuntoOperativo] Int,
        '[NomePuntoOperativo] Text(255),
        '[CreatoDa] Int,
        '[CreatoIl] Date,
        '[ModificatoDa] int,
        '[ModificatoIl] Date,
        '[Stato] int
        ')

        '#End If
        '            End If
        '            Return ret
        '        End Function

     

        Friend Sub doRichiestaCreata(ByVal e As VisureEventArgs)
            RaiseEvent RichiestaCreata(e)
        End Sub

        Friend Sub doRichiestaEffettuata(ByVal e As VisureEventArgs)
            RaiseEvent RichiestaEffettuata(e)
        End Sub

        Friend Sub doRichiestaRifiutata(ByVal e As VisureEventArgs)
            RaiseEvent RichiestaRifiutata(e)
        End Sub

        Friend Sub doRichiestaAnnullata(ByVal e As VisureEventArgs)
            RaiseEvent RichiestaAnnullata(e)
        End Sub

        Friend Sub doRichiestaRitirata(ByVal e As VisureEventArgs)
            RaiseEvent RichiestaRitirata(e)
        End Sub


    End Class

    Private Shared m_Visure As CVisureClass = Nothing

    Public Shared ReadOnly Property Visure As CVisureClass
        Get
            If (m_Visure Is Nothing) Then m_Visure = New CVisureClass
            Return m_Visure
        End Get
    End Property

End Class