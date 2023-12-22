![Build and Tests](https://github.com/gustavocalheiros/humidifier/actions/workflows/nuke_build.yml/badge.svg) 
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gustavocalheiros_humidifier&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gustavocalheiros_humidifier)

This is a personal project that turns automatically on a Humidifier once the Humidity sensor gets below 35%

 There is a Python module (running into a Docker Container inside a Raspeberry PI 3b+), that reads the Temperature and Humidity info from a DHT 22  sensor, and writes the info into a SQLite table every X minutes.

 There is a C# module that reads this table and copies the information into an Azure table, then cleans up the local DB.

 Every X minutes, a job in the the C# module will read the information from the Azure Table and, based on the Humidity, will turn on a Humidifier that is plugged to a smart socket, via an webhook (created with https://ifttt.com/maker_webhooks)
