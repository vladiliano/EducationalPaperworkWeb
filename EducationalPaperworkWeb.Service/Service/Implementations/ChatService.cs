using EducationalPaperworkWeb.Domain.Domain.Enums.Chat;
using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Helpers.Hashing;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _repository;

        public ChatService(IUnitOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat)
        {
            try
            {
                if (chat == null) throw new Exception("Не було надано об'єкт типу Chat.");

                chat.TimeStamp = DateTime.Now;
                chat.Name = SecurityUtility.EncodeMessage(chat.Name);

                await _repository.ChatRepository.CreateAsync(chat);

                var existingChat = _repository.ChatRepository.GetAll()
                    .OrderBy(x => x.TimeStamp)
                    .Last();

                existingChat.Name = SecurityUtility.DecodeMessage(existingChat.Name);

                if (existingChat == null) throw new Exception("При спробі отримати останній доданий чат сталася помилка!");

                return new BaseResponse<Chat>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = existingChat
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Chat>()
                {
                    Description = nameof(CreateChatAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Message>> CreateMessageAsync(long userId, long chatId, string text, MessageContentType messageType)
        {
            try
            {
                if (text == null || text == string.Empty)
                {
                    return new BaseResponse<Message>()
                    {
                        Description = "Повідомлення порожнє!",
                        StatusCode = OperationStatusCode.NoContent,
                    };
                }

                var chat = await _repository.ChatRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == chatId && x.State == ChatState.InProcessing) 
                    ?? throw new Exception($"Чату з id = {chatId} не знайдено!");

                if (chat == null)
                {
                    return new BaseResponse<Message>()
                    {
                        Description = "Не було отриманно дані чату так як він знаходиться не в обробці!",
                        StatusCode = OperationStatusCode.OK,
                    };
                }

                await _repository.MessageRepository.CreateAsync(new Message()
                {
                    ChatId = chatId,
                    SenderId = userId,
                    RecipientId = userId == chat.StudentId ? chat.AdminId : chat.StudentId,
                    Type = messageType,
                    Content = SecurityUtility.EncodeMessage(text),
                    TimeStamp = DateTime.Now
                });

                var message = await _repository.MessageRepository.GetAll()
                    .Where(x => x.ChatId == chatId)
                    .OrderByDescending(x => x.TimeStamp)
                    .FirstOrDefaultAsync();

                message.Content = SecurityUtility.DecodeMessage(message.Content);

                return new BaseResponse<Message>()
                {
                    Description = "Повідомлення було успішно завантажено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Message>()
                {
                    Description = nameof(CreateMessageAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long chatId)
        {
            try
            {
                var messages = await _repository.MessageRepository.GetAll()
                    .Where(x => x.ChatId == chatId)
                    .OrderBy(x => x.TimeStamp)
                    .ToListAsync();

                messages.ForEach(x => { x.Content = SecurityUtility.DecodeMessage(x.Content); });

                if (messages == null || messages.Count == 0)
                {
                    return new BaseResponse<List<Message>>()
                    {
                        StatusCode = OperationStatusCode.NoContent,
                        Description = "Повідомлень в чаті не знайдено!",
                        Data = new List<Message>(0)
                    };
                }

                return new BaseResponse<List<Message>>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = messages
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Message>>()
                {
                    Description = nameof(GetChatMessagesAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId)
        {
			try
			{
                var chats = await _repository.ChatRepository.GetAll()
                    .Where(x => x.StudentId == userId || x.AdminId == userId)
                    .OrderByDescending(x => x.TimeStamp)
                    .ToListAsync();

                chats.ForEach(x => { x.Name = SecurityUtility.DecodeMessage(x.Name); });

                if (chats == null || chats.Count == 0)
				{
                    return new BaseResponse<List<Chat>>()
                    {
                        StatusCode = OperationStatusCode.NoContent,
                        Description = "Чатів не знайдено!",
                        Data = new List<Chat>()
					};
				}

				return new BaseResponse<List<Chat>>()
				{
					Description = "Чат був успішно завантажений!",
					StatusCode = OperationStatusCode.OK,
					Data = chats
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Chat>>()
				{
                    Description = nameof(GetUserChatsAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
				};
			}
		}

        public async Task<IBaseResponse<User>> GetCompanionAsync(long userId, long chatId)
        {
            try
            {
                var chat = await _repository.ChatRepository.GetAll().FirstOrDefaultAsync(x => x.Id == chatId) 
                    ?? throw new Exception($"Чату з id = {chatId} не знайдено!");

                var companionId = chat.StudentId == userId? chat.AdminId : chat.StudentId;

                var companion = await _repository.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Id == companionId) 
                    ?? throw new Exception($"Співбесдника з id = {companionId} не знайдено!");

                return new BaseResponse<User>()
                {
                    Description = "Користувача знайдено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = companion
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = nameof(GetCompanionAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<Message> GetPreviousMessage(long chatId, Message message)
        {
            try
            {
                var previousMessage = _repository.MessageRepository.GetAll()
                    .Where(x => x.ChatId == chatId && x.TimeStamp < message.TimeStamp)
                    .OrderByDescending(x => x.TimeStamp)
                    .FirstOrDefault();

                if (previousMessage == null)
                {
                    return new BaseResponse<Message>()
                    {
                        Description = "Повідомлення не знайдено!",
                        StatusCode = OperationStatusCode.NoContent
                    };
                }

                previousMessage.Content = SecurityUtility.DecodeMessage(previousMessage.Content);

                return new BaseResponse<Message>()
                {
                    Description = "Повідомлення успішно знайдено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = previousMessage
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Message>()
                {
                    Description = nameof(CreateMessageAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Chat>>> GetUnselectedChatsAsync()
        {
            try
            {
                var chats = await _repository.ChatRepository.GetAll()
                    .Where(x => x.State == ChatState.WaitingForResponse).ToListAsync();

                chats.ForEach(x => { x.Name = SecurityUtility.DecodeMessage(x.Name); });

                if (chats == null || chats.Count == 0)
                {
                    return new BaseResponse<List<Chat>>()
                    {
                        StatusCode = OperationStatusCode.NoContent,
                        Description = "Чатів не знайдено!",
                        Data = new List<Chat>()
                    };
                }

                return new BaseResponse<List<Chat>>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = chats
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Chat>>()
                {
                    Description = nameof(GetUserChatsAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Chat>> AcceptRequestAsync(long chatId, long adminId)
        {
            try
            {
                var chat = await _repository.ChatRepository.GetAll()
                    .Where(x => x.State == ChatState.WaitingForResponse && x.Id == chatId)
                    .FirstOrDefaultAsync();

                if (chat == null)
                {
                    return new BaseResponse<Chat>()
                    {
                        StatusCode = OperationStatusCode.NoContent,
                        Description = "Схоже виникла помилка! Цей запит вже в обробці!",
                    };
                }

                chat.State = ChatState.InProcessing;
                chat.AdminId = adminId;

                await _repository.ChatRepository.UpdateAsync(chat);

                chat.Name = SecurityUtility.DecodeMessage(chat.Name);

                return new BaseResponse<Chat>()
                {
                    Description = "Запит був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = chat
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Chat>()
                {
                    Description = nameof(GetUserChatsAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Chat>> CloseRequestAsync(long chatId)
        {
            try
            {
                var chat = await _repository.ChatRepository.GetAll()
                    .Where(x => x.State == ChatState.InProcessing && x.Id == chatId)
                    .FirstOrDefaultAsync();

                if (chat == null)
                {
                    return new BaseResponse<Chat>()
                    {
                        StatusCode = OperationStatusCode.NoContent,
                        Description = "Схоже виникла помилка! Цей запит не в обробці!",
                    };
                }

                chat.State = ChatState.Closed;

                await _repository.ChatRepository.UpdateAsync(chat);

                chat.Name = SecurityUtility.DecodeMessage(chat.Name);

                return new BaseResponse<Chat>()
                {
                    Description = "Статус запиту був успішно змінений на \"Розглянуто\"!",
                    StatusCode = OperationStatusCode.OK,
                    Data = chat
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Chat>()
                {
                    Description = nameof(CloseRequestAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }
    }
}
