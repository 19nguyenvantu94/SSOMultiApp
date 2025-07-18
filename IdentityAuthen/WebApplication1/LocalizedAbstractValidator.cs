﻿using Authen.Users.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorIdentity.Users
{
    public class LocalizedAbstractValidator<T, S> : AbstractValidator<T>
    {
        protected readonly IStringLocalizer<S> L;
        public LocalizedAbstractValidator(IStringLocalizer<S> l)
        {
            L = l;
        }
    }

    public class LocalizedAbstractValidator<T> : LocalizedAbstractValidator<T, Global>
    {
        public LocalizedAbstractValidator(IStringLocalizer<Global> l) : base(l)
        {

        }
    }
}
