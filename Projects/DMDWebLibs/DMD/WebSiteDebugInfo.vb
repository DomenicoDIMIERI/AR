Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Web

Partial Class WebSite

    ''' <summary>
    ''' Questa classe serve per stabilire un sistema di comunicazione ad eventi 
    ''' con 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WebSiteDebugInfo
        Inherits SysDebuInfo

        Public Sub New()
            ' DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Overrides Sub Initialize()
            MyBase.Initialize()

            Me.Notes &= ""
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
            Me.Notes &= "SESSIONI ATTIVE" & vbNewLine
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine

            Dim col As CCollection(Of CSessionInfo) = DirectCast(Sistema.ApplicationContext, WebApplicationContext).GetAllSessions
            For Each info As CSessionInfo In col
                With info
                    Me.Notes &= vbNewLine
                    Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine

                    Me.Notes &= "SessionID: " & .CurrentSession.SessionID & vbNewLine
                    Me.Notes &= "StartTime: " & Formats.FormatUserDateTime(.CurrentSession.StartTime)
                    Me.Notes &= vbNewLine
                    If (.CurrentUser IsNot Nothing) Then
                        Me.Notes &= "User: " & .CurrentUser.UserName & vbNewLine
                    End If
                    If (.CurrentLogin IsNot Nothing) Then
                        Me.Notes &= "LogIn: " & .CurrentLogin.RemoteIP & ": " & .CurrentLogin.RemotePort & " (" & .CurrentLogin.NomeUfficio & ")" & vbNewLine
                        Me.Notes &= "       " & Formats.FormatUserDateTime(.CurrentLogin.LogInTime) & " - " & Formats.FormatUserDateTime(.CurrentLogin.LogOutTime) & " (" & [Enum].GetName(GetType(LogOutMethods), .CurrentLogin.LogoutMethod) & ")" & vbNewLine
                        Me.Notes &= "       " & .CurrentLogin.UserAgent & vbNewLine
                    End If
                    Me.Notes &= vbNewLine
                    SyncLock .CurrentSession
                        Me.Notes &= "Cursori aperti: " & .RemoteOpenedCursors.Count & vbNewLine
                        For Each c As DBObjectCursorBase In .RemoteOpenedCursors
                            Me.Notes &= TypeName(c) & "[" & c.Token & ", " & [Enum].GetName(GetType(DBCursorStatus), c.CursorStatus) & "]: " & c.GetFullSQL & vbNewLine
                        Next
                    End SyncLock
                    Me.Notes &= vbNewLine
                    SyncLock info
                        Me.Notes &= ""
                        Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
                        Me.Notes &= "QUERIS PENDENTI " & vbNewLine
                        Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine

                        'For Each item As StatsItem In info.QueriesInfo
                        '    Me.Notes &= Formats.FormatUserDateTime(item.LastRun) & " " & item.Name & vbNewLine
                        'Next

                        'Me.Notes &= ""
                        'Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
                        'Me.Notes &= "RICHIESTE PENDENTI " & vbNewLine
                        'Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine

                        'For Each item As StatsItem In info.PagesInfo
                        '    Me.Notes &= Formats.FormatUserDateTime(item.LastRun) & " " & item.Name & vbNewLine
                        'Next
                    End SyncLock

                End With
            Next

        End Sub

    End Class

End Class
