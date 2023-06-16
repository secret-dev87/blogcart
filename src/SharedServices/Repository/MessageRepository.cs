﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedServices.Data;
using SharedServices.Models;
using SharedServices.Repository.IRepository;

namespace SharedServices.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;


        public MessageRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDTO>> GetAllByConversationId(int conversationId)
        {
            try
            {
                var favMessages = await _db.Messages
                    .Where(m => m.ConversationId == conversationId && m.IsFav)
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();

                var nonFavMessages = await _db.Messages
                    .Where(m => m.ConversationId == conversationId && !m.IsFav)
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();

                var messages = favMessages.Concat(nonFavMessages).OrderBy(m => m.Timestamp);

                return _mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MessageRepository.GetAllByConversationId: {ex.Message}");
                return null;
            }
        }

        //public async Task<IEnumerable<object>> GetMessagesForApiRequest(int conversationId)
        //{
        //    var conversation = await _db.Conversations
        //        .Include(c => c.Messages)
        //        .FirstOrDefaultAsync(c => c.Id == conversationId);

        //    if (conversation == null)
        //    {
        //        return Enumerable.Empty<object>();
        //    }

        //    var favMessages = conversation.Messages
        //        .Where(message => message.IsFav)
        //        .OrderByDescending(message => message.Timestamp)
        //        .Take(6)
        //        .Select(message => new
        //        {
        //            role = message.IsUserMessage ? "user" : "assistant",
        //            content = message.Content
        //        });

        //    var recentNonFavMessages = conversation.Messages
        //        .Where(message => !message.IsFav)
        //        .OrderByDescending(message => message.Timestamp)
        //        .Take(6)
        //        .Select(message => new
        //        {
        //            role = message.IsUserMessage ? "user" : "assistant",
        //            content = message.Content
        //        });

        //    return favMessages.Concat(recentNonFavMessages);
        //}


        public async Task<MessageDTO> Create(MessageDTO objDTO)
        {
            try
            {
                // Delete messages with the default response and their prompts

                var obj = _mapper.Map<MessageDTO, Message>(objDTO);
                obj.Timestamp = DateTime.UtcNow;
                obj.IsUserMessage = objDTO.IsUserMessage;

                var addedObj = await _db.Messages.AddAsync(obj);
                await _db.SaveChangesAsync();

                var conversation = await _db.Conversations
                    .Include(c => c.Client)
                    .FirstOrDefaultAsync(c => c.Id == obj.ConversationId);

                var clientId = conversation?.ClientId ?? 0;

                return _mapper.Map<Message, MessageDTO>(addedObj.Entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MessageRepository.Create: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var obj = await _db.Messages.FirstOrDefaultAsync(m => m.Id == id);
                if (obj != null)
                {
                    _db.Messages.Remove(obj);
                    return await _db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MessageRepository.Delete: {ex.Message}");
                return -1;
            }
        }

        public async Task<bool> ToggleFavorite(int id)
        {
            try
            {
                var message = await _db.Messages.FirstOrDefaultAsync(m => m.Id == id);
                if (message != null)
                {
                    message.IsFav = !message.IsFav;
                    _db.Messages.Update(message);

                    var followingMessage = await _db.Messages
                        .FirstOrDefaultAsync(m => m.ConversationId == message.ConversationId && m.Id == id + 1);

                    if (followingMessage != null)
                    {
                        followingMessage.IsFav = !followingMessage.IsFav;
                        _db.Messages.Update(followingMessage);
                    }

                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MessageRepository.ToggleFavorite: {ex.Message}");
                return false;
            }
        }

        public async Task<MessageDTO> Update(MessageDTO objDTO)
        {
            try
            {
                var objFromDb = await _db.Messages.FirstOrDefaultAsync(m => m.Id == objDTO.Id);
                if (objFromDb != null)
                {
                    objFromDb.Content = objDTO.Content;
                    objFromDb.Timestamp = objDTO.Timestamp;
                    objFromDb.IsUserMessage = objDTO.IsUserMessage;
                    objFromDb.ConversationId = objDTO.ConversationId;
                    objFromDb.IsFav = objDTO.IsFav;

                    _db.Messages.Update(objFromDb);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<Message, MessageDTO>(objFromDb);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MessageRepository.Update: {ex.Message}");
                return null;
            }
        }
    }
}