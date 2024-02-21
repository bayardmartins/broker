# broker
Sample message broker application

## Structure
This solution is composed by four parts:
* BrokerApi - A minimal API responsible for receiving requests, mapping them and then publishing messages to a queue in RabbitMQ
* RabbitMQ - An instance of RabbitMQ running in a Docker container
* BrokerConsumer - A simple console application responsible for consuming messages in RabbitMQ queues and save them in the database
* MongoDB - An instance of MongoDB running in a Docker container

### Resources

#### RabbitMQ 
default url: http://localhost:15672/
default user and password: rabbitmq