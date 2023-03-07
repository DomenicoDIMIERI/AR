Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Class Anagrafica

    Public NotInheritable Class CLuoghiClass
        Inherits CGeneralClass(Of Luogo)

        Friend Sub New()
            MyBase.New("Luoghi", GetType(LuogoCursor(Of Luogo)))
        End Sub

        Public Function FindLuoghi(ByVal value As String) As CCollection(Of Luogo)
            Dim col As CCollection(Of Luogo) = Luoghi.Comuni.Find(value)
           
            If (col.Count = 0) Then
                col = Luoghi.Nazioni.Find(value)
            End If
            col.Comparer = New NomeComuniSorter(value)
            col.Sort()

            Return col
        End Function

        Public Function FormatNome(ByVal nomeComune As String, ByVal nomeProvincia As String) As String
            Dim ret As String = Trim(nomeComune)
            nomeProvincia = Trim(nomeProvincia)
            If nomeProvincia <> vbNullString Then ret &= " (" & nomeProvincia & ")"
            Return ret
        End Function

        Public Function GetCodiceCatastale(ByVal comune As String, ByVal provincia As String) As String
            ' Dim ret As String = vbNullString
            comune = Trim(comune)
            provincia = Trim(provincia)
            If (comune = vbNullString) Then Throw New ArgumentNullException("comune")
            If (UCase(provincia) <> "EE") Then
                'ret = Formats.ToString(APPConn.ExecuteScalar("SELECT [Codice_Catasto] FROM [tbl_Luoghi_Comuni] WHERE [Nome]='" & comune & "' AND [Provincia]='" & provincia & "' AND [Stato]=" & ObjectStatus.OBJECT_VALID))
                For Each c As CComune In Luoghi.Comuni.LoadAll
                    If (Strings.Compare(c.Nome, comune) = 0 AndAlso (Strings.Compare(c.Provincia, provincia) = 0 OrElse Strings.Compare(c.Sigla, provincia) = 0)) Then Return c.CodiceCatasto
                Next
            End If
            'If (ret = "") Then
            'ret = Formats.ToString(APPConn.ExecuteScalar("SELECT [Codice_Catasto] FROM [tbl_Luoghi_Comuni] WHERE [Nome]='" & comune & "' AND [Sigla]='" & provincia & "' AND [Stato]=" & ObjectStatus.OBJECT_VALID))
            'End If
            'If (ret = "") Then
            'ret = Formats.ToString(APPConn.ExecuteScalar("SELECT [Codice_Catasto] FROM [tbl_Luoghi_Nazioni] WHERE [Nome]='" & comune & "' AND [Stato]=" & ObjectStatus.OBJECT_VALID))
            'End If
            For Each n As CNazione In Luoghi.Nazioni.LoadAll
                If (Strings.Compare(n.Nome, comune) = 0) Then Return n.CodiceCatasto
            Next

            Return ""
            'Return ret
        End Function

        ''' <summary>
        ''' Espande il nome della provincia a partire dalla sua sigla. Se il valore è già un nome viene restituito il valore stesso
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EspandiNomeProvincia(ByVal value As String) As String

            For Each p As CProvincia In Luoghi.Province.LoadAll
                If (Strings.Compare(p.Sigla, value) = 0) Then Return p.Nome
            Next
            Return ""
        End Function



        Public Function GetComune(ByVal text As String) As String
            Dim i, j As Integer
            i = InStr(text, "(")
            If (i > 0) Then j = InStr(i, text, ")")
            If (i > 0) And (j > i) Then
                Return Trim(Left(text, i - 1))
            Else
                Return Trim(text)
            End If
        End Function

        Public Function GetProvincia(ByVal text As String) As String
            Dim i, j As Integer
            i = InStr(text, "(")
            If (i > 0) Then j = InStr(i, text, ")")
            If (i > 0) And (j > i) Then
                Return Trim(Mid(text, i + 1, j - i - 1))
            Else
                Return vbNullString
            End If
        End Function

        Public Function GetItemByCodiceCatastale(ByVal code As String) As Luogo
            Dim item As Luogo = Comuni.GetItemByCodiceCatastale(code)
            If (item Is Nothing) Then item = Nazioni.GetItemByCodiceCatastale(code)
            Return item
        End Function

        Private _toponimiSupportati As String() = Nothing
        Public Function GetToponimiSupportati() As String()
            If (_toponimiSupportati Is Nothing) Then
                _toponimiSupportati = {"Via", "Zona Industriale", "Parco", "Rione", "Largo", "L.go", "Viale", "V.le", "Corso", "C.so", "Vicolo", "V.lo", "Strada provinciale", "SP", "Strada Statale", "SS", "Piazza", "P.zza", "C.da", "Cda", "Contrada", "Piazzale", "P.tta", "Piazzetta", "Str. vic.le", "Str. vicinale", "Loc.", "Località", "Vico", "Largo", "Vicolo", "Rampa", "Salita"}
                Arrays.Sort(_toponimiSupportati, 0, _toponimiSupportati.Length, New CFunctionComparer(AddressOf CompareToponimi))
            End If
            Return _toponimiSupportati
        End Function

        Private Function CompareToponimi(ByVal a As Object, ByVal b As Object) As Integer
            Dim al As Integer = Strings.Len(a)
            Dim bl As Integer = Strings.Len(b)
            Return bl - al
        End Function


        Public Function MergeComuneeProvincia(ByVal nomeCitta As String, ByVal siglaProvincia As String) As String
            Dim ret As String = Trim(nomeCitta)
            siglaProvincia = Trim(siglaProvincia)
            If (siglaProvincia <> "") Then ret = ret & " (" & siglaProvincia & ")"
            Return ret
        End Function

        Private m_Indirizzi As CIndirizziClass = Nothing
        Public ReadOnly Property Indirizzi As CIndirizziClass
            Get
                If (m_Indirizzi Is Nothing) Then m_Indirizzi = New CIndirizziClass
                Return m_Indirizzi
            End Get
        End Property

        Private m_Comuni As CComuniClass = Nothing
        Public ReadOnly Property Comuni As CComuniClass
            Get
                If (m_Comuni Is Nothing) Then m_Comuni = New CComuniClass
                Return m_Comuni
            End Get
        End Property

        Private m_Nazioni As CNazioniClass = Nothing
        Public ReadOnly Property Nazioni As CNazioniClass
            Get
                If m_Nazioni Is Nothing Then m_Nazioni = New CNazioniClass
                Return m_Nazioni
            End Get
        End Property

        Private m_Province As CProvinceClass = Nothing
        Public ReadOnly Property Province As CProvinceClass
            Get
                If m_Province Is Nothing Then m_Province = New CProvinceClass
                Return m_Province
            End Get
        End Property

        Private m_Regioni As CRegioniClass = Nothing
        Public ReadOnly Property Regioni As CRegioniClass
            Get
                If (m_Regioni Is Nothing) Then m_Regioni = New CRegioniClass
                Return m_Regioni
            End Get
        End Property

#Region "Comparer"

        Private Class NomeComuniSorter
            Implements IComparer

            Private m_Text As String

            Public Sub New(ByVal nome As String)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.m_Text = nome
            End Sub

            Private Function GetText(ByVal l As Luogo) As String
                If (TypeOf (l) Is CComune) Then
                    Return DirectCast(l, CComune).CittaEProvincia
                Else
                    Return DirectCast(l, CNazione).Nome
                End If
            End Function

            Private Function Compare(x As Luogo, y As Luogo) As Integer
                Dim str1 As String = GetText(x)
                Dim str2 As String = GetText(y)
                Dim i1 As Integer = InStr(str1, Me.m_Text, CompareMethod.Text)
                Dim i2 As Integer = InStr(str2, Me.m_Text, CompareMethod.Text)
                Dim ret As Integer = i1 - i2
                If (ret = 0) Then ret = Strings.Compare(str1, str2, CompareMethod.Text)
                Return ret
            End Function

            Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
                Return Me.Compare(x, y)
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

#End Region

    End Class

    Private Shared m_Luoghi As CLuoghiClass = Nothing

    Public Shared ReadOnly Property Luoghi As CLuoghiClass
        Get
            If (m_Luoghi Is Nothing) Then m_Luoghi = New CLuoghiClass
            Return m_Luoghi
        End Get
    End Property

End Class