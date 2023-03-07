Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * <p>This represents the general property dictionaries from the build property dictionary.</p>
    ' *
    ' * @see PDPropBuild
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDPropBuildDataDict
        Implements COSObjectable

        Private dictionary As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setDirect(True) ' the specification claim to use direct objects
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
            dictionary.setDirect(True) ' the specification claim to use direct objects
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
        ' * The name of the software module that was used to create the signature.
        ' * @return the name of the software module
        ' */
        Public Function getName() As String
            Return dictionary.getString(COSName.NAME)
        End Function

        '/**
        ' * The name of the software module that was used to create the signature.
        ' *
        ' * @param name is the name of the software module
        ' */
        Public Sub setName(ByVal name As String)
            dictionary.setName(COSName.NAME, name)
        End Sub

        '/**
        ' * The build date of the software module.
        ' *
        ' * @return the build date of the software module
        ' */
        Public Function getDate() As String
            Return dictionary.getString(COSName.DATE)
        End Function

        '/**
        ' * The build date of the software module. This string is normally produced by the
        ' * compiler under C++.
        ' *
        ' * @param date is the build date of the software module
        ' */
        Public Sub setDate(ByVal [date] As String)
            dictionary.setString(COSName.DATE, [date])
        End Sub

        '/**
        ' * The software module revision number, corresponding to the Date attribute.
        ' *
        ' * @return the revision of the software module
        ' */
        Public Function getRevision() As Long
            Return dictionary.getLong(COSName.R)
        End Function

        '/**
        ' * The software module revision number, corresponding to the Date attribute.
        ' *
        ' * @param revision is the software module revision number
        ' */
        Public Sub setRevision(ByVal revision As Long)
            dictionary.setLong(COSName.R, revision)
        End Sub

        '/**
        ' * The software module revision number, used to determinate the minimum version
        ' * of software that is required in order to process this signature.
        ' *
        ' * @return the revision of the software module
        ' */
        Public Function getMinimumRevision() As Long
            Return dictionary.getLong(COSName.V)
        End Function

        '/**
        ' * The software module revision number, used to determinate the minimum version
        ' * of software that is required in order to process this signature.
        ' *
        ' * @param revision is the software module revision number
        ' */
        Public Sub setMinimumRevision(ByVal revision As Long)
            dictionary.setLong(COSName.V, revision)
        End Sub

        '/**
        ' * A flag that can be used by the signature handler or software module to
        ' * indicate that this signature was created with unrelease software.
        ' *
        ' * @return true if the software module or signature handler was a pre release.
        ' */
        Public Function getPreRelease() As Boolean
            Return dictionary.getBoolean(COSName.PRE_RELEASE, False)
        End Function

        '/**
        ' * A flag that can be used by the signature handler or software module to
        ' * indicate that this signature was created with unrelease software.
        ' *
        ' * @param preRelease is true if the signature was created with a unrelease
        ' *                   software, otherwise false.
        ' */
        Public Sub setPreRelease(ByVal preRelease As Boolean)
            dictionary.setBoolean(COSName.PRE_RELEASE, preRelease)
        End Sub

        '/**
        ' * Indicates the operation system. The format isn't specified yet.
        ' *
        ' * @return a the operation system id or name.
        ' */
        Public Function getOS() As String
            Return dictionary.getString(COSName.OS)
        End Function

        '/**
        ' * Indicates the operation system. The format isn't specified yet.
        ' *
        ' * @param os is a string with the system id or name.
        ' */
        Public Sub setOS(ByVal os As String)
            dictionary.setString(COSName.OS, os)
        End Sub

        '/**
        ' * If there is a LegalPDF dictionary in the catalog
        ' * of the PDF file and the NonEmbeddedFonts attribute in this dictionary
        ' * has a non zero value, and the viewing application has a preference
        ' * set to suppress the display of this warning then the value of this
        ' * attribute will be set to true.
        ' *
        ' * @return true if NonEFontNoWarn is set to true
        ' */
        Public Function getNonEFontNoWarn() As Boolean
            Return dictionary.getBoolean(COSName.NON_EFONT_NO_WARN, True)
        End Function

        '/*
        ' * setNonEFontNoWarn missing. Maybe not needed or should be self
        ' * implemented.
        ' *
        ' * Documentation says:
        ' * (Optional; PDF 1.5) If there is a LegalPDF dictionary in the catalog
        ' * of the PDF file and the NonEmbeddedFonts attribute in this dictionary
        ' * has a non zero value, and the viewing application has a preference
        ' * set to suppress the display of this warning then the value of this
        ' * attribute will be set to true.
        ' */

        '/**
        ' * If true, the application was in trusted mode when signing took place.
        ' *
        ' * @return true if the application was in trusted mode while signing.
        ' *              default: false
        ' */
        Public Function getTrustedMode() As Boolean
            Return dictionary.getBoolean(COSName.TRUSTED_MODE, False)
        End Function

        '/**
        ' * If true, the application was in trusted mode when signing took place.
        ' *
        ' * @param trustedMode true if the application is in trusted mode.
        ' */
        Public Sub setTrustedMode(ByVal trustedMode As Boolean)
            dictionary.setBoolean(COSName.TRUSTED_MODE, trustedMode)
        End Sub

    End Class

End Namespace
