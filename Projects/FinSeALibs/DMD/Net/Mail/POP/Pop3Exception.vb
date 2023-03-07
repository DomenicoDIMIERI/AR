Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents a Pop3 Exception.
    ''' </summary>
    <Global.System.Serializable>
    Public Class Pop3Exception
        Inherits System.Exception

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Exception"/> class.
        ''' </summary>
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub
        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Exception"/> class.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub
        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Exception"/> class.
        ''' </summary>
        ''' <param name="message">The message.</param>
        ''' <param name="innerException">The inner.</param>
        Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
            MyBase.New(message, innerException)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Exception"/> class.
        ''' </summary>
        ''' <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        ''' <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        ''' <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        ''' <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace