﻿using Penguin.SystemExtensions.Abstractions.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Penguin.Web.Headers
{
    public class HttpHeader : IConvertible<string>
    {
        [Display(Order = 1)]
        public string Key { get; set; }

        [Display(Order = 3)]
        public virtual HeaderMode Mode => HeaderMode.Single;

        [Display(Order = 2)]
        public string Value { get; set; }

        public HttpHeader()
        {
        }

        string IConvertible<string>.Convert() => ToString();

        void IConvertible<string>.Convert(string fromT)
        {
            string source = fromT.Trim();

            if (!source.Contains(":"))
            {
                return;
            }

            int c = source.IndexOf(':');

            Key = source.Substring(0, c).Trim();
            Value = source.Substring(c + 1).Trim();
        }

        public override string ToString() => $"{Key}: {Value}";
    }
}