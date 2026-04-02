using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Middleware;
using TaskFlow.Application.Common;
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
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();