Namespace My
    ' I seguenti eventi sono disponibili per MyApplication:
    ' Startup: generato all'avvio dell'applicazione, prima della creazione del form di avvio.
    ' Shutdown: generato dopo la chiusura di tutti i form dell'applicazione. L'evento non è generato se l'applicazione termina in modo anomalo.
    ' UnhandledException: generato se nell'applicazione si verifica un'eccezione non gestita.
    ' StartupNextInstance: generato all'avvio di un'applicazione a istanza singola se l'applicazione è già attiva. 
    ' NetworkAvailabilityChanged: generato se la connessione di rete è connessa o disconnessa.
    Partial Friend Class MyApplication


        Protected Overrides Function OnStartup(eventArgs As ApplicationServices.StartupEventArgs) As Boolean
            frmMain.autoSync = eventArgs.CommandLine.Count > 0

            ' Add any initialization after the InitializeComponent() call.
            Dim ac As New MyApplicationContext
            DMD.Sistema.SetApplicationContext(ac)
            ac.Start()


            Return MyBase.OnStartup(eventArgs)
        End Function

        Protected Overrides Sub OnStartupNextInstance(eventArgs As ApplicationServices.StartupNextInstanceEventArgs)
            eventArgs.BringToForeground = True
            MyBase.OnStartupNextInstance(eventArgs)
        End Sub


    End Class
End Namespace
