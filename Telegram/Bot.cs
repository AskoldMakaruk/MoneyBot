﻿using System;
using System.Linq;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using StickerMemeBot.Telegram.Commands;
using StickerMemeBot.Telegram.Queries;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;

namespace MoneyBot.Telegram
{
    public class Bot : TelegramBotClient
    {

        public Bot(string token) : base(token)
        {
            OnMessage += OnMessageRecieved;
            StartReceiving();
        }

        public void OnMessageRecieved(object sender, MessageEventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.Message.From.Username + ": " + e.Message.Text);
            try
            {
                var contoller = new TelegramController();
                contoller.Start();

                var account = contoller.FromMessage(e.Message);

                var baseType = typeof(Command);
                var assembly = baseType.Assembly;
                var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c, e.Message, this, account) as Command).OrderByDescending(c => c.Suitability()).First();
                command.Controller = contoller;
                Console.WriteLine($"Command type: {command.ToString()}, account status: {account.Status.ToString()}");
                command.Execute();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        public void OnQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.CallbackQuery.From.Username + ": " + e.CallbackQuery.Data);
                var contoller = new TelegramController();
                contoller.Start();

                var account = contoller.FromMessage(e.CallbackQuery.Message.Chat);
                if (account == null)
                {
                    AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Your account doesn't exist.");
                    return;
                }

                var baseType = typeof(Query);
                var assembly = baseType.Assembly;

                var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c, e.CallbackQuery, this, account) as Query).First(c => c.IsSuitable());

                command.Controller = contoller;
                command.Execute();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        public async void SendMessage(long chatId, object mes)
        {
            await SendTextMessageAsync(chatId, mes.ToString());
        }
        public async void SendMessage(Account account, object mes)
        {
            await SendTextMessageAsync(account.ChatId, mes.ToString());
        }

    }
}