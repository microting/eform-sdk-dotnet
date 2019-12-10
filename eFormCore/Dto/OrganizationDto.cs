namespace Microting.eForm.Dto
{
    public class OrganizationDto
    {
        #region con
        public OrganizationDto()
        {

        }

        public OrganizationDto(int id, string name, int customerNo, int unitLicenseNumber, string awsAccessKeyId, string awsSecretAccessKey, string awsEndPoint, string comAddress, string comAddressBasic, string comSpeechToText, string comAddressPdfUpload)
        {
            Id = id;
            Name = name;
            CustomerNo = customerNo;
            UnitLicenseNumber = unitLicenseNumber;
            AwsAccessKeyId = awsAccessKeyId;
            AwsSecretAccessKey = awsSecretAccessKey;
            AwsEndPoint = awsEndPoint;
            ComAddressApi = comAddress;
            ComAddressBasic = comAddressBasic;
            ComAddressPdfUpload = comAddressPdfUpload;
            ComSpeechToText = comSpeechToText;
        }
        #endregion

        #region var
        public int Id { get; }
        public string Name { get; }
        public int CustomerNo { get; }
        public int UnitLicenseNumber { get; }
        public string AwsAccessKeyId { get; }
        public string AwsSecretAccessKey { get; }
        public string AwsEndPoint { get; }
        public string ComAddressApi { get; }
        public string ComAddressBasic { get; }
        public string ComAddressPdfUpload { get; }
        public string ComSpeechToText { get; }
        #endregion

        public override string ToString()
        {
            return "OrganizationUid:" + Id + " / Name:" + Name + " / CustomerNo:" + CustomerNo + " / UnitLicenseNumber:" + UnitLicenseNumber
                   + " / AwsAccessKeyId:" + AwsAccessKeyId + " / AwsSecretAccessKey:" + AwsSecretAccessKey + " / AwsEndPoint:" + AwsEndPoint
                   + " / ComAddress:" + ComAddressApi + " / ComAddressBasic:" + ComAddressBasic + " / ComAddressPdfUpload:" + ComAddressPdfUpload + ".";
        }
    }
}