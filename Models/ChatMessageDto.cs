using System.ComponentModel.DataAnnotations;

namespace HataChatSerives.Models;

public class ChatMessageDto
{
    [Required]
    public int ReceiverId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Message { get; set; }
}