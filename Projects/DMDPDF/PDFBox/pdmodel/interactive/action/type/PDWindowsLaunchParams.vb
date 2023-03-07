Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * Launch paramaters for the windows OS.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDWindowsLaunchParams
        Implements COSObjectable

        ''' <summary>
        ''' The open operation for the launch.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OPERATION_OPEN As String = "open"

        ''' <summary>
        ''' The print operation for the lanuch.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OPERATION_PRINT As String = "print"

        ''' <summary>
        ''' The params dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected params As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            params = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param p The params dictionary.
        ' */
        Public Sub New(ByVal p As COSDictionary)
            params = p
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return params
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return params
        End Function

        '/**
        ' * The file to launch.
        ' *
        ' * @return The executable/document to launch.
        ' */
        Public Function getFilename() As String
            Return params.getString("F")
        End Function

        '/**
        ' * Set the file to launch.
        ' *
        ' * @param file The executable/document to launch.
        ' */
        Public Sub setFilename(ByVal file As String)
            params.setString("F", file)
        End Sub

        '/**
        ' * The dir to launch from.
        ' *
        ' * @return The dir of the executable/document to launch.
        ' */
        Public Function getDirectory() As String
            Return params.getString("D")
        End Function

        '/**
        ' * Set the dir to launch from.
        ' *
        ' * @param dir The dir of the executable/document to launch.
        ' */
        Public Sub setDirectory(ByVal dir As String)
            params.setString("D", dir)
        End Sub

        '/**
        ' * Get the operation to perform on the file.  This method will not return null,
        ' * OPERATION_OPEN is the default.
        ' *
        ' * @return The operation to perform for the file.
        ' * @see PDWindowsLaunchParams#OPERATION_OPEN
        ' * @see PDWindowsLaunchParams#OPERATION_PRINT
        ' */
        Public Function getOperation() As String
            Return params.getString("O", OPERATION_OPEN)
        End Function

        '/**
        ' * Set the operation to perform..
        ' *
        ' * @param op The operation to perform on the file.
        ' */
        Public Sub setOperation(ByVal op As String)
            params.setString("D", op)
        End Sub

        '/**
        ' * A parameter to pass the executable.
        ' *
        ' * @return The parameter to pass the executable.
        ' */
        Public Function getExecuteParam() As String
            Return params.getString("P")
        End Function

        '/**
        ' * Set the parameter to pass the executable.
        ' *
        ' * @param param The parameter for the executable.
        ' */
        Public Sub setExecuteParam(ByVal param As String)
            params.setString("P", param)
        End Sub

    End Class

End Namespace
