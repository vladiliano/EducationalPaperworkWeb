using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Interfaces;

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

                if (existingChat == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(existingChat)} == {existingChat}");

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
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long chatId)
        {
            try
            {
				//var messages = await _repository.MessageRepository.GetAll()
				//    .Where(x => x.ChatId == chatId)
				//    .OrderByDescending(x => x.TimeStamp)
				//    .ToListAsync();

				List<Message> messages = new List<Message>();

				if (chatId == 1)
				messages = new List<Message>
			{
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 14, 14, 30, 0),
					SenderId = 1,
					ChatId = 1,
					Id = 1,
					RecipientId = 2,
					Content = "Привет бро"
				},
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 15, 14, 45, 15),
					SenderId = 2,
					ChatId = 1,
					Id = 2,
					RecipientId = 1,
					Content = "кукуку"
				},
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 16, 14, 45, 15),
					SenderId = 2,
					ChatId = 1,
					Id = 2,
					RecipientId = 1,
					Content = "хай, как ты?"
				},
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 16, 16, 20, 0),
					SenderId = 1,
					ChatId = 1,
					Id = 3,
					RecipientId = 2,
					Content = "Привет! Как прошел твой день?"
				},
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 17, 10, 15, 45),
					SenderId = 2,
					ChatId = 1,
					Id = 4,
					RecipientId = 1,
					Content = "День прошел хорошо, спасибо. У тебя как?"
				},
				new Message
				{
					TimeStamp = new DateTime(2024, 3, 17, 14, 10, 20),
					SenderId = 1,
					ChatId = 1,
					Id = 5,
					RecipientId = 2,
					Content = "У меня тоже все отлично! Планируем встретиться завтра?"
				}
			};

				if (messages == null || messages.Count == 0)
                {
                    return new BaseResponse<List<Message>>()
                    {
                        StatusCode = OperationStatusCode.NotFound,
                        Description = "Повідомлень в чаті не знайдено!",
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
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId)
        {
			try
			{
				//var chats = await _repository.ChatRepository.GetAll()
				//	.Where(x => x.StudentId == userId || x.AdminId == userId)
				//	.OrderByDescending(x => x.TimeStamp)
				//	.ToListAsync();

				var chats = new List<Chat>
			{
				new Chat
				{
					Name = "Отримання документу",
					StudentId = 1,
					AdminId = 2,
					Id = 1,
					TimeStamp = new DateTime(2024, 3, 14, 14, 30, 0)
				},
				new Chat
				{
					Name = "Документ ЦНАП",
					StudentId = 1,
					AdminId = 3,
					Id = 2,
					TimeStamp = new DateTime(2024, 3, 15, 17, 42, 0)
				},
				new Chat
				{
					Name = "Довідка",
					StudentId = 1,
					AdminId = 21,
					Id = 3,
					TimeStamp = new DateTime(2024, 3, 10, 9, 13, 0)
				}
			};

				if (chats == null || chats.Count == 0)
				{
					return new BaseResponse<List<Chat>>()
					{
						StatusCode = OperationStatusCode.NotFound,
						Description = "Повідомлень в чаті не знайдено!",
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
					Description = ex.Message,
					StatusCode = OperationStatusCode.InternalServerError,
				};
			}
		}

        public async Task<IBaseResponse<Message>> SendMessageAsync(Message message)
        {
            try
            {
                if (message == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(message)} == {message}");

                message.TimeStamp = DateTime.Now;

                await _repository.MessageRepository.CreateAsync(message);

                var existingMessage = _repository.MessageRepository.GetAll()
                    .OrderBy(x => x.TimeStamp).Last();

                if (existingMessage == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(existingMessage)} == {existingMessage}");

                return new BaseResponse<Message>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = existingMessage
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Message>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }
    }
}
