# Microservice application example
 
Just concept of microservice application utilizing RabbitMQ message broke for service communication.
Machine service create collection of machines where machine generate data (5 values, Value1 .. Value5 with random int) in period between 8-14s, machine can be started and stopped and have order number. Default machine count is 50 (names M1 .. M50)

## RestAPI
Nginx is used as gateway and have these methods:

### Machine overview
- GET http://{yourendpoint}:8100/machine/allmachines
- GET http://{yourendpoint}:8100/machine/M1

### Machine data
- GET http://{yourendpoint}:8100/machinedata/M1/2019-03-26T00:00:00/2019-03-27T00:00:00

### Machine commands
- POST http://{yourendpoint}:8100/machinecommand/M1/start
- POST http://{yourendpoint}:8100/machinecommand/M1/stop
- POST http://{yourendpoint}:8100/machinecommand/M1/order/42

## Application deployment
All images are ready on https://cloud.docker.com/u/zaoralj/repository/list and you can deploy it by running command
```console
docker stack deploy -c docker-compose.yml --orchestrator swarm test-app
```
## Application schema
![Application diagram](https://github.com/ZaoralJ/microservice-app-example/blob/master/AppSchema.png)
