using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HataChatSerives.Models;

public class ChatMessage
{
    public int Id { get; set; }

    public int SenderId { get; set; }
    [ForeignKey("SenderId")]
    public User Sender { get; set; }

    [Required]
    public int ReceiverId { get; set; }
    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; }

    [Required]
    [StringLength(1000)]
    public string Message { get; set; }

    public DateTime Timestamp { get; set; }
}