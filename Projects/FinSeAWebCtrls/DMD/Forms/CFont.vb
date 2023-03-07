Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases
Imports System.Drawing

Namespace Forms

    Public Class CFont
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Name As String
        Private m_Size As String
        Private m_Flags As Integer

        Public Sub New()
            Me.New("", "12pt", 0)
        End Sub

        Public Sub New(ByVal name As String)
            Me.New(name, "12pt", 0)
        End Sub

        Public Sub New(ByVal name As String, ByVal size As String)
            Me.New(name, size, 0)
        End Sub

        Public Sub New(ByVal name As String, ByVal size As String, ByVal flags As Integer)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Name = Strings.Trim(name)
            Me.m_Size = Strings.Trim(size)
            Me.m_Flags = flags
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property Size As String
            Get
                Return Me.m_Size
            End Get
        End Property

        Public Function isBold() As Boolean
            Return (Me.m_Flags And 1) = 1
        End Function

        Public Function isItalic() As Boolean
            Return (Me.m_Flags And 2) = 2
        End Function

        Public Function isUnderline() As Boolean
            Return (Me.m_Flags And 4) = 4
        End Function

        Public Function resize(ByVal newSize As String) As CFont
            Return New CFont(Me.m_Name, newSize, Me.m_Flags)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Name & " " & Me.m_Size & Me.m_Flags
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Size" : Me.m_Size = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("Size", Me.m_Size)
            writer.WriteAttribute("Flags", Me.m_Flags)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function GetSystemFont() As Font
            Dim style As System.Drawing.FontStyle
            If (Me.isBold) Then style = style Or Drawing.FontStyle.Bold
            If (Me.isItalic) Then style = style Or Drawing.FontStyle.Italic
            If (Me.isUnderline) Then style = style Or Drawing.FontStyle.Underline
            Dim unit As System.Drawing.GraphicsUnit = GraphicsUnit.Point
            Dim sizestr As String = LCase(Me.Size)
            Dim size As Single
            If sizestr.EndsWith(" pt") Then
                size = Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2))
                unit = GraphicsUnit.Point
            ElseIf (sizestr.EndsWith(" mm")) Then
                size = Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2))
                unit = GraphicsUnit.Millimeter
            ElseIf (sizestr.EndsWith(" px")) Then
                size = Formats.ToDouble(sizestr.Substring(0, sizestr.Length - 2))
                unit = GraphicsUnit.Pixel
            End If
            Return New System.Drawing.Font(Me.Name, size, style, unit)
        End Function
    End Class

End Namespace