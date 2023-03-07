Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Class CFonteExternalStats
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public Fonte As CFonte               'Fonte
        Public IDAnnuncio As String          'Stringa che identifica la fonte sui siti esterni
        Public Conteggio As Integer          'Numero di visualizzazioni totali

        Public DI As Nullable(Of Date)       'Data della prima visualizzazione
        Public DF As Nullable(Of Date)       'Data dell'ultima visualizzazione
        Public DV_AVE As Integer             'Visualizzazioni per giorno (medie)
        Public DV_MIN As Integer             'Visualizzazioni per giorno (minimo)
        Public DV_MAX As Integer             'Visualizzazioni per giorno (massimo)
        Public DV_STD As Integer             'Visualizzazioni per giorno (deviazione standard)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Fonte = Nothing
            Me.Conteggio = 0
            Me.DI = Nothing
            Me.DF = Nothing
        End Sub

        Public Sub New(ByVal fonte As CFonte)
            Me.New()
            If (fonte Is Nothing) Then Throw New ArgumentNullException("fonte")
            Me.Fonte = fonte
        End Sub

        Public Sub New(ByVal idAnnuncio As String)
            Me.New()
            idAnnuncio = Trim(idAnnuncio)
            If (idAnnuncio = "") Then Throw New ArgumentNullException("idAnnuncio")
            Me.IDAnnuncio = idAnnuncio
        End Sub

        Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmp As CFonteExternalStats = obj
            Dim ret As Integer = tmp.Conteggio - Me.Conteggio
            If (ret = 0) Then ret = Sistema.Strings.Compare(Me.IDAnnuncio, tmp.IDAnnuncio)
            Return ret
        End Function

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Fonte"
                Case "IDAnnuncio" : Me.IDAnnuncio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DI" : Me.DI = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DF" : Me.DF = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DV_AVE" : Me.DV_AVE = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DV_MIN" : Me.DV_MIN = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DV_MAX" : Me.DV_MAX = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DV_STD" : Me.DV_STD = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Fonte", GetID(Me.Fonte))
            writer.WriteAttribute("IDAnnuncio", Me.IDAnnuncio)
            writer.WriteAttribute("Conteggio", Me.Conteggio)
            writer.WriteAttribute("DI", Me.DI)
            writer.WriteAttribute("DF", Me.DF)
            writer.WriteAttribute("DV_AVE", Me.DV_AVE)
            writer.WriteAttribute("DV_MIN", Me.DV_MIN)
            writer.WriteAttribute("DV_MAX", Me.DV_MAX)
            writer.WriteAttribute("DV_STD", Me.DV_STD)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class