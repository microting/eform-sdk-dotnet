using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Inspection
{
    public class Cases
    {
        public Cases (string finished_at, int site_id, int done_by_user_id, int created_by_user_id, string microting_check_uuid, int microting_uuid, string element_id)
        {
            this.ElementId = element_id;
            this.FinishedAt = finished_at;
            this.SiteId = site_id;
            this.DoneByUserId = done_by_user_id;
            this.CreatedByUserId = created_by_user_id;
            this.MicrotingCheckUuid = microting_check_uuid;
            this.MicrotingUuid = microting_uuid;
            this.DbFacade = DbFacade.Instance;
            
        }

        public Cases()
        {

        }
        public string ElementId { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public string FinishedAt { get; set; }
        public int SiteId { get; set; }
        public int DoneByUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public string MicrotingCheckUuid { get; set; }
        public int MicrotingUuid { get; set; }
        public int UnitId { get; set; }
        public DbFacade DbFacade { get; set; }

        public string toXml()
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter writer = XmlWriter.Create(memoryStream, xmlWriterSettings);

            Element element = this.DbFacade.findElementById(this.ElementId);
            element.mainToXml(writer);

            writer.Flush();
            writer.Close();

            string xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());

            return xmlString;
        }

    }
}
