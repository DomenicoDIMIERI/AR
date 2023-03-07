Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

<Flags>
Public Enum ProxyFlags As Integer
    None = 0
    Disabled = 1
End Enum

Public NotInheritable Class ProxyProfile
    Inherits DBObjectBase

    Private m_Flags As ProxyFlags
    Private m_Name As String
    Private m_AllowedURLs As String()
    Private m_BlockedURLs As String()

    Public Sub New()
        Me.m_Flags = ProxyFlags.None
        Me.m_Name = ""
        Me.m_AllowedURLs = {}
        Me.m_BlockedURLs = {}
    End Sub

    Public Property Flags As ProxyFlags
        Get
            Return Me.m_Flags
        End Get
        Set(value As ProxyFlags)
            Dim oldValue As ProxyFlags = Me.m_Flags
            If (oldValue = value) Then Return
            Me.m_Flags = value
            Me.DoChanged("Flags", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta il nome del profilo
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As String
        Get
            Return Me.m_Name
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_Name
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_Name = value
            Me.DoChanged("Name", value, oldValue)
        End Set
    End Property

    Public Function GetAllowedURLs() As String()
        Return CType(Me.m_AllowedURLs.Clone, String())
    End Function

    Public Function GetBlockedURLs() As String()
        Return CType(Me.m_BlockedURLs.Clone, String())
    End Function

    Public Sub SetAllowedURLs(ByVal value As String())
        Me.m_AllowedURLs = CType(value.Clone, String())
        Me.DoChanged("AllowedURLs", value)
    End Sub

    Public Sub SetBlockedURLs(ByVal value As String())
        Me.m_BlockedURLs = CType(value.Clone, String())
        Me.DoChanged("BlockedURLs", value)
    End Sub

    Public Function IsURLAllowed(ByVal value As String) As Boolean
        Dim ret As Boolean = True
        For Each a As String In Me.m_BlockedURLs
            If value Like a Then
                ret = False
                Exit For
            End If
        Next

        For Each a As String In Me.m_AllowedURLs
            If value Like a Then
                ret = True
                Exit For
            End If
        Next

        Return ret
    End Function




    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Public Overrides Function GetModule() As DMD.Sistema.CModule
        Return DialTPApp.ProxyProfiles.Module
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_ProxyProfiles"
    End Function

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Me.m_Flags = reader.Read("Flags", Me.m_Flags)
        Me.m_Name = reader.Read("Name", Me.m_Name)
        Me.m_AllowedURLs = Me.strtoarr(reader.Read("AllowedURLs", ""))
        Me.m_BlockedURLs = Me.strtoarr(reader.Read("BlockedURLs", ""))

        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        writer.Write("Flags", Me.m_Flags)
        writer.Write("Name", Me.m_Name)
        writer.Write("AllowedURLs", Me.arrtostr(Me.m_AllowedURLs))
        writer.Write("BlockedURLs", Me.arrtostr(Me.m_BlockedURLs))

        Return MyBase.SaveToRecordset(writer)
    End Function

    Private Function arrtostr(ByVal arr() As String) As String
        If (arr Is Nothing) Then Return ""
        Return Strings.Join(arr, ",")
    End Function

    Private Function strtoarr(ByVal str As String) As String()
        If (String.IsNullOrEmpty(str)) Then Return {}
        Dim ret As New System.Collections.ArrayList
        Dim token As New System.Text.StringBuilder
        For Each ch As Char In str
            Select Case ch
                Case " "c
                Case ","c, ControlChars.Cr, ControlChars.Lf, ControlChars.Tab
                    If (token.Length > 0) Then
                        ret.Add(token.ToString)
                        token.Clear()
                    End If
                Case Else
                    token.Append(ch)
            End Select
        Next
        If (token.Length > 0) Then
            ret.Add(token.ToString)
            token.Clear()
        End If
        Return DirectCast(ret.ToArray(GetType(String)), String())
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute("Flags", Me.m_Flags)
        writer.WriteAttribute("Name", Me.m_Name)
        MyBase.XMLSerialize(writer)
        writer.WriteTag("AllowedURLs", arrtostr(Me.m_AllowedURLs))
        writer.WriteTag("BlockedURLs", arrtostr(Me.m_BlockedURLs))
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "Flags" : Me.m_Flags = CType(XML.Utils.Serializer.DeserializeInteger(fieldValue), ProxyFlags)
            Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "AllowedURLs" : Me.m_AllowedURLs = strtoarr(CStr(fieldValue))
            Case "BlockedURLs" : Me.m_BlockedURLs = strtoarr(CStr(fieldValue))
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub

    Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
        Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
        DialTPApp.ProxyProfiles.UpdateCached(Me)
        Return ret
    End Function



End Class
