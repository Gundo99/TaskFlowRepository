using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Middleware;
using TaskFlow.Application.Common.Behaviours;
using TaskFlow.Application.Common.Validation;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Commands.UpdateTask;
using TaskFlow.Application.Tasks.EventHandler;
using TaskFlow.Application.Tasks.Handler;
using TaskFlow.Application.Tasks.Queries;
using TaskFlow.Application.Users.EventHandlers;
using TaskFlow.Application.Users.Handlers;
using TaskFlow.Application.Users.Queries.GetTasksByUserId;
using TaskFlow.Application.Users.Queries.GetUserById;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Tasks;
using TaskFlow.Domain.Tasks.Events;
using TaskFlow.Domain.Users;
using TaskFlow.Domain.Users.Commands;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Persistence.Repositories;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Infrastructure.Auth;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\n\nExample: Bearer abc123"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<RegisterUserCommandHandler>();
builder.Services.AddScoped<GetUsersQueryHandler>();
builder.Services.AddScoped<GetUserByIdQueryHandler>();
builder.Services.AddScoped<UpdateUserEmailCommandHandler>();
builder.Services.AddScoped<DeleteUserCommandHandler>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<CreateTaskCommandHandler>();
builder.Services.AddScoped<GetTasksByUserIdQueryHandler>();
builder.Services.AddScoped<CompleteTaskCommdandHandler>();
builder.Services.AddScoped<UpdateTaskCommandHandler>();
builder.Services.AddScoped<DeleteTaskCommandHandler>();
builder.Services.AddScoped<GetTaskByIdQueryHandler>();
builder.Services.AddScoped<TaskFlow.Infrastructure.Persistence.DomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<TaskCompletedEvent>, TaskCompletedEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<TaskCompletedEvent>, TaskCompletedNotificationHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserRegisteredEvent>, UserWelcomeNotificationHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = "TaskFlowAPI",
        ValidAudience = "TaskFlowAPI",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();