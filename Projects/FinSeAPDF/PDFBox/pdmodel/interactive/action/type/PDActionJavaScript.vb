Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents a JavaScript action.
    ' *
    ' * @author Michael Schwarzenberger (mi2kee@gmail.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDActionJavaScript
        Inherits PDAction

        ''' <summary>
        ''' This type of action this object represents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "JavaScript"

        '/**
        ' * Constructor #1.
        ' */
        Public Sub New()
            MyBase.New()
            setSubType(SUB_TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param js Some javascript code.
        ' */
        Public Sub New(ByVal js As String)
            Me.New()
            setAction(js)
        End Sub

        '/**
        ' * Constructor #2.
        ' *
        ' *  @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            MyBase.New(a)
        End Sub

        '/**
        ' * @param sAction The JavaScript.
        ' */
        Public Sub setAction(ByVal sAction As PDTextStream)
            action.setItem("JS", sAction)
        End Sub

        '/**
        ' * @param sAction The JavaScript.
        ' */
        Public Sub setAction(ByVal sAction As String)
            action.setString("JS", sAction)
        End Sub

        '/**
        ' * @return The Javascript Code.
        ' */
        Public Function getAction() As PDTextStream
            Return PDTextStream.createTextStream(action.getDictionaryObject("JS"))
        End Function


    End Class

End Namespace
