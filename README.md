# broker
Sample message broker application

## Structure
This solution is composed of four parts:
* Broker.Api - A minimal API responsible for receiving requests, mapping them and then publishing messages to a queue in RabbitMQ
* RabbitMQ - An instance of RabbitMQ running in a Docker container
* Broker.Consumer - A simple console application responsible for consuming messages in RabbitMQ queues and save them in the database
* MongoDB - An instance of MongoDB running in a Docker container

## Hou does it work?
* Broker.Api receive POST messages and publish them in a queue in RabbitMQ
* Broker.Consumer receive the message from RabbitMQ and store it in MongoDB

### Using the solution

#### Running
If you have Make installed you can easily run the command:

```
make run
```

If you don't, you can run the docker-compose individually:

```
docker-compose up -d rabbitmq
docker-compose up -d mongo
docker-compose up -d broker-consumer
docker-compose up -d broker-api
```

#### Using and Testing
1. Open the `Broker.postman_collection.json` in Postman
2. Send the `POST Publish Message` request
3. Send the `GET Get All Messages` request and verify if the message was stored