Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags>
    Public Enum CImportExportSourceUserMatcF As Integer
        None = 0

        ''' <summary>
        ''' Se impostato l'utente è disabilitato e le operazioni dal server remoto fatte da questo utente
        ''' verranno bloccate
        ''' </summary>
        Blocca = 1
    End Enum

    ''' <summary>
    ''' Rappresenta una corrispondenza tra gli utenti dei due sistemi
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CImportExportSourceUserMatc
        Implements XML.IDMDXMLSerializable

        Public RemoteUserID As Integer
        Public RemoteUserName As String
        Public Flags As CImportExportSourceUserMatcF
        Private m_LocalUserID As Integer
        Private m_LocalUser As CUser



        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.RemoteUserID = 0
            Me.RemoteUserName = ""
            Me.m_LocalUserID = 0
            Me.m_LocalUser = Nothing
            Me.Flags = CImportExportSourceUserMatcF.None
        End Sub

        Public Property LocalUserID As Integer
            Get
                Return GetID(Me.m_LocalUser, Me.m_LocalUserID)
            End Get
            Set(value As Integer)
                Me.m_LocalUserID = value
                Me.m_LocalUser = Nothing
            End Set
        End Property

        Public Property LocalUser As CUser
            Get
                If (Me.m_LocalUser Is Nothing) Then Me.m_LocalUser = Sistema.Users.GetItemById(Me.m_LocalUserID)
                Return Me.m_LocalUser
            End Get
            Set(value As CUser)
                Me.m_LocalUser = value
                Me.m_LocalUserID = GetID(value)
            End Set
        End Property


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "RemoteUserID" : Me.RemoteUserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RemoteUserName" : Me.RemoteUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LocalUserID" : Me.m_LocalUserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("RemoteUserID", Me.RemoteUserID)
            writer.WriteAttribute("RemoteUserName", Me.RemoteUserName)
            writer.WriteAttribute("LocalUserID", Me.LocalUserID)
            writer.WriteAttribute("Flags", Me.Flags)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class





End Class