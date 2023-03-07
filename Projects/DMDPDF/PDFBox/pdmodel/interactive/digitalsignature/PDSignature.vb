Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * This represents a digital signature that can be attached to a document.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDSignature
        Implements COSObjectable

        Private dictionary As COSDictionary

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly FILTER_ADOBE_PPKLITE As COSName = COSName.ADOBE_PPKLITE

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly FILTER_ENTRUST_PPKEF As COSName = COSName.ENTRUST_PPKEF

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly FILTER_CICI_SIGNIT As COSName = COSName.CICI_SIGNIT

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly FILTER_VERISIGN_PPKVS As COSName = COSName.VERISIGN_PPKVS

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SUBFILTER_ADBE_X509_RSA_SHA1 As COSName = COSName.ADBE_X509_RSA_SHA1

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SUBFILTER_ADBE_PKCS7_DETACHED As COSName = COSName.ADBE_PKCS7_DETACHED

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SUBFILTER_ETSI_CADES_DETACHED As COSName = COSName.getPDFName("ETSI.CAdES.detached")

        ''' <summary>
        ''' A signature filter value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SUBFILTER_ADBE_PKCS7_SHA1 As COSName = COSName.ADBE_PKCS7_SHA1

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setItem(COSName.TYPE, COSName.SIG)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' * 
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return getDictionary()
        End Function

        '/**
        ' * Convert this standard java object to a COS dictionary.
        ' * 
        ' * @return The COS dictionary that matches this Java object.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Set the dictionary type.
        ' *
        ' * @param type is the dictionary type.
        ' */
        Public Sub setType(ByVal type As COSName)
            dictionary.setItem(COSName.TYPE, type)
        End Sub

        '/**
        ' * Set the filter.
        ' * 
        ' * @param filter the filter to be used
        ' */
        Public Sub setFilter(ByVal filter As COSName)
            dictionary.setItem(COSName.FILTER, filter)
        End Sub

        '/**
        ' * Set a subfilter that specify the signature that should be used. 
        ' * 
        ' * @param subfilter the subfilter that shall be used.
        ' */
        Public Sub setSubFilter(ByVal subfilter As COSName)
            dictionary.setItem(COSName.SUBFILTER, subfilter)
        End Sub

        '/**
        ' * Sets the name.
        ' * @param name the name to be used
        ' */
        Public Sub setName(ByVal name As String)
            dictionary.setString(COSName.NAME, name)
        End Sub

        '/**
        ' * Sets the location.
        ' * @param location the location to be used
        ' */
        Public Sub setLocation(ByVal location As String)
            dictionary.setString(COSName.LOCATION, location)
        End Sub

        '/**
        ' * Sets the reason.
        ' * 
        ' * @param reason the reason to be used
        ' */
        Public Sub setReason(ByVal reason As String)
            dictionary.setString(COSName.REASON, reason)
        End Sub

        '/**
        ' * Sets the contact info.
        ' * 
        ' * @param contactInfo the contact info to be used
        ' */
        Public Sub setContactInfo(ByVal contactInfo As String)
            dictionary.setString(COSName.CONTACT_INFO, contactInfo)
        End Sub

        '/**
        ' * Set the sign date.
        ' * 
        ' * @param cal the date to be used as sign date
        ' */
        Public Sub setSignDate(ByVal cal As NDate) 'Calendar 
            dictionary.setDate(COSName.M, cal)
        End Sub

        '/**
        ' * Returns the filter.
        ' * @return the filter
        ' */
        Public Function getFilter() As String
            Return dictionary.getNameAsString(COSName.FILTER)
        End Function

        '/**
        ' * Returns the subfilter.
        ' * 
        ' * @return the subfilter
        ' */
        Public Function getSubFilter() As String
            Return dictionary.getNameAsString(COSName.SUBFILTER)
        End Function

        '/**
        ' * Returns the name.
        ' * 
        ' * @return the name
        ' */
        Public Function getName() As String
            Return dictionary.getString(COSName.NAME)
        End Function

        '/**
        ' * Returns the location.
        ' * 
        ' * @return the location
        ' */
        Public Function getLocation() As String
            Return dictionary.getString(COSName.LOCATION)
        End Function

        '/**
        ' * Returns the reason.
        ' * 
        ' * @return the reason
        ' */
        Public Function getReason() As String
            Return dictionary.getString(COSName.REASON)
        End Function

        '/**
        ' * Returns the contact info.
        ' * 
        ' * @return teh contact info
        ' */
        Public Function getContactInfo() As String
            Return dictionary.getString(COSName.CONTACT_INFO)
        End Function

        '/**
        ' * Returns the sign date.
        ' * 
        ' * @return the sign date
        ' */
        Public Function getSignDate() As NDate 'Calendar 
            Try
                Return dictionary.getDate(COSName.M)
            Catch e As IOException
                Return Nothing
            End Try
        End Function

        '/**
        ' * Sets the byte range.
        ' * 
        ' * @param range the byte range to be used
        ' */
        Public Sub setByteRange(ByVal range() As Integer)
            If (range.Length <> 4) Then
                Return
            End If
            Dim ary As COSArray = New COSArray()
            For Each i As Integer In range
                ary.add(COSInteger.get(i))
            Next

            dictionary.setItem(COSName.BYTERANGE, ary)
        End Sub

        '/**
        ' * Read out the byterange from the file.
        ' * 
        ' * @return a integer array with the byterange
        ' */
        Public Function getByteRange() As Integer()
            Dim byteRange As COSArray = dictionary.getDictionaryObject(COSName.BYTERANGE)
            Dim ary() As Integer = Array.CreateInstance(GetType(Integer), byteRange.size())
            For i As Integer = 0 To ary.Length - 1
                ary(i) = byteRange.getInt(i)
            Next
            Return ary
        End Function

        '/**
        ' * Will return the embedded signature between the byterange gap.
        ' * 
        ' * @param pdfFile The signed pdf file as InputStream
        ' * @return a byte array containing the signature
        ' * @throws IOException if the pdfFile can't be read
        ' */
        Public Function getContents(ByVal pdfFile As InputStream) As Byte() ' throws IOException
            Dim byteRange() As Integer = getByteRange()
            Dim begin As Integer = byteRange(0) + byteRange(1) + 1
            Dim [end] As Integer = byteRange(2) - begin

            Return getContents(New COSFilterInputStream(pdfFile, New Integer() {begin, [end]}))
        End Function

        '/**
        ' * Will return the embedded signature between the byterange gap.
        ' * 
        ' * @param pdfFile The signed pdf file as byte array
        ' * @return a byte array containing the signature
        ' * @throws IOException if the pdfFile can't be read
        ' */
        Public Function getContents(ByVal pdfFile() As Byte) As Byte() 'throws IOException
            Dim byteRange() As Integer = getByteRange()
            Dim begin As Integer = byteRange(0) + byteRange(1) + 1
            Dim [end] As Integer = byteRange(2) - begin

            Return getContents(New COSFilterInputStream(pdfFile, New Integer() {begin, [end]}))
        End Function

        Private Function getContents(ByVal fis As COSFilterInputStream) As Byte() 'throws IOException 
            Dim byteOS As ByteArrayOutputStream = New ByteArrayOutputStream(1024)
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
            Dim c As Integer
            c = fis.read(buffer)
            While (c > 0)
                ' Filter < and (
                If (buffer(0) = &H3C OrElse buffer(0) = &H28) Then
                    byteOS.Write(buffer, 1, c)
                    'Filter > and )
                ElseIf (buffer(c - 1) = &H3E OrElse buffer(c - 1) = &H29) Then
                    byteOS.Write(buffer, 0, c - 1)
                Else
                    byteOS.Write(buffer, 0, c)
                End If
                c = fis.read(buffer)
            End While
            fis.Close()
            Return COSString.createFromHexString(byteOS.ToString()).getBytes()
        End Function

        '/**
        ' * Sets the contents.
        ' * 
        ' * @param bytes contents to be used
        ' */
        Public Sub setContents(ByVal bytes() As Byte)
            Dim [string] As COSString = New COSString(bytes)
            [string].setForceHexForm(True)
            dictionary.setItem(COSName.CONTENTS, [string])
        End Sub

        '/**
        ' * Will return the signed content of the document.
        ' * 
        ' * @param pdfFile The signed pdf file as InputStream
        ' * @return a byte array containing only the signed part of the content
        ' * @throws IOException if the pdfFile can't be read
        ' */
        Public Function getSignedContent(ByVal pdfFile As InputStream) As Byte() ' throws IOException
            Dim fis As COSFilterInputStream = Nothing
            Try
                fis = New COSFilterInputStream(pdfFile, getByteRange())
                Return fis.toByteArray()
            Finally
                If (fis IsNot Nothing) Then
                    fis.Close()
                End If
            End Try
        End Function

        '/**
        ' * Will return the signed content of the document.
        ' * 
        ' * @param pdfFile The signed pdf file as byte array
        ' * @return a byte array containing only the signed part of the content
        ' * @throws IOException if the pdfFile can't be read
        ' */
        Public Function getSignedContent(ByVal pdfFile As Byte()) As Byte() ' throws IOException
            Dim fis As COSFilterInputStream = Nothing
            Try
                fis = New COSFilterInputStream(pdfFile, getByteRange())
                Return fis.toByteArray()
            Finally
                If (fis IsNot Nothing) Then
                    fis.Close()
                End If
            End Try
        End Function

        '/**
        ' * PDF signature build dictionary. Provides informations about the signature handler.
        ' *
        ' * @return the pdf signature build dictionary.
        ' */
        Public Function getPropBuild() As PDPropBuild
            Dim propBuild As PDPropBuild = Nothing
            Dim propBuildDic As COSDictionary = dictionary.getDictionaryObject(COSName.PROP_BUILD)
            If (propBuildDic IsNot Nothing) Then
                propBuild = New PDPropBuild(propBuildDic)
            End If
            Return propBuild
        End Function

        '/**
        ' * PDF signature build dictionary. Provides informations about the signature handler.
        ' *
        ' * @param propBuild the prop build
        ' */
        Public Sub setPropBuild(ByVal propBuild As PDPropBuild)
            dictionary.setItem(COSName.PROP_BUILD, propBuild)
        End Sub


    End Class

End Namespace

