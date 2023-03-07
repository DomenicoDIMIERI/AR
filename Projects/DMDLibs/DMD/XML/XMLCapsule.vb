Imports DMD.Sistema
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.XML.Utils


Namespace XML

    <Serializable> _
    Public Class XMLCapsule
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Value As Object
        Private m_ValueType As String
        Private m_ValueXML As String
        Private m_buildVal As Boolean
        Private m_buoldXml As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Property Value As Object
            Get
                Return Me.m_Value
            End Get
            Set(value As Object)
                Me.m_Value = value
                Me.m_ValueType = VbTypeName(value)
            End Set
        End Property



        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Value"
                    Select Case Me.m_ValueType

                    End Select
                Case "ValueType" : Me.m_ValueType = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            Select Case Me.m_ValueType
                Case "Byte", "SByte"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Byte)(Me.m_Value))
                Case "Short", "UShort", "Int16", "UInt16"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Short)(Me.m_Value))
                Case "Integer", "UInteger", "Int32", "UInt32"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Short)(Me.m_Value))
                Case "Long", "ULong", "Int16", "UInt64"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Short)(Me.m_Value))
                Case "Single"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Single)(Me.m_Value))
                Case "Double"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Double)(Me.m_Value))
                Case "Decimal"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Decimal)(Me.m_Value))
                Case "Date", "DateTime"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Date)(Me.m_Value))
                Case "Boolean"
                    writer.WriteAttribute("Value", Me.MakeValue(Of Boolean)(Me.m_Value))

            End Select
        End Sub

        Private Function MakeValue(Of T As Structure)(ByVal v As Object) As Nullable(Of T)
            Return v
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace