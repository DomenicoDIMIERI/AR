Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Flags> _
    Public Enum CRMFilterFlags As Integer
        NONE = 0
        'APPUNTAMENTI = 1
        'TELEFONATE = 2
        SOLOAZIENDE = 4
    End Enum

    Public Enum CRMFilterSortFlags As Integer
        MOSTIMPORTANT = 0
        MOSTRECENT = 1
        LEASTRECENT = 2
        NAME = 3
    End Enum

    Public Class CRMFilter
        Implements DMD.XML.IDMDXMLSerializable, ICloneable, IComparer '(Of CActivePerson)

        Public TipiAppuntamento As New CCollection(Of String)
        Public Categorie As New CCollection(Of String)
        Public TipiRapporto As New CCollection(Of String)
        Public Flags As CRMFilterFlags = CRMFilterFlags.NONE ' CRMFilterFlags.APPUNTAMENTI Or CRMFilterFlags.TELEFONATE
        Public Motivo As String = ""
        Public NomeLista As String = ""
        Public IDPuntoOperativo As Integer = 0
        Public Periodo As String = ""
        Public DataInizio As Nullable(Of Date) = Nothing
        Public DataFine As Nullable(Of Date) = Nothing
        Public IDOperatore As Integer = 0
        Public SortOrder As CRMFilterSortFlags = CRMFilterSortFlags.MOSTIMPORTANT
        Public nMax As Nullable(Of Integer) = Nothing
        Public fromDate As Nullable(Of Date) = Nothing
        Public ResidenteA As String = ""

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Property MostraSoloAziende As Boolean
            Get
                Return Sistema.TestFlag(Me.Flags, CRMFilterFlags.SOLOAZIENDE)
            End Get
            Set(value As Boolean)
                Me.Flags = Sistema.SetFlag(Me.Flags, CRMFilterFlags.SOLOAZIENDE, value)
            End Set
        End Property

        Public Property MostraTelefonate As Boolean
            Get
                Return (Me.TipiAppuntamento.Count() = 0) OrElse (Me.TipiAppuntamento.Contains("Telefonata"))
            End Get
            Set(value As Boolean)
                If (value) Then
                    If (Not Me.TipiAppuntamento.Contains("Telefonata")) Then Me.TipiAppuntamento.Add("Telefonata")
                Else
                    If (Me.TipiAppuntamento.Contains("Telefonata")) Then Me.TipiAppuntamento.Remove("Telefonata")
                End If
            End Set
        End Property

        Public Property MostraAppuntamenti As Boolean
            Get
                Return (Me.TipiAppuntamento.Count() = 0) OrElse (Me.TipiAppuntamento.Contains("Appuntamento"))
            End Get
            Set(value As Boolean)
                If (value) Then
                    If (Not Me.TipiAppuntamento.Contains("Appuntamento")) Then Me.TipiAppuntamento.Add("Appuntamento")
                Else
                    If (Me.TipiAppuntamento.Contains("Appuntamento")) Then Me.TipiAppuntamento.Remove("Appuntamento")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la ricerca deve restituire o meno i promemoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MostraPromemoria As Boolean
            Get
                Return (Me.TipiAppuntamento.Count() = 0) OrElse (Me.TipiAppuntamento.Contains("Promemoria"))
            End Get
            Set(value As Boolean)
                If (value) Then
                    If (Not Me.TipiAppuntamento.Contains("Promemoria")) Then Me.TipiAppuntamento.Add("Promemoria")
                Else
                    If (Me.TipiAppuntamento.Contains("Promemoria")) Then Me.TipiAppuntamento.Remove("Promemoria")
                End If
            End Set
        End Property

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "TipiAppuntamento" : Me.TipiAppuntamento.Clear() : Me.TipiAppuntamento.AddRange(fieldValue)
                Case "Categorie" : Me.Categorie.Clear() : Me.Categorie.AddRange(fieldValue)
                Case "TipiRapporto" : Me.TipiRapporto.Clear() : Me.TipiRapporto.AddRange(fieldValue)
                Case "Flags" : Me.Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Motivo" : Me.Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeLista" : Me.NomeLista = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Periodo" : Me.Periodo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SortOrder" : Me.SortOrder = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "nMax" : Me.nMax = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "fromDate" : Me.fromDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ResidenteA" : Me.ResidenteA = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Flags", Me.Flags)
            writer.WriteAttribute("Motivo", Me.Motivo)
            writer.WriteAttribute("NomeLista", Me.NomeLista)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("Periodo", Me.Periodo)
            writer.WriteAttribute("DataInizio", Me.DataInizio)
            writer.WriteAttribute("DataFine", Me.DataFine)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("SortOrder", Me.SortOrder)
            writer.WriteAttribute("nMax", Me.nMax)
            writer.WriteAttribute("fromDate", Me.fromDate)
            writer.WriteAttribute("ResidenteA", Me.ResidenteA)
            writer.WriteTag("TipiAppuntamento", Me.TipiAppuntamento)
            writer.WriteTag("Categorie", Me.Categorie)
            writer.WriteTag("TipiRapporto", Me.TipiRapporto)
        End Sub

        Public Function check(ByVal ric As CRicontatto) As Boolean
            Dim p As CPersona = ric.Persona()


            '(ric.NomeLista = Me.NomeLista) AndAlso
            Dim ret As Boolean =
                   (ric.IDPuntoOperativo = Me.IDPuntoOperativo OrElse ric.IDAssegnatoA = Me.IDOperatore) AndAlso
                   Calendar.CheckBetween(ric.DataPrevista, Me.DataInizio, Me.DataFine) AndAlso
                   (Me.Motivo = "" OrElse Strings.InStr(ric.Note, Me.Motivo) > 0)
            If (ret AndAlso Me.TipiAppuntamento.Count() > 0) Then
                ret = False
                For Each value As String In Me.TipiAppuntamento
                    If (ric.TipoAppuntamento = value) Then
                        ret = True
                        Exit For
                    End If
                Next
            End If
            If (ret AndAlso Me.TipiRapporto.Count() > 0) Then
                p = ric.Persona()
                Dim tr As String = ""
                If ((TypeOf (p) Is CPersonaFisica) AndAlso DirectCast(p, CPersonaFisica).ImpiegoPrincipale() IsNot Nothing) Then
                    tr = DirectCast(p, CPersonaFisica).ImpiegoPrincipale.TipoRapporto
                End If
                For Each value As String In Me.TipiRapporto
                    If (value = tr) Then
                        ret = True
                        Exit For
                    End If
                Next
            End If
            If (ret AndAlso Me.Categorie.Count() > 0) Then
                ret = False
                For Each value As String In Me.Categorie
                    If (ric.Categoria = value) Then
                        ret = True
                        Exit For
                    End If
                Next
            End If
            If (ret AndAlso Me.ResidenteA <> "") Then
                p = ric.Persona
                ret = ret AndAlso p.ResidenteA.NomeComune = Me.ResidenteA
            End If
            Return ret
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim ret As New CRMFilter()
            ret.TipiAppuntamento.AddRange(Me.TipiAppuntamento)
            ret.Categorie.AddRange(Me.Categorie)
            ret.TipiRapporto.AddRange(Me.TipiRapporto)
            ret.Flags = Me.Flags
            ret.Motivo = Me.Motivo
            ret.NomeLista = Me.NomeLista
            ret.IDPuntoOperativo = Me.IDPuntoOperativo
            ret.DataInizio = Me.DataInizio
            ret.DataFine = Me.DataFine
            ret.IDOperatore = Me.IDOperatore
            ret.Periodo = Me.Periodo
            ret.SortOrder = Me.SortOrder
            ret.nMax = Me.nMax
            ret.fromDate = Me.fromDate
            ret.ResidenteA = Me.ResidenteA
            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function

        Public Function Compare(ByVal a As CActivePerson, b As CActivePerson) As Integer
            Dim ret As Integer = 0
            Select Case Me.SortOrder
                Case CRMFilterSortFlags.LEASTRECENT
                    ret = Calendar.Compare(a.Data, b.Data)
                    If (ret = 0) Then ret = Strings.Compare(a.Nominativo, b.Nominativo, CompareMethod.Text)
                Case CRMFilterSortFlags.MOSTIMPORTANT
                    Dim p1 As Integer = 0
                    Dim p2 As Integer = 0
                    If (a.Ricontatto IsNot Nothing) Then p1 = a.Ricontatto.Priorita
                    If (b.Ricontatto IsNot Nothing) Then p2 = b.Ricontatto.Priorita
                    ret = p1.CompareTo(p2)
                Case CRMFilterSortFlags.MOSTRECENT
                    ret = -Calendar.Compare(a.Data, b.Data)
                    If (ret = 0) Then ret = Strings.Compare(a.Nominativo, b.Nominativo, CompareMethod.Text)
                Case CRMFilterSortFlags.NAME
                    ret = Strings.Compare(a.Nominativo, b.Nominativo, CompareMethod.Text)
                Case Else
                    ret = -Calendar.Compare(a.Data, b.Data)
                    If (ret = 0) Then ret = Strings.Compare(a.Nominativo, b.Nominativo, CompareMethod.Text)
            End Select
            Return ret
        End Function
    End Class


End Class