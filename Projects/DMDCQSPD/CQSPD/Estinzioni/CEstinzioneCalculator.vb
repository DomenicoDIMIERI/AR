Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    Public Class CEstinzioneCalculator
        Private m_Rata As Nullable(Of Decimal) '[Double]   
        Private m_Durata As Integer '[INT]      Durata in numero di rate
        Private m_TAN As Double '[Double]    
        Private m_NumeroRateInsolute As Integer '[int] Numero di rate insolute
        Private m_Penale As Double '[Double] Percentuale da corrispondere come penale per estinzione anticipata
        Private m_NumeroRatePagate As Integer
        Private m_Calculated As Boolean
        Private m_AbbuonoInteressi As Decimal
        Private m_PenaleEstinzione As Double
        Private m_SpeseAccessorie As Decimal


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Rata = 0
            Me.m_Durata = 0
            Me.m_TAN = 0
            Me.m_NumeroRatePagate = 0
            Me.m_Calculated = False
            Me.m_AbbuonoInteressi = 0
            Me.m_PenaleEstinzione = 1
            Me.m_SpeseAccessorie = 0
        End Sub

        Protected Overridable Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)
            ' Me.Invalidate()
        End Sub

        Public Sub Validate()
            If Me.m_Calculated = False Then Me.Calculate()
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la percentuale sul capitale da restituire da pagare come penale per l'anticipata estinzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PenaleEstinzione As Double
            Get
                Return Me.m_PenaleEstinzione
            End Get
            Set(value As Double)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo")
                Dim oldValue As Double = Me.m_PenaleEstinzione
                If oldValue = value Then Exit Property
                Me.m_PenaleEstinzione = value
                Me.Invalidate()
                Me.DoChanged("PenaleEstinzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore delle spese accessorie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpeseAccessorie As Decimal
            Get
                Return Me.m_SpeseAccessorie
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo")
                Dim oldValue As Decimal = Me.m_SpeseAccessorie
                If oldValue = value Then Exit Property
                Me.m_SpeseAccessorie = value
                Me.Invalidate()
                Me.DoChanged("SpeseAccessorie", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote scadute
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteScadute As Integer
            Get
                Return Me.m_NumeroRatePagate
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate scadute non può essere negativo")
                Dim oldValue As Integer = Me.m_NumeroRatePagate
                If oldValue = value Then Exit Property
                Me.m_NumeroRatePagate = value
                Me.Invalidate()
                Me.DoChanged("NumeroRatePagate", value, oldValue)
            End Set
        End Property

        Public Property NumeroRateResidue As Integer
            Get
                Return Me.Durata - Me.NumeroQuoteScadute
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo")
                Dim oldValue As Integer = Me.NumeroRateResidue
                If oldValue = value Then Exit Property
                Me.m_NumeroRatePagate = Me.Durata - value
                Me.Invalidate()
                Me.DoChanged("NumeroRateResidue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroRateInsolute As Integer
            Get
                Return Me.m_NumeroRateInsolute
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo")
                Dim oldValue As Integer = Me.m_NumeroRateInsolute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRateInsolute = value
                Me.DoChanged("NumeroRateInsolute", value, oldValue)
            End Set
        End Property



        Public Property Rata As Nullable(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Rata
                If oldValue = value Then Exit Property
                Me.m_Rata = value
                Me.Invalidate()
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Durata
                If oldValue = value Then Exit Property
                Me.m_Durata = value
                Me.Invalidate()
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAN
                If oldValue = value Then Exit Property
                Me.m_TAN = value
                Me.Invalidate()
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property


        Public Property DebitoIniziale As Nullable(Of Decimal)
            Get
                If Me.m_Rata.HasValue Then Return Me.m_Rata.Value * Me.m_Durata
                Return Nothing
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.DebitoIniziale
                If (oldValue = value) Then Exit Property
                If Me.m_Durata <= 0 Then Me.m_Durata = 1
                Me.m_Rata = value / Me.m_Durata
                Me.DoChanged("DebitoIniziale", value, oldValue)
            End Set
        End Property

        Public Property DebitoResiduo As Nullable(Of Decimal)
            Get
                If (Me.DebitoIniziale.HasValue AndAlso Me.m_Rata.HasValue) Then
                    Return Me.DebitoIniziale.Value - Me.m_Rata.Value * Me.m_NumeroRatePagate
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.DebitoResiduo
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
                Me.DoChanged("DebitoResiduo", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property AbbuonoInteressi As Decimal
            Get
                Me.Validate()
                Return Me.m_AbbuonoInteressi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il capitale da rimborsare 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CapitaleDaRimborsare As Decimal
            Get
                If Me.DebitoResiduo.HasValue Then
                    Return Me.DebitoResiduo.Value - Me.AbbuonoInteressi
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il valore della penale di estinzione calcolata sul capitale da rimborsare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValorePenale As Decimal
            Get
                Return Me.CapitaleDaRimborsare * Me.m_PenaleEstinzione / 100
            End Get
        End Property

        Public ReadOnly Property TotaleDaRimborsare As Decimal
            Get
                Return Me.CapitaleDaRimborsare + Me.ValorePenale + Me.SpeseAccessorie + Me.ValoreQuoteInsolute
            End Get
        End Property

        Public ReadOnly Property ValoreQuoteInsolute As Decimal
            Get
                Me.Validate()
                If (Me.m_Rata.HasValue) Then
                    Return Me.m_NumeroRateInsolute * Me.m_Rata.Value
                Else
                    Return 0
                End If
            End Get
        End Property


        Public Function Calculate() As Boolean
            'Dim Dk As Decimal   'Debito residuo per k=1..n-1
            Dim Ik As Decimal 'Interesse in ciascun periodo k=1...n
            Dim k As Integer
            Dim i As Double

            'Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
            'Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

            i = (Me.m_TAN / 100) / 12

            Me.m_AbbuonoInteressi = 0
            For k = Me.m_NumeroRatePagate + 1 To Me.m_Durata
                Ik = Me.m_Rata.Value * (1 - 1 / ((1 + i) ^ (Me.m_Durata - k + 1)))
                Me.m_AbbuonoInteressi = Me.m_AbbuonoInteressi + Ik
            Next

            Me.m_Calculated = True

            Return True
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class





End Class