Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases

 
Namespace Drivers

    Public MustInherit Class Driver
        Implements DMD.XML.IDMDXMLSerializable

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected MustOverride Function GetDefaultSettingsFileName() As String
            
        Private m_DefaultOptions As DriverOptions = Nothing

        ''' <summary>
        ''' Restituisce il valore di una proprietà del driver in base al suo nome.
        ''' Se il driver non definisce la proprietà viene generato un errore di tipo NotSupportedException
        ''' </summary>
        ''' <param name="capName">[in] Nome della proprietà di cui si desidera conoscere il valore</param>
        ''' <returns>Valore della proprietà richiesta</returns>
        ''' <remarks></remarks>
        ''' <exception cref="NotSupportedException">Se il driver non definisce la proprietà richiesta</exception>
        Public Overridable Function GetDriverCAP(ByVal capName As String) As Object
            Throw New NotSupportedException(capName)
        End Function

        Public Overridable Function GetDefaultOptions() As DriverOptions
            If (m_DefaultOptions Is Nothing) Then
                Me.m_DefaultOptions = Me.InstantiateNewOptions
                Try
                    Dim path As String = Me.GetDefaultSettingsFileName ' Global.System.IO.Path.Combine(WebSite.Instance.AppContext.SystemDataFolder, "\TrendooSMS\config.cfg")
                    DMD.Sistema.FileSystem.CreateRecursiveFolder(Sistema.FileSystem.GetFolderName(path))
                    If (DMD.Sistema.FileSystem.FileExists(path)) Then Me.m_DefaultOptions.LoadFromFile(path)
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
            End If
            Return Me.m_DefaultOptions
        End Function


        Public Overridable Sub SetDefaultOptions(value As DriverOptions)
            Dim c As Boolean = Me.IsConnected
            If (c) Then Me.Disconnect()
            Dim path As String = Me.GetDefaultSettingsFileName 'Global.System.IO.Path.Combine(WebSite.Instance.AppContext.SystemDataFolder, "TrendooSMS\config.cfg")
            DMD.Sistema.FileSystem.CreateRecursiveFolder(Sistema.FileSystem.GetFolderName(path))
            value.SaveToFile(path)
            Me.m_DefaultOptions = value
            If (c) Then Me.Connect()
        End Sub

        ''' <summary>
        ''' Restituisce una stringa che identifica univocamente il driver
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetUniqueID() As String

        ''' <summary>
        ''' Restituisce un nome descrittivo per il driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property Description As String

        ''' <summary>
        ''' Restituisce le impostazioni predefinite del driver
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected MustOverride Function InstantiateNewOptions() As DriverOptions

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("UniqueID", Me.GetUniqueID)
            writer.WriteAttribute("Description", Me.Description)
        End Sub

        Protected Overridable Sub SetFieldInternal(ByVal fielName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal

        End Sub


        ''' <summary>
        ''' Carica le impostazioni predefinite
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub LoadDefaultOptions()
            Dim options As DriverOptions = Me.GetDefaultOptions
        End Sub

        Private m_IsConnectd As Boolean = False

        Public Overridable Function IsConnected() As Boolean
            Return Me.m_IsConnectd
        End Function

        ''' <summary>
        ''' Effettua la connessione
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Connect()
            If (Me.IsConnected) Then Throw New InvalidOperationException("Già connesso")
            Me.LoadDefaultOptions()
            Me.InternalConnect()
            Me.m_IsConnectd = True
        End Sub

        Protected MustOverride Sub InternalConnect()


        Public Sub Disconnect()
            If (Me.IsConnected = False) Then Throw New InvalidOperationException("Non connesso")
            Me.InternalDisconnect()
            Me.m_IsConnectd = False
        End Sub

        Protected MustOverride Sub InternalDisconnect()

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace