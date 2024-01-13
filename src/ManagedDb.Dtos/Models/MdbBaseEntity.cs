using System.ComponentModel.DataAnnotations;

namespace ManagedDb.Dtos.Models;

public abstract class MdbBaseEntity
{
    [Key]
    public int Id { get; set; }
}