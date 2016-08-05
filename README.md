eFormCSharp
=====================================

C# Example code to communicate with Microting eForm API and Notification Server



License
=======

The MIT License (MIT)

Copyright (c) 2014 Microting

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



Overview
=====================================
This software package is made up of 4 DLLs and 1 winForm for showcase purpose.

eFormCommunicator.dll 
Is responsible for sending and receiving XML messages from and to Microting’s eForm API.

eFormRequest.dll
Is responsible for converting the XML to C# classes, and back. It contains all the classes needed for any of the eForm elements.

eFormResponse.dll
Is responsible for converting the XML to C# classes, and back. It contains all the classes needed for any of the eForm responses.

eFormSubscriber.dll
Is responsible for connecting and keeping the connection open to Microting’s notification server. 

eFormTester.exe
Is included for testing and instructional reasons.





Getting started and prof of concept
=====================================
Items listed as ADVANCED, can be skipped to begin with, as the are either may not needed by you or might be better to learn after the core mechanics.



![Screenshot]
(https://raw.githubusercontent.com/microting/eFormCSharp/master/screenshot.png)



eForm API

1. Contact Microting at +45 66 11 10 66 and get an account for https://my.microting.com (Developers backend)
2. Retrieve token, srv_name, site_id from above link (referred to as 'token', 'address' 'apiId' in the DLL)
3. Start the Tester program and fill in "Config (step 1)" section with the information obtained from Developers backend: Token, Address and API Id. Leave Organization Id empty for now.
4. Click on "Send sample". This will check that the Tester can connect to Microting and get a response with the entered data.
- If there is any errors try to fix these before moving on, and contact Microting if needed
5. Click on "Generate XML". This will generate a sample XML that will be shown in "Message sent to Microting (the box)" to the left.
* ADVANCED - It's possible to alter this XML before in 'the box' and by clicking the "Verify XML" above 'the box'. The software will check if it's possible for it to still read the XML, and convert into a fully working C# class, and back into XML. WARNING does not guaranty that the XML is fully acceptable by the Microting API.
* ADVANCED - In order to use Entity_Select and Entity_Search elements needs an 'Organization Id' and entered. If the XML contains any of these elements click on the "Send Ext. XML" instead. BETA version: Entity_Select and Entity_Search elements are not implemented server side yet.
* ADVANCED - By using the drop-down list to the right of the "Generate Sample" button, its possible to have that button generate XML samples of increasing complexity. Sample 1-3 are covers all but the rarest cases. Sample 4 contains Entity_Select and Entity_Search elements, see above.
6. Click on "Send XML". This will send the XML in 'the box' to the Microting server, and display the response in the box on the right side "Message received from Microting".
- If this was a successful, as in the Microting server received, and queue the eForm for processing, there will be an eForm Id listed in the response. It's possible to dig out this Id by clicking "Get Id".
* ADVANCED - By clicking the "Verify XML" above the response. The software will check if it's possible for the software to read the XML, and convert into a fully working C# class, and back into XML.
* ADVANCED - By entering the Id of the wanted eForm in 'the box' (or using the "Get It") and clicking on "Check Id", you will be informed of the eFrom's status. Is it still only queued? Is it being processed? Or is it ready to be used?.  
7. Once the eForm has been processed it's possible to retrieve its data, by clicking on the "Fetch Id" and the wanted eForm Id in 'the box'. In the reply it's possible to see if the eForm has filled or not, and if filled all of the data.
8. Once an eForm is no longer wanted, use the "Delete Id" to mark the eForm for deletion.
* ADVANCED - Trace messages can be seen in the log, found in the same folder as the Tester program. Name should be DD-MM-YYYY_Log.txt (Ex. 30-12-2001_Log.txt).


ADVANCED - Notification Subscriber

1. Contact Microting at +45 66 11 10 66 and get the needed credentials
2. Receive a token and the notification server's address (referred to as 'token' and 'address' in the DLL)
3. Start the Tester program and fill in "Notification Subscriber" section with the information obtained from Developers backend: Token, Address.
4. Click on "Subscribe". This will open an asynchronous connection to notification server, which will log all messages back and forth in the log file. See details last line of eForm API. A box will pop-up when connection is made.
5. Click on "Unsubscribe". This will start the process of closing the connection. Once the connection is closed a pop-up box will appear.
  
  

Detailed looked and how to
=====================================
These are examples are intended as a help and suggestion on how to implement the eForm API and Notification Subscriber DLL. It's not necessary the best way for you to use them.
-Hint: By looking at the source code for eFormTester.exe, you can find more comments, and see one way to implement the most of the functions.



eForm API
=====================================

* Sending and receiving XML messages from and to Microting’s eForm API.
---------------------------------------------------------------
eFormCommunicator.Communicator is the class that handles the communication, and is the main part of eFormCommunicator.dll.
- Example:
Communicator communicator = new Communicator(apiId, serverToken, serverAddress);
- Hints:
using eFormCommunicator;



* Sending an eForm to Microting
---------------------------------------------------------------
Communicator.PostXml(string xmlString) handles sending the eForm to Microting, and returns the response.
- Example:
string response = communicator.PostXml(request);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Sending an eForm containing Entity_Select or Entity_Search to Microting (BETA version: Not ready server side)
---------------------------------------------------------------
Communicator.PostXmlExtended(string xmlString, string organizationId) handles sending the eForm to Microting, and returns the response, containing complex elements and is therefor also a bit slower than PostXml.
- Example:
string response = communicator.PostXmlExtended(request, organizationId);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Checking the status of an eForm
---------------------------------------------------------------
Communicator.CheckStatus(string eFormId) handles checking the status of an eForm, and returns the response.
- Example:
string response = communicator.CheckStatus(id);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Getting an eForm
---------------------------------------------------------------
Communicator.Retrieve(string eFormId) handles getting an eForm, and returns the eForm and any data it might contain.
- Example:
string response = communicator.Retrieve(id);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Getting an eForm element from an eFormResponseId its children only
---------------------------------------------------------------
Communicator.RetrieveFromId(string eFormId, int eFormResponseId) handles getting an eForm element and its children, and returns the eForm elements and any data they might contain.
- Example:
string response = communicator.RetrieveFromId(id, rId);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Deleting an eForm
---------------------------------------------------------------
Communicator.Delete(string eFormId) handles marking an eForm for deletion. Deleting might happened up to days later.
- Example:
string response = communicator.Delete(request);
- Hints:
Further details about the XML request and XML response structure can be found in the documentation of the Microting API.



* Creating XML eForm and C# eForm.
---------------------------------------------------------------
eFormRequest.MainElement is the class that handles converting XML eForm to C# eForm, converting C# eForm to XML eForm, and creating an C# eForm
- Example:
DateTime startDate = DateTime.Now;
DateTime endDate = DateTime.Now.AddYears(1);
MainElement sample = new MainElement("A check list", "Sample 1", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, new List<Element>());

DataElement e1 = new DataElement("My basic check list", "Basic list", 1, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
sample.ElementList.Add(e1);

e1.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", DataItemColors.Blue, 1, 0, 1000, 2, 0, ""));
e1.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", DataItemColors.Green, 8, "true", 100, false, false, true));
e1.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", DataItemColors.Blue, 3, "value", 10000, false));
e1.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", DataItemColors.Yellow, 4, 1, true));
e1.DataItemList.Add(new Check_Box("5", false, true, "Check box", "this is a description", DataItemColors.Purple, 15, true, true));
e1.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", DataItemColors.Red, 16, startDate, startDate, startDate.ToString()));
e1.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", DataItemColors.Yellow, 7));
- Hints:
using eFormRequest;
Is the core class of a C# eForm.
Further details about the XML request structure can be found in the documentation of the Microting API.



* Converting between XML eForm and C# eForm.
---------------------------------------------------------------
MainElement.XmlToClass(string xmlStr) handles converting XML eForm to C# eForm.
MainElement.ClassToXml() handles converting C# eForm to XML eForm.
- Example:
MainElement main = new MainElement();
main = main.XmlToClass(xmlStr);
string xmlStrResult = main.ClassToXml();
- Hints:
using eFormRequest;
The XML might change from been converted back and forth, but the resulting eForm should be the same.
Further details about the XML request structure can be found in the documentation of the Microting API.



* Converting between XML Microting response and C# Microting response.
---------------------------------------------------------------
Response.XmlToClass(string xmlStr) handles converting XML Microting response to C# Microting response.
Response.ClassToXml() handles converting C# Microting response to XML Microting response.
- Example:
Response resp = new Response();
resp = resp.XmlToClass(xmlStr);
string xmlStrResult = resp.ClassToXml();
- Hints:
using eFormResponse;
There should be no real use for ClassToXml();
The XML might change from been converted back and forth, but the resulting Microting response should be the same.
Further details about the XML response structure can be found in the documentation of the Microting API.



Notification Subscriber
=====================================

* Connecting, receiving notifications, and responding to Microting’s Notification Server.
---------------------------------------------------------------
eFormSubscriber.Subscriber is the class that handles the communication, and is the main part of eFormSubscriber.dll.
- Example:
subscriber = new Subscriber(notificationToken, notificationAddress);
- Hints:
using eFormSubscriber;



* Starting the Subscriber and subscribing to Microting’s Notification Server.
---------------------------------------------------------------
Subscriber.Start() handles the communication, keeps the connection alive.
subscriber.EventMsgServer handles the notifications from the Microting’s Notification server. This is an EVENT.
subscriber.EventMsgClient handles the notifications from the Subscriber. This is an EVENT.
- Example:
subscriber.EventMsgServer += SubscriberEventMsgServer;
subscriber.EventMsgClient += SubscriberEventMsgClient;
subscriber.Start();
- Hints:
Make sure you listen to the EVENTs before starting the Subscriber, or you might miss out on EVENTs.
Further details about the Notification message structure can be found in the documentation of the Microting API.



* Handling a Notification message.
---------------------------------------------------------------
When the Notification server sends a notification out, your code needs to process the notification, and then return a confirm message back before the Notification server will send the next message.
subscriber.ConfirmId(string notificationId) handles confirming a notification from Microting Notification server.
- Example:
private void SubscriberEventMsgServer(object sender, EventArgs args)
{
    AddToLog("Server # " + sender.ToString()); //Trace messages. For testing and tracking mainly. Can be removed.

    string reply = sender.ToString();
    if (reply.Contains("-update\",\"data") && reply.Contains("\"id\\\":"))
    {
        //something with the 'reply'
        //
        //
        //
        //done something with the 'reply'

        string nfId = Locate(reply, "\"id\\\":", ",");
        subscriber.ConfirmId(nfId);
    }
}
- Hints:
See source code eFormTester.exe for more details.
Further details about the Notification message structure can be found in the documentation of the Microting API.



* Closing the Subscriber and unsubscribing to Microting’s Notification Server.
---------------------------------------------------------------
Subscriber.Close(bool wait) handles closing down the communication. If wait is true, the method will not return until connection is closed. If false, the method will return instant and let your code continue, while Subscriber closes the connection. The waiting period can be up to very long. 
- Example:
subscriber.Close(false);
subscriber.EventMsgServer -= SubscriberEventMsgServer;
subscriber.EventMsgClient -= SubscriberEventMsgClient;
- Hints:
Be aware of from the moment you disconnect the EVENTs, you will no longer be informed. So it might not be a good idea to do so at all.
Further details about the Notification message structure can be found in the documentation of the Microting API.



* Checking if the Subscriber is still active*
---------------------------------------------------------------
Subscriber.IsActive() tells if the Subscriber is running. Not if it’s connected or not. Nor if it’s in the process of closing down.
If false, the Subscriber will no longer trigger EVENTs. If true, it might trigger EVENTs.