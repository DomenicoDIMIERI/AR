Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CCosto
        Inherits DBObjectBase

        Private m_FinoA As Double
        Private m_Costo As Decimal

        Public Sub New()
            Me.m_FinoA = 0
            Me.m_Costo = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property FinoA As Double
            Get
                Return Me.m_FinoA
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_FinoA
                If (oldValue = value) Then Exit Property
                Me.m_FinoA = value
                Me.DoChanged("FinoA", value, oldValue)
            End Set
        End Property

        Public Property Costo As Decimal
            Get
                Return Me.m_Costo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Costo
                If (oldValue = value) Then Exit Property
                Me.m_Costo = value
                Me.DoChanged("Costo", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "Costi"
        End Function

        Protected Overrides Function GetIDFieldName() As String
            Return "idcosto"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("FinoA", Me.m_FinoA)
            reader.Read("Costo", Me.m_Costo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("FinoA", Me.m_FinoA)
            writer.Write("Costo", Me.m_Costo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return "FinoA: " & Me.m_FinoA & " -> " & Formats.FormatValuta(Me.m_Costo)
        End Function

    End Class


End Class
