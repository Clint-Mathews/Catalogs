# Catelogs

Catelog in a CRUD .NET 6 Application which uses MongoDb as Database. And made production ready through docker and kubernetes.

## To run locally require mongodb locally using docker

To create the mongo docker container : 
docker run  -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo

Also require the use of password from UserSecrets as it is not provided from app settings.

## To run the code using docker containers
Use Commands

Create a new network - catalogNetwork.

Create the mongo container : docker run  -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=catalogNetwork mongo

Create the catalog container : docker run -it --rm --name catalogs -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=catalogNetwork catalog:v1


## To Run the Application directly

You can pull the docker container from container hub : https://hub.docker.com/r/clintmathews/catalog

