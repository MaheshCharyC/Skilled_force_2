using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models.DB;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Skilled_Force_VS_22.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public ChatController(ILogger<HomeController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }

        [HttpPost]
        public IActionResult CreateChat(string ToUserId)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            User user = GetUser(userId);

            Chat? chat = skilledForceDB.Chat.Where(c => c.FromUserId == userId && c.ToUserId == ToUserId ||
            c.FromUserId == ToUserId && c.ToUserId == userId).FirstOrDefault();
            if (chat == null)
            {
                chat = new Chat();
                chat.ToUser = GetUser(ToUserId);
                chat.FromUser = user;
                chat.IsRead = false;
                chat.UpdatedTime = DateTime.Now;
                skilledForceDB.Chat.Add(chat);
            }
            else {
                chat.UpdatedTime = DateTime.Now;
                skilledForceDB.Chat.Update(chat);
            }
            
            skilledForceDB.SaveChanges();
            List<Chat> chats = getUserChats(user);

            return View("GetChatList", chats);

        }

        [HttpGet]
        public IActionResult GetChatList()
        {
            User user = GetUser(HttpContext.Session.GetString("UserId"));
            List<Chat> chats = getUserChats(user);
            return View("GetChatList", chats);
        }

        private List<Chat> getUserChats(User user)
        {
            List<Chat> chats = skilledForceDB.Chat.Where(c => c.ToUser.Equals(user) || c.FromUser.Equals(user))
                            .Include(c => c.ToUser).Include(c => c.FromUser).OrderByDescending(c => c.UpdatedTime).ToList();
            if (chats.Count() > 0)
                ViewBag.messages = GetMessagesById(chats[0].ChatId).ToList();
            else
                ViewBag.messages = new List<Message>();
            return chats;
        }

        [HttpPost]
        public IActionResult SendMessage(string chatId, string userMessage)
        {
            User user = GetUser(HttpContext.Session.GetString("UserId"));
            
            Message message = new Message();
            message.ChatId = chatId;
            message.Time = DateTime.Now;
            message.UserMessage = userMessage??"";
            message.FromUser = user;
            Chat chat = skilledForceDB.Chat.Where(c=>c.ChatId== chatId).First();
            chat.UpdatedTime = DateTime.Now;

            skilledForceDB.Message.Add(message);
            skilledForceDB.SaveChanges();

            List<Chat> chats = getUserChats(user);
            return View("GetChatList", chats);
        }

        [HttpGet]
        public IActionResult GetMessages(string chatId)
        {
            return View(GetMessagesById(chatId));
        }

        private List<Message> GetMessagesById(string chatId)
        {
            User user = GetUser(HttpContext.Session.GetString("UserId"));
            List<Message> messages = skilledForceDB.Message.Where(m => m.ChatId.Equals(chatId)).OrderBy(m => m.Time).ToList();
            if (messages != null && messages.Count > 0 )
            {
                Chat chat = skilledForceDB.Chat.Where(c => c.ChatId.Equals(chatId)).FirstOrDefault();
                chat.IsRead = true;
                skilledForceDB.Chat.Update(chat);
                skilledForceDB.SaveChanges();
            }
            return messages;
        }

        private User GetUser(string userId)
        {
            return skilledForceDB.User.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
        }

    }
}
