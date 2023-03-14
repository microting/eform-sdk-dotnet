/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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
*/

namespace Microting.eForm.Dto
{
    public class OrganizationDto
    {
        #region con

        public OrganizationDto()
        {
        }

        public OrganizationDto(int id, string name, int customerNo, int unitLicenseNumber, string awsAccessKeyId,
            string awsSecretAccessKey, string awsEndPoint, string comAddress, string comAddressBasic,
            string comSpeechToText, string comAddressPdfUpload)
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
        public string S3Key { get; set; }
        public string S3Id { get; set; }
        public string S3Endpoint { get; set; }

        #endregion

        public override string ToString()
        {
            return "OrganizationUid:" + Id + " / Name:" + Name + " / CustomerNo:" + CustomerNo +
                   " / UnitLicenseNumber:" + UnitLicenseNumber
                   + " / AwsAccessKeyId:" + AwsAccessKeyId + " / AwsSecretAccessKey:" + AwsSecretAccessKey +
                   " / AwsEndPoint:" + AwsEndPoint
                   + " / ComAddress:" + ComAddressApi + " / ComAddressBasic:" + ComAddressBasic +
                   " / ComAddressPdfUpload:" + ComAddressPdfUpload + ".";
        }
    }
}