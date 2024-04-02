using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Helpers.Hashing;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IBaseResponse<Queue<Message>>> CreateMessageAsync(long userId, long chatId, string text)
        {
            try
            {
                if (text == null || text == string.Empty)
                {
                    return new BaseResponse<Queue<Message>>()
                    {
                        Description = "Повідомлення порожнє!",
                        StatusCode = OperationStatusCode.NoContent,
                    };
                }

                var chat = await _repository.ChatRepository.GetAll().FirstOrDefaultAsync(x => x.Id == chatId) 
                    ?? throw new Exception($"Чату з id = {chatId} не знайдено!");

                await _repository.MessageRepository.CreateAsync(new Message()
                {
                    ChatId = chatId,
                    SenderId = userId,
                    RecipientId = userId == chat.StudentId ? chat.AdminId : chat.StudentId,
                    Content = SecurityUtility.EncodeMessage(text),
                    TimeStamp = DateTime.Now
                });

                var mess = await _repository.MessageRepository.GetAll()
                    .Where(x => x.ChatId == chatId)
                    .OrderByDescending(x => x.TimeStamp)
                    .Take(2)
                    .ToListAsync();

                mess.ForEach(x => { x.Content = SecurityUtility.DecodeMessage(x.Content); });
                var messages = new Queue<Message>(mess);

                return new BaseResponse<Queue<Message>>()
                {
                    Description = "Повідомлення було успішно завантажено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = messages
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Queue<Message>>()
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
    }
}
