# Domain Driven Design

In dit hoofdstuk trachten we drie technische componenten van DDD te implementeren:

1. Aggregate root
2. Invariants
3. Value objects

Eerst en vooral nog eens benadrukken dat DDD veel breder is dan deze technische implementaties. Het gaat om design, over het modeleren van een probleem naar een oplossing. Het ecosysteem van DDD is door de jaren heen ook stevig gegroeid. Bijvoorbeeld, het is belangrijk dat developers dezelfde duidelijke en ondubbelzinnige taal spreken als de business, daarom is het techno-sociale aspect ook heel belangrijk.

Uit het modeleren, bijvoorbeeld door gebruik van Event Storming [1], komen een aantal Bounded Contexten te voorschijn. Binnen deze Bounded Context spreekt men eenzelfde taal, die kan verschillen van een andere Bounded Context ook al is die in dezelfde oplossing opgenomen. Bijvoorbeeld: als je een koffie besteld in Rome krijg je wellicht een straffe koffie in een kleine tas, terwijl je in New York een grote beker met zoetere koffie krijgt. Of dichter bij huis, het verschil van het woord bank en zetel bij Vlamingen en Nederlanders. Met andere woorden, taal heeft zijn grenzen.
Een product voor de marketing afdeling is voor hun alles wat gerelateerd is met de product catalogus, terwijl voor de pricing manager enkel de prijs telt, en voor de magazijnier hoeveel items van dat product nog in stock zijn.
Vertaald naar het technische spreken we niet meer van een Bounded Context, maar van een Aggregate. Deze Aggregate bestaat uit één of meerdere domain entities die als het ware aan elkaar hangen. Per Aggregate is er ook één Aggregate Root. In principe gebeuren alle acties in een Aggregate, altijd via de Aggregate Root. 
In ons model hebben we een Order en verschillende Orderlines die daaronder hangen. Een Orderline kan niet bestaan zonder een Order en als we een Orderline willen toevoegen, aanpassen of verwijderen, dan gaan we de dit altijd doen via de Order en nooit rechtstreeks op de Orderline.

Een belangrijke taak van de Aggregate Root is ook om te controleren of hij altijd in een valid state is. Als bijvoorbeeld de Order status aangepast wordt naar confirmed, dan moéten er Orderlines zijn. Deze validatieregels zijn direct te mappen met de business rules, en daarom horen ze ook thuis in de Aggregate Root en we noemen ze Invariants. 
Deze regels worden ook in de front-end afgedwongen.
Er zijn ook regels die buiten de context vallen, zoals de controle of er wel genoeg stock aanwezig is vooraleer een Order kan verzonden worden. Deze kunnen perfect een laag hoger afgedwongen worden, aangezien dit ook deel uitmaakt van DDD. 

Soms zijn er validatieregels die zeer gericht zijn tot één property. Soms is zijn er ook properties die een groep kunnen zijn, zoals Streetname, Housenumber, PostalCode en City wat kan samengenomen worden tot Address. Voor deze (en meer) gevallen, kan je gebruik maken van Value objects. In tegenstelling tot domain entiteiten die een identity property hebben (vb ID), hebben Value objects geen identity property. Hun identiteit wordt verkregen door hun waarde. Bijvoorbeeld: 5 euro is 5 euro. De eerste 5 euro echter is samengesteld door 5 stukken van 1 euro, en de tweede 5 euro is een biljet van 5 euro. 
Value objects zijn ook immutable en we maken ze aan via een factory method, waar we al validaties kunnen uitvoeren. [2]

Hoog tijd om deze concepten toe te passen in ons project.

## Buyyu project

### Wat zijn de bounded contexts?

Als we even terug kijken naar onze applicatie, dan kan je veel meer contexten of groepen van functionaliteit herkennen dan we in de originele opzet gebruiken en onder onze order geplaatst hebben.

We onderscheiden:

- Order: om de bestelling samen te stellen, te valideren, en op te volgen
- Product: om de productcatalogus te bekijken
- Warehouse: om de bestelling uit de stock te halen en te verzenden en stock aan te vullen
- Payment: om de factuur te betalen

### DDD root base class

Voor onze entities en aggregate roots hebben we een aantal base classes nodig. Deze zitten in een apart project:



## 

### Aanmaak domain project



## Unit testen



## Taken

1. Pas nu alle andere domain entiteiten aan zodat deze voldoen aan de encapsulation regels uitgelegd in dit hoofdstuk.
2. Pas de unit tests aan zodat deze werkende blijven.
3. Test of de applicatie nog werkt zoals voorheen

## Volgende stap

In de volgende stap gaan we meer technische componenten van DDD inbouwen zoals een aggregate root, valueobjects en invariants.

## Referenties

[1]: https://www.eventstorming.com/	"Event Storming (Brandolini)"
[2]: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects	"Implement value objects"

