eFormCSharp
================

C# Example code to communicate with Microting eForm API and Notification Server



License
=======

The MIT License (MIT)

Copyright (c) 2014 microting

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



Getting started and prof of concept
===================================
Items listed as ADVANCED, can be skipped to begin with, as the are either may not needed by you or might be better to learn after the core mechanics.



eForm API

1. Contact Microting at +45 66 11 10 66 and get an account for https://my.microting.com (Developers backend)
2. Retrieve token, srv_name, site_id from above link (referred to as 'token', 'address' 'apiId' in the DLL)
3. Start the Tester program and fill in "Config (step 1)" section with the information obtained from Developers backend: Token, Address and API Id. Leave Organization Id empty for now.
4. Click on "Send sample". This will check that the Tester can connect to Microting and get a response with the entered data.
  -  If there is any errors try to fix these before moving on, and contact Microting if needed
5. Click on "Generate XML". This will generate a sample XML that will be shown in "Message sent to Microting (the box)" to the left.
  *  ADVANCED - It's possible to alter this XML before in 'the box' and by clicking the "Verify XML" above 'the box'. The software will check if it's possible for it to still read the XML, and convert into a fully working C# class, and back into XML. WARNING does not guaranty that the XML is fully acceptable by the Microting API.
  *  ADVANCED - In order to use Entity_Select and Entity_Search elements needs an 'Organization Id' and entered. If the XML contains any of these elements click on the "Send Ext. XML" instead. BETA version: Entity_Select and Entity_Search elements are not implemented server side yet.
  *  ADVANCED - By using the drop-down list to the right of the "Generate Sample" button, its possible to have that button generate XML samples of increasing complexity. Sample 1-3 are covers all but the rarest cases. Sample 4 contains Entity_Select and Entity_Search elements, see above.
6. Click on "Send XML". This will send the XML in 'the box' to the Microting server, and display the response in the box on the right side "Message received from Microting".
  -  If this was a successful, as in the Microting server received, and queue the eForm for processing, there will be an eForm Id listed in the response. It's possible to dig out this Id by clicking "Get Id".
  *  ADVANCED - By clicking the "Verify XML" above the response. The software will check if it's possible for the software to read the XML, and convert into a fully working C# class, and back into XML.
  *  ADVANCED - By entering the Id of the wanted eForm in 'the box' (or using the "Get It") and clicking on "Check Id", you will be informed of the eFrom's status. Is it still only queued? Is it being processed? Or is it ready to be used?.  
7. Once the eForm has been processed it's possible to retrieve its data, by clicking on the "Fetch Id" and the wanted eForm Id in 'the box'. In the reply it's possible to see if the eForm has filled or not, and if filled all of the data.
8. Once an eForm is no longer wanted, use the "Delete Id" to mark the eForm for deletion.
  *  ADVANCED - Trace messages can be seen in the log, found in the same folder as the Tester program. Name should be DD-MM-YYYY_Log.txt (Ex. 30-12-2001_Log.txt).


ADVANCED - Notification Subscriber

1. Contact Microting at +45 66 11 10 66 and get the needed credentials
2. Receive a token and the notification server's address (referred to as 'token' and 'address' in the DLL)
3. Start the Tester program and fill in "Notification Subscriber" section with the information obtained from Developers backend: Token, Address.
4. Click on "Subscribe". This will open an asynchronous connection to notification server, which will log all messages back and forth in the log file. See details last line of eForm API. A box will pop-up when connection is made.
5. Click on "Unsubscribe". This will start the process of closing the connection. Once the connection is closed a pop-up box will appear.
  
  

![Screenshot]
(https://raw.githubusercontent.com/microting/eFormCSharp/master/screenshot.png)



Detailed looked and how to
==========================
These are examples are intended as a help and suggestion on how to implement the eForm API and Notification Subscriber DLL. It's not necessary the best way for you to use these.


eForm API

Notification Subscriber