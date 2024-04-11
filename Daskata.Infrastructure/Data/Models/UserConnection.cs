using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Comment("Established relationship connection between users")]
public class UserConnection
{
    [Key]
    [Comment("Unique identifier for the relationship")]
    public Guid Id { get; set; }

    [Comment("Date when the relationship was established")]
    public DateTime EstablishedDate { get; set; }

    [Comment("The first user in the relationship")]
    public Guid FirstUserId { get; set; }

    [ForeignKey(nameof(FirstUserId))]
    [Comment("Reference to one user in the relationship")]
    public virtual UserProfile FirstUser { get; set; } = null!;

    [Comment("The second user in the relationship")]
    public Guid SecondUserId { get; set; }

    [ForeignKey(nameof(SecondUserId))]
    [Comment("Reference to the other user in the relationship")]
    public virtual UserProfile SecondUser { get; set; } = null!;
}