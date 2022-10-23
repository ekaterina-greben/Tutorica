#!/bin/bash

docker run --rm -p 7001:8080 -v $PWD:/files --name Tutorica_swagger-ui swaggerapi/swagger-ui


