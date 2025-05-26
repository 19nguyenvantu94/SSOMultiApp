﻿using Factories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Authen.Users.Models.Email.Validators
{
    public class EmailDtoValidator : LocalizedAbstractValidator<EmailDto>
    {
        public EmailDtoValidator(IStringLocalizer<Global> l) : base(l)
        {
            RuleFor(p => p.ToAddress)
                .NotEmpty()
                .EmailAddress().WithName("Email");
        }
    }
}
