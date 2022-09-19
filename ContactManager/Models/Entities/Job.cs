using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Models.Entities;

public class Job
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint JobId { get; set; }
    
    [Required]
    public string JobTitle { get; set; }
}