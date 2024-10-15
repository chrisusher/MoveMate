using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ChrisUsher.MoveMate.API.Services.Database;

public class MigrationTable
{
#if RELEASE
    [Key]
#elif DEBUG
    public ObjectId _id { get; set; }
#endif
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