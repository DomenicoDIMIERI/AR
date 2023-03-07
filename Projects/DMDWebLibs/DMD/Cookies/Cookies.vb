Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Partial Public Class WebSite


    Public NotInheritable Class CCookiesClass
        Friend Sub New()
        End Sub

        Public Function GetCookie(ByVal name As String, Optional ByVal defValue As String = vbNullString) As String
            Dim ret As System.Web.HttpCookie = WebSite.ASP_Request.Cookies(name)
            If ret Is Nothing Then Return defValue
            Try
                Return ret.Value
            Catch ex As Exception
                Return defValue
            End Try
        End Function

        Public Function IsCookieSet(ByVal name As String) As Boolean
            Dim ret As System.Web.HttpCookie = WebSite.ASP_Request.Cookies(name)
            Return (ret IsNot Nothing)
        End Function

        Public Function CanWriteCookies() As Boolean
            Return Me.GetCookie("__DMDWLIBFSE", "") = "YES"
        End Function

        Public Sub SetCanWriteCookies(ByVal value As Boolean)
            If (value) Then
                Me._SetCookie("__DMDWLIBFSE", "YES")
            Else
                Me._SetCookie("__DMDWLIBFSE", "", Now)
            End If
        End Sub

        Private Sub _SetCookie(ByVal cookieName As String, ByVal value As String, Optional ByVal expireDate As Date? = Nothing)
            Dim c As System.Web.HttpCookie
            If IsCookieSet(cookieName) Then
                c = WebSite.ASP_Response.Cookies(cookieName)
            Else
                c = New System.Web.HttpCookie(cookieName, value)
                WebSite.ASP_Response.Cookies.Add(c)
            End If
            If (expireDate.HasValue) Then
                c.Expires = expireDate.Value
            Else
                c.Expires = DateUtils.MakeDate(2500, 1, 1)
            End If
        End Sub

        Public Sub SetCookie(ByVal cookieName As String, ByVal value As String, Optional ByVal expireDate As Date? = Nothing)
            If (Not Me.CanWriteCookies) Then
                Debug.Print("Permesso Negato per il cookie: " & cookieName & ", " & value)
                Return
            Else

            End If

            Dim c As System.Web.HttpCookie
            If IsCookieSet(cookieName) Then
                c = WebSite.ASP_Response.Cookies(cookieName)
            Else
                c = New System.Web.HttpCookie(cookieName, value)
                WebSite.ASP_Response.Cookies.Add(c)
            End If
            If (expireDate.HasValue) Then
                c.Expires = expireDate.Value
            Else
                c.Expires = DateUtils.MakeDate(2500, 1, 1)
            End If
        End Sub

    End Class

    Public Shared ReadOnly Cookies As New CCookiesClass

End Class
