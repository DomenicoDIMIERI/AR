Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents a launch action that can be executed in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDActionLaunch
        Inherits PDAction

        ''' <summary>
        ''' This type of action this object represents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Launch"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            MyBase.New()
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

        '/**
        ' * This will get the application to be launched or the document
        ' * to be opened or printed. It is required if none of the entries
        ' * Win, Mac or Unix is present. If this entry is absent and the
        ' * viewer application does not understand any of the alternative
        ' * entries it should do nothing.
        ' *
        ' * @return The F entry of the specific launch action dictionary.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getFile() As PDFileSpecification 'throws IOException
            Return PDFileSpecification.createFS(getCOSDictionary().getDictionaryObject("F"))
        End Function

        '/**
        ' * This will set the application to be launched or the document
        ' * to be opened or printed. It is required if none of the entries
        ' * Win, Mac or Unix is present. If this entry is absent and the
        ' * viewer application does not understand any of the alternative
        ' * entries it should do nothing.
        ' *
        ' * @param fs The file specification.
        ' */
        Public Sub setFile(ByVal fs As PDFileSpecification)
            getCOSDictionary().setItem("F", fs)
        End Sub

        '/**
        ' * This will get a dictionary containing Windows-specific launch parameters.
        ' *
        ' * @return The Win entry of of the specific launch action dictionary.
        ' */
        Public Function getWinLaunchParams() As PDWindowsLaunchParams
            Dim win As COSDictionary = action.getDictionaryObject("Win")
            Dim retval As PDWindowsLaunchParams = Nothing
            If (win IsNot Nothing) Then
                retval = New PDWindowsLaunchParams(win)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a dictionary containing Windows-specific launch parameters.
        ' *
        ' * @param win The action to be performed.
        ' */
        Public Sub setWinLaunchParams(ByVal win As PDWindowsLaunchParams)
            action.setItem("Win", win)
        End Sub

        '/**
        ' * This will get the file name to be launched or the document to be opened
        ' * or printed, in standard Windows pathname format. If the name string includes
        ' * a backslash character (\), the backslash must itself be preceded by a backslash.
        ' * This value must be a single string; it is not a file specification.
        ' *
        ' * @return The F entry of the specific Windows launch parameter dictionary.
        ' */
        Public Function getF() As String
            Return action.getString("F")
        End Function

        '/**
        ' * This will set the file name to be launched or the document to be opened
        ' * or printed, in standard Windows pathname format. If the name string includes
        ' * a backslash character (\), the backslash must itself be preceded by a backslash.
        ' * This value must be a single string; it is not a file specification.
        ' *
        ' * @param f The file name to be launched.
        ' */
        Public Sub setF(ByVal f As String)
            action.setString("F", f)
        End Sub

        '/**
        ' * This will get the string specifying the default directory in standard DOS syntax.
        ' *
        ' * @return The D entry of the specific Windows launch parameter dictionary.
        ' */
        Public Function getD() As String
            Return action.getString("D")
        End Function

        '/**
        ' * This will set the string specifying the default directory in standard DOS syntax.
        ' *
        ' * @param d The default directory.
        ' */
        Public Sub setD(ByVal d As String)
            action.setString("D", d)
        End Sub

        '/**
        ' * This will get the string specifying the operation to perform:
        ' * open to open a document
        ' * print to print a document
        ' * If the F entry designates an application instead of a document, this entry
        ' * is ignored and the application is launched. Default value: open.
        ' *
        ' * @return The O entry of the specific Windows launch parameter dictionary.
        ' */
        Public Function getO() As String
            Return action.getString("O")
        End Function

        '/**
        ' * This will set the string specifying the operation to perform:
        ' * open to open a document
        ' * print to print a document
        ' * If the F entry designates an application instead of a document, this entry
        ' * is ignored and the application is launched. Default value: open.
        ' *
        ' * @param o The operation to perform.
        ' */
        Public Sub setO(ByVal o As String)
            action.setString("O", o)
        End Sub

        '/**
        ' * This will get a parameter string to be passed to the application designated by the F entry.
        ' * This entry should be omitted if F designates a document.
        ' *
        ' * @return The P entry of the specific Windows launch parameter dictionary.
        ' */
        Public Function getP() As String
            Return action.getString("P")
        End Function

        '/**
        ' * This will set a parameter string to be passed to the application designated by the F entry.
        ' * This entry should be omitted if F designates a document.
        ' *
        ' * @param p The parameter string.
        ' */
        Public Sub setP(ByVal p As String)
            action.setString("P", p)
        End Sub

        '/**
        ' * This will specify whether to open the destination document in a new window.
        ' * If this flag is false, the destination document will replace the current
        ' * document in the same window. If this entry is absent, the viewer application
        ' * should behave in accordance with the current user preference. This entry is
        ' * ignored if the file designated by the F entry is not a PDF document.
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
