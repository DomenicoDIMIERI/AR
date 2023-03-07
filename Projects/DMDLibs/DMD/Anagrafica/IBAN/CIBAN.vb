Partial Public Class Anagrafica

    ''' <summary>
    ''' Classe che rappresenta un codice IBAN
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CIBAN
        Inherits DMD.Databases.DBObjectBase

        Public Const ValidDigits As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZZ"

        Private m_CodiceNazione As String
        Private m_CodiceDiControllo As String
        Private m_BBAN As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal codice As String)
            Me.New()
            Me.IBAN = codice
            Me.SetChanged(False)
        End Sub

        Public Property CodiceNazione As String
            Get
                Return Me.m_CodiceNazione
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 2))
                Dim oldValue As String = Me.m_CodiceNazione
                If (oldValue = value) Then Return
                Me.m_CodiceNazione = value
                Me.DoChanged("CodiceNazione", value, oldValue)
            End Set
        End Property

        Public Property CodiceDiControllo As String
            Get
                Return Me.m_CodiceDiControllo
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 2))
                Dim oldValue As String = Me.m_CodiceDiControllo
                If (oldValue = value) Then Return
                Me.m_CodiceDiControllo = value
                Me.DoChanged("CodiceDiControllo", value, oldValue)
            End Set
        End Property

        Public Property BBAN As String
            Get
                Return Me.m_BBAN
            End Get
            Set(value As String)
                value = UCase(Trim(value))
                Dim oldValue As String = Me.m_BBAN
                If (oldValue = value) Then Return
                Me.m_BBAN = value
                Me.DoChanged("BBAN", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice IBAN rappresentato dall'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IBAN As String
            Get
                Return Me.m_CodiceNazione & Me.m_CodiceDiControllo & Me.m_BBAN
            End Get
            Set(value As String)
                value = UCase(Replace(value, " ", ""))
                Dim oldValue As String = Me.IBAN
                If (oldValue = value) Then Exit Property
                Me.m_CodiceNazione = Left(value, 2)
                Me.m_CodiceDiControllo = Mid(value, 3, 2)
                Me.m_BBAN = Mid(value, 5)
                Me.DoChanged("IBAN", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.IBAN
        End Function

        ''' <summary>
        ''' Restituisce vero se il codice rappresentato è valido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid() As Boolean
            Dim text As String = Me.IBAN
            Dim i, j As Integer
            Dim ch As String
            Dim numStr As String
            Dim r As Integer

            If (Len(text) < 5) Then Return False

            text = Mid(text, 5) & Left(text, 4)
            numStr = ""
            For i = 1 To Len(text)
                ch = Mid(text, i, 1)
                j = InStr(ValidDigits, ch) - 1
                If (j < 0) Then Return False
                numStr &= CStr(j)
            Next

            Dim nibbles As New System.Collections.ArrayList
            i = Len(numStr) - 8
            While (numStr <> "")
                If (Len(numStr) > 8) Then
                    nibbles.Add(Mid(numStr, Len(numStr) - 7, 8))
                    numStr = Left(numStr, Len(numStr) - 8)
                Else
                    nibbles.Add(numStr)
                    numStr = ""
                End If
            End While

            r = 0
            For i = nibbles.Count - 1 To 0 Step -1
                r = CLng(CStr(r) & CStr(nibbles(i))) Mod 97
            Next

            Return (r Mod 97) = 1
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return DMD.Databases.APPConn
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_IBANCodes"
        End Function
    End Class

End Class