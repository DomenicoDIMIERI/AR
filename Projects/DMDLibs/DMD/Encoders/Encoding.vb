Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Public NotInheritable Class Encoding
    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Shared Function Encode(ByVal buffer As String, ByVal encoder As Encoder) As String
        Return encoder.Encode(buffer)
    End Function

    '        void golombEncode(char* source, char* dest, int M)
    '{
    '    IntReader intreader(source);
    '    BitWriter bitwriter(dest);
    '    while(intreader.hasLeft())
    '    {
    '        int num = intreader.getInt();
    '        int q = num / M;
    '        for (int i = 0 ; i < q; i++)
    '            bitwriter.putBit(true);   // write q ones
    '        bitwriter.putBit(false);      // write one zero
    '        int v = 1;
    '        for (int i = 0 ; i < log2(M); i++)
    '        {            
    '            bitwriter.putBit( v & num );  
    '            v = v << 1;         
    '        }
    '    }
    '    bitwriter.close();
    '    intreader.close();
    '}

    '       void golombDecode(char* source, char* dest, int M)
    '{
    '    BitReader bitreader(source);
    '    IntWriter intwriter(dest);
    '    int q = 0;
    '    int nr = 0;
    '    while (bitreader.hasLeft())
    '    {
    '        nr = 0;
    '        q = 0;
    '        while (bitreader.getBit()) q++;     // potentially dangerous with malformed files.
    '        for (int a = 0; a < log2(M); a++)   // read out the sequential log2(M) bits
    '            if (bitreader.getBit())
    '                nr += 1 << a;
    '        nr += q*M;                          // add the bits and the multiple of M
    '        intwriter.putInt(nr);               // write out the value
    '    }
    '    bitreader.close();
    '    intwriter.close();
    '}
End Class

