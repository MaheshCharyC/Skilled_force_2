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
            Chat chat = new Chat();
            chat.ToUser = GetUser(ToUserId);
            chat.FromUser = GetUser(HttpContext.Session.GetString("UserId"));
            chat.CreatedAt = DateTime.Now;
            chat.IsRead = false;
            skilledForceDB.Chat.Add(chat);
            skilledForceDB.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult GetChatList()
        {
            User user = GetUser(HttpContext.Session.GetString("UserId"));
            List<Chat> chats = skilledForceDB.Chat.Where(c => c.ToUser.Equals(user) || c.FromUser.Equals(user))
                .Include(c => c.ToUser).Include(c => c.FromUser).OrderByDescending(c => c.CreatedAt).ToList();
            if(chats.Count() > 0)
                ViewBag.messages = GetMessagesById(chats[0].ChatId).ToList();
            else
                ViewBag.messages = new List<Message>();
            return View(chats);
        }

        [HttpPost]
        public IActionResult SendMessage(string chatId, string userMessage, string ToUserId)
        {
            Message message = new Message();
            message.ChatId = chatId;  
            message.ToUser = GetUser(ToUserId);
            message.FromUser = GetUser(HttpContext.Session.GetString("UserId"));
            message.CreatedAt = DateTime.Now;
            message.UserMessage = userMessage;
           
            skilledForceDB.Message.Add(message);
            skilledForceDB.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult GetMessages(string chatId)
        {
            return View(GetMessagesById(chatId));
        }

        private List<Message> GetMessagesById(string chatId)
        {
            User user = GetUser(HttpContext.Session.GetString("UserId"));
            List<Message> messages = skilledForceDB.Message.Where(m => m.ChatId.Equals(chatId)).OrderByDescending(m => m.CreatedAt).ToList();
            if (messages !=null && messages.Count > 0 && messages[0].ToUser == user)
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
