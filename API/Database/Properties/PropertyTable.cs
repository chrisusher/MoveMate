using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;

namespace ChrisUsher.MoveMate.API.Database.Properties
{
    public class PropertyTable
    {
        [Key]
        public Guid PropertyId { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }

        public bool CurrentProperty { get; set; }

        public Guid AccountId { get; set; }

        public StampDutyResponse StampDuty { get; set; }
    }
}