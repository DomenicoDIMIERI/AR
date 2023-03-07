#include <iostream>
#include <vector>  
#include <string>  
#include <stdio.h>  
#include <stdlib.h> 
#include <string.h> 

//#include <cgicc/CgiDefs.h> 
//#include <cgicc/Cgicc.h> 
//#include <cgicc/HTTPHTMLHeader.h> 
//#include <cgicc/HTMLClasses.h>

using namespace std;
//using namespace cgicc;


void urldecode(char *pszDecodedOut, size_t nBufferSize, const char *pszEncodedIn)
{
    memset(pszDecodedOut, 0, nBufferSize);

    enum DecodeState_e
    {
        STATE_SEARCH = 0, ///< searching for an ampersand to convert
        STATE_CONVERTING, ///< convert the two proceeding characters from hex
    };

    DecodeState_e state = STATE_SEARCH;

    for(unsigned int i = 0; i < strlen(pszEncodedIn)-1; ++i)
    {
        switch(state)
        {
        case STATE_SEARCH:
            {
                if(pszEncodedIn[i] != '%')
                {
                    strncat(pszDecodedOut, &pszEncodedIn[i], 1);
                    //assert(strlen(pszDecodedOut) < nBufferSize);
                    break;
                }

                // We are now converting
                state = STATE_CONVERTING;
            }
            break;

        case STATE_CONVERTING:
            {
                // Conversion complete (i.e. don't convert again next iter)
                state = STATE_SEARCH;

                // Create a buffer to hold the hex. For example, if %20, this
                // buffer would hold 20 (in ASCII)
                char pszTempNumBuf[3] = {0};
                strncpy(pszTempNumBuf, &pszEncodedIn[i], 2);

                // Ensure both characters are hexadecimal
                bool bBothDigits = true;

                for(int j = 0; j < 2; ++j)
                {
                    if(!isxdigit(pszTempNumBuf[j]))
                        bBothDigits = false;
                }

                if(!bBothDigits)
                    break;

                // Convert two hexadecimal characters into one character
                int nAsciiCharacter;
                sscanf(pszTempNumBuf, "%x", &nAsciiCharacter);

                // Ensure we aren't going to overflow
                //assert(strlen(pszDecodedOut) < nBufferSize);

                // Concatenate this character onto the output
                strncat(pszDecodedOut, (char*)&nAsciiCharacter, 1);

                // Skip the next character
                i++;
            }
            break;
        }
    }
}

int main(int argc, char** argv) {
	//Cgicc cgi;
	const char* ENV[ 24 ] = {                 
        "COMSPEC", "DOCUMENT_ROOT", "GATEWAY_INTERFACE",   
        "HTTP_ACCEPT", "HTTP_ACCEPT_ENCODING",             
        "HTTP_ACCEPT_LANGUAGE", "HTTP_CONNECTION",         
        "HTTP_HOST", "HTTP_USER_AGENT", "PATH",            
        "QUERY_STRING", "REMOTE_ADDR", "REMOTE_PORT",      
        "REQUEST_METHOD", "REQUEST_URI", "SCRIPT_FILENAME",
        "SCRIPT_NAME", "SERVER_ADDR", "SERVER_ADMIN",      
        "SERVER_NAME","SERVER_PORT","SERVER_PROTOCOL",     
        "SERVER_SIGNATURE","SERVER_SOFTWARE" };   
    const char* command = "C:\\Users\\Domenico.DiMieri\\Documents\\Visual Studio 2012\\WebSites\\WebSite1\\cgi-bin\\pdf2image-cmd.exe";
	    
    char* qs = getenv( "QUERY_STRING");
    char* options = new char[strlen(qs) + 1048];
    
    urldecode(options, strlen(qs) + 1048, qs);
    
    char* buffer = new char[strlen(options) + 1048];
    buffer = strcpy(buffer, "");
    //buffer = strcat(buffer, "\"");
    buffer = strcat(buffer, command);
    //buffer = strcat(buffer, "\"");
    buffer = strcat(buffer, " ");
    //buffer = strcat(buffer, "\"");
    buffer = strcat(buffer, options);
    //buffer = strcat(buffer, "\"");
    
	system(buffer);
	
	cout << "Content-type:text/html\r\n\r\n";
	cout << "<html>\n";
	cout << "<head>\n";
	cout << "<title>Test</title>\n";
	cout << "</head>\n";
	cout << "<body>\n";
	cout << buffer << "\n";
	
	// get list of files to be uploaded
	//const_file_iterator file = cgi.getFile("userfile");
	//if(file != cgi.getFiles().end()) {
	//  // send data type at cout.
	//  cout << HTTPContentHeader(file->getDataType());
	//  // write content at cout.
	//  file->writeToStream(cout);
	//}
	//cout << "<File uploaded successfully>\n";
	cout << "</body>\n";
	cout << "</html>\n";
    
    delete buffer;
    delete options;
   
	return 0;
}
