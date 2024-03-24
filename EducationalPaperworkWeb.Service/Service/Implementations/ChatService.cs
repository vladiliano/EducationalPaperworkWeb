using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Http;
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
                if (chat == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(chat)} == {chat}");

                chat.TimeStamp = DateTime.Now;

                await _repository.ChatRepository.CreateAsync(chat);

                var existingChat = _repository.ChatRepository.GetAll()
                    .OrderBy(x => x.TimeStamp)
                    .Last();

                if (existingChat == null) throw new Exception($"При спробі отримати останній доданий чат сталася помилка! {nameof(existingChat)} == {existingChat}");

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

        public IBaseResponse<long> GetUserId(HttpContext context)
        {
            try
            {
                if (!long.TryParse(context.User.FindFirst("UserId")?.Value, out long userId))
                    throw new Exception("Помилка при спробі отримати Id користувача!");

                return new BaseResponse<long>()
                {
                    Description = "Id користувача успішно отримано!",
                    StatusCode = OperationStatusCode.OK,
                    Data = userId
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>()
                {
                    Description = nameof(GetUserId) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Message>> CreateMessageAsync(long userId, string text, UserViewModel userState)
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

                var chatId = userState.SelectedChatId;
                var chat = userState.UserChats.FirstOrDefault(x => x.Chat.Id == chatId);

                await _repository.MessageRepository.CreateAsync(new Message()
                {
                    ChatId = chatId,
                    SenderId = userId,
                    RecipientId = userId == chat.Chat.StudentId ? chat.Chat.AdminId : chat.Chat.StudentId,
                    Content = text,
                    TimeStamp = DateTime.Now
                });

                var existingMessage = _repository.MessageRepository.GetAll()
                    .OrderBy(x => x.TimeStamp).Last();

                if (existingMessage == null) throw new Exception($"При спробі отримати останнє додане повідомлення сталася помилка! {nameof(existingMessage)} == {existingMessage}.");

                chat.Messages.Add(existingMessage);

                return new BaseResponse<Message>()
                {
                    Description = "Повідомлення було успішно завантажено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = existingMessage
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

                if (chats == null || chats.Count == 0)
				{
					return new BaseResponse<List<Chat>>()
					{
						StatusCode = OperationStatusCode.NoContent,
						Description = "Чатів не знайдено!",
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
    }
}
