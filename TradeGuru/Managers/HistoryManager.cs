using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TradeGuru.Views;

namespace TradeGuru.Managers
{
    public class HistoryManager
    {
        private static HistoryManager historyManager { get; set; }

        private HistoryUserControl control { get; set; }
        public List<ItemList> HistoryList { get; private set; }

        private HistoryManager(HistoryUserControl control)
        {
            this.control = control;
            this.HistoryList = new List<ItemList>();

            var itemList = Serializer.DeserializeObjectList<ItemList>(SettingsManager.CurrentSetting.HistoryFileLocation);
            if (itemList != null)
            {
                AddItems(itemList);
            }
        }

        public static HistoryManager Create(HistoryUserControl control)
        {
            historyManager = historyManager ?? new HistoryManager(control);
            return historyManager;
        }

        public void AddItems(ItemList items)
        {
            HistoryList.Add(items);
            control.AddItems(items);
        }

        public void AddItems(List<ItemList> items)
        {
            HistoryList.AddRange(items);
            control.AddItems(items);
        }

        public void SaveHistory()
        {
            Serializer.SerializeObjectList(SettingsManager.CurrentSetting.HistoryFileLocation, HistoryList);
        }

        public void ClearHistory()
        {
            HistoryList.Clear();
        }
    }
}
