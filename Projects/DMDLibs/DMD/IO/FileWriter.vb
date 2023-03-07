Namespace Io

    ''' <summary>
    ''' Convenience class for writing character files. The constructors of this class assume that the default character encoding and the default byte-buffer size are acceptable. To specify these values yourself, construct an OutputStreamWriter on a FileOutputStream.
    ''' Whether or not a file is available or may be created depends upon the underlying platform. Some platforms, in particular, allow a file to be opened for writing by only one FileWriter (or other file-writing object) at a time. In such situations the constructors in this class will fail if the file involved is already open.
    ''' FileWriter is meant for writing streams of characters. For writing streams of raw bytes, consider using a FileOutputStream.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FileWriter
        Inherits OutputStreamWriter

        ''' <summary>
        ''' Constructs a FileWriter object given a File object.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal file As DMD.Io.File)
            MyBase.New(New FileOutputStream(file.getFullName))
        End Sub


        ''' <summary>
        ''' Constructs a FileWriter object given a File object.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal file As DMD.Io.File, ByVal append As Boolean)
            MyBase.New(New FileOutputStream(file.getFullName))
        End Sub



        ''' <summary>
        ''' Constructs a FileWriter object given a File object.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal fileName As String)
            MyBase.New(New FileOutputStream(fileName))
        End Sub


        ''' <summary>
        ''' Constructs a FileWriter object given a File object.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal fileName As String, ByVal append As Boolean)
            MyBase.New(New FileOutputStream(fileName))
        End Sub



    End Class


End Namespace