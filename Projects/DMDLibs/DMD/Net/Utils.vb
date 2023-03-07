Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets

Namespace Net

    Public NotInheritable Class Utils
        Private Sub New()

        End Sub

        Public Shared Function GetLocalIP() As IPAddress
            For Each ip As IPAddress In System.Net.Dns.GetHostAddresses("")
                If ip.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                    Dim gw As IPAddress = GetDefaultGateway(ip)
                    If (gw IsNot Nothing) Then
                        Return ip
                    End If
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetDefaultGateway(ByVal ip As IPAddress) As IPAddress
            Dim myNetworkAdapters() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces
            Dim myAdapterProps As IPInterfaceProperties = Nothing
            Dim myGateways As GatewayIPAddressInformationCollection = Nothing

            For Each adapter As NetworkInterface In myNetworkAdapters
                For Each unicastIPAddressInformation As System.Net.NetworkInformation.UnicastIPAddressInformation In adapter.GetIPProperties().UnicastAddresses
                    If unicastIPAddressInformation.Address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                        If ip.Equals(unicastIPAddressInformation.Address) Then
                            'Subnet Mask
                            ' Label3.Text = unicastIPAddressInformation.IPv4Mask.ToString()

                            Dim adapterProperties As System.Net.NetworkInformation.IPInterfaceProperties = adapter.GetIPProperties()
                            For Each gateway As System.Net.NetworkInformation.GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                'Default Gateway
                                If (gateway.Address.AddressFamily = AddressFamily.InterNetwork) Then
                                    Return gateway.Address '.ToString()
                                End If
                            Next

                            ''DNS1
                            'If adapterProperties.DnsAddresses.Count > 0 Then
                            '    Label5.Text = adapterProperties.DnsAddresses(0).ToString()
                            'End If

                            ''DNS2
                            'If adapterProperties.DnsAddresses.Count > 1 Then
                            '    Label6.Text = adapterProperties.DnsAddresses(1).ToString()
                            'End If
                        End If
                    End If
                Next
            Next

            Return Nothing
        End Function

    End Class


End Namespace
