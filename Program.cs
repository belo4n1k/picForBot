using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGBot
{
   class Program 
   {
      private static string m_Token { get; set; } = "5235001097:AAFUQV7s4xZ7Cw7E3E7CoahrQL2hXlhIn50";
      private static TelegramBotClient m_Client; 
      
      
      static async Task Main(string[] args)
      {
         m_Client = new TelegramBotClient(m_Token);
         using var cts = new CancellationTokenSource();
         var receiverOptions = new ReceiverOptions
         {
            AllowedUpdates = { } 
         };
         m_Client.StartReceiving(
            HandleUpdateAsync, 
            HandleErrorAsync, 
            receiverOptions, 
            cancellationToken: cts.Token);

         var me = await m_Client.GetMeAsync();
         Console.WriteLine($"Start listening for @{me.Username}");
         Console.ReadLine();
         cts.Cancel();
      } 
      static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
      {
         if (update.Type != UpdateType.Message)
            return;
         if (update.Message!.Type != MessageType.Text)
            return;

         var chatId = update.Message.Chat.Id;
         var messageText = update.Message.Text;

         Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

         
         messageText = messageText.ToLower();
         

         if (messageText.Contains("привет"))
         {
            string text = "ну здарова";
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text ,
               cancellationToken: cancellationToken);
         }

         else if (messageText.Contains("как дела"))
         {
            string text = "я норм, сам как?";
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text ,
               cancellationToken: cancellationToken);
         }

         else if (messageText.Contains("хорош") || messageText.Contains("норм"))
         {
            string text = "вот и ладненько";
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text ,
               cancellationToken: cancellationToken);
         }
         else if (messageText.Contains("гуся"))
         {
            Random rnd = new Random();
            int num = rnd.Next(1, 4);
            Console.WriteLine();
            Message message = await botClient.SendPhotoAsync(
               chatId: chatId,
               photo:$"https://github.com/belo4n1k/picForBot/blob/main/pics/{num}.jpg?raw=true",
               caption: "<b>Вот</b>.",
               parseMode: ParseMode.Html,
               cancellationToken: cancellationToken);
         }
         else
         {
            string text = "га!";
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: text ,
               cancellationToken: cancellationToken);
         }
      }

      static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
      {
         var ErrorMessage = exception switch
         {
            ApiRequestException apiRequestException
               => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
         };

         Console.WriteLine(ErrorMessage);
         return Task.CompletedTask;
      }
      
   }
}