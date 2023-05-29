using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Database.Properties
{
    public class PropertyTable
    {
        [Key]
        public Guid PropertyId { get; set; } = Guid.NewGuid();
        
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }
        
        public double MinValue { get; set; }
        
        public double MaxValue { get; set; }

        public StampDutyResponse StampDuty { get; set; }
        
        public PropertyType PropertyType { get; set; }

        public Property ToProperty()
        {
            return new Property
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
        }
    }
}