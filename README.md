### Catelogs

Catelog in a CRUD .NET 6 Application which uses MongoDb as Database. And made production ready through docker and kubernetes.

## To run the code using docker containers
Use Commands

Create a new network mine is catalogNetwork.
Create the mongo container:
docker run  -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=catalogNetwork mongo
Create the catalog container:
docker run -it --rm --name catalogs -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=catalogNetwork catalog:v1


