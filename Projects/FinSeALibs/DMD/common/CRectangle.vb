Imports DMD.Sistema
Imports DMD.Anagrafica


Public Class CRectangle
    Implements DMD.XML.IDMDXMLSerializable

    Private Shared empty As New CRectangle

    Private m_Left As Double
    Private m_Top As Double
    Private m_Width As Double
    Private m_Height As Double

    Public Sub New()
        Me.New(0, 0, 0, 0)
    End Sub

    Public Sub New(ByVal left As Double, ByVal top As Double, ByVal width As Double, ByVal height As Double)
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_Left = left
        Me.m_Top = top
        Me.m_Width = width
        Me.m_Height = height
    End Sub

    Public Sub New(ByVal rec As CRectangle)
        Me.New(rec.m_Left, rec.m_Top, rec.m_Width, rec.m_Height)
    End Sub

    Public Property X As Double
        Get
            Return Me.m_Left
        End Get
        Set(value As Double)
            Me.m_Left = value
        End Set
    End Property

    Public Property Y As Double
        Get
            Return Me.m_Top
        End Get
        Set(value As Double)
            Me.m_Top = value
        End Set
    End Property

    Public Property Left As Double
        Get
            Return Me.m_Left
        End Get
        Set(value As Double)
            Me.m_Left = value
        End Set
    End Property

    Public Property Top As Double
        Get
            Return Me.m_Top
        End Get
        Set(value As Double)
            Me.m_Top = value
        End Set
    End Property

    Public Property Width As Double
        Get
            Return Me.m_Width
        End Get
        Set(value As Double)
            Me.m_Width = value
        End Set
    End Property

    Public Property Height As Double
        Get
            Return Me.m_Height
        End Get
        Set(value As Double)
            Me.m_Height = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "[" & Me.m_Left & ", " & Me.m_Top & ", " & Me.m_Width & ", " & Me.m_Height & "]"
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is CRectangle) Then Return False
        Dim b As CRectangle = obj
        Return (Me.m_Left = b.m_Left) AndAlso (Me.m_Top = b.m_Top) AndAlso (Me.m_Width = b.m_Width) AndAlso (Me.m_Height = b.m_Height)
    End Function

    Public Shared Widening Operator CType(ByVal rec As Rectangle) As CRectangle
        Return New CRectangle(rec.Left, rec.Top, rec.Width, rec.Height)
    End Operator

    Public Shared Narrowing Operator CType(ByVal rec As CRectangle) As Rectangle
        Return New Rectangle(rec.Left, rec.Top, rec.Width, rec.Height)
    End Operator

    Public Shared Operator =(ByVal a As CRectangle, ByVal b As CRectangle) As Boolean
        Return a.Equals(b)
    End Operator

    Public Shared Operator <>(ByVal a As CRectangle, ByVal b As CRectangle) As Boolean
        Return Not a.Equals(b)
    End Operator

    Public Shared Operator IsTrue(ByVal a As CRectangle) As Boolean
        Return a.Equals(empty)
    End Operator

    Public Shared Operator IsFalse(ByVal a As CRectangle) As Boolean
        Return Not a.Equals(empty)
    End Operator


    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Left" : Me.m_Left = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            Case "Top" : Me.m_Top = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            Case "Width" : Me.m_Width = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            Case "Height" : Me.m_Height = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Left", Me.m_Left)
        writer.WriteAttribute("Top", Me.m_Top)
        writer.WriteAttribute("Width", Me.m_Width)
        writer.WriteAttribute("Height", Me.m_Height)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class