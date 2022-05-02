# PoCEventos
Uma prova de conceitos para um sistema web com event streaming

Subir um MySQL para desenvolvimento:
```
docker run --name mysql-dev -p 3307:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8.0.28
```

Subir o ambiente todo:
```
docker-compose up
```