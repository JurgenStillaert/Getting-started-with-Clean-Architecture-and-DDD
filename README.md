# Encapsulation

Laten we even terug gaan naar de eerste keer dat we over Object Oriented Programming geleerd hebben. Daar hebben we geleerd dat er objecten zijn die zowel data als code hebben, een "state" en een "behavior".

Stel, we hebben een auto, en die heeft 2 pedalen: om te versnellen, en om te vertragen.

<img src="https://www.leuvenmindgate.be/files/_contentImage/feb.png" alt="Formula Electric Belgium reveals digital design of… | Leuven MindGate" style="zoom: 33%;" />

| Gedrag          | State          |
| --------------- | -------------- |
| Versnellen(100) | Snelheid = 100 |
| Vertragen(80)   | Snelheid = 20  |

In code zouden we dit als volgt kunnen schrijven:

```c#
public class Car
{
	public int Speed { get; private set; }

	public void Accelerate(int speedUp)
	{
		if (Speed + speedUp > 120)
		{
			Speed = 120;
		}
		else
		{
			Speed += speedUp;
		}
	}

	public void SlowDown(int speedDown)
	{
		if (Speed - speedDown < 0)
		{
			Speed = 0;
		}
		else
		{
			Speed -= speedDown;
		}
	}
}
```

We regelen de snelheid via de methodes die beschikbaar zijn, en doen bovendien nog enkele controles om zeker te zijn dat we geen snelheidsboete krijgen of dat we geen negatieve snelheid hebben.

We leerden ook dat er 4 pijlers zijn bij OOP:

1. Inheritance
2. Abstraction
3. Polymorpism
4. en Encapsulation

Op Wikipedia [1] lezen we:

> Objecten vormen de koppeling tussen enerzijds gegevens en anderzijds bewerkingen die op die gegevens worden uitgevoerd. We spreken van [inkapselen](https://nl.wikipedia.org/wiki/Encapsulatie) of *encapsulatie* als andere programma-eenheden de gegevens niet rechtstreeks aanspreken, maar wel via de tussenlaag van een bewerking. Programmeurs die een object gebruiken hoeven zich slechts bewust te zijn van de interface van dat object, terwijl de implementatie alleen een zaak is van de programmeurs die het object ontwikkelen.[[3\]](https://nl.wikipedia.org/wiki/Objectgeoriënteerd#cite_note-Stiller-3)
>
> Bij klasse-gebaseerde talen betekent encapsulatie dat de programmeur die een object van een klasse gebruikt, niet hoeft na te denken over de interne werking van die klasse. Een cirkelklasse kan bijvoorbeeld zijn [attribuut](https://nl.wikipedia.org/wiki/Attribuut_(informatica)) "`diameter`" niet publiek maken maar de verschillende [methoden](https://nl.wikipedia.org/wiki/Methode_(objectoriëntatie)) om de oppervlakte op te vragen of de diameter te veranderen wel. De cirkelklasse zou dan eenvoudig aangepast kunnen worden om toch in plaats van de diameter intern de straal op te slaan, zonder dat de gebruiker van de cirkelklasse dit hoeft te weten: de publieke toegang tot de klasse is immers niet veranderd.

Encapsulation gaat niet enkel over het wegnemen van de implementatie-details voor de gebruiker van de klasse, maar ook over het bewaken van state.

En dan komen er Object Relational Managers (ORM's) zoals Entity Framework, nHibernate, Dapper,... en vergeten we deze regel en wordt onze code als volgt:

```c#
public class Car
{
	public int Speed { get; set; }
}
```

En kunnen we van elke plaats het volgende doen:

```c#
var car = new Car();
car.Speed = -9999;
```

In DDD spreken we hier van een anemic domain model [2], een model met bloedarmoede, betekenende dat er geen behavior is gedefinieerd in het model. 
En zoals Martin Fowler het verwoordt:

> The fundamental horror of this anti-pattern is that it's so contrary to the basic idea of object-oriented design; which is to combine data and process together.

## Buyyu project

We gaan nu de eerste aanpassingen doen in het project. We zouden kunnen nadenken over de behavior die nodig is en de methods gaan definiëren, gelijklopende als deze nu in de business logic laag zitten. Maar aangezien we dit al hebben, gaan we een andere manier proberen.

De eerste stap zal zijn om de setters van onze domain modellen private te zetten. Dit gaat een hele reeks fouten opleveren. In het Data project pas ik dus bijvoorbeeld Order.cs aan als volgt:

```c#
using System;
using System.Collections.Generic;

namespace buyyu.Data
{
	public class Order
	{
		public Guid Id { get; private set; }
		public Guid ClientId { get; private set; }
		public Guid OrderStateId { get; private set; }
		public DateTime OrderDate { get; private set; }
		public decimal TotalAmount { get; private set; }
		public decimal PaidAmount { get; private set; }

		public OrderState State { get; private set; }
		public List<Orderline> Lines { get; private set; }
		public List<Payment> Payments { get; private set; }
	}
}
```

Na een build, geeft dit al meteen 19 errors:

![image-20210223202128421](README.assets/image-20210223202128421.png)

Als we kijken in welke bestanden deze fouten nu voorkomen dan zien we OrderService.cs en OrderServiceTests.cs.  We moeten deze bestanden en Order.cs nu gaan refactoren. 

```c#
public async Task<OrderDto> CreateOrder(OrderDto orderDto)
{
	var newOrderState = await _orderStateRepository.GetOrderStateByCode(NEWSTATECODE);

	var newOrder = new Order
	{
		ClientId = orderDto.ClientId,
		OrderStateId = newOrderState.Id,
		OrderDate = DateTime.Now,
		PaidAmount = 0,
		Lines = new List<Orderline>()
	};
	await ProcessOrderLines(orderDto, newOrder);

	await _orderRepository.Save(newOrder);

	return await GetOrder(newOrder.Id);
}
```



Regel 5 tem 12 geven fouten omdat we trachten properties aan te passen die private setters hebben. We zouden voor deze stap een contructor kunnen maken die deze parameters aanneemt en een nieuw object van Order terug geeft. Echter, een betere manier is te werken met een factory method. Zodus, voor Order.cs maken de volgende static method aan:

```c#
public static Order Create(
	Guid clientId,
	Guid orderStateId)
{
	if (clientId == null || clientId == Guid.Empty)
	{
		throw new ArgumentNullException(nameof(clientId), "ClientId cannot be empty");
	}
	if (orderStateId == null || orderStateId == Guid.Empty)
	{
		throw new ArgumentNullException(nameof(orderStateId), "OrderStateId cannot be empty");
	}

	var order = new Order
	{
		ClientId = clientId,
		OrderStateId = orderStateId,
		OrderDate = DateTime.Now,
		PaidAmount = 0,
		Lines = new List<Orderline>()
	};

	return order;
}
```

We geven voorlopig nog de status id (orderStateId) mee, maar in een latere stap gaan we dit aanpassen.

Om het op dit ogenblik makkelijker te maken, maken we een aparte functie aan om orderlines toe te voegen.

```c#
public void AddOrderLine(Guid productId, decimal price, int qty)
{
	if (productId == null || productId == Guid.Empty)
	{
		throw new ArgumentNullException(nameof(productId), "ProductId cannot be empty");
	}

	if (Lines.Any(ol => ol.ProductId == productId))
	{
		throw new InvalidOperationException("Product is already added");
	}
	if (qty <= 0)
	{
		throw new ArgumentNullException(nameof(qty), "Qty must be a positive integer");
	}


	Lines.Add(new Orderline() { Price = price, ProductId = productId, Qty = qty });
}
```

De CreateOrder method in de OrderService kunnen we nu als volgt aanpassen:

```c#
public async Task<OrderDto> CreateOrder(OrderDto orderDto)
{
	var newOrderState = await _orderStateRepository.GetOrderStateByCode(NEWSTATECODE);

	var newOrder = Order.Create(orderDto.ClientId, newOrderState.Id);
	foreach (var orderline in orderDto.Orderlines)
	{
		var product = await _productRepository.GetProduct(orderline.ProductId);
		newOrder.AddOrderLine(product.Id, product.Price, orderline.Qty);
	}

	await _orderRepository.Save(newOrder);

	return await GetOrder(newOrder.Id);
}
```

Op dezelfde manier kunnen de andere methods van OrderService verplaatst worden naar de Order class.

## Unit testen

Na het aanpassen van de OrderService en de Order class, blijven er nog errors over in onze unit test, meer bepaalt CreateOrder_OrderWithTwoProducts_OrderUpdated waar we nu geen waarden aan ons object kunnen toewijzen vanwege de private setters.

Deze code block moeten we dus aanpakken:

```c#
var newOrder = new Order
{
	ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
	Id = inDto.OrderId,
	Lines = new List<Orderline>
	{
		new Orderline
		{
			Id = Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"),
			Order = null,
			OrderId = Guid.Empty,
			Price = 295.00m,
			Product = null,
			ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
			Qty = 10
		},
		new Orderline
		{
			Id = Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"),
			Order = null,
			OrderId = Guid.Empty,
			Price = 263.00m,
			Product = null,
			ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
			Qty = 20
		}
	},
	OrderDate = new DateTime(2021, 2, 19, 17, 26, 57),
	OrderStateId = Guid.Parse("bd8be3d2-8028-45e2-a211-bf737a2508c1"),
	PaidAmount = 0m,
	Payments = new List<Payment>(),
	State = NewOrderState,
	TotalAmount = 8210.00m
};
```

Wat zijn onze mogelijkheden?

- Met Moq, kunnen we via SetupGet een waarde toewijzen aan onze property. Echter kunnen we de waarde daarna niet meer wijzigen, en bovendien moeten we de properties virtual markeren. Dit is niet gewenst.
- Er kan gewerkt worden met reflection om waarden toe te wijzen aan private setters:

```c#
var t = typeof(Order);
var newOrder = (Order)Activator.CreateInstance(t);
typeof(Order).GetProperty(nameof(newOrder.ClientId)).SetValue(newOrder, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
typeof(Order).GetProperty(nameof(newOrder.Id)).SetValue(newOrder, orderId);
typeof(Order).GetProperty(nameof(newOrder.OrderDate)).SetValue(newOrder, new DateTime(2021, 2, 19, 17, 26, 57));
typeof(Order).GetProperty(nameof(newOrder.OrderStateId)).SetValue(newOrder, newOrderState.Id);
typeof(Order).GetProperty(nameof(newOrder.PaidAmount)).SetValue(newOrder, 0m);
typeof(Order).GetProperty(nameof(newOrder.Payments)).SetValue(newOrder, new List<Payment>());
typeof(Order).GetProperty(nameof(newOrder.State)).SetValue(newOrder, newOrderState);
typeof(Order).GetProperty(nameof(newOrder.TotalAmount)).SetValue(newOrder, 8210.00m);

typeof(Order).GetProperty(nameof(newOrder.Lines)).SetValue(newOrder, new List<Orderline>
{
	new Orderline
	{
		Id = Guid.Parse("fc0e1862-21c3-4cd0-ae9c-e02986b8f283"),
		Order = null,
		OrderId = Guid.Empty,
		Price = 295.00m,
		Product = null,
		ProductId = Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
		Qty = 10
	},
	new Orderline
	{
		Id = Guid.Parse("3510659d-d3c0-43bf-aca5-d2bbede87685"),
		Order = null,
		OrderId = Guid.Empty,
		Price = 263.00m,
		Product = null,
		ProductId = Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"),
		Qty = 20
	}
});
```

- Of er kan gewerkt worden met het opbouwen van een object door zijn geschiedenis af te spelen. Deze methode heeft de voorkeur in het licht van Event Sourcing, wat later zal besproken worden. Echter stoten we hier op een probleem, zijnde we laten de database de ID's zetten. Het probleem hiermee is dat we business logica hebben verplaatst naar de database. Beter was geweest om onze applicatie de ID te laten zetten. Denk ook aan Stored Procedures en triggers die data gaan wijzigen buiten de applicatie om en aan onze opzet voor Clean Architecture. Voorlopig passen we de gegevens die gezet worden in de database aan, met reflection, maar later gaan we die functionaliteit verhuizen naar code.

```c#
public static Order NewOrderWithTwoProducts(Guid orderId, OrderState newOrderState)
{
	var newOrder = Order.Create(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), newOrderState.Id);
	newOrder.AddOrderline(Guid.Parse("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), 295.00m, 10);
	newOrder.AddOrderline(Guid.Parse("32f75bce-16a0-4070-9fac-4289678c191f"), 263, 20);

	var t = typeof(Order);
	typeof(Order).GetProperty(nameof(newOrder.Id)).SetValue(newOrder, orderId);
	typeof(Order).GetProperty(nameof(newOrder.State)).SetValue(newOrder, newOrderState);

	return newOrder;
}
```

Om onze unit tests kleiner te maken, gaan we de code voor het opbouwen van onze objecten verhuizen naar een builder helper class.

## Taken

1. Pas nu alle andere domain entiteiten aan zodat deze voldoen aan de encapsulation regels uitgelegd in dit hoofdstuk.
2. Pas de unit tests aan zodat deze werkende blijven.
3. Test of de applicatie nog werkt zoals voorheen

## Volgende stap

In de volgende stap gaan we meer technische componenten van DDD inbouwen zoals een aggregate root, valueobjects en invariants.

## Referenties

[1]: https://nl.wikipedia.org/wiki/Objectgeori%C3%ABnteerd#Inkapselen_van_data_(encapsulatie)	"Objectgeoriënteerd (encapsulatie)"
[2]: https://www.martinfowler.com/bliki/AnemicDomainModel.html	"AnemicDomainModel (Martin Fowler - 2003)"

