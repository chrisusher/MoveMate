using System.ComponentModel.DataAnnotations;

namespace ChrisUsher.MoveMate.API.Services.Database;

public class MigrationTable
{
    [Key]
    public Guid MigrationId { get; set; }

    [Required]
    public string MigrationName { get; set; }

    [Required]
    public string MigrationTypeName { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public DateTime MigrationDate { get; set; }
}