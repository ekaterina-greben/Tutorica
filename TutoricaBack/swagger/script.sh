#!/bin/bash

docker run --rm -p 7001:8080 -v $PWD:/files -e SWAGGER_JSON=/files/Tutorica_back_api.yml  --name Tutorica_swagger-ui swaggerapi/swagger-ui