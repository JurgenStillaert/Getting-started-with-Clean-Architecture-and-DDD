using MediatR;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace buyyu.DDD
{
	public abstract class CreateCommandHandler<TCommand, TAggregate, TKey> : CommandHandler<TCommand, TAggregate, TKey>
			where TAggregate : AggregateRoot<TKey>
			where TCommand : IRequest
			where TKey : Value<TKey>
	{
		protected CreateCommandHandler(IRepository<TAggregate, TKey> repo)
			: base(repo, HandlerTypeEnum.Create)
		{
		}
	}

	public abstract class UpdateCommandHandler<TCommand, TAggregate, TKey> : CommandHandler<TCommand, TAggregate, TKey>
		where TAggregate : AggregateRoot<TKey>
		where TCommand : IRequest
		where TKey : Value<TKey>
	{
		protected UpdateCommandHandler(IRepository<TAggregate, TKey> repo)
			: base(repo, HandlerTypeEnum.Update)
		{ }

		protected abstract void PreHandle(TCommand command);
	}

public abstract class CommandHandler<TCommand, TAggregate, TKey> : IRequestHandler<TCommand>
	where TAggregate : AggregateRoot<TKey>
	where TCommand : IRequest
	where TKey : Value<TKey>
{
	protected CommandHandler(
		IRepository<TAggregate, TKey> repo,
		HandlerTypeEnum handlerType)
	{
		Repo = repo;
		CommandType = handlerType;
	}

	public IRepository<TAggregate, TKey> Repo { get; }
	public TKey AggregateId { get; protected set; }
	public TAggregate AggregateRoot { get; protected set; }
	private HandlerTypeEnum CommandType { get; }

	public bool skipSave = false;
	private bool isDeleted = false;

	public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
	{
		if (CommandType == HandlerTypeEnum.Update)
		{
			//Execute prehandle method to get aggregateId
			var preHandleMethod = GetHandleMethod(command, HandleMethodType.PreHandle);

			try
			{
				preHandleMethod.Invoke(this, new object[] { command });
			}
			catch (TargetInvocationException targetInvocationException)
			{
				throw targetInvocationException.InnerException;
			}
			catch (Exception)
			{
				throw;
			}

			AggregateRoot = await GetAggregateFromRepo();
		}

		try
		{
			await Apply(command);

			if (AggregateRoot != null)
			{
				if (!isDeleted)
				{
					if (!skipSave)
					{
						if (CommandType == HandlerTypeEnum.Create)
						{
							await Repo.Add(AggregateRoot);
						}
						else
						{
							await Repo.Save(AggregateRoot);
						}
					}
				}
			}
		}
		catch (TargetInvocationException targetInvocationException)
		{
			throw targetInvocationException.InnerException;
		}
		catch (Exception ex)
		{
			throw;
		}

		return await Unit.Task;
	}

	protected async Task DeleteAggregateRoot()
	{
		isDeleted = true;
		await Repo.Delete(AggregateRoot);
	}

	protected async Task<TAggregate> GetAggregateFromRepo()
	{
		return await Repo.Load(AggregateId);
	}

	protected abstract Task Apply(TCommand command);

	private MethodInfo GetHandleMethod(TCommand command, HandleMethodType handleMethodType)
	{
		//Get the handle methods
		var handleMethod = this.GetType().GetMethod(
				Enum.GetName(typeof(HandleMethodType), handleMethodType),
				BindingFlags.Instance | BindingFlags.NonPublic,
				Type.DefaultBinder,
				new Type[] { command.GetType() },
				null);

		if (handleMethod == null)
		{
			throw new MissingMethodException($"Handle method with event { command.GetType()} is missing");
		}

		return handleMethod;
	}

	private enum HandleMethodType
	{
		Handle,
		PreHandle
	}

	protected enum HandlerTypeEnum
	{
		Update,
		Create
	}
}
}