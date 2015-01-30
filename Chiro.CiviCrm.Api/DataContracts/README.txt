In general:

Entities are things that the API returns.
Requests are things that you send to the API.

E.g. if you are searching for a contact, you send a ContactRequest containing
your serach criteria and chaining requests to the API. The API then returns
zero, one or more contacts (contact entities).

The differences between entities and requests are:
* all properties of a request are nullable
* null properties of a request are ignored most of the time
* the structure of a chained call (used in a request) differs from the
  structure of a chained result (found in entities)

The classes in EntityRequests are both entity and request. Mainly because
they don't support chaining (yet).
