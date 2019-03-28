using System.Collections;
using System.Collections.Generic;
using TwitchToolkit.Store;

namespace TwitchToolkit.Settings
{
    partial class Settings_Events
    {
        enum SortType { PriceAsc, PriceDesc, LabelAsc, LabelDesc, Karma}

        static SortType sortType = SortType.PriceAsc;

        static void SortEvents(ref List<IncItem> input)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException(nameof(input));
            }

            SortedList output = new SortedList();

            foreach (IncItem item in input) output.Add(item.price, item);

            switch(sortType)
            {
                default:
                    foreach (IncItem item in input) output.Add(item.price, item);
                    break;
            }

            input = new List<IncItem>();
            foreach (KeyValuePair<int, object> keyValuePair in output)
            input.Add(keyValuePair.Value as IncItem);
        }
    }
}
