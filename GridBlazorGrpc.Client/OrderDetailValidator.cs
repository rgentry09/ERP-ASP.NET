﻿using FluentValidation;
using GridBlazorGrpc.Shared.Models;

namespace GridBlazorGrpc.Client
{
    public class OrderDetailValidator : AbstractValidator<OrderDetail>
    {
        public OrderDetailValidator()
        {
            RuleFor(p => p.Quantity).NotEmpty().WithMessage("You must enter a quantity");
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("You must enter a unit price");
        }
    }
}
