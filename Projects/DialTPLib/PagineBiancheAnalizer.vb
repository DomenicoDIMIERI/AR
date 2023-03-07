Imports System.Windows.Forms
Imports DMD

Public Class PagineBiancheAnalizer
    Inherits WebPageAnalizer

    Public Class Item
        Public Number As String
        Public Name As String
        Public Link As String
        Public Insegna As String
        Public Addresses As New CCollection(Of ItemAddress)

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.Number & ") " & Me.Name & " (" & Me.Insegna & ")")
            For Each a As ItemAddress In Me.Addresses
                ret.Append(vbCrLf)
                ret.Append(a.ToString)
            Next
            ret.Append(vbCrLf)
            Return ret.ToString
        End Function
    End Class

    Public Class ItemAddress
        Public StreetAddress As String
        Public PostalCode As String
        Public Locality As String
        Public Latitude As String
        Public Longitude As String

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.StreetAddress)
            ret.Append(" - ")
            ret.Append(Me.PostalCode)
            ret.Append(" ")
            ret.Append(Me.Locality)
            Return ret.ToString
        End Function
    End Class

    Private m_Results As New CCollection(Of Item)

    Public Sub New()
    End Sub

    Public ReadOnly Property Results As CCollection(Of Item)
        Get
            Return Me.m_Results
        End Get
    End Property

    Public Sub CercaInfoNumero(ByVal numero As String)
        Me.m_Results = New CCollection(Of Item)
        Me.Analize("http://www.paginebianche.it/ricerca-da-numero?qs=" & Replace(numero, " ", ""))
    End Sub

    Private Function GetChildOfType(ByVal node As System.Xml.XmlElement, ByVal tagName As String) As System.Xml.XmlElement
        Dim items As System.Xml.XmlNodeList = node.GetElementsByTagName(tagName)
        If (items Is Nothing OrElse items.Count = 0) Then
            Return Nothing
        Else
            Return CType(items(0), System.Xml.XmlElement)
        End If
    End Function


    Protected Overrides Sub OnDataReady(e As DataEventArgs)
        Dim doc As New System.Xml.XmlDocument
        doc.LoadXml(e.Data)

        Me.m_Results.Clear()
        Dim i As Integer = 1
        Dim node As System.Xml.XmlElement = doc.GetElementById("co_" & i)
        While (node IsNot Nothing)
            Dim o As New Item
            o.Number = CStr(i)
            Dim a As System.Xml.XmlElement = Me.GetChildOfType(node, "a")
            If (a Is Nothing) Then Exit Sub
            o.Name = a.InnerText
            o.Link = a.GetAttribute("href")
            Dim j As Integer = 1
            Dim node1 As System.Xml.XmlElement = doc.GetElementById("addr_" & j)
            While (node1 IsNot Nothing)
                Dim oa As New ItemAddress
                For Each e1 As HtmlElement In node1.ChildNodes
                    Select Case Trim(e1.GetAttribute("itemprop"))
                        Case "streetAddress" : oa.StreetAddress = e1.InnerText
                        Case "postalCode" : oa.PostalCode = e1.InnerText
                        Case "addressLocality" : oa.Locality = e1.InnerText
                    End Select
                Next
                o.Addresses.Add(oa)
                j += 1
                node1 = doc.GetElementById("addr_" & j)
            End While

            Me.m_Results.Add(o)
            i += 1
            node = doc.GetElementById("co_" & i)
        End While

        MyBase.OnDataReady(e)
    End Sub

    Public Overrides Function ToString() As String
        Dim ret As New System.Text.StringBuilder
        For Each o As Item In Me.Results
            If (ret.Length > 0) Then ret.Append(vbCrLf)
            ret.Append(o.ToString)
        Next
        Return ret.ToString
    End Function

    Public Overrides Sub Cancel()
        Me.m_Results = New CCollection(Of Item)
        MyBase.Cancel()
    End Sub

End Class
