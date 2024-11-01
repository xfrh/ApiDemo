﻿using ApiDemoApp.Models;
using Microsoft.AspNetCore.Components;

namespace ApiDemoApp.Services
{
    public class AppState
    {
       
        public EventCallback<string> OnErrorSelected { get; set; }
        public RegisterAGVFrom SelectedModel { get; set; }
        public ActionType SelectedType { get; private set; }
   
        public event Action OnChange;

        public void SetModel(RegisterAGVFrom model,ActionType type)
        {
            SelectedModel = model;
            SelectedType = type;
            NotifyStateChanged();
        }

        public void OnError(string msg)
        {
            LogService.LogMessage(msg);
        }

         private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public enum ActionType
    {
        ADD =1,
        UPDATE,
        DELETE,
        SELECTED,
        NAVIGATE,
        CANCEL,
        CHARGE,
        MANUL,
        STOPPED,
        ADDTASK,
        PAUSE,
        RESUME,
        COMPLETE,
        WATCHLIST,
      
    }
}
