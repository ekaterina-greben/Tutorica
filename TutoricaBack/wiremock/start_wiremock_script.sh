#!/bin/bash

docker run --rm -p 7020:8080 -v $PWD:/home/wiremock --name Tutorica_wiremock wiremock/wiremock --verbose
