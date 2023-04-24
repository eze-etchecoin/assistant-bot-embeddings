﻿using System.Collections.Generic;

namespace AssistantBot.Services.Interfaces
{
    public interface IMemoryStorage
    {
        /// <summary>
        /// For saving values into storage.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);

        /// <summary>
        /// For getting values from storage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Search in storage according to certain value, and get first N results
        /// </summary>
        /// <param name="valueToSearch"></param>
        /// <returns></returns>
        IEnumerable<object> SearchForValues(object valueToSearch, int numberOfResults = 10);
    }
}
