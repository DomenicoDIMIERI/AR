Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases


Namespace Forms

    <Serializable> _
    Public Class WebControl
        Implements DMD.XML.IDMDXMLSerializable
        Private Shared WebControls_m_Curr As Integer = 0

        <NonSerialized> Private m_Parent As WebControl
        Private m_AutoScroll As Boolean
        Private m_AutoGrow As Boolean
        Private m_Dock As DockType
        Private m_Text As String
        Private m_Attributes As CWebControlAttributes
        Private m_Controls As CWebControlsCollection
        Private m_Enabled As Boolean
        Private m_Visible As Boolean
        Private m_LayoutSuspended As Boolean
        Private m_ShouldLayout As Boolean
        Private m_CanRaiseEvents As Boolean
        Private m_Bounds As CRectangle
        Private domInit As Boolean
        Private m_Style As WebControlStyle
        Private m_Font As CFont
        Private m_TextAlign As TextAlignEnum
        Private __DMDINIT As Boolean
        Private _tooltip As String
        Private m_IsUpdating As Boolean
        Private m_Tag As Object
        Private m_IsRefresh As Boolean
        Private m_Name As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Parent = Nothing
            Me.m_AutoScroll = False
            Me.m_AutoGrow = False
            Me.m_Dock = DockType.DOCK_NONE
            Me.m_Text = ""
            Me.m_Attributes = Nothing
            Me.m_Controls = Nothing
            Me.m_Enabled = True
            Me.m_Visible = True
            Me.m_LayoutSuspended = False
            Me.m_ShouldLayout = True
            Me.m_CanRaiseEvents = True
            Me.m_Bounds = Nothing
            Me.domInit = False
            Me.m_Style = New WebControlStyle(Me)
            Me.m_Style.Border = New WebControlBorder("none") '1px solid #e0e0e0");
            Me.m_Font = Nothing
            Me.m_TextAlign = TextAlignEnum.TEXTALIGN_DEFAULT
            Me.__DMDINIT = False
            Me._tooltip = ""

            Me.m_Name = ""
            Me.m_IsRefresh = False
            'Me.m_Style.Border = "solid 1px #e0e0e0"
            Me.Enabled = True
        End Sub

        Public Sub New(ByVal elemName As String)
            Me.New()
            Me.m_Name = Trim(elemName)
        End Sub
         

        Public Property Style As WebControlStyle
            Get
                Return Me.m_Style
            End Get
            Set(value As WebControlStyle)
                Me.m_Style = value
            End Set
        End Property

        Public Property Bounds As CRectangle
            Get
                If (Me.m_Bounds Is Nothing) Then
                    Dim b As CSize = Me.GetPreferredSize()
                    Return New CRectangle(0, 0, b.Width, b.Height())
                Else
                    Return New CRectangle(Me.m_Bounds)
                End If
            End Get
            Set(value As CRectangle)
                'var b = (arguments.length == 1) ? arguments[0] : new CRectangle(Formats.ToInteger(arguments[0]), Formats.ToInteger(arguments[1]), Formats.ToInteger(arguments[2]), Formats.ToInteger(arguments[3]));
                If value.Equals(Me.Bounds) Then Return
                Me.SetBounds(value)
                Dim e As New System.EventArgs
                'var e = new Object();
                'e.Size = this.getSize();
                'e.Location = this.getLocation();
                Me.OnLocationChanged(e)
                Me.OnResize(e)
            End Set
        End Property

        Protected Overridable Sub OnLocationChanged(ByVal e As System.EventArgs)

        End Sub

        Protected Overridable Sub OnResize(ByVal e As System.EventArgs)

        End Sub

        Protected Friend Overridable Sub SetBounds(ByVal b As CRectangle)
            Me.m_Bounds = b
            'Me._updateInterface()
        End Sub

        'Public Function GetParameter1(ByVal paramName As String) As String
        '    Return Sistema.ApplicationContext.GetParameter(paramName)
        'End Function

        Public Function GetParameter(Of T As Structure)(ByVal paramName As String, Optional ByVal defValue As Object = Nothing) As Nullable(Of T)
            Dim tp As System.Type = GetType(T)
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            If param = vbNullString Then Return defValue

            Dim ret As Object = Nothing

            If (tp) Is GetType(Boolean) Then ret = Formats.ParseBool(param)
            If (tp) Is GetType(Byte) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(SByte) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Int16) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(UInt16) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Int32) OrElse tp.IsEnum Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(UInt32) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Long) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(ULong) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Single) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Double) Then ret = Formats.ParseDouble(param)
            If (tp) Is GetType(Decimal) Then ret = Formats.ParseValuta(param)
            If (tp) Is GetType(Date) Then ret = Formats.ParseDate(param)
            If (tp) Is GetType(Char) Then ret = Formats.ToString(param)
            If (tp) Is GetType(String) Then ret = Formats.ToString(param)

            Debug.Assert(ret IsNot Nothing)

            Return CType(ret, T)
        End Function



        Public Function IsParameterSet(ByVal paramName As String) As Boolean
            Return Me.GetParameter(paramName, vbNullString) <> vbNullString
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As Integer) As Integer
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As Nullable(Of Integer) = Formats.ParseInteger(param)
            If (ret.HasValue) Then Return ret
            Return defValue
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As Boolean) As Boolean
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As Nullable(Of Boolean) = Formats.ParseBool(param)
            If (ret.HasValue) Then Return ret
            Return defValue
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As Double) As Double
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As Nullable(Of Double) = Formats.ParseDouble(param)
            If (ret.HasValue) Then Return ret
            Return defValue
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As Decimal) As Decimal
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As Nullable(Of Decimal) = Formats.ParseValuta(param)
            If (ret.HasValue) Then Return ret
            Return defValue
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As Date) As Date
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As Nullable(Of Date) = Formats.ParseDate(param)
            If (ret.HasValue) Then Return ret
            Return defValue
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As String) As String
            Dim param As String = Sistema.ApplicationContext.GetParameter(paramName)
            Dim ret As String = Formats.ToString(param)
            If (ret = vbNullString) Then Return defValue
            Return ret
        End Function

        ''' <summary>
        ''' Organizza i controlli in base al loro layout
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub DoLayout()
            Me.m_ShouldLayout = False
            'Me.ParsePostbackData()

            For Each ctrl As WebControl In Me.Controls
                ctrl.DoLayout()
                'ctrl.ParsePostbackData()
            Next
        End Sub



        Protected Shared Function WebControl_NewName() As String
            WebControls_m_Curr += 1
            Dim tmp As String = "DMDCTRL"
            Dim p As Integer = InStr(tmp, "(")
            If (p > 0) Then
                tmp = Strings.Left(tmp, p - 1)
            End If
            Return tmp & WebControls_m_Curr
        End Function



        Public Property Visible As Boolean
            Get
                Return Me.m_Visible
            End Get
            Set(value As Boolean)
                Me.m_Visible = value
            End Set
        End Property

        Public ReadOnly Property Controls As CWebControlsCollection
            Get
                If (Me.m_Controls Is Nothing) Then
                    Me.m_Controls = New CWebControlsCollection
                    Me.m_Controls.SetOwner(Me)
                End If
                Return Me.m_Controls
            End Get
        End Property

        Public Overridable Property Text As String
            Get
                Return Me.m_Text
            End Get
            Set(value As String)
                Me.m_Text = Strings.HtmlEncode(value)
            End Set
        End Property

        Public Overridable Property InnerHTML As String
            Get
                Return Me.m_Text
            End Get
            Set(value As String)
                Me.m_Text = value
            End Set
        End Property

        Public Function GetPreferredSize() As CSize
            Return New CSize(100, 100)
        End Function

        Public Property AutoScroll As Boolean
            Get
                Return Me.m_AutoScroll
            End Get
            Set(value As Boolean)
                Me.m_AutoScroll = value
            End Set
        End Property

        Public Property AutoGrow As Boolean
            Get
                Return Me.m_AutoGrow
            End Get
            Set(value As Boolean)
                Me.m_AutoGrow = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Me.m_Name = Utils.FormsUtils.GetValidControlName(value)
                Me.OnNameChanged(New System.EventArgs)
            End Set
        End Property



        Protected Overridable Sub OnNameChanged(ByVal e As System.EventArgs)
            Me.SetAttribute("name", Me.Name)
            Me.SetAttribute("id", Me.Name)
        End Sub

        Public Property Left As String
            Get
                Return Me.Bounds.Left
            End Get
            Set(value As String)
                Dim b As CRectangle = Me.Bounds
                b.Left = value
                Me.Bounds = b
            End Set
        End Property

        Public Property Top As String
            Get
                Return Me.Bounds.Top
            End Get
            Set(value As String)
                Dim b As CRectangle = Me.Bounds
                b.Top = value
                Me.Bounds = b
            End Set
        End Property

        Public Overridable Property Width As String
            Get
                Return Me.Bounds.Width
            End Get
            Set(value As String)
                Dim b As CRectangle = Me.Bounds
                b.Width = value
                Me.Bounds = b
            End Set
        End Property

        Public Overridable Property Height As String
            Get
                Return Me.Bounds.Height
            End Get
            Set(value As String)
                Dim b As CRectangle = Me.Bounds
                b.Height = value
                Me.Bounds = b
            End Set
        End Property

        Public Property Dock As DockType
            Get
                Return Me.m_Dock
            End Get
            Set(value As DockType)
                Me.m_Dock = value
            End Set
        End Property

        Public Property BackColor As String
            Get
                Return Me.Style.Background.Color
            End Get
            Set(value As String)
                Me.Style.Background.Color = Trim(value)
            End Set
        End Property

        Public Property TextColor As String
            Get
                Return Me.Style.Color
            End Get
            Set(value As String)
                Me.Style.Color = Trim(value)
            End Set
        End Property

        Public ReadOnly Property Parent As WebControl
            Get
                Return Me.m_Parent
            End Get
        End Property
        Protected Friend Overridable Sub SetParent(ByVal value As WebControl)
            Me.m_Parent = value
        End Sub

        Public ReadOnly Property Attributes As CWebControlAttributes
            Get
                If Me.m_Attributes Is Nothing Then Me.m_Attributes = New CWebControlAttributes
                Return Me.m_Attributes
            End Get
        End Property

        Public Property Enabled As Boolean
            Get
                Return Me.m_Enabled
            End Get
            Set(value As Boolean)
                Me.m_Enabled = value
            End Set
        End Property

        Public Property OnClick As String
            Get
                Return Me.Attributes("onclick")
            End Get
            Set(value As String)
                Me.Attributes("onclick") = Trim(value)
            End Set
        End Property

        Public Property OnChange As String
            Get
                Return Me.Attributes("onchange")
            End Get
            Set(value As String)
                Me.Attributes("onchange") = Trim(value)
            End Set
        End Property

        Public Property OnKeyPress As String
            Get
                Return Me.GetAttribute("onkeypress")
            End Get
            Set(value As String)
                Me.SetAttribute("onkeypress", Trim(value))
            End Set
        End Property

        Public Sub SetAttribute(ByVal name As String, ByVal value As String)
            Me.Attributes.SetItemByKey(name, value)
        End Sub

        Public Function GetAttribute(ByVal name As String) As String
            Return Me.Attributes.GetItemByKey(name)
        End Function

        Public Sub RemoveAttribute(ByVal name As String)
            If (Me.Attributes.ContainsKey(name)) Then Me.Attributes.RemoveByKey(name)
        End Sub

        Public Overridable Sub GetAttributesHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            If (Me.Dock <> DockType.DOCK_NONE) Then Me.SetAttribute("frm_dock", Me.Dock)
            If (Me.Name <> "") Then
                Me.SetAttribute("name", Me.Name)
                Me.SetAttribute("id", Me.Name)
            End If
            'Stile
            Dim style As String = Me.GetStyleHTML
            If style <> "" Then
                Me.SetAttribute("style", style)
            Else
                Me.RemoveAttribute("style")
            End If

            For Each key In Me.Attributes.Keys
                If (key <> vbNullString) Then
                    writer.Write(" ")
                    writer.Write(key)

                    Dim value As String = Me.Attributes(key)
                    If (value <> vbNullString) Then
                        writer.Write("=")
                        writer.Write(Chr(34))
                        writer.Write(value)
                        writer.Write(Chr(34))
                    End If
                End If
            Next
        End Sub

        Public Overridable Function GetStyleHTML() As String
            Dim html As String = Me.m_Style.ToString
            If Me.AutoScroll = True Then
                Me.Style.Overflow = "auto"
            Else
                Me.Style.Overflow = "hidden"
            End If
            Me.Style.WhiteSpace = "nowrap"

            Select Case Me.Dock
                Case DockType.DOCK_LEFT
                    Me.Style.Left = 0
                    Me.Style.Height = "100%"
                    Me.Style.Width = Me.Width
                Case DockType.DOCK_TOP
                    Me.Style.Top = 0
                    Me.Style.Width = "100%"
                    Me.Style.Height = Me.Height
                Case DockType.DOCK_RIGHT
                    Me.Style.Right = 0
                    Me.Style.Height = "100%"
                    Me.Style.Width = Me.Width
                Case DockType.DOCK_BOTTOM
                    Me.Style.Bottom = 0
                    Me.Style.Width = "100%"
                    Me.Style.Height = Me.Height
                Case DockType.DOCK_FILL
                    Me.Style.Left = 0
                    Me.Style.Top = 0
                    Me.Style.Width = "100%"
                    Me.Style.Height = "100%"
                Case Else
                    'Case DOCK_NONE
                    If Me.m_AutoGrow Then
                        Me.Style.MinWidth = Me.Width & "px"
                        Me.Style.MinHeight = Me.Height & "px"
                    Else
                        Me.Style.Width = Me.Width
                        Me.Style.Height = Me.Height
                    End If
            End Select
            'If m_BackColor <> "" Then html = Strings.Combine(html, "background-color:" & m_BackColor, ";")
            If (Me.Style.Display = "" AndAlso Me.Visible = False) Then Me.Style.Display = "none"

            Return Me.Style.ToString
        End Function

        Public Overridable Sub GetInnerHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim i As Integer
            For i = 0 To Me.Controls.Count - 1
                Me.Controls.Item(i).CreateHTML(writer)
            Next
            writer.Write(Me.Text)
        End Sub

        Public Overridable Function GetElementName() As String
            Return "div"
        End Function

        'Public Overridable Function CreateHTML() As String
        '    Dim w As New System.Text.StringBuilder
        '    Dim writer As New System.Web.UI.HtmlTextWriter(w)
        '    Me.CreateHTML(writer)
        '    Dim ret As String = writer.ToString
        '    'writer.Dispose()
        '    Return ret
        'End Function

        Public Overridable Sub CreateHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            If (Me.m_ShouldLayout) Then Me.DoLayout()
            Dim elemName As String = Me.GetElementName

            writer.Write("<")
            writer.Write(elemName)
            Me.GetAttributesHTML(writer)
            writer.Write(">")
            'writer.CheckInTag()
            'If (Not writer.CanCloseTag(Me.GetElementName)) Then
            Me.GetInnerHTML(writer)
            'End If
            'writer.EndTag()
            writer.Write("</")
            writer.Write(elemName)
            writer.Write(">")

            'Attributi


            If (Me.m_Parent Is Nothing) Then
                writer.Write("<script type=""text/javascript"" language=""javascript"">" & vbNewLine)
                Me.GetScriptHTML(writer)
                writer.Write("</script>")
            End If
        End Sub


        Public Overridable Sub GetScriptHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            writer.Write("var _" & Me.Name & " = new " & TypeName(Me) & "(""" & Me.Name & """);")
            For Each c As WebControl In Me.Controls
                c.GetScriptHTML(writer)
            Next
        End Sub


        Public Function IsRefresh() As Boolean
            Return Me.m_IsRefresh
        End Function

        'Public Sub Refresh()
        '    Me.m_IsRefresh = True
        '    Me.WriteHTML()
        '    Me.m_IsRefresh = False
        'End Sub

        ''' <summary>
        ''' Chiamata in fase di inizializzazione per ricostruire lo stato dell'oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub ParsePostbackData()

        End Sub

        Public Property Font As CFont
            Get
                Return Me.m_Font
            End Get
            Set(value As CFont)
                Me.m_Font = value
            End Set
        End Property

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AutoScroll" : Me.m_AutoScroll = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "AutoGrow" : Me.m_AutoGrow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Dock" : Me.m_Dock = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Enabled" : Me.m_Enabled = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "LayoutSuspended" : Me.m_LayoutSuspended = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ShouldLayout" : Me.m_ShouldLayout = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CanRaiseEvents" : Me.m_CanRaiseEvents = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Bounds" : Me.m_Bounds = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Text" : Me.m_Text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributes" : Me.m_Attributes = fieldValue : Me.m_Attributes.SetOwner(Me)
                Case "Style" : Me.m_Style = fieldValue : Me.m_Style.SetOwner(Me)
                Case "Controls" : Me.m_Controls = fieldValue : Me.m_Controls.SetOwner(Me)
                Case "Font" : Me.m_Font = XML.Utils.Serializer.ToObject(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("AutoScroll", Me.m_AutoScroll)
            writer.WriteAttribute("AutoGrow", Me.m_AutoGrow)
            writer.WriteAttribute("Dock", Me.m_Dock)
            writer.WriteAttribute("Enabled", Me.m_Enabled)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("LayoutSuspended", Me.m_LayoutSuspended)
            writer.WriteAttribute("ShouldLayout", Me.m_ShouldLayout)
            writer.WriteAttribute("CanRaiseEvents", Me.m_CanRaiseEvents)
            writer.WriteAttribute("TextAlign", Me.m_TextAlign)
            writer.WriteTag("Bounds", Me.m_Bounds)
            writer.WriteTag("Text", Me.m_Text)
            writer.WriteTag("Attributes", Me.Attributes)
            writer.WriteTag("Style", Me.Style)
            writer.WriteTag("Controls", Me.Controls)
            writer.WriteTag("Font", Me.Font)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace