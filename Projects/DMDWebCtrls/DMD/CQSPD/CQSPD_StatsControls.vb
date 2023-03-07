Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms

#Region "Modulo Statistiche"

    Public Class CQSPDReportsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

    End Class


#End Region



#Region "Statistiche per prodotto"

    Public Enum InfoStatoEnum As Integer
        CONTATTO = 0
        LIQUIDATO = 1
        ANNULLATO = 2
        ALTRO = 3
    End Enum

    Public Class InfoStato
        Implements DMD.XML.IDMDXMLSerializable

        Public Descrizione As String
        Public stato As InfoStatoEnum
        Public Conteggio As Integer
        Public Montante As Decimal

        Public Sub New(ByVal descrizione As String, ByVal stato As InfoStatoEnum, ByVal conteggio As Integer, ByVal montante As Decimal)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Descrizione = descrizione
            Me.stato = stato
            Me.Conteggio = conteggio
            Me.Montante = montante
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select fieldName
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Stato" : Me.stato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Montante" : Me.Montante = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("Stato", Me.stato)
            writer.WriteAttribute("Conteggio", Me.Conteggio)
            writer.WriteAttribute("Montante", Me.Montante)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class CompareByAnnullato
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.ANNULLATO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.ANNULLATO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByContatto
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.CONTATTO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.CONTATTO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByLiquidato
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.LIQUIDATO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.LIQUIDATO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByAltriStati
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Dim v1 As Decimal = x.Item(InfoStatoEnum.ALTRO).Montante
            Dim v2 As Decimal = y.Item(InfoStatoEnum.ALTRO).Montante
            If (v1 < v2) Then Return 1
            If (v1 > v2) Then Return -1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    Public Class CompareByKey
        Implements IComparer, IComparer(Of CRigaStatisticaPerStato)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CRigaStatisticaPerStato, y As CRigaStatisticaPerStato) As Integer Implements IComparer(Of CRigaStatisticaPerStato).Compare
            Return Strings.Compare(x.Descrizione, y.Descrizione, CompareMethod.Text)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class

    <Serializable> _
    Public Class CRigaStatisticaPerStato
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public Descrizione As String = ""
        Public m_Items() As InfoStato
        Public Tag As String
        Public Tag1 As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            ReDim Me.m_Items(3)
            Me.m_Items(0) = New InfoStato("Contatto", InfoStatoEnum.CONTATTO, 0, 0)
            Me.m_Items(1) = New InfoStato("Liquidato", InfoStatoEnum.LIQUIDATO, 0, 0)
            Me.m_Items(2) = New InfoStato("Annullato", InfoStatoEnum.ANNULLATO, 0, 0)
            Me.m_Items(3) = New InfoStato("Altri Stati", InfoStatoEnum.ALTRO, 0, 0)
        End Sub

        Public ReadOnly Property Item(ByVal stato As InfoStatoEnum) As InfoStato
            Get
                Select Case stato
                    Case InfoStatoEnum.ALTRO : Return Me.m_Items(3)
                    Case InfoStatoEnum.ANNULLATO : Return Me.m_Items(2)
                    Case InfoStatoEnum.LIQUIDATO : Return Me.m_Items(1)
                    Case Else : Return Me.m_Items(0)
                End Select
            End Get
        End Property

        Public Function CompareTo(ByVal b As CRigaStatisticaPerStato) As Integer
            Return Strings.Compare(Me.Descrizione, b.Descrizione, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tag" : Me.Tag = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tag1" : Me.Tag1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Items" : Me.m_Items = XML.Utils.Serializer.ToArray(Of InfoStato)(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("Tag", Me.Tag)
            writer.WriteAttribute("Tag1", Me.Tag1)
            writer.WriteTag("Items", Me.m_Items)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    <Serializable> _
    Public Class CRigaStatisticaPerStatoCollection
        Inherits CKeyCollection(Of CRigaStatisticaPerStato)

        Public Sub New()
        End Sub

        Public Shadows Function Add(ByVal descrizione As String) As CRigaStatisticaPerStato
            Dim item As New CRigaStatisticaPerStato
            item.Descrizione = descrizione
            MyBase.Add("" & descrizione, item)
            'MyBase.Sort()
            Return item
        End Function

    End Class

#End Region

End Namespace