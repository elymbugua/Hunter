using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.UI.Models
{
    public class LatestDateEntity
    {
        public DateTime LatestDate { get; set; }
        public ObjectId _id { get; set; }
    }
}
