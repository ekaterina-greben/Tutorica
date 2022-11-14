#!/bin/bash
rm -rf ./newman/*

newman run 'Tutorica.postman_collection.json' -e 'Tutorica.postman_environment.json' --reporters html,cli,json --reporter-json-export newman_report.json
