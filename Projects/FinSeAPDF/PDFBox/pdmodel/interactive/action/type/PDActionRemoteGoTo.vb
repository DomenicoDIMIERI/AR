Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents a remote go-to action that can be executed in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDActionRemoteGoTo
        Inherits PDAction

        ''' <summary>
        ''' This type of action this object represents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "GoToR"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            action = New COSDictionary()
            setSubType(SUB_TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            MyBase.New(a)
        End Sub

        ''/**
        '' * Convert this standard java object to a COS object.
        '' *
        '' * @return The cos object that matches this Java object.
        '' */
        'Public Function getCOSObject() As COSBase
        '    Return action
        'End Function

        ''/**
        '' * Convert this standard java object to a COS object.
        '' *
        '' * @return The cos object that matches this Java object.
        '' */
        'Public Function getCOSDictionary() As COSDictionary
        '    Return action
        'End Function

        '/**
        ' * This will get the type of action that the actions dictionary describes.
        ' * It must be GoToR for a remote go-to action.
        ' *
        ' * @return The S entry of the specific remote go-to action dictionary.
        ' */
        Public Function getS() As String
            Return action.getNameAsString("S")
        End Function

        '/**
        ' * This will set the type of action that the actions dictionary describes.
        ' * It must be GoToR for a remote go-to action.
        ' *
        ' * @param s The remote go-to action.
        ' */
        Public Sub setS(ByVal s As String)
            action.setName("S", s)
        End Sub

        '/**
        ' * This will get the file in which the destination is located.
        ' *
        ' * @return The F entry of the specific remote go-to action dictionary.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getFile() As PDFileSpecification 'throws IOException
            Return PDFileSpecification.createFS(action.getDictionaryObject("F"))
        End Function

        '/**
        ' * This will set the file in which the destination is located.
        ' *
        ' * @param fs The file specification.
        ' */
        Public Sub setFile(ByVal fs As PDFileSpecification)
            action.setItem("F", fs)
        End Sub

        '/**
        ' * This will get the destination to jump to.
        ' * If the value is an array defining an explicit destination,
        ' * its first element must be a page number within the remote
        ' * document rather than an indirect reference to a page object
        ' * in the current document. The first page is numbered 0.
        ' *
        ' * @return The D entry of the specific remote go-to action dictionary.
        ' */

        ' Array or String.
        Public Function getD() As COSBase
            Return action.getDictionaryObject("D")
        End Function

        '/**
        ' * This will set the destination to jump to.
        ' * If the value is an array defining an explicit destination,
        ' * its first element must be a page number within the remote
        ' * document rather than an indirect reference to a page object
        ' * in the current document. The first page is numbered 0.
        ' *
        ' * @param d The destination.
        ' */

        ' In case the value is an array.
        Public Sub setD(ByVal d As COSBase)
            action.setItem("D", d)
        End Sub

        '/**
        ' * This will specify whether to open the destination document in a new window.
        ' * If this flag is false, the destination document will replace the current
        ' * document in the same window. If this entry is absent, the viewer application
        ' * should behave in accordance with the current user preference.
        ' *
        ' * @return A flag specifying whether to open the destination document in a new window.
        ' */
        Public Function shouldOpenInNewWindow() As Boolean
            Return action.getBoolean("NewWindow", True)
        End Function

        '/**
        ' * This will specify the destination document to open in a new window.
        ' *
        ' * @param value The flag value.
        ' */
        Public Sub setOpenInNewWindow(ByVal value As Boolean)
            action.setBoolean("NewWindow", value)
        End Sub

    End Class

End Namespace