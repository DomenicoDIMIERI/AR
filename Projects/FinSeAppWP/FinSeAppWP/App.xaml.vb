Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Offre facile accesso al frame radice dell'applicazione Windows Phone.
    ''' </summary>
    ''' <returns>Nome radice dell'applicazione Windows Phone.</returns>
    Public Property RootFrame As PhoneApplicationFrame

    ''' <summary>
    ''' Costruttore dell'oggetto Application.
    ''' </summary>
    Public Sub New()
        ' Inizializzazione Silverlight standard
        InitializeComponent()

        ' Inizializzazione specifica del telefono
        InitializePhoneApplication()

        ' Visualizza informazioni di profilatura delle immagini durante il debug.
        If System.Diagnostics.Debugger.IsAttached Then
            ' Visualizza i contatori della frequenza fotogrammi corrente.
            Application.Current.Host.Settings.EnableFrameRateCounter = True

            ' Visualizza le aree dell'applicazione che vengono ridisegnate in ogni fotogramma.
            'Application.Current.Host.Settings.EnableRedrawRegions = True

            ' Abilitare la modalità di visualizzazione dell'analisi non di produzione, 
            ' che consente di visualizzare le aree di una pagina passate alla GPU con una sovrapposizione colorata.
            'Application.Current.Host.Settings.EnableCacheVisualization = True


            ' Disabilitare il rilevamento dell'inattività dell'applicazione impostando la proprietà UserIdleDetectionMode
            ' dell'oggetto PhoneApplicationService dell'applicazione su Disabled.
            ' Attenzione: utilizzare questa opzione solo in modalità di debug. L'applicazione che disabilita il rilevamento dell'inattività dell'utente continuerà ad essere eseguita
            ' e a consumare energia quando l'utente non utilizza il telefono.
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled
        End If

    End Sub

    ' Codice da eseguire all'avvio dell'applicazione (ad esempio da Start)
    ' Questo codice non verrà eseguito quando l'applicazione viene riattivata
    Private Sub Application_Launching(ByVal sender As Object, ByVal e As LaunchingEventArgs)
    End Sub

    ' Codice da eseguire quando l'applicazione viene attivata (portata in primo piano)
    ' Questo codice non verrà eseguito al primo avvio dell'applicazione
    Private Sub Application_Activated(ByVal sender As Object, ByVal e As ActivatedEventArgs)
    End Sub

    ' Codice da eseguire quando l'applicazione viene disattivata (inviata in background)
    ' Questo codice non verrà eseguito alla chiusura dell'applicazione
    Private Sub Application_Deactivated(ByVal sender As Object, ByVal e As DeactivatedEventArgs)
    End Sub

    ' Codice da eseguire alla chiusura dell'applicazione (ad esempio se l'utente fa clic su Indietro)
    ' Questo codice non verrà eseguito quando l'applicazione viene disattivata
    Private Sub Application_Closing(ByVal sender As Object, ByVal e As ClosingEventArgs)
    End Sub

    ' Codice da eseguire se un'operazione di navigazione ha esito negativo
    Private Sub RootFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        If Diagnostics.Debugger.IsAttached Then
            ' Un'operazione di navigazione ha avuto esito negativo; inserire un'interruzione nel debugger
            Diagnostics.Debugger.Break()
        End If
    End Sub

    Public Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

        ' Visualizza informazioni di profilatura delle immagini durante il debug.
        If Diagnostics.Debugger.IsAttached Then
            Diagnostics.Debugger.Break()
        Else
            e.Handled = True
            MessageBox.Show(e.ExceptionObject.Message & Environment.NewLine & e.ExceptionObject.StackTrace,
                            "Error", MessageBoxButton.OK)
        End If
    End Sub

#Region "Inizializzazione dell'applicazione Windows Phone"
    ' Evitare la doppia inizializzazione
    Private phoneApplicationInitialized As Boolean = False

    ' Non aggiungere altro codice a questo metodo
    Private Sub InitializePhoneApplication()
        If phoneApplicationInitialized Then
            Return
        End If

        ' Creare il fotogramma ma non impostarlo ancora come RootVisual; in questo modo
        ' la schermata iniziale rimane attiva finché non viene completata la preparazione al rendering dell'applicazione.
        RootFrame = New PhoneApplicationFrame()
        AddHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication

        ' Gestisce gli errori di navigazione
        AddHandler RootFrame.NavigationFailed, AddressOf RootFrame_NavigationFailed

        ' Accertarsi che l'inizializzazione non venga ripetuta
        phoneApplicationInitialized = True
    End Sub

    ' Non aggiungere altro codice a questo metodo
    Private Sub CompleteInitializePhoneApplication(ByVal sender As Object, ByVal e As NavigationEventArgs)
        ' Impostare l'elemento visivo radice per consentire il rendering dell'applicazione
        If RootVisual IsNot RootFrame Then
            RootVisual = RootFrame
        End If

        ' Rimuovere il gestore in quanto non più necessario
        RemoveHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication
    End Sub
#End Region

End Class