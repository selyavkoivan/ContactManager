using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using ContactManager.Models.Values;

namespace ContactManager.Models.Entities;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint ContactId { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public Job Position { get; set; }

    [Required]
    public string Phone { get; set; }
    
    [Required]
    public DateTime Birthday { get; set; }
    
    public static implicit operator Contact(JsonElement json)
    {
        return new Contact
        {
            Name = json.GetProperty(DataBaseComponentName.NameField).ToString(),
            Phone = json.GetProperty(DataBaseComponentName.PhoneField).ToString(),
            Birthday = DateTime.Parse(json.GetProperty(DataBaseComponentName.BirthDateField).ToString()!),
            Position = new Job{JobTitle = json.GetProperty(DataBaseComponentName.JobTitleForeignKeyField).ToString()}
        };
    }

    public void Update(Contact contact)
    {
        Name = contact.Name;
        Phone = contact.Phone;
        Birthday = contact.Birthday;
        Position = contact.Position;
    }
}

