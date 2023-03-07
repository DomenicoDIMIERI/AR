Namespace Io

    Public Class File

        Private _name As String

        Sub New(name As String)
            ' TODO: Complete member initialization 
            _name = name
        End Sub

        Public Shared ReadOnly separator As String = "\"

        Function getParentFile() As File
            Throw New NotImplementedException
        End Function

        Function getAbsolutePath() As String
            Throw New NotImplementedException
        End Function

        Function getFullName() As String
            Return Me._name
        End Function

        Function exists() As Boolean
            Return System.IO.File.Exists(Me._name)
        End Function

        Function getParent() As Object
            Return System.IO.Path.GetDirectoryName(Me._name)
        End Function

        Function Length() As Integer
            Throw New NotImplementedException
        End Function

        Function getPath() As String
            Throw New NotImplementedException
        End Function

        Function getName() As String
            Throw New NotImplementedException
        End Function

        Function delete() As Boolean
            Throw New NotImplementedException
        End Function


    End Class

End Namespace