# Catalogs

Catalog is a CRUD .NET 6 Application which uses MongoDb as Database. And made production ready through docker and kubernetes.

## To run locally require mongodb locally using docker

To create the mongo docker container : 
docker run  -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo

Also require the use of password from UserSecrets as it is not provided from app settings.

## To run the code using docker containers
Use Commands

Create a new network - catalogNetwork.
Also need to create the docker image for the project locally by using command in the root of the project : docker build -t catalog:v1 .

Create the mongo container : docker run  -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=catalogNetwork mongo

Create the catalog container : docker run -it --rm --name catalogs -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=catalogNetwork catalog:v1


## To Run the Application directly

You can pull the docker image from docker container hub and run locally: https://hub.docker.com/r/clintmathews/catalog

## To run the services through kubernetes

Commands to create pods:

Open terminal in Kuberntes folder:

kubectl apply -f .\catalog.yaml
kubectl apply -f .\mongodb.yaml

Create generic secret for mongodb password :
kubectl create secret generic catalog-secrets --from-literal=mongodb-password='Pass#word1'

To check deployments and pods:

kubectl get deployments
kubectl get pods  

The service will be up in locahost:80 and the mongodb as a statefull service.

To Scale the catalog service : kubectl scale deployments catalog-deployment --replicas=3




