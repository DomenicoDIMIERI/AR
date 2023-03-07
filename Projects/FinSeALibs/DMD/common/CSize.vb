Imports DMD.Sistema
Imports DMD.Anagrafica


Public Class CSize
    Implements DMD.XML.IDMDXMLSerializable
    Private Shared empty As New CSize

    Private m_Width As Double
    Private m_Height As Double

    Public Sub New()
        Me.New(0, 0)
    End Sub

    Public Sub New(ByVal width As Double, ByVal height As Double)
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_Width = width
        Me.m_Height = height
    End Sub

    Public Sub New(ByVal rec As CSize)
        Me.New(rec.m_Width, rec.m_Height)
    End Sub
     
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
        Return "[" & Me.m_Width & ", " & Me.m_Height & "]"
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is CSize) Then Return False
        Dim b As CSize = obj
        Return (Me.m_Width = b.m_Width) AndAlso (Me.m_Height = b.m_Height)
    End Function

    Public Shared Widening Operator CType(ByVal rec As Size) As CSize
        Return New CSize(rec.Width, rec.Height)
    End Operator

    Public Shared Narrowing Operator CType(ByVal rec As CSize) As Size
        Return New Size(rec.Width, rec.Height)
    End Operator

    Public Shared Operator =(ByVal a As CSize, ByVal b As CSize) As Boolean
        Return a.Equals(b)
    End Operator

    Public Shared Operator <>(ByVal a As CSize, ByVal b As CSize) As Boolean
        Return Not a.Equals(b)
    End Operator

    Public Shared Operator IsTrue(ByVal a As CSize) As Boolean
        Return a.Equals(empty)
    End Operator

    Public Shared Operator IsFalse(ByVal a As CSize) As Boolean
        Return Not a.Equals(empty)
    End Operator

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Width" : Me.m_Width = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            Case "Height" : Me.m_Height = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Width", Me.m_Width)
        writer.WriteAttribute("Height", Me.m_Height)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class