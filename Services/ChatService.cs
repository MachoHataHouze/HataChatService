using System.Security.Claims;
using HataChatSerives.Data;
using HataChatSerives.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HataChatSerives.Services;

public class ChatService
{
    private readonly ApplicationDbContext _context;

    public ChatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessage> SendMessageAsync(ChatMessage message)
    {
        var sender = await _context.Users.FindAsync(message.SenderId);
        var receiver = await _context.Users.FindAsync(message.ReceiverId);

        if (sender == null || receiver == null)
        {
            throw new Exception("Invalid sender or receiver ID.");
        }

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<ChatMessage>> GetMessagesAsync(int currentUserId, int otherUserId)
    {
        return await _context.ChatMessages
            .Where(m => (m.ReceiverId == currentUserId && m.SenderId == otherUserId) ||
                        (m.ReceiverId == otherUserId && m.SenderId == currentUserId))
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToListAsync();
    }

    public async Task<List<object>> GetUserChatsAsync(int userId)
    {
        var chats = await _context.ChatMessages
            .Where(c => c.SenderId == userId || c.ReceiverId == userId)
            .GroupBy(c => c.SenderId == userId ? c.ReceiverId : c.SenderId)
            .Select(g => new
            {
                UserId = g.Key,
                OtherUserName = g.First().SenderId == userId ? g.First().Receiver.FirstName + " " + g.First().Receiver.LastName : g.First().Sender.FirstName + " " + g.First().Sender.LastName
            })
            .ToListAsync();

        return chats.Cast<object>().ToList();
    }
}