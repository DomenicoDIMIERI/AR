using System;
using System.IO;
using System.Net.Sockets;
using DNS.Protocol;
using System.Threading.Tasks;
using System.Net;

namespace DNS.Client.RequestResolver {
    public class TcpRequestResolver : IRequestResolver {
        public async Task<ClientResponse> Request(ClientRequest request) {
            IPEndPoint dns = request.Dns;

            using(TcpClient tcp = new TcpClient()) {
                await tcp.ConnectAsync(dns.Address, dns.Port);

                Stream stream = tcp.GetStream();
                byte[] buffer = request.ToArray();
                byte[] length = BitConverter.GetBytes((ushort) buffer.Length);

                if (BitConverter.IsLittleEndian) {
                    Array.Reverse(length);
                }

                await stream.WriteAsync(length, 0, length.Length);
                await stream.WriteAsync(buffer, 0, buffer.Length);

                buffer = new byte[2];
                await Read(stream, buffer);

                if (BitConverter.IsLittleEndian) {
                    Array.Reverse(buffer);
                }

                buffer = new byte[BitConverter.ToUInt16(buffer, 0)];
                await Read(stream, buffer);

                Response response = Response.FromArray(buffer);

                return new ClientResponse(request, response, buffer);
            }
        }

        private static async Task Read(Stream stream, byte[] buffer) {
            int length = buffer.Length;
            int offset = 0;
            int size = 0;

            while (length > 0 && (size = await stream.ReadAsync(buffer, offset, length)) > 0) {
                offset += size;
                length -= size;
            }

            if (length > 0) {
                throw new IOException("Unexpected end of stream");
            }
        }
    }
}
