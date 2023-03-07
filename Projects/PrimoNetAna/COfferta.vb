Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Public Class COffertaPN
    Inherits DBObjectBase

    Public Preventivo As CPreventivoPN
    Public Codice As String
    Public Istututo As String
    Public Netto As Decimal?
    Public Assicurazione As String
    Public GiorLav As Integer?
    Public N As String
    Public C As String
    Public TAN As Double?
    Public TEG As Double?
    Public TAEG As Double?
    Public TEG_MAX As Double?
    Public TAEG_MAX As Double?
    Public ProvvNetwork As Double?
    Public ProvvAge As Double?
    Public Sconto As Double?

    Public Sub New()

    End Sub

    Public Overrides Function GetModule() As CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_Preventivi"
    End Function

    Protected Overrides Function GetConnection() As CDBConnection
        Return Form1.Conn
    End Function

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        Return MyBase.SaveToRecordset(writer)
    End Function

    '<td> <a  onMouseover = "ddrivetip('Non esistono coefficienti rischio vita  per l\'eta\': 68 anni, sesso: M<br>','',500)"; onMouseout="hideddrivetip()" ; > <img border="0" src="/menuicons/disabled.png" width="16"></a></td>
    '			<td>11762568</td>
    '			<td> <a  href = "#"  onMouseover="ddrivetip('ATTENZIONE: <b>Utente NON ancora censito dalla Societ&agrave; erogante.</b> <br>Non &egrave; possibile procedere con la simulazione della Banca scelta.<br> <b>Seleziona altro preventivo.</b><br>Per informazioni e censimenti contatta il tuo referente commerciale o manda una mail a <b>censimento.rete@primonetwork.it</b>','white',500)"; onMouseout="hideddrivetip()" ; > <img border="0" src="/menuicons/ana_no.png" ></a></td>
    '			<td> <a  href = "#"  onMouseover="ddrivetip('<table width=\'100%\'><tr><td colspan=\'2\' align=\'right\'><b>CQP PENSIONATI 59</b></td></tr><tr><td colspan=\'2\' align=\'right\'><hr/></td></tr><tr><td><b>Provvigioni:</b></td><td align=\'right\'>0,00</td></tr><tr><td><b>Volume:</b></td><td align=\'right\'>0,00</td></tr><tr><td><b>% Volume:</b></td><td align=\'right\'>0</td></tr><tr><td><b>Premio:</b></td><td align=\'right\'>0,00</td></tr><tr><td><b>Totale:</b></td><td align=\'right\'>0,00</td></tr></table>','',200)"; onMouseout="hideddrivetip()" ; > <img border="0" src="/menuicons/pn_5.png" width="18px"></a></td>
    '			<td> <a  target = "_blank" href=""  onMouseover="ddrivetip('<b>Preventivo a bassa redditivit&agrave;, NO Punti Premio.</b><br>','',350)"; onMouseout="hideddrivetip()" ; > <img border="0" src="/menuicons/no_regalo.png" ></a></td>
    '			<td> FINCONTINUO SPA</td>
    '			<td>0,00</td>
    '			<td> AXA CQP 59</td>
    '			<td>--</td>
    '			<td></td>
    '			<td></td>
    '			</tr>


End Class
