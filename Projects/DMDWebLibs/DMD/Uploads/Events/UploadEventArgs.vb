Partial Class WebSite

    <Serializable> _
    Public Class UploadEventArgs
        Inherits System.EventArgs

        <NonSerialized> Private m_Uploader As CFileUploader

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal u As CFileUploader)
            Me.New()
            Me.m_Uploader = u
        End Sub

        Public ReadOnly Property Upload As CFileUploader
            Get
                Return Me.m_Uploader
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
