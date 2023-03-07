Imports System
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates
Imports DMD.Sistema

Namespace Net.HTTPProxy

    Friend NotInheritable Class ProxyServerUtils

        Friend Shared ReadOnly semiSplit As Char() = New Char() {";"c}
        Friend Shared ReadOnly equalSplit As Char() = New Char() {"="c}
        Friend Shared ReadOnly colonSpaceSplit As String() = New String() {": "}
        Friend Shared ReadOnly spaceSplit As Char() = New Char() {" "c}
        Friend Shared ReadOnly commaSplit As Char() = New Char() {","c}
        Friend Shared ReadOnly cookieSplitRegEx As Regex = New Regex(",(?! )")


    End Class


End Namespace

