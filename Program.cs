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

         Message sentMessage;
         switch (messageText)
         {
            case "привет" :
               MessagesCase("ну здарова", botClient, update, cancellationToken);
               break;
            case "как дела?" :  
               MessagesCase("я норм, ты как?", botClient, update, cancellationToken);
               break;
            case "га" :  
               MessagesPhoto("не дразни гуся", botClient, update, cancellationToken);
               break;
            case "покажи гуся" :
               Random rnd = new Random();
               int num = rnd.Next(1, 4);
               Message message = await botClient.SendPhotoAsync(
                  chatId: chatId,
                  photo:$"https://github.com/belo4n1k/picForBot/blob/main/pics/{num}.jpg?raw=true",
                  caption: "<b>Вот</b>.",
                  parseMode: ParseMode.Html,
                  cancellationToken: cancellationToken);
               break;
            default:
               MessagesCase("га!", botClient, update, cancellationToken);
               break;
         
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

      private static async Task MessagesCase(string text, ITelegramBotClient botClient , Update  chatId, CancellationToken cancellationToken)
      {
         Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId.Message.Chat.Id,
            text: text ,
            cancellationToken: cancellationToken);
      }

      private static async Task MessagesPhoto(string text, ITelegramBotClient botClient, Update chatId, CancellationToken cancellationToken)
      {
         Message message = await botClient.SendPhotoAsync(
            chatId: chatId.Message.Chat.Id,
            photo:$"https://github.com/belo4n1k/picForBot/blob/main/pics/ga.jpg?raw=true",
            caption: text,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
      }
   }
}