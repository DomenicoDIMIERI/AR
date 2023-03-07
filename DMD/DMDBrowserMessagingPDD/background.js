/* COPYRIGHTS
 * Author: Domenico DI MIERI
 * Date: 1/6/2017
 * 
 * This file is copyrighted and CANNOT be modified or distributed without prior written permission from the author.
 * 
 * Description:
 * This file provide the background scripts that the browser keeps in memory when it loads the plugin.
 * When loaded it installs a listener for chrome.browserAction.onClicked that allows the user to
 * send the content of the active tab to this script for parsing.
 * If this script recognizes a WhatsApp conversation it is sent to the SERVICEURL for parsing
 * further computing.
 *
 */
var DEBUG = true;
//var SERVICEURL = "http://localhost:3016/bd/whatsapp.aspx"; //The service to notify conversations
//var SERVICEURL = "http://areariservata.finsea.net/bd/whatsapp.aspx"; //The service to notify conversations
var SERVICEURL = "http://www.prestitidonato.it/bd/whatsapp.aspx"; //The service to notify conversations
var SERVICEUSERNAME = "PDD";                     //A preshared key to identify the destination 
var SERVICENUMBER = "3713300459";                          //A preshared key to identify the destination 

 
function sendDataToRemoteServer(xml) {
    try {
        var parameters = 'pageContent=' + encodeURIComponent(xml);
        var req = new XMLHttpRequest();
        req.open('POST', SERVICEURL + "?_a=SendPage&un=" + SERVICEUSERNAME + "&sn=" + SERVICENUMBER, true);
        req.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        req.send(parameters);
        console.log("sendBody: ok");
    } catch (ex) {
        console.log("sendBody: " + ex);
    }
};
  
// A function to use as callback
function doStuffWithDom(data) {
    console.log("Il servizio WhatsApp background ha ricevuto i dati");
    return sendDataToRemoteServer(data);
}

// When the browser-action button is clicked...
chrome.browserAction.onClicked.addListener(function (tab) {
    // ...check the URL of the active tab against our pattern and...
    //if (urlRegex.test(tab.url)) {
    // ...if it matches, send a message specifying a callback too
    chrome.tabs.sendMessage(tab.id, { text: 'report_back' }, doStuffWithDom);
    //}
});

  