using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;
using MongoDB.Bson;

namespace ChrisUsher.MoveMate.API.Services.Database.Properties
{
    public class PropertyTable
    {
#if RELEASE
        [Key]
#elif DEBUG
        public ObjectId _id { get; set; }
#endif
        public Guid PropertyId { get; set; } = Guid.NewGuid();

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public StampDutyResponse StampDuty { get; set; }

        public PropertyType PropertyType { get; set; }

        public Equity Equity { get; set; }

        public Property ToProperty()
        {
            var property = new Property
            {
                PropertyId = PropertyId,
                AccountId = AccountId,
                Name = Name,
                MinValue = MinValue,
                MaxValue = MaxValue,
                PropertyType = PropertyType,
                IsDeleted = IsDeleted,
                Created = Created
            };

            if (Equity != null)
            {
                property.Equity = Equity;
            }
            return property;
        }
    }
}