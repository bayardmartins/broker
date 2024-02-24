.PHONY: run

run:
	@docker-compose up -d rabbitmq
	@docker-compose up -d mongo
	@docker-compose up -d broker-api