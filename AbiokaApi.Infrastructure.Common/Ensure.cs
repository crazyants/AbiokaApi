﻿using System;

namespace AbiokaApi.Infrastructure.Common
{
    public class Ensure
    {
        public static void IsNotNull(object parameter, string name) {
            if (parameter == null)
                throw new ArgumentNullException(name);
        }
    }
}
