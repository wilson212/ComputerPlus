﻿using ComputerPlus.Controllers.Models;
using Rage;
using Stealth.Plugins.ALPRPlus.API;
using Stealth.Plugins.ALPRPlus.API.Types;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using System;

namespace ComputerPlus
{
    internal static class ALPRPlusFunctions
    {
        internal static event EventHandler<ALPR_Arguments> OnAlprPlusMessage;
        internal static void RegisterForEvents()
        {
            Functions.ALPRResultDisplayed += Events_ALPRResultDisplayed;

        }

        private static void Events_ALPRResultDisplayed(Vehicle vehicle, ALPREventArgs args)
        {            
            EventHandler<ALPR_Arguments> handler = (EventHandler<ALPR_Arguments>)OnAlprPlusMessage;
            if (handler != null && vehicle.Exists())
            {
                ALPR_Position position = ALPR_Position.FRONT;
                switch (args.Camera)
                {
                    case ECamera.Driver_Front:
                        position = ALPR_Position.FRONT_DRIVER;
                        break;
                    case ECamera.Passenger_Front:
                        position = ALPR_Position.FRONT_PASSENGER;
                        break;
                    case ECamera.Driver_Rear:
                        position = ALPR_Position.REAR_DRIVER;
                        break;
                    case ECamera.Passenger_Rear:
                        position = ALPR_Position.REAR_PASSENGER;
                        break;                    
                }                               
                handler(null, new ALPR_Arguments(vehicle, position, args.Result.Result));
            }
            else
            {
                Function.LogDebug("C+: ALPRPlusFunctions has no handler");
            }
                
        }
    }
}
