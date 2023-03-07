Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class CQSPD

    
    Public Class CBeneficiarioRichiestaAssegni
        Inherits DBObjectBase

        Private m_RichiestaID As Integer
        Private m_Richiesta As CRichiestaAssegni
        Private m_Nome As String
        Private m_Field As String
        Private m_Importo As Decimal
        Private m_Position As Integer

        Public Sub New()
            m_Richiesta = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return RichiesteAssegni.Module
        End Function

        Protected Friend Sub SetRichiesta(ByVal value As CRichiestaAssegni)
            Me.m_Richiesta = value
            Me.m_RichiestaID = GetID(value)
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Field As String
            Get
                Return Me.m_Field
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Field
                If (oldValue = value) Then Exit Property
                Me.m_Field = value
                Me.DoChanged("Field", value, oldValue)
            End Set
        End Property

        Public Property Importo As Decimal
            Get
                Return Me.m_Importo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Importo
                If (oldValue = value) Then Exit Property
                Me.m_Importo = value
                Me.DoChanged("Importo", value, oldValue)
            End Set
        End Property

        Public Property Position As Integer
            Get
                Return Me.m_Position
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Position
                If (oldValue = value) Then Exit Property
                Me.m_Position = value
                Me.DoChanged("Position", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_RichiestaAssegniCircolariBeneficiari"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("NomeBeneficiario", Me.m_Nome)
            reader.Read("Campo1", Me.m_Field)
            reader.Read("Importo", Me.m_Importo)
            reader.Read("Posizione", Me.m_Position)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("NomeBeneficiario", Me.m_Nome)
            writer.Write("Campo1", Me.m_Field)
            writer.Write("Importo", Me.m_Importo)
            writer.Write("Posizione", Me.m_Position)
            writer.Write("Richiesta", GetID(Me.m_Richiesta, Me.m_RichiestaID))
            Return MyBase.SaveToRecordset(writer)
        End Function

    End Class

End Class