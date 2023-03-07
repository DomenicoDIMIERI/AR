Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Text.RegularExpressions

Namespace Net.HTTPProxy

    Friend Class ProxyUtils


        Friend Shared ReadOnly semiSplit As Char() = New Char() {";"c}
        Friend Shared ReadOnly equalSplit As Char() = New Char() {"="c}
        Friend Shared ReadOnly colonSpaceSplit As String() = New String() {": "}
        Friend Shared ReadOnly spaceSplit As Char() = New Char() {" "c}
        Friend Shared ReadOnly commaSplit As Char() = New Char() {","c}
        Friend Shared ReadOnly cookieSplitRegEx As Regex = New Regex(",(?! )")

    End Class

End Namespace
