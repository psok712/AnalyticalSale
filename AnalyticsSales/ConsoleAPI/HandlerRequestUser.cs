using System;
using System.Globalization;
using AnalysisSales.Interfaces;
using AnalysisSales.Models;
using AnalysisSales.Views;
using DataAccess;
using DataAccess.Exceptions;
using Domain.Interfaces;

namespace AnalysisSales;

public class HandlerRequestUser : IHandlerRequestUser
{
    public void RunApplication()
    {
        do
        {
            try
            {
                InputRequest();
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DayOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine("An unknown error has occurred.");
            }

            Message message = new Message("Do you want to continue writing queries? " +
                                          "If yes, then press any other button, otherwise spacebar.");
            DisplayMessage.OnConsole(message);
        } while (Console.ReadKey(true).Key != ConsoleKey.Spacebar);
    }
    
    private void InputRequest()
    {
        IAnalysisSalesService service = new AnalysisSalesService();
        
        Message message = new Message("Enter your request: ");
        DisplayMessage.OnConsole(message);
        RequestUser requestUser = CreateUser(Console.ReadLine()!);
        DisplayMessage.OnConsole(AnalyticsRequestUser(requestUser, service));
    }

    private Message AnalyticsRequestUser(RequestUser requestUser, IAnalysisSalesService service)
    {
        Message message = new Message();
        
        switch (requestUser.Command)
        {
            case CommandRequestEnum.Ads:
                message.MessageLine = "ADS: " +
                    service.GetAverageDaySales(requestUser.IdProduct).ToString(CultureInfo.InvariantCulture);
                break;
            case CommandRequestEnum.Demand:
                message.MessageLine = "Demand: " +
                    service.GetSalesDemand(requestUser.IdProduct, requestUser.Days).ToString(CultureInfo.InvariantCulture);
                break;
            case CommandRequestEnum.Prediction:
                message.MessageLine = "Prediction: " +
                    service.GetSalesPrediction(requestUser.IdProduct, requestUser.Days).ToString(CultureInfo.InvariantCulture);
                break;
            default:
                message.MessageLine = "Sorry, there is no such command.";
                break;
        }

        return message;
    }
    
    private RequestUser CreateUser(string strCommand) 
    {
        var arrCommand = strCommand.Split();

        if (arrCommand.Length > 1 && long.TryParse(arrCommand[1], out long idProduct))
        {
            switch (arrCommand.Length)
            {
                case 2:
                {
                    if (RequestCommandHandler(arrCommand[0]) != CommandRequestEnum.Prediction 
                        && RequestCommandHandler(arrCommand[0]) != CommandRequestEnum.Demand)
                        return new RequestUser(RequestCommandHandler(arrCommand[0]), idProduct);
                    
                    break;
                }
                case 3:
                {
                    if (long.TryParse(arrCommand[2], out long days) 
                        && RequestCommandHandler(arrCommand[0]) != CommandRequestEnum.Ads) 
                        return new RequestUser(RequestCommandHandler(arrCommand[0]), idProduct, days);
                    
                    break;
                }
            }
        }
        
        return new RequestUser(CommandRequestEnum.None, -1);
    }
    
    private CommandRequestEnum RequestCommandHandler(string strCommand)
    {
        switch (strCommand.ToLower())
        {
            case "ads": return CommandRequestEnum.Ads;
            case "prediction": return CommandRequestEnum.Prediction;
            case "demand": return CommandRequestEnum.Demand;
            default: return CommandRequestEnum.None;
        }
    }
}