using System.ComponentModel.DataAnnotations;

namespace ChrisUsher.MoveMate.API.Database.Properties
{
    public class CurrentPropertyTable
    {
        [Key]
        public Guid CurrentPropertyId { get; set; } = Guid.NewGuid();

        public Guid PropertyId { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }

        public double RemainingMortgage { get; set; }
    }
}