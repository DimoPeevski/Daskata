using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace
{
    [Comment("Connection request between users")]
    public class ConnectionRequest
    {
        [Key]
        [Comment("Unique identifier for the user connection request")]
        public Guid Id { get; set; }

        [Comment("Status of the connection request ('Pending', 'Accepted', 'Rejected', 'Blocked')")]
        public StatusCategory RequestStatus { get; set; }

        [Comment("User ID who sent the connection request")]
        public Guid FromUserId { get; set; }

        [ForeignKey(nameof(FromUserId))]
        [Comment("Reference to the user who sent the connection request")]
        public virtual UserProfile FromUser { get; set; } = null!;

        [Comment("User ID who received the connection request")]
        public Guid ToUserId { get; set; }

        [ForeignKey(nameof(ToUserId))]
        [Comment("Reference to the user who received the friend request")]
        public virtual UserProfile ToUser { get; set; } = null!;
    }
}