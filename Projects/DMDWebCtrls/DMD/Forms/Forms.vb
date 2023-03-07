Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases


Namespace Forms

    Public Enum TextAlignEnum As Integer
        TEXTALIGN_DEFAULT = 0
        TEXTALIGN_LEFT = 1
        TEXTALIGN_RIGHT = 2
        TEXTALIGN_CENTER = 3
        TEXTALIGN_JUSTIFY = 4
    End Enum

    Public Enum DockType As Integer
        DOCK_NONE = 0
        DOCK_LEFT = 1
        DOCK_TOP = 2
        DOCK_RIGHT = 3
        DOCK_BOTTOM = 4
        DOCK_FILL = 5
    End Enum


#Region "WebControlAttributes"



    Public Class CWebControlAttributes
        Inherits CKeyCollection(Of String)

        Private m_Owner As WebControl

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Private Shadows Sub Add()
        End Sub
        Private Shadows Sub Insert()
        End Sub

        Default Public Shadows Property Item(ByVal attrName As String) As String
            Get
                attrName = LCase(Trim(attrName))
                Dim i As Integer = Me.IndexOfKey(attrName)
                If (i < 0) Then Return vbNullString
                Return MyBase.Item(i)
            End Get
            Set(value As String)
                attrName = LCase(Trim(attrName))
                Dim i As Integer = Me.IndexOfKey(attrName)
                If (i < 0) Then
                    MyBase.Add(attrName, "" & value)
                Else
                    MyBase.Item(i) = "" & value
                End If
            End Set
        End Property

        Default Public Shadows Property Item(ByVal i As Integer) As String
            Get
                Return MyBase.Item(i)
            End Get
            Set(value As String)
                MyBase.Item(i) = "" & value
            End Set
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As WebControl)
            Me.m_Owner = owner
        End Sub

    End Class

#End Region




    '----------------------------------------------------
    Public Class CWebControlsCollection
        Inherits CCollection(Of WebControl)

        Private m_Owner As WebControl

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public ReadOnly Property Owner As WebControl
            Get
                Return Me.m_Owner
            End Get
        End Property
        Protected Friend Sub SetOwner(ByVal value As WebControl)
            Me.m_Owner = value
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, WebControl).SetParent(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub


    End Class




    Public Class CSearchStateItem
        Public fieldName As String
        Public fieldValue As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub FromXML(ByVal value As String)
            Dim ret As String
            ret = XML.Utils.Serializer.GetInnerTAG(value, "CSearchStateItem")
            Me.fieldName = Strings.HtmlDecode(XML.Utils.Serializer.GetInnerTAG(ret, "name"))
            Me.fieldValue = Strings.HtmlDecode(XML.Utils.Serializer.GetInnerTAG(ret, "value"))
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String
            ret = ""
            ret = ret & "<CSearchStateItem>"
            ret = ret & "<name>" & Strings.HtmlEncode(Me.fieldName) & "</name>"
            ret = ret & "<value>" & Strings.HtmlEncode(Me.fieldValue) & "</value>"
            ret = ret & "</CSearchStateItem>"
            Return ret
        End Function

    End Class

    Public Class CSearchState
        Inherits CCollection(Of CSearchStateItem)

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Crea un nuovo oggetto CSearchStateItem e lo aggiunge in coda alla collezione.
        ''' </summary>
        ''' <param name="fieldName"></param>
        ''' <param name="fieldValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function Add(ByVal fieldName As String, ByVal fieldValue As String) As CSearchStateItem
            Return Me.SetValue(fieldName, fieldValue)
        End Function

        Public Function IndexOfKey(ByVal fieldName As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If UCase(Me.Item(i).fieldName) = UCase(Trim(fieldName)) Then Return i
            Next
            Return -1
        End Function

        Public Function GetValue(ByVal fieldName As String) As String
            Dim i As Integer = Me.IndexOfKey(fieldName)
            If (i >= 0) Then
                Return Me.Item(i).fieldValue
            Else
                Return vbNullString
            End If
        End Function

        Public Function SetValue(ByVal fieldName As String, ByVal fieldValue As String) As CSearchStateItem
            Dim i As Integer
            Dim item As CSearchStateItem
            i = Me.IndexOfKey(fieldName)
            If (i >= 0) Then
                item = MyBase.Item(i)
            Else
                item = New CSearchStateItem
                Call MyBase.Add(item)
            End If
            item.fieldName = Trim(fieldName)
            item.fieldValue = fieldValue
            Return item
        End Function

        Public Sub RemoveByKey(ByVal fieldName As String)
            Dim i As Integer
            i = Me.IndexOfKey(fieldName)
            MyBase.RemoveAt(i)
        End Sub

        Public Function ContainsKey(ByVal fieldName As String) As Boolean
            Return (Me.IndexOfKey(fieldName) >= 0)
        End Function
        Public Overrides Function ToString() As String
            Dim ret As String
            ret = "<CSearchState count=""" & Me.Count & """>"
            For i As Integer = 0 To Me.Count - 1
                ret = ret & MyBase.Item(i).ToString()
            Next
            ret = ret & "</CSearchState>"
            Return ret
        End Function

        Public Sub FromXML(ByVal value As String)
            Dim tmp As String
            Dim i As Integer
            Dim Item As CSearchStateItem
            Me.Clear()
            tmp = XML.Utils.Serializer.GetInnerTAG(value, "CSearchState")

            i = InStr(tmp, "</CSearchStateItem>")
            While (i > 0)
                Item = New CSearchStateItem
                Call Item.FromXML(Left(tmp, i + Len("</CSearchStateItem>")))
                Call MyBase.Add(Item)
                tmp = Mid(tmp, i + Len("</CSearchStateItem>"))
                i = InStr(tmp, "</CSearchStateItem>")
            End While
        End Sub

        Public Sub FromPOST()
            Dim s As String
            Me.Clear()
            For Each s In WebSite.ASP_Request.Form
                Call Me.Add(s, WebSite.ASP_Request.Form(s))
            Next
        End Sub


    End Class




End Namespace