# Domain Events & Event driven

> Als een order is bevestigd, dan moet er een bevestigingsmail naar de klant verzonden worden.

Bovenstaande is een zin die business wel eens zou kunnen zeggen. Het eerste deel, order bevestigd, is een event op ons domain object. Een event is een gebeurtenis dat vast staat, wat niet meer terug gedraaid kan worden. Als er een fout is gebeurd, dan is de enige manier om dat op te lossen door een compenserend event.

Bijvoorbeeld, stel dat de bank op uw zichtrekening te veel geld heeft afgehouden. Deze verrichting staat nu op uw overzicht. Wat de bank niet gaat doen om deze fout recht te zetten is de foute verrichting schrappen of aanpassen. In de plaats daarvan maken ze een nieuwe verrichting waarbij er geld gestort wordt op uw rekening.

Het tweede deel van bovenstaande zin, "dan moer er...", zijn  gevolgen die aan een event gekoppeld worden. Dit kunnen er geen zijn, één of meerdere. Momenteel is dit in de code als volgt:

```c#
//Send email
await _mailService.SendOrderConfirmationMail(AggregateRoot);
```

Telkens er een gevolg aan een event wordt aangebreid, wordt een nieuwe regel of regels in onze use case toegevoegd. Echter, dit is niet volgens het Single Responsibility principe. Er zijn nu meerdere redenen om een class aan te passen, bijvoorbeeld stel dat er andere parameters vereist worden door de mailservice. Dit heeft niets te maken met een order bevestigen.

Beter zou zijn om het event dat plaats gevonden heeft op een queue te plaatsen waarop andere classes kunnen inschrijven. Op deze manier wordt een event driven applicatie gebouwd en worden de verantwoordelijkheden verder uit elkaar getrokken. Als we de events als messages op een externe queue plaatsen kunnen ook andere services hierop reageren. Stel dat een klant inactief wordt geplaatst door de service  voor klantenbeheer, dan kunnen openstaande orders verwijderd worden door de order service.

Aangezien deze events op vele plaatsen kunnen gebruikt worden, zijn de properties van deze event classes altijd primitive types.
De naamgeving van een event is altijd een beschrijving in het verleden: OrderCreated, OrderlineAdded.

## Buyyu project

### **Domain Event**

Volgende interface dient om aan te duiden welke de domain events zijn. Deze erft ook van MediatR's INotification, dewelke dient om te publishen waarop meerdere classes op kunnen luisteren.

```c#
public interface IDomainEvent : INotification
{
}
```

We maken dan ook de overeenkomstige domain events aan. Aangezien dit weer een communicatiemiddel is naar buiten toe, plaatsen we dit weer in de Models folder (het is duidelijk dat Models niet meer de lading dekt, het is aan te raden dit te hernoemen naar messages of iets dergelijk).

```c#
public sealed class OrderCreated : IDomainEvent
{
	public OrderCreated(
		Guid orderId,
		Guid clientId,
		DateTime orderDate)
	{
		OrderId = orderId;
		ClientId = clientId;
		OrderDate = orderDate;
	}

	public Guid OrderId { get; }
	public Guid ClientId { get; }
	public DateTime OrderDate { get; }
}
```



### **AggregateRoot**

We breiden de AggregateRoot base class uit zodat de events kunnen bijgehouden worden, en om de flow wat anders te laten lopen zodoende dat er altijd een zekerheid is dat de event bijgehouden wordt en er altijd de valid state gecontroleerd wordt.

```C#
public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : Value<TKey>
{
	private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();
	public IReadOnlyList<IDomainEvent> Changes => _changes.ToList();

	public void ClearChanges() => _changes.Clear();

	/// <summary>
	/// Find Handle methods in the implementation with parameter of type @event
	/// </summary>
	/// <param name="event"></param>
	protected void When(IDomainEvent @event)
	{
		//Get the handle methods
		var handleMethod = this.GetType().GetMethod(
				"Handle",
				BindingFlags.Instance | BindingFlags.NonPublic,
				Type.DefaultBinder,
				new Type[] { @event.GetType() },
				null);

		if (handleMethod == null)
		{
			throw new MissingMethodException($"Handle method with event { @event.GetType()} is missing");
		}

		try
		{
			handleMethod.Invoke(this, new object[] { @event });
		}
		catch (TargetInvocationException targetInvocationException)
		{
			throw targetInvocationException.InnerException;
		}
	}

	protected void Apply(IDomainEvent @event)
	{
		When(@event);
		EnsureValidState();
		_changes.Add(@event);
	}

	public void Replay(List<IDomainEvent> history)
	{
		foreach (var @event in history)
		{
			When(@event);
		}
	}

	protected abstract void EnsureValidState();
}
```

Dit impliceert dat de volgende stappen gedaan moeten worden in een aggregate root implementatie:

1. Een publieke methode als command aanmaken die parameters accepteert als value objects
2. De eventuele nodige programmatie doen in deze methode
3. Aanmaken van een event
4. Aanroepen van Apply(event)
5. Aanmaken van een private Handle(event) methode, hier gebeurt enkel het zetten van state.

De Apply(event) gaat op zoek naar de corresponderende Handle methode, voert de methode EnsureValidState() uit om na te gaan of de aggregate root nog in een geldige state is en voegt het event dan toe aan de lijst van events.

Er is ook een mogelijkheid om de events te herspelen met de Replay methode. Deze voert enkel de Handle(event) methodes uit. Dit is al een eigenschap voor Event Sourcing.

De aanpassingen op de OrderRoot ziet er nu als volgt uit:

```c#
public static OrderRoot Create(OrderId orderId, ClientId clientId)
{
	var order = new OrderRoot();

	order.Apply(new v1.OrderCreated(orderId, clientId, DateTime.Now));
			
	return order;
}

public void AddOrderline(ProductId productId, Money price, Quantity qty)
{
	if (!State.IsNewState())
	{
		throw new InvalidOperationException("Cannot add orderlines to a confirmed order");
	}

	if (Lines.Any(ol => ol.ProductId == productId))
	{
		throw new InvalidOperationException("Product is already added");
	}

	Apply(new v1.OrderlineAdded(Id, OrderlineId.GenerateNew(), productId, price.Amount, price.Currency, qty));
}

private void Handle(v1.OrderCreated @event)
{
	Id = OrderId.FromGuid(@event.OrderId);
	ClientId = ClientId.FromGuid(@event.ClientId);
	State = OrderState.FromEnum(OrderState.OrderStateEnum.NEW);
	OrderDate = OrderDate.FromDateTime(@event.OrderDate);
	TotalAmount = Money.Empty("EUR");
	PaidAmount = Money.Empty("EUR");
	Lines = new List<Orderline>();
}

private void Handle(v1.OrderlineAdded @event) {
	Lines.Add(Orderline.Create(
		OrderlineId.FromGuid(@event.OrderlineId), 
		ProductId.FromGuid(@event.ProductId),
		Money.FromDecimalAndCurrency(@event.Price, @event.Currency),
		Quantity.FromInt(@event.Quantity)));

	TotalAmount = Money.FromDecimalAndCurrency(Lines.Select(x => x.Price.Amount * x.Qty).Sum(), "EUR");
}
```

### Command handlers

In de base command handler gaan we na het bewaren van de aggregate root de events die op de aggregate root zitten uitsturen.

```c#
if (!skipSave)
{
	//Public events
	foreach (var @event in AggregateRoot.Changes)
	{
		await _mediator.Publish(@event);
	}

	AggregateRoot.ClearChanges();
}
```

### Event handlers

In het BL project maken we naast Command en Queries een nieuwe folder aan Events. Daarin kunnen we de event handlers plaatsen. Een voorbeeld is om de mailservice methods om te vormen naar een event handler. Dus volgende method:

```c#
public async Task SendPaymentConfirmationMail(OrderRoot order)
{
	_logger.LogInformation("Payment order confirmation");
	return;
}
```

Wordt omgezet naar de volgende event handler:

```c#
public class SendOrderConfirmationMail : INotificationHandler<v1.OrderConfirmed>
{
	private readonly ILogger<SendOrderConfirmationMail> _logger;

	public SendOrderConfirmationMail(ILogger<SendOrderConfirmationMail> logger)
	{
		_logger = logger;
	}

	public async Task Handle(v1.OrderConfirmed notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Sending order confirmation");
	}
}
```

De bovenste event handler reageert nu op een event OrderConfirmed, en is enkel verantwoordelijk voor het verzenden van de bevestigingsmail.

Door de wijze waarop hier de events worden opgegooid, worden deze in hetzelfde proces afgehandeld. In combinatie met bijvoorbeeld Hangserver kan je deze als een job in de background uitvoeren. Om meer naar een microservice manier toe te werken kunnen deze events ook gedispatched worden met een AMQP provider.

Een belangrijke noot - het is al geschreven geweest hierboven - is dat we willen reageren op een event van een andere context, of we willen iets afhandelen in de infrastructure-laag zoals het verzenden van een mail, sms, datawarehouse vullen,...

## Volgende stap

Momenteel zijn de projecten onderverdeeld in hun technische laag. Wanneer een project groeit kan het lastiger zijn om de verschillende functionele onderdelen die in elke laag samenwerken te vinden. Stel u voor dat je in 100 controllers de ordercontroller moet vinden, dan weer in 200 use case de juiste command of query handler, en dan weer in een ander project de entities. Vele beter zou zijn om de functionele blokken samen te nemen en dan eventueel op te delen in technische lagen.