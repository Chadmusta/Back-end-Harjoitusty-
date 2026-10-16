using Harjoitustyo.Models;
using Microsoft.EntityFrameworkCore;

namespace Harjoitustyo.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        MessageServiceContext _context;

        public MessageRepository(MessageServiceContext context)
        {
            _context = context;
        }

        public async Task<Message?> CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            Message? message = await _context.Messages.FindAsync(id);
            if(message == null) 
            {

                return false;
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Message?> GetMessageAsync(long id)
        {
            Message? message = await _context.Messages.FindAsync(id);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.Messages.Include(s => s.Sender)
                .Include(s => s.Receiver).Where(x => x.Receiver == null).OrderByDescending(x => x.SendTime).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMyRecievedMessages(User user)
        {
            return await _context.Messages.Include(s => s.Sender)
                .Include(s => s.Receiver).Where(x => x.Receiver == user).ToListAsync();
        }

        public async Task<bool> UpdateMessageAsync(Message message)
        {
            Message? dbMessage = await _context.Messages.FindAsync(message.Id);
            if(dbMessage == null) 
            {
                return false;
            }
            dbMessage.Title = message.Title;
            dbMessage.Body = message.Body;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO! mitä tehdään virhetapauksessa
                return false;
            }
            return true;
        }
    }
}
