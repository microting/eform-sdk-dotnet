using Amazon;
using Amazon.SQS;
using eForm.Messages;
using eFormShared;
using eFormSqlController;
using Newtonsoft.Json.Linq;
using Rebus.Bus;
using System;
using System.Threading;

namespace eFormCore.Services
{

    public class AmazonEventIngestionService : IIngestEvents
    {
        private readonly SqlController sqlController;
        private readonly IBus bus;
        private AmazonSQSClient sqsClient;
        private Timer timer;
        private string awsQueueUrl;

        public AmazonEventIngestionService(SqlController sqlController, IBus bus)
        {
            this.sqlController = sqlController;
            this.bus = bus;
        }

        public void StartProcessEvents()
        {
            InitializeSqsClient();

            awsQueueUrl = sqlController.SettingRead(Settings.awsEndPoint) + sqlController.SettingRead(Settings.token);

            StartTimer();
        }

        private void InitializeSqsClient()
        {
            string awsAccessKeyId = sqlController.SettingRead(Settings.awsAccessKeyId);
            string awsSecretAccessKey = sqlController.SettingRead(Settings.awsSecretAccessKey);
           
            sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
        }

        private void StartTimer()
        {
            Console.WriteLine("Start fetching");
            timer = new Timer(g => 
            {
                Console.WriteLine("Fetch");
                //FetchMessages();
                timer.Change(5000, Timeout.Infinite);
            }, null, 0, Timeout.Infinite);
        }

        private void FetchMessages()
        {
            var res = sqsClient.ReceiveMessage(awsQueueUrl);

            if (res.Messages.Count > 0)
                foreach (var message in res.Messages)
                {
                    var parsedData = JRaw.Parse(message.Body);
                    string notificationUId = parsedData["id"].ToString();
                    string microtingUId = parsedData["microting_uuid"].ToString();
                    string action = parsedData["text"].ToString();
                    
                    switch (action)
                    {
                        case Constants.Notifications.RetrievedForm:
                            bus.SendLocal(new EformRetrieved(notificationUId, microtingUId)).Wait();
                            break;
                        case Constants.Notifications.Completed:
                            bus.SendLocal(new EformCompleted(notificationUId, microtingUId)).Wait();
                            break;
                    }

                    sqsClient.DeleteMessage(awsQueueUrl, message.ReceiptHandle);
                }
        }

        public void StopProcessingEvents()
        {
            if (timer == null) return;
            Console.WriteLine("Stopping fetching");
            timer = null;
        }
    }
}
