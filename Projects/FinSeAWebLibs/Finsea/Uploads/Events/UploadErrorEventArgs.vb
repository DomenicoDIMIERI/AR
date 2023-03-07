Partial Class WebSite

    <Serializable>
    Public Class UploadErrorEventArgs
        Inherits UploadEventArgs

        Private m_Cause As System.Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal u As CFileUploader, ByVal e As Exception)
            MyBase.New(u)
            Me.m_Cause = e
        End Sub

        Public ReadOnly Property Cause As System.Exception
            Get
                Return Me.m_Cause
            End Get
        End Property

    End Class


End Class
