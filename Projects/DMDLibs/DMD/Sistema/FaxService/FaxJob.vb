Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Enum FaxJobStatus As Integer
        PREPARING = -1
        QUEUED = 0
        DIALLING = 1
        SENDING = 2
        COMPLETED = 3
        WAITRETRY = 4
        CANCELLED = 5
        TIMEOUT = 6
        [ERROR] = 7
    End Enum

    Public NotInheritable Class FaxJob
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Date As Date
        Private m_JobID As String
        Private m_Driver As BaseFaxDriver
        Private m_Options As FaxDriverOptions
        Private m_Modem As FaxDriverModem
        Private m_Status As FaxJobStatus
        Private m_StatusMessage As String
        Private m_InternalStatus As Object
        Private m_Tag As Object
        Private m_DeliveryDate As Date?
        Private m_QueueDate As Date?

        'Private m_Document As Object

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal driver As BaseFaxDriver, ByVal modem As FaxDriverModem)
            Me.New
            Me.m_Modem = modem
            Me.m_Driver = driver
        End Sub

        Public Property Tag As Object
            Get
                Return Me.m_Tag
            End Get
            Set(value As Object)
                Me.m_Tag = value
            End Set
        End Property

        Public ReadOnly Property [Date] As Date
            Get
                Return Me.m_Date
            End Get
        End Property

        Protected Friend Sub SetDate(ByVal value As Date)
            Me.m_Date = value
        End Sub

        Public ReadOnly Property DeliveryDate As Date?
            Get
                Return Me.m_DeliveryDate
            End Get
        End Property

        Public ReadOnly Property QueueDate As Date?
            Get
                Return Me.m_QueueDate
            End Get
        End Property

        Protected Friend Sub SetDeliveryDate(ByVal v As Date?)
            Me.m_DeliveryDate = v
        End Sub

        Protected Friend Sub SetFailDate(ByVal v As Date?)
            Me.m_DeliveryDate = v
        End Sub

        Protected Friend Sub SetQueueDate(ByVal v As Date?)
            Me.m_QueueDate = v
        End Sub

        ''' <summary>
        ''' Restituisce lo stato del job
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property JobStatus As FaxJobStatus
            Get
                Return Me.m_Status
            End Get
        End Property

        Protected Friend Sub SetJobStatus(ByVal value As FaxJobStatus)
            Me.m_Status = value
        End Sub

        Public ReadOnly Property JobID As String
            Get
                Return Me.m_JobID
            End Get
        End Property

        Protected Friend Sub SetJobID(ByVal value As String)
            Me.m_JobID = value
        End Sub

        ''' <summary>
        ''' Restituisce una stringa che descrive lo stato del lavoro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property JobStatusMessage As String
            Get
                Return Me.m_StatusMessage
            End Get
        End Property

        Friend Sub SetJobStatusMessage(ByVal value As String)
            Me.m_StatusMessage = Trim(value)
        End Sub

        ''' <summary>
        ''' Restituisce il driver che gestisce questo job
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Driver As BaseFaxDriver
            Get
                Return Me.m_Driver
            End Get
        End Property

        Protected Friend Sub SetDriver(ByVal driver As BaseFaxDriver)
            Me.m_Driver = driver
        End Sub

        ''' <summary>
        ''' Restituisce le opzioni di invio/ricezione utilizzate per questo job
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Options As FaxDriverOptions
            Get
                Return Me.m_Options
            End Get
        End Property

        Protected Friend Sub SetOptions(ByVal value As FaxDriverOptions)
            Me.m_Options = value
        End Sub

        ''' <summary>
        ''' Invia 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Send()
            Me.Driver.Send(Me)
        End Sub

        Public Sub Cancel()
            Me.Driver.CancelJob(Me.m_JobID)
            Me.m_Status = FaxJobStatus.CANCELLED
        End Sub

      

        Public Property InternalStatus As Object
            Get
                Return Me.m_InternalStatus
            End Get
            Set(value As Object)
                Me.m_InternalStatus = value
            End Set
        End Property

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "JobID" : Me.m_JobID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Data" : Me.m_Date = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Driver" : Me.m_Driver = Sistema.FaxService.GetDriver(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "Status" : Me.m_Status = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "QueueDate" : Me.m_QueueDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DeliveryDate" : Me.m_QueueDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatusMessage" : Me.m_StatusMessage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Options"
                    Me.m_Options = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("JobID", Me.m_JobID)
            writer.WriteAttribute("Data", Me.m_Date)
            writer.WriteAttribute("Driver", Me.Driver.GetUniqueID)
            writer.WriteAttribute("Status", Me.m_Status)
            writer.WriteAttribute("QueueDate", Me.m_QueueDate)
            writer.WriteAttribute("DeliveryDate", Me.m_QueueDate)
            writer.WriteAttribute("StatusMessage", Me.m_StatusMessage)
            writer.WriteTag("Options", Me.Options)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class