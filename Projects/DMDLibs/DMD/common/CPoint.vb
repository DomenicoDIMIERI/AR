Imports DMD.Sistema
Imports DMD.Anagrafica

Public Class CPoint
    Implements DMD.XML.IDMDXMLSerializable

    Private Shared empty As New CPoint

    Private m_X As Double
    Private m_Y As Double

    Public Sub New()
        Me.New(0, 0)
    End Sub

    Public Sub New(ByVal x As Double, ByVal y As Double)
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_X = x
        Me.m_Y = y
    End Sub

    Public Sub New(ByVal p As CPoint)
        Me.New(p.X, p.Y)
    End Sub

    Public Property X As Double
        Get
            Return Me.m_X
        End Get
        Set(value As Double)
            Me.m_X = value
        End Set
    End Property

    Public Property Y As Double
        Get
            Return Me.m_Y
        End Get
        Set(value As Double)
            Me.m_Y = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "[" & Me.m_X & ", " & Me.m_Y & "]"
    End Function

    Public Function Add(ByVal p As CPoint) As CPoint
        Return New CPoint(Me.m_X + p.m_X, Me.m_Y + p.m_Y)
    End Function

    Public Function Subtract(ByVal p As CPoint) As CPoint
        Return New CPoint(Me.m_X - p.m_X, Me.m_Y - p.m_Y)
    End Function


    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is CPoint) Then Return False
        Dim b As CPoint = obj
        Return (Me.m_X = b.m_X) AndAlso (Me.m_Y = b.m_Y)
    End Function

    Public Shared Operator =(ByVal a As CPoint, ByVal b As CPoint) As Boolean
        Return a.Equals(b)
    End Operator

    Public Shared Operator <>(ByVal a As CPoint, ByVal b As CPoint) As Boolean
        Return Not a.Equals(b)
    End Operator

    Public Shared Operator IsTrue(ByVal a As CPoint) As Boolean
        Return a.Equals(empty)
    End Operator

    Public Shared Operator IsFalse(ByVal a As CPoint) As Boolean
        Return Not a.Equals(empty)
    End Operator

    Public Shared Operator +(ByVal a As CPoint, ByVal b As CPoint) As CPoint
        Return a.Add(b)
    End Operator

    Public Shared Operator -(ByVal a As CPoint, ByVal b As CPoint) As CPoint
        Return a.Subtract(b)
    End Operator

    Public Shared Operator -(ByVal a As CPoint) As CPoint
        Return New CPoint(-a.m_X, -a.m_Y)
    End Operator

    Public Shared Operator +(ByVal a As CPoint) As CPoint
        Return New CPoint(a)
    End Operator

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "X" : Me.m_X = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            Case "Y" : Me.m_Y = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("X", Me.m_X)
        writer.WriteAttribute("Y", Me.m_Y)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class