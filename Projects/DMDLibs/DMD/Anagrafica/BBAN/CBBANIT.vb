Partial Public Class Anagrafica

    ''' <summary>
    ''' Classe che rappresenta un codice BBAN intaliano
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBBAN_IT
        Inherits CBBAN

        Public Sub New()
        End Sub

        Public Sub New(ByVal codice As String)
            MyBase.New(codice)
        End Sub

        Public Property CIN As String
            Get
                Return Left(Me.Codice, 1)
            End Get
            Set(value As String)
                value = Left(Trim(value), 1)
                If (Me.Codice = "") Then Me.Codice = Sistema.Strings.NChars(27, "0")
                Me.Codice = value & Mid(Me.Codice, 2)
            End Set
        End Property

        Public Property ABI As String
            Get
                Return Mid(Me.Codice, 2, 5)
            End Get
            Set(value As String)
                value = Left(Trim(value), 5)
                If (Me.Codice = "") Then Me.Codice = Sistema.Strings.NChars(27, "0")
                Me.Codice = Left(Me.Codice, 1) & value & Mid(Me.Codice, 7)
            End Set
        End Property

        Public Property CAB As String
            Get
                Return Mid(Me.Codice, 7, 5)
            End Get
            Set(value As String)
                value = Left(Trim(value), 5)
                If (Me.Codice = "") Then Me.Codice = Sistema.Strings.NChars(27, "0")
                Me.Codice = Left(Me.Codice, 6) & value & Mid(Me.Codice, 12)
            End Set
        End Property

        Public Property NumeroDiContoCorrente As String
            Get
                Return Right(Me.Codice, 12)
            End Get
            Set(value As String)
                value = Left(Trim(value), 12)
                If (Me.Codice = "") Then Me.Codice = Sistema.Strings.NChars(27, "0")
                Me.Codice = Left(Me.Codice, 6) & value & Mid(Me.Codice, 12)
            End Set
        End Property

        Public Overrides Function IsValid() As Boolean
            If Len(Me.Codice) <> 27 Then Return False
            Return True
        End Function

    End Class

End Class