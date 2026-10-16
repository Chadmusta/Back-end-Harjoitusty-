using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harjoitustyo.Models
{
    public class Message
    {
        public long Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; } = string.Empty;
        public string? Body { get; set; } = string.Empty;
        public User Sender { get; set; }
        public User? Receiver { get; set; }
        public Message? PrevMessage { get; set; }

        public DateTime? SendTime { get; set; }

    }

    public class MessageDTO// Verkon yli lähetettävä viesti
    {
        public long Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string SenderID { get; set; }
        public string? ReceiverID { get; set; }
        public long? PrevMessageID { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
