Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A name record in the name table.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class NameRecord
        ''' <summary>
        ''' A constant for the platform.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLATFORM_APPLE_UNICODE = 0
        Public Const PLATFORM_MACINTOSH = 1
        Public Const PLATFORM_ISO = 2
        Public Const PLATFORM_WINDOWS = 3

        ''' <summary>
        ''' Platform specific encoding.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLATFORM_ENCODING_WINDOWS_UNDEFINED = 0
        Public Const PLATFORM_ENCODING_WINDOWS_UNICODE = 1

        ''' <summary>
        ''' A name id.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_COPYRIGHT = 0
        Public Const NAME_FONT_FAMILY_NAME = 1
        Public Const NAME_FONT_SUB_FAMILY_NAME = 2
        Public Const NAME_UNIQUE_FONT_ID = 3
        Public Const NAME_FULL_FONT_NAME = 4
        Public Const NAME_VERSION = 5
        Public Const NAME_POSTSCRIPT_NAME = 6
        Public Const NAME_TRADEMARK = 7


        Private platformId As Integer
        Private platformEncodingId As Integer
        Private languageId As Integer
        Private nameId As Integer
        Private stringLength As Integer
        Private stringOffset As Integer
        Private [string] As String

        '/**
        ' * @return Returns the stringLength.
        ' */
        Public Function getStringLength() As Integer
            Return stringLength
        End Function

        '/**
        ' * @param stringLengthValue The stringLength to set.
        ' */
        Public Sub setStringLength(ByVal stringLengthValue As Integer)
            Me.stringLength = stringLengthValue
        End Sub

        '/**
        ' * @return Returns the stringOffset.
        ' */
        Public Function getStringOffset() As Integer
            Return stringOffset
        End Function

        '/**
        ' * @param stringOffsetValue The stringOffset to set.
        ' */
        Public Sub setStringOffset(ByVal stringOffsetValue As Integer)
            Me.stringOffset = stringOffsetValue
        End Sub

        '/**
        ' * @return Returns the languageId.
        ' */
        Public Function getLanguageId() As Integer
            Return languageId
        End Function

        '/**
        ' * @param languageIdValue The languageId to set.
        ' */
        Public Sub setLanguageId(ByVal languageIdValue As Integer)
            Me.languageId = languageIdValue
        End Sub

        '/**
        ' * @return Returns the nameId.
        ' */
        Public Function getNameId() As Integer
            Return nameId
        End Function

        '/**
        ' * @param nameIdValue The nameId to set.
        ' */
        Public Sub setNameId(ByVal nameIdValue As Integer)
            Me.nameId = nameIdValue
        End Sub

        '/**
        ' * @return Returns the platformEncodingId.
        ' */
        Public Function getPlatformEncodingId() As Integer
            Return platformEncodingId
        End Function

        '/**
        ' * @param platformEncodingIdValue The platformEncodingId to set.
        ' */
        Public Sub setPlatformEncodingId(ByVal platformEncodingIdValue As Integer)
            Me.platformEncodingId = platformEncodingIdValue
        End Sub

        '/**
        ' * @return Returns the platformId.
        ' */
        Public Function getPlatformId() As Integer
            Return platformId
        End Function

        '/**
        ' * @param platformIdValue The platformId to set.
        ' */
        Public Sub setPlatformId(ByVal platformIdValue As Integer)
            Me.platformId = platformIdValue
        End Sub

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            platformId = data.readUnsignedShort()
            platformEncodingId = data.readUnsignedShort()
            languageId = data.readUnsignedShort()
            nameId = data.readUnsignedShort()
            stringLength = data.readUnsignedShort()
            stringOffset = data.readUnsignedShort()
        End Sub

        '/**
        ' * Return a string representation of Me class.
        ' * 
        ' * @return A string for Me class.
        ' */
        Public Overrides Function toString() As String
            Return "platform=" & platformId & " pEncoding=" & platformEncodingId & " language=" & languageId & " name=" & nameId
        End Function

        '/**
        ' * @return Returns the string.
        ' */
        Public Function getString() As String
            Return [string]
        End Function

        '/**
        ' * @param stringValue The string to set.
        ' */
        Public Sub setString(ByVal stringValue As String)
            Me.string = stringValue
        End Sub

    End Class


End Namespace
