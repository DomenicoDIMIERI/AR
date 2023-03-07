Imports FinSeA
Imports FinSeA.SMSGateway

Partial Class widgets_websvcf_checksms
    Inherits System.Web.UI.Page
       
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        SyncLock SMSGateway.Lock
            SMSGateway.CheckInit()

        End SyncLock
    End Sub

    Private Function Terminate(ByVal writer As HtmlTextWriter, ByVal code As ErrorCode, ByVal message As String) As ErrorCode
        writer.Write(Right("00" & Hex(code), 2) & "|" & message)
        Return code
    End Function

End Class
