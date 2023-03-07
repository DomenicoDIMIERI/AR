<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style>
        .ImageDIV
        {
	        height:400px; 
	        width:500px; 
	        overflow:auto;
	        border:1px solid;
	        cursor:pointer;
        }
        .LinkImage
        {
            cursor:pointer;
        }
    </style>
</head>
<body onkeypress="javascript:fnKeyPress();">
    <form id="form1" runat="server">

        <script type="text/javascript">
        /*
            Added : Variables required for image uploading & validation checking.
            By : Amit Champaneri
            On : 4th April 2008
        */
        var hoverColor = "#00000b"; //DIV Color when mouse is hover the DIV
        var defaultColor = "black"; //DIV default color
        var selectColor = "#000000"; // DIV color when its selected.
        var selectedDIV = "";        //ID of the DIV user has currently selected(it will be 1,2,3 or 4)                
        var objActiveX; //Object of Clipboard ActiveX control.
        
        //Success flags ,whenever it turns to true,it indicates corresponding image is uploaded to the server.
        //And when all 4 flags are TRUE we will actually post the form to the server.
        var success1 = false; 
        
        
        // Flag to indicate uploading is being started,so if in middle user tries to paste image or remove it,we can stop doing that.
        var flagUploadingStarted = false;
        // Variable to get the Current URL and attach httpUtil Page to be passed in ActiveX for sending httpRequest.
        var strURL;
        // Variable holding the Post strings for URL like Random number and type.-
        var postVar;        
        </script>

        <script language="javascript">
        window.onload = function() {            
            
           //Detect ActiveX component using try . . . catch block.
            try {                                        
                objActiveX = document.getElementById("ClipboardActiveX");                    
                
            } catch(ex) {
                alert("Cannot initialize the ActiveX control. Please contact your administrator regarding this error.");                    
            }
            
            // Initializing URL and Post Variables.
            strURL = window.location.toString();
            strURL = strURL.substring(0, strURL.lastIndexOf("/"));
            strURL = strURL + "/HttpUtil1.aspx";
            postVar = "id=1&type=p";
        }
        </script>

        <div>
            <object id="ClipboardActiveX" classid="CLSID:D472AFDE-DA5E-43BA-9A5F-4F21882AAE4A"
                codebase="ClipboardActiveX.dll">
            </object>
            <br />
            &nbsp;</div>

        <script type="text/javascript">
        //Function to highlight the DIV.
            function Highlight(id) 
            {                
                document.getElementById("div" + id).style.border = "solid 2px " + hoverColor;
            }
        //Function to remove the highlight from DIV.
            function removeHighlight(id)
            {                         
                if(selectedDIV != id)       
                    document.getElementById("div" + id).style.border = "solid 1px " + defaultColor;
            }
        //Function to Change the border for the grid which is selected,and re-set for others.
            function selectDiv(divID)
            {
                selectedDIV = divID;                
                document.getElementById("div1").style.border="solid 1px" + defaultColor;                
                document.getElementById("div" + divID).style.border = "solid 3px" + selectColor;
            }
        //Function to handle Paste Image click event for all Images.
        function pasteOnClick(id)
            {        
                if(flagUploadingStarted == true)
                    return;
                    
                selectDiv(id);
                fnCall();
            }
        //Function be called when user presses CTRL + V.
        function fnKeyPress(sender, args) {            
            if(window.event.keyCode == 22)
                fnCall();                                                
            else if(window.event.keyCode == 24 && selectedDIV != "")
            {                                                                
                deleteOnClick(selectedDIV);
            }
        }
        /*
        Added : Function to handle Paste Image click event for all Images.
        By : Amit Champaneri
        On : 7th April 2008
    */
    function pasteOnClick(id)
    {        
        if(flagUploadingStarted == true)
            return;
            
        selectDiv(id);
        fnCall();
    }
    /*
        Added : Function to handle Remove Image click event for all Images.
        By : Amit Champaneri
        On : 7th April 2008
    */
    function deleteOnClick(id)
    {
        if(flagUploadingStarted == true)
            return;
        switch(id)
        {
            case '1':
                    document.getElementById("img1").src = "";
                    document.getElementById("img1").style.display = "none";
                    document.getElementById("<%=hdnImageFileName1.ClientID %>").value = "";
                    success1 = false;
                    break;            
        }
        selectDiv(id);
    }
        </script>

        <script type="text/javascript">
                                                   
            // Main Function to be called when user presses CTRL + V to paste the image into selected DIV.
            /*
                Added : Function to handle Additional Location Details list & enable/disable validator.
                By : Amit Champaneri
                On : 1st April 2008
            */
            function fnCall()
            {                                                                                                                                                                                                                                      
                try
                {                                                            
                    // Getting the Physical path on which,the images will be stored on client machine.
                    var PhysicalPathForTempImage = "c:\\Amit\\";
                    
                    // Getting ActiveX
                    //var objActiveX = document.getElementById("ClipboardActiveX");    
                    
                    //Newly created images will be saved at below path which is passed.
                    
                    var strFilePath = objActiveX.getCopiedImage(PhysicalPathForTempImage); 

                    // If Image file is not copied.
                    if (strFilePath.indexOf("ERR1") > -1)
                    {
                        alert("The file you have attempted to paste is not in a form that gTicket can store. It should be file with the extension '.jpg','.bmp', or '.gif'. Please contact your system administrator for help in uploading this file, as it may need to be converted to an appropriate format");
                        //document.getElementById("imgProgress").style.display = "none";
                        return false;
                    }
                    // If user has copied the contents which are not in Image format like text etc.
                    else if (strFilePath.indexOf("ERR2") > -1)
                    {
                        alert("The contents you have attempted to paste is not in a form that gTicket can store. It should be in format which can be saved in file with the extension '.jpg','.bmp', or '.gif'. Please contact your system administrator for help in uploading this file, as it may need to be converted to an appropriate format");
                        //document.getElementById("imgProgress").style.display = "none";
                        return false;   
                    }
                    else if (strFilePath.indexOf("ERR3") > -1)
                    {
                        alert("Failure occured in generating Image file from clipboard.Please try again after some time or if error continues contact your administrator quoiting this error.")
                        //document.getElementById("imgProgress").style.display = "none";
                        return false;
                    }
                    else if (strFilePath.indexOf("ERR4") > -1)
                    {
                        alert("System failed for creating target directory path provided in configuration file.Either create directory yourself or contact administrator to help you in creating / changing the directory path")
                        //document.getElementById("imgProgress").style.display = "none";
                        return false;
                    }
                    else
                    {
                        // The case if user has not selected any DIV and tried to paste the image.
                        if(selectedDIV == "")
                        {
                            alert("Please first select any DIV to paste an Image");
                            //document.getElementById("imgProgress").style.display = "none";
                        }
                        else
                        {
                            
                            //Image is displayed with selected source.
                            document.getElementById("img" + selectedDIV).style.display = "block";
                            document.getElementById("img" + selectedDIV).src = strFilePath;
                            
                            //Getting Image File Name.                        
                            //var strFileName = "";
                            //strFileName = strFilePath.substring(strFilePath.lastIndexOf("\\") + 1,strFilePath.length);
                            
                            //Storing the image File name in corresponding hidden Image variable.
                            switch(selectedDIV)
                            {
                                case "1":
                                            document.getElementById("<%= hdnImageFileName1.ClientID%>").value = strFilePath;
                                            break;
                            }                                                                    
                        }
                    }   
                }
                catch(ex)
                {
                    
                    var strError = ex.message;
                    // This Error may occur if user has not created the Temp Image Directory mentioned in Configuration File.
                    if(strError.indexOf("GDI+") > -1)
                    {
                        alert("The physical path you have provided in Configuration file,is not found to store temporary image files.Contact your administrator quoting this error.");                                                                
                        return false;
                    }                
                }         
            }                                                    
        </script>

        <script type="text/javascript">
        //---------------------------------------------------IMAGE 1 UPLOAD--------------------------------------------------------------
            function uploadImage1(strFileName)
            {                        
                var response;
                try{        
                    response = objActiveX.UploadFiles(strFileName,strURL,postVar);
                    if(response == "")
                    {
                        success1 = true;
                        spImage1Saved.style.visibility = "visible";
                        alert("Image uploaded successfully...");
                        /*
                            Added : If all success bits are true we can post the form to the server
                            By : Amit Champaneri
                            On : 3th April 2008
                        */
                        return true;
                    }
                    else
                    {
                        alert("Image 1 Uploading failed....See the response received....");
                        alert(response);
                        return false;
                    }    
                   }
                catch(ex)
                {
                    alert("Image 1 Uploading failed....See the response received....");
                    alert(response);
                }
            }
            /*
                Added : Functions to upload all available images to the server.
                By : Amit Champaneri
                On : 3th April 2008
            */
            function uplImages()
            {                                                
                    flagUploadingStarted = true;
                    
                    // Uploading Image 1 if its present.
                    if(document.getElementById("<%= hdnImageFileName1.ClientID%>").value != "")
                    {            
                        uploadImage1(document.getElementById("<%= hdnImageFileName1.ClientID%>").value);
                    }
            }
        </script>

        <table border="0" cellpadding="2" cellspacing="3" style="width: 50%; height: 50%;
            background-color: papayawhip;">
            <tr>
                <td style="height: 22px" align="center">
                    <img id="imgPaste1" src="Images/imgPaste.gif" class="LinkImage" title="Click here to Paste any copied Image"
                        onclick="javascript:pasteOnClick('1');" />&nbsp;
                    <img id="imgDel1" src="Images/imgDelete.gif" class="LinkImage" title="Click here to Remove this Image"
                        onclick="javascript:deleteOnClick('1');" />
                    <span id="spImage1Saved" class="DefaultLabelBold" style="visibility: hidden;">Image
                        Uploaded...</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="div1" class="ImageDIV" onclick="selectDiv('1');" onmouseover="Highlight('1');"
                        onmouseout="removeHighlight('1');" title="Select this DIV & Press CTRL+V to paste any copied Image.">
                        <img id="img1" style="display: none;" />
                        <asp:HiddenField runat="server" ID="hdnImageFileName1" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <a href="javascript:uplImages();" style="font-family: Courior New">Upload This Image
                        To Server</a>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
            </tr>
        </table>        
    </form>
</body>
</html>
