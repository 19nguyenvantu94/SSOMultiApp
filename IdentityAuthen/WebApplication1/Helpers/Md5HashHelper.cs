﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Security.Cryptography;
using System.Text;

namespace Authen.Helpers
{

    /// <summary>
    /// Helper-class to create Md5hashes from strings
    /// </summary>
    public static class Md5HashHelper
    {
        /// <summary>
        /// Computes a Md5-hash of the submitted string and returns the corresponding hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();

                foreach (var dataByte in bytes)
                {
                    sBuilder.Append(dataByte.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
