A logging provider developed with large scale distributed systems in mind.

Consists of a connector to the logging system that is used to push logs from a system developed in any framework/programming language. This lends
itself to microservices architectures where each node can publish to the central logging system and thus all system logs can be managed 
from a single central place.

The main logging system is implemented using a Microsoft .Net framework MVC/Web Api where logs published are streamed to a browser using
Asp.net SignalR server push technology.

The connector from the application publishes the logs to a message bus(RabbitMQ). The logging system subscribes to the logging queues and saves the logs and also streams them to the browser if there active connections.

The problem is reduced to developing the connector once the logging system is in place.


