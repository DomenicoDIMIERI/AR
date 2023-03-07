Imports DMD.Sistema

Partial Public Class Anagrafica

    ''' <summary>
    ''' Classe che consente di calcolare il codice fiscale di una persona fisica
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CFCalculator
        Private Const m_consonanti As String = "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ"
        Private Const Mesi As String = "ABCDEHLMPRST"
        Private Const arrPari As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Private Const arrDispariCh As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Private Const arrDispariVal As String = "1|0|5|7|9|13|15|17|19|21|1|0|5|7|9|13|15|17|19|21|2|4|18|20|11|3|6|8|12|14|16|10|22|25|24|23"
        Private m_Nome As String
        Private m_Cognome As String
        Private m_NatoAComune As String
        Private m_NatoAProvincia As String
        Private m_CodiceCatasto As String
        Private m_NatoIl As Date
        Private m_Sesso As String
        Private m_ErrorCode As Integer
        Private m_ErrorDescription As String
        Private m_CodiceFiscale As String
        Private m_Calculated As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public ReadOnly Property ErrorCode As Integer
            Get
                Return Me.m_ErrorCode
            End Get
        End Property

        Public ReadOnly Property ErrorDescription As String
            Get
                Return Me.m_ErrorDescription
            End Get
        End Property

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Me.m_Nome = Trim(value)
                Me.Invalidate()
            End Set
        End Property

        Public Property Cognome As String
            Get
                Return Me.m_Cognome
            End Get
            Set(value As String)
                Me.m_Cognome = Trim(value)
                Me.Invalidate()
            End Set
        End Property

        Public Property NatoAComune As String
            Get
                Return Me.m_NatoAComune
            End Get
            Set(value As String)
                Me.m_NatoAComune = Trim(value)
                Me.Invalidate()
            End Set
        End Property

        Public Property NatoAProvincia As String
            Get
                Return Me.m_NatoAProvincia
            End Get
            Set(value As String)
                Me.m_NatoAProvincia = Trim(value)
                Me.Invalidate()
            End Set
        End Property

        Public Property CodiceCatasto As String
            Get
                Return Me.m_CodiceCatasto
            End Get
            Set(value As String)
                Me.m_CodiceCatasto = Trim(value)
                Me.Invalidate()
            End Set
        End Property

        Public Property NatoIl As Date
            Get
                Return Me.m_NatoIl
            End Get
            Set(value As Date)
                Me.m_NatoIl = value
                Me.Invalidate()
            End Set
        End Property

        Public Property Sesso As String
            Get
                Return Me.m_Sesso
            End Get
            Set(value As String)
                Me.m_Sesso = UCase(Left(Trim(value), 1))
                Me.Invalidate()
            End Set
        End Property

        Public Property CodiceFiscale As String
            Get
                Me.Validate()
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                Me.m_CodiceFiscale = value
            End Set
        End Property

        Public Sub Validate()
            If Me.m_Calculated = False Then Me.Calcola()
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        ''' <summary>
        ''' Se è stato inserito il codice fiscale inverte
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Inverti()
            Const mesi As String = "ABCDEHLMPRST"
            Dim cf As String = Formats.ParseCodiceFiscale(Me.m_CodiceFiscale)
            If (Len(cf) <> 16) Then Exit Sub
            Me.m_CodiceCatasto = Strings.Mid(cf, 12, 4)
            Dim luogo As Luogo = Luoghi.GetItemByCodiceCatastale(Me.m_CodiceCatasto)
            If (TypeOf (luogo) Is CComune) Then
                With DirectCast(luogo, CComune)
                    Me.m_NatoAComune = .Nome
                    Me.m_NatoAProvincia = .Provincia
                End With
            ElseIf (TypeOf (luogo) Is CNazione) Then
                With DirectCast(luogo, CNazione)
                    Me.m_NatoAComune = .Nome
                End With
            End If
            Dim gg As Integer = Formats.ToInteger(Strings.Mid(cf, 10, 2))
            Dim mm As Integer = 1 + mesi.IndexOf(Strings.Mid(cf, 9, 1))
            Dim aa As Integer = 1900 + Formats.ToInteger(Strings.Mid(cf, 7, 2))
            If (gg > 40) Then
                Me.m_Sesso = "F"
                gg -= 40
            Else
                Me.m_Sesso = "M"
            End If
            Me.m_NatoIl = DateUtils.MakeDate(aa, mm, gg)
        End Sub

        ''' <summary>
        ''' Calcola il codice fiscale
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Calcola() As String
            Dim CodiceCatastale As String 'Codice Catastale del comune
            Dim CodiceCognome As String
            Dim CodiceNome As String
            Dim CodiceDataNascita As String
            Dim ret As New System.Text.StringBuilder

            Me.m_Calculated = True
            Me.m_ErrorCode = 255
            Me.m_ErrorDescription = "Errore generico"

            If Me.Nome = "" Or Me.Cognome = "" Then
                Me.m_ErrorCode = 1
                Me.m_ErrorDescription = "Nome o Cognome non valido"
                Return vbNullString
            End If

            If Me.NatoIl = Nothing Then
                Me.m_ErrorCode = 2
                Me.m_ErrorDescription = "Data di nascita non valida"
                Return vbNullString
            End If

            If Me.m_CodiceCatasto <> "" Then
                CodiceCatastale = Me.m_CodiceCatasto
            Else
                CodiceCatastale = Luoghi.GetCodiceCatastale(Me.NatoAComune, Me.NatoAProvincia)
            End If
            If CodiceCatastale = "" Then
                Me.m_ErrorCode = 2
                Me.m_ErrorDescription = Strings.JoinW("Luogo di nascita non valido: ", NatoAComune, " (", NatoAProvincia, ")")
                Return vbNullString
            End If

            CodiceCognome = Me.CalcolaCognome(Strings.OnlyChars(Me.Cognome))
            CodiceNome = Me.CalcolaNome(Strings.OnlyChars(Me.Nome))
            CodiceDataNascita = Me.CalcolaNascita(Day(Me.NatoIl), Month(Me.NatoIl), Year(Me.NatoIl), Me.Sesso)

            ret.Append(CodiceCognome)
            ret.Append(" ")
            ret.Append(CodiceNome)
            ret.Append(" ")
            ret.Append(CodiceDataNascita)
            ret.Append(" ")
            ret.Append(CodiceCatastale)
            ret.Append(" ")

            ret.Append(Me.CalcolaK(CodiceCognome & CodiceNome & CodiceDataNascita & CodiceCatastale))

            Me.m_ErrorCode = 0
            Me.m_ErrorDescription = ""

            Me.m_CodiceFiscale = ret.ToString

            Return Me.m_CodiceFiscale
        End Function

        Private Function CalcolaCognome(ByVal Cognome As String) As String
            Dim code As String = Me.GetConsonanti(Cognome)
            If Len(code) >= 3 Then
                code = Left(code, 3)
            Else
                code = code & Left(Me.GetVocali(Cognome), 3 - Len(code))
                While (Len(code) < 3)
                    code = Strings.JoinW(code, "X")
                End While
            End If
            Return code
        End Function

        Private Function CalcolaNome(ByVal Nome As String) As String
            Dim code As String
            Dim cons As String
            cons = Me.GetConsonanti(Nome)
            If Len(cons) > 3 Then
                'code = Mid(cons.substring(0, 1) + cons.substring(2, 3) + cons.substring(3, 4);
                code = Mid(cons, 1, 1) & Mid(cons, 3, 1) & Mid(cons, 4, 1)
            ElseIf Len(cons) = 3 Then
                code = cons
            Else
                code = cons & Left(Me.GetVocali(Nome), 3 - Len(cons))
                While (Len(code) < 3)
                    code = Strings.JoinW(code, "X")
                End While
            End If
            Return code
        End Function

        Private Function GetConsonanti(ByVal Stringa As String) As String
            Dim cons As String
            Dim i As Integer
            Dim ch As String

            cons = ""
            For i = 1 To Len(Stringa)
                ch = Mid(Stringa, i, 1)
                If InStr(m_consonanti, ch) > 0 Then
                    cons = cons & ch
                End If
            Next

            Return UCase(cons)
        End Function

        Private Function GetVocali(ByVal Stringa As String) As String
            Dim voc As String
            Dim i As Integer
            Dim ch As String

            voc = ""
            For i = 1 To Len(Stringa)
                ch = Mid(Stringa, i, 1)
                If (InStr(m_consonanti, ch) < 1) And (ch <> " ") Then
                    voc = voc & ch
                End If
            Next

            Return UCase(voc)
        End Function

        Private Function CalcolaMese(ByVal mese As Integer) As String
            Return Mid(Mesi, mese, 1)
        End Function

        Private Function CalcolaNascita(ByVal Giorno As Integer, ByVal Mese As Integer, ByVal Anno As Integer, ByVal Sesso As String) As String
            Dim code As New System.Text.StringBuilder
            Sesso = UCase(Trim(Sesso))
            If Left(Sesso, 1) = "F" Then
                Giorno = Giorno + 40
            End If
            code.Append(Right("0000" & Anno, 2))
            code.Append(Me.CalcolaMese(Mese))
            code.Append(Right("00" & Giorno, 2))
            Return code.ToString
        End Function

        Private Function CalcolaK(ByVal Stringa As String) As String
            Dim somma As Integer = 0
            Dim k As Integer
            Dim arrDispari() As String
            Dim i As Integer
            Dim j As Integer
            Dim ch As String

            Stringa = UCase(Stringa)
            arrDispari = Split(arrDispariVal, "|")
            somma = 0

            For i = 1 To Len(Stringa) Step 2
                ch = Mid(Stringa, i, 1)
                j = InStr(arrDispariCh, ch)
                somma = somma + CInt(arrDispari(j - 1))
            Next

            For i = 2 To Len(Stringa) Step 2
                ch = Mid(Stringa, i, 1)
                If Not IsNumeric(ch) Then
                    somma = somma + InStr(arrPari, ch) - 1
                Else
                    somma = somma + CInt(ch)
                End If
            Next
            k = somma Mod 26

            Return Mid(arrPari, k + 1, 1)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class
