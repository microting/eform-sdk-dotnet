using System;

namespace eFormResponse
{
    [Serializable()]
    public class GeolocationData
    {
        internal GeolocationData()
        {
            
        }

        #region var
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public string Heading { get; set; }
        public string Accuracy { get; set; }
        public string Date { get; set; }
        #endregion
    }
}