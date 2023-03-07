Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD


    Public Enum TipoCalcoloProvvigionale As Integer
        ''' <summary>
        ''' Il provvigionale è calcolato solo come un riconoscimento per la pratica
        ''' </summary>
        ''' <remarks></remarks>
        SOLOBASE = 0

        ''' <summary>
        ''' Il provvigionale è calcolato come base più una percentuale sul ML
        ''' </summary>
        ''' <remarks></remarks>
        BASEPIUPERCML = 1


    End Enum

    <Serializable> _
    Public Class CProvvigionale
        Inherits DBObjectBase

        Private m_Tipo As TipoCalcoloProvvigionale
        Private m_ValoreBase As Nullable(Of Decimal)
        Private m_ValorePercentuale As Nullable(Of Decimal)


        Public Sub New()
            Me.m_Tipo = TipoCalcoloProvvigionale.BASEPIUPERCML
            Me.m_ValoreBase = Nothing
            Me.m_ValorePercentuale = Nothing
        End Sub

        Public Property Tipo As TipoCalcoloProvvigionale
            Get
                Return Me.m_Tipo
            End Get
            Set(value As TipoCalcoloProvvigionale)
                Dim oldValue As TipoCalcoloProvvigionale = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property ValoreBase As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreBase
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ValoreBase
                If (oldValue = value) Then Exit Property
                Me.m_ValoreBase = value
                Me.DoChanged("ValoreBase", value, oldValue)
            End Set
        End Property

        Public Property ValorePercentuale As Nullable(Of Decimal)
            Get
                Return Me.m_ValorePercentuale
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ValorePercentuale
                If (oldValue = value) Then Exit Property
                Me.m_ValorePercentuale = value
                Me.DoChanged("ValorePercentuale", value, oldValue)
            End Set
        End Property

        Public Function ValoreTotale() As Nullable(Of Decimal)
            Select Case Me.Tipo
                Case TipoCalcoloProvvigionale.SOLOBASE : Return Me.m_ValoreBase
                Case Else ' TipoCalcoloProvvigionale.BASEPIUPERCML
                    If (Me.m_ValoreBase.HasValue AndAlso Me.m_ValorePercentuale.HasValue) Then
                        Return Me.m_ValoreBase.Value + Me.m_ValorePercentuale.Value
                    Else
                        Return Nothing
                    End If
            End Select
        End Function


        Public Sub CalcolaSu(ByVal pratica As CRapportino)
            Throw New NotImplementedException
        End Sub


        Protected Overrides Function GetConnection() As CDBConnection
            Return Nothing
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return ""
        End Function

        Public Property PercentualeSu(ByVal ml As Nullable(Of Decimal)) As Nullable(Of Single)
            Get
                Select Case Me.Tipo
                    Case TipoCalcoloProvvigionale.SOLOBASE
                        Return 0
                    Case Else
                        If (Me.ValoreTotale.HasValue = False) OrElse (ml.HasValue = False) Then Return Nothing
                        Return Me.ValoreTotale.Value * 100 / ml.Value
                End Select
            End Get
            Set(value As Nullable(Of Single))
                Select Case Me.Tipo
                    Case TipoCalcoloProvvigionale.SOLOBASE
                        Me.m_ValorePercentuale = Nothing
                    Case Else
                        If (value.HasValue) Then
                            If (ml.HasValue) Then
                                Me.m_ValorePercentuale = value.Value * ml.Value / 100
                            Else
                                Throw New ArgumentNullException("ml")
                            End If
                        Else
                            Me.m_ValorePercentuale = Nothing
                        End If
                End Select
            End Set
        End Property
     
        
    End Class



End Class