Namespace Drawings

    Public Class ImageReader
        Implements IDisposable

        Sub setInput(input As ImageInputStream)
            Throw New NotImplementedException
        End Sub


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose

        End Sub


        Public Overridable Function getImageMetadata(p1 As Integer) As IIOMetadata
            Throw New NotImplementedException
        End Function

        Public Overridable Function getDefaultReadParam() As Object
            Throw New NotImplementedException
        End Function

        Public Overridable Function readRaster(p1 As Integer, p2 As Object) As Raster
            Throw New NotImplementedException
        End Function

        Public Overridable Function read(p1 As Integer) As Image
            Throw New NotImplementedException
        End Function

    End Class

End Namespace