﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public sealed class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
