# product_search_ms

## Create db docker image
```
cd .\cyberplace_product_search_db\ 
docker build -t cyberplace_product_search_db .
```
## Create ms docker image
```
cd .\cyberplace_product_search_ms\
docker build -t cyberplace_product_search_ms .
```
## Run docker-compose
```
docker-compose up
```
## Stop docker-compose
```
docker-compose down
```
