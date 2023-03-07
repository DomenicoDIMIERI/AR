Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.Databases

Namespace Internals

    Public NotInheritable Class CASPSecurityClass
        Const SESSIONTIMEOUT As Integer = 120

        Friend Lock As New Object

        Friend Sub New()
        End Sub

        ''' <summary>
        ''' Restituisce il token in base alla stringa che lo identifica univocamente
        ''' </summary>
        ''' <param name="token"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetToken(ByVal token As String) As CSecurityToken
            Dim db As CDBConnection = APPConn
            Dim ret As CSecurityToken = Nothing
            token = Strings.Trim(token)
            If (token = "") Then Return Nothing

            If (db.IsRemote) Then
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=Sistema&_a=GetToken", "token", RPC.str2n(token))
                ret = XML.Utils.Serializer.Deserialize(tmp)
            Else
                Dim dbSQL As String = "SELECT * FROM [tbl_SecurityTokens] WHERE [Token]='" & token & "'"
                Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
                If dbRis.Read Then
                    ret = New CSecurityToken
                    APPConn.Load(ret, dbRis)
                End If
                dbRis.Dispose()
                dbRis = Nothing
            End If
            Return ret
        End Function

        Friend Function GetAvailableToken() As String
            Dim strToken As String = vbNullString
            Dim isNew As Boolean = False
            While Not isNew
                'Prepariamo il token
                strToken = GetRandomKey(20)
                Dim dbSQL As String = "SELECT [ID] FROM [tbl_SecurityTokens] WHERE [Token]='" & strToken & "'"
                Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)

                isNew = dbRis.Read = False
                dbRis.Dispose()
                dbRis = Nothing
            End While

            Return strToken
        End Function

        Private Function FindTokenByName(ByVal name As String) As CSecurityToken
            Dim dbSQL As String = "SELECT * FROM [tbl_SecurityTokens] WHERE [TokenName]=" & DBUtils.DBString(name) & ""
            Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
            Dim ret As CSecurityToken = Nothing
            If dbRis.Read Then
                ret = New CSecurityToken
                APPConn.Load(ret, dbRis)
            End If
            dbRis.Dispose()
            dbRis = Nothing
            Return ret
        End Function

        ''' <summary>
        ''' Salva il valore specificato e restituisce un token valido solo all'interno della sessione corrente
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateToken(ByVal name As String, ByVal value As String, Optional ByVal expireCount As Integer = 1, Optional ByVal expireTime As Nullable(Of Date) = Nothing) As CSecurityToken
            Dim db As CDBConnection = APPConn
            Dim ret As CSecurityToken = Nothing
            If (db.IsRemote) Then
                name = Strings.Trim(name)
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=Sistema&_a=CreateToken", "name", RPC.str2n(name), "value", RPC.str2n(value), "ec", RPC.int2n(expireCount), "et", RPC.date2n(expireTime))
                ret = XML.Utils.Serializer.Deserialize(tmp)
            Else
                'name = Trim(name)
                SyncLock ASPSecurity.Lock
                    ret = New CSecurityToken
                    ret.m_TokenID = ASPSecurity.GetAvailableToken
                    ret.m_TokenName = name
                    ret.m_Valore = value
                    ret.m_ExpireCount = expireCount
                    ret.m_ExpireTime = expireTime
                    ret.m_CreatoDa = Sistema.Users.CurrentUser
                    ret.m_CreatoDaID = GetID(ret.m_CreatoDa)
                    ret.m_CreatoIl = Now
                    ret.Save(True)
                End SyncLock
            End If
            Return ret
        End Function



        ''' <summary>
        ''' Cerca il token associato al nome specificato e se non esiste lo crea
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindTokenOrCreate(ByVal name As String, ByVal value As String, Optional ByVal expireCount As Integer = 1, Optional ByVal expireTime As Nullable(Of Date) = Nothing) As CSecurityToken
            Dim db As CDBConnection = APPConn
            If (db.IsRemote) Then
                name = Strings.Trim(name)
                'var expireCount = (arguments.length > 2)? Formats.ToInteger(arguments[2]) : 1;
                'var expireTime = (arguments.length > 3)? Formats.ParseDate(arguments[3]) : NULL;
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=Sistema&_a=FindTokenOrCreate", "name", RPC.str2n(name), "value", RPC.str2n(value), "ec", RPC.int2n(expireCount), "et", RPC.date2n(expireTime))
                Return XML.Utils.Serializer.Deserialize(tmp)
            Else
                SyncLock Me.Lock
                    Dim token As CSecurityToken = Me.GetToken(name)
                    If (token Is Nothing) Then token = Me.CreateToken(name, value, expireCount, expireTime)
                    Return token
                End SyncLock
            End If


        End Function


        ''' <summary>
        ''' Genera una chiave alfanumerica della lunghezza specificata
        ''' </summary>
        ''' <param name="keyLen"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRandomKey(ByVal keyLen As Integer) As String
            Const CHARS As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            Dim strToken As String
            Dim i, p As Integer
            Randomize(Timer)
            strToken = ""
            For i = 1 To keyLen
                Do
                    p = Rnd(1) * (Len(CHARS) + 1)
                Loop Until (p >= 1) And (p <= Len(CHARS))
                strToken = strToken & Mid(CHARS, p, 1)
            Next
            Return strToken
        End Function

    End Class
End Namespace

Partial Public Class Sistema



    Private Shared m_ASPSecurity As CASPSecurityClass = Nothing

    Public Shared ReadOnly Property ASPSecurity As CASPSecurityClass
        Get
            If (m_ASPSecurity Is Nothing) Then m_ASPSecurity = New CASPSecurityClass
            Return m_ASPSecurity
        End Get
    End Property

End Class

