Imports System.IO

Namespace org.apache.fontbox.cff

    Public Class CFFFontROS
        Inherits CFFFont
        Private registry As String
        Private ordering As String
        Private supplement As Integer

        Private fontDictionaries As List(Of Map(Of String, Object)) = New LinkedList(Of Map(Of String, Object))()
        Private privateDictionaries As List(Of Map(Of String, Object)) = New LinkedList(Of Map(Of String, Object))()
        Private fdSelect As CIDKeyedFDSelect = Nothing

        '/**
        ' * Returns the registry value.
        ' * @return the registry
        ' */
        Public Function getRegistry() As String
            Return registry
        End Function

        '/**
        ' * Sets the registry value.
        ' * 
        ' * @param registry the registry to set
        ' */
        Public Sub setRegistry(ByVal registry As String)
            Me.registry = registry
        End Sub

        '/**
        ' * Returns the ordering value.
        ' * 
        ' * @return the ordering
        ' */
        Public Function getOrdering() As String
            Return ordering
        End Function

        '/**
        ' * Sets the ordering value.
        ' * 
        ' * @param ordering the ordering to set
        ' */
        Public Sub setOrdering(ByVal ordering As String)
            Me.ordering = ordering
        End Sub

        '/**
        ' * Returns the supplement value.
        ' * 
        ' * @return the supplement
        ' */
        Public Function getSupplement() As Integer
            Return supplement
        End Function

        '/**
        ' * Sets the supplement value.
        ' * 
        ' * @param supplement the supplement to set
        ' */
        Public Sub setSupplement(ByVal supplement As Integer)
            Me.supplement = supplement
        End Sub

        '/**
        ' * Returns the font dictionaries.
        ' * 
        ' * @return the fontDict
        ' */
        Public Function getFontDict() As List(Of Map(Of String, Object))
            Return fontDictionaries
        End Function

        '/**
        ' * Sets the font dictionaries.
        ' * 
        ' * @param fontDict the fontDict to set
        ' */
        Public Sub setFontDict(ByVal fontDict As List(Of Map(Of String, Object)))
            Me.fontDictionaries = fontDict
        End Sub

        '/**
        ' * Returns the private dictionary.
        ' * 
        ' * @return the privDict
        ' */
        Public Function getPrivDict() As List(Of Map(Of String, Object))
            Return privateDictionaries
        End Function

        '/**
        ' * Sets the private dictionary.
        ' * 
        ' * @param privDict the privDict to set
        ' */
        Public Sub setPrivDict(ByVal privDict As List(Of Map(Of String, Object)))
            Me.privateDictionaries = privDict
        End Sub

        '/**
        ' * Returns the fdSelect value.
        ' * 
        ' * @return the fdSelect
        ' */
        Public Function getFdSelect() As CIDKeyedFDSelect
            Return fdSelect
        End Function

        '/**
        ' * Sets the fdSelect value.
        ' * 
        ' * @param fdSelect the fdSelect to set
        ' */
        Public Sub setFdSelect(ByVal fdSelect As CIDKeyedFDSelect)
            Me.fdSelect = fdSelect
        End Sub

        '/**
        ' * Returns the Width value of the given Glyph identifier
        ' * 
        ' * @param CID
        ' * @return -1 if the SID is missing from the Font.
        ' * @throws IOException
        ' */
        Public Overrides Function getWidth(ByVal CID As Integer) As Integer
            '// ---- search the right FDArray index in the FDSelect according to the Character identifier
            '// 		Me index will be used to access the private dictionary which contains useful values 
            '//		to compute width.
            Dim fdArrayIndex As Integer = Me.fdSelect.getFd(CID)
            If (fdArrayIndex = -1 AndAlso CID = 0) Then ' --- notdef char
                Return MyBase.getWidth(CID)
            ElseIf (fdArrayIndex = -1) Then
                Return 1000
            End If

            Dim fontDict As Map(Of String, Object) = Me.fontDictionaries.get(fdArrayIndex)
            Dim privDict As Map(Of String, Object) = Me.privateDictionaries.get(fdArrayIndex)

            Dim nominalWidth As Integer = 0
            If (privDict.containsKey("nominalWidthX")) Then nominalWidth = DirectCast(privDict.get("nominalWidthX"), Number).intValue()
            Dim defaultWidth As Integer = 1000
            If (privDict.containsKey("defaultWidthX")) Then defaultWidth = DirectCast(privDict.get("defaultWidthX"), Number).intValue()

            For Each m As Mapping In getMappings()
			    If (m.getSID() = CID) Then
                    Dim csr As CharStringRenderer = Nothing
                    Dim charStringType As Number = getProperty("CharstringType")
                    If (charStringType.intValue() = 2) Then

                        Dim lSeq As List(Of Object) = m.toType2Sequence()
                        csr = New CharStringRenderer(False)
                        csr.render(lSeq)
                    Else
                        Dim lSeq As List(Of Object) = m.toType1Sequence()
                        csr = New CharStringRenderer()
                        csr.render(lSeq)
                    End If

                    '// ---- If the CharString has a Width nominalWidthX must be added, 
                    '/	    otherwise it is the default width.
                    Return IIf(csr.getWidth() <> 0, csr.getWidth() + nominalWidth, defaultWidth)
                End If
            Next

            ' ---- CID Width not found, return the notdef width
            Return getNotDefWidth(defaultWidth, nominalWidth)
        End Function


    End Class

End Namespace
